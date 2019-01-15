using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class PlatformPlacer : BasePlacer {
		// PRAGMA MARK - Internal
		[Header("Line Renderers")]
		[SerializeField]
		private LineRenderer topRightLineRenderer_;
		[SerializeField]
		private LineRenderer bottomRightLineRenderer_;
		[SerializeField]
		private LineRenderer bottomLeftLineRenderer_;
		[SerializeField]
		private LineRenderer topLeftLineRenderer_;

		private Vector3 pressedStartPosition_;

		private bool ShouldScaleFromStartPosition {
			get { return InputDevice_.Action3.IsPressed; }
		}

		private void Update() {
			if (PlacablePrefab_ == null) {
				return;
			}

			if (InputDevice_.Action3.WasPressed) {
				pressedStartPosition_ = this.transform.position;
				RefreshPositionAndScale(ignoreCheck: true);
			}

			if (InputDevice_.Action3.WasReleased) {
				Place();
				this.transform.localScale = Vector3.one;
				RefreshPositionAndScale(ignoreCheck: true);
			}
		}

		protected override int SerializeToDynamicData() {
			return DynamicArenaData_.SerializeObject(PlacablePrefab_, this.transform.position, Quaternion.identity, this.transform.localScale);
		}

		protected override void HandleCusorMoved() {
			RefreshPositionAndScale();
		}

		protected override void HandlePreviewObjectCreated() {
			RefreshPositionAndScale();
		}

		private void RefreshPositionAndScale(bool ignoreCheck = false) {
			// Race Condition - Cursor moves before Update() above is hit - need to prevent this until after Update is run
			if (!ignoreCheck && (InputDevice_.Action3.WasReleased || InputDevice_.Action3.WasPressed)) {
				return;
			}

			// snap onto grid - assume preview object is 1x1 for now
			Vector3 cursorSnappedPosition = LevelEditorUtil.SnapToGridCenter(LevelEditor_.Cursor.transform.position);
			if (ShouldScaleFromStartPosition) {
				Vector3 unsnappedMidpoint = Vector3.Lerp(pressedStartPosition_, cursorSnappedPosition, 0.5f);

				Vector3 startPosition = SnappedToHintedVertex(pressedStartPosition_, pressedStartPosition_ - unsnappedMidpoint);
				Vector3 endPosition = SnappedToHintedVertex(cursorSnappedPosition, cursorSnappedPosition - unsnappedMidpoint);

				Vector3 midpoint = Vector3.Lerp(startPosition, endPosition, 0.5f);

				Vector3 diagonalVector = endPosition - startPosition;
				Vector3 localScale = new Vector3(Mathf.Max(Mathf.Abs(diagonalVector.x), 1.0f),
												 1.0f,
												 Mathf.Max(Mathf.Abs(diagonalVector.z), 1.0f));

				if (this.transform.position == midpoint && this.transform.localScale == localScale) {
					return;
				}

				this.transform.position = midpoint;
				this.transform.localScale = localScale;
			} else {
				if (this.transform.position == cursorSnappedPosition) {
					return;
				}

				this.transform.position = cursorSnappedPosition;
			}

			RefreshPreviewObjectValidity();
			RefreshGuideLines();
		}

		private void RefreshPreviewObjectValidity() {
			if (PreviewObject_ == null) {
				return;
			}

			Vector3 halfExtents = this.transform.localScale / 2.0f;
			halfExtents = halfExtents.SetX(halfExtents.x - 0.1f);
			halfExtents = halfExtents.SetZ(halfExtents.z - 0.1f);

			bool hit = Physics.BoxCast(this.transform.position + Vector3.up, halfExtents: halfExtents, direction: -Vector3.up, orientation: Quaternion.identity, maxDistance: Mathf.Infinity, layerMask: InGameConstants.PlatformsLayerMask);
			foreach (var renderer in PreviewObject_.GetComponentsInChildren<Renderer>()) {
				renderer.material.color = hit ? Color.red : Color.green;
			}
		}

		private Vector3 SnappedToHintedVertex(Vector3 position, Vector3 hintVector) {
			// NOTE (darren): this doesn't respect grid size - would need to scale vectors by grid size first
			Vector3 snapped = position;
			if (!Mathf.Approximately(hintVector.x, 0.0f)) {
				snapped = snapped.SetX(hintVector.x > 0.0f ? Mathf.Ceil(snapped.x) : Mathf.Floor(snapped.x));
			}
			if (!Mathf.Approximately(hintVector.z, 0.0f)) {
				snapped = snapped.SetZ(hintVector.z > 0.0f ? Mathf.Ceil(snapped.z) : Mathf.Floor(snapped.z));
			}
			return snapped;
		}

		private void RefreshGuideLines() {
			// assumption is 1x1 block still
			Vector3 minPoint = this.transform.position - (this.transform.localScale / 2.0f);
			Vector3 maxPoint = this.transform.position + (this.transform.localScale / 2.0f);

			minPoint = minPoint.SetY(0.0f);
			maxPoint = maxPoint.SetY(0.0f);

			Vector3 topRightPoint = maxPoint;
			Vector3 topLeftPoint = maxPoint.SetX(minPoint.x);
			Vector3 bottomLeftPoint = minPoint;
			Vector3 bottomRightPoint = minPoint.SetX(maxPoint.x);

			Vector3 gridTopRightPoint = topRightPoint.SetZ(LevelEditorConstants.kArenaHalfHeight);
			Vector3 gridRightTopPoint = topRightPoint.SetX(LevelEditorConstants.kArenaHalfWidth);
			topRightLineRenderer_.positionCount = 4;
			topRightLineRenderer_.SetPositions(new Vector3[] { topRightPoint, gridTopRightPoint, topRightPoint, gridRightTopPoint });

			Vector3 gridTopLeftPoint = topLeftPoint.SetZ(LevelEditorConstants.kArenaHalfHeight);
			Vector3 gridLeftTopPoint = topLeftPoint.SetX(-LevelEditorConstants.kArenaHalfWidth);
			topLeftLineRenderer_.positionCount = 4;
			topLeftLineRenderer_.SetPositions(new Vector3[] { topLeftPoint, gridTopLeftPoint, topLeftPoint, gridLeftTopPoint });

			Vector3 gridBottomRightPoint = bottomRightPoint.SetZ(-LevelEditorConstants.kArenaHalfHeight);
			Vector3 gridRightBottomPoint = bottomRightPoint.SetX(LevelEditorConstants.kArenaHalfWidth);
			bottomRightLineRenderer_.positionCount = 4;
			bottomRightLineRenderer_.SetPositions(new Vector3[] { bottomRightPoint, gridBottomRightPoint, bottomRightPoint, gridRightBottomPoint });

			Vector3 gridBottomLeftPoint = bottomLeftPoint.SetZ(-LevelEditorConstants.kArenaHalfHeight);
			Vector3 gridLeftBottomPoint = bottomLeftPoint.SetX(-LevelEditorConstants.kArenaHalfWidth);
			bottomLeftLineRenderer_.positionCount = 4;
			bottomLeftLineRenderer_.SetPositions(new Vector3[] { bottomLeftPoint, gridBottomLeftPoint, bottomLeftPoint, gridLeftBottomPoint });
		}
	}
}