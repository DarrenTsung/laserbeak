using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class ObjectPlacer : MonoBehaviour, IRecycleCleanupSubscriber {
		public void Init(DynamicArenaData dynamicArenaData, InputDevice inputDevice, LevelEditorCursor cursor) {
			dynamicArenaData_ = dynamicArenaData;
			inputDevice_ = inputDevice;

			cursor_ = cursor;
			cursor_.OnMoved += RefreshPositionAndScale;

			RefreshPositionAndScale();
		}

		public void SetObjectToPlace(GameObject prefab) {
			if (prefab == null) {
				Debug.LogWarning("Cannot set preview object of null object!");
				return;
			}

			placablePrefab_ = prefab;
			previewObject_ = ObjectPoolManager.Create(prefab, parent: this.gameObject);
			foreach (var collider in previewObject_.GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}
			RefreshPreviewObjectValidity();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (previewObject_ != null) {
				GameObject.Destroy(previewObject_);
				previewObject_ = null;
			}

			if (cursor_ != null) {
				cursor_.OnMoved -= RefreshPositionAndScale;
				cursor_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private const float kPressedLengthToScale = 0.2f;

		private DynamicArenaData dynamicArenaData_;
		private InputDevice inputDevice_;
		private LevelEditorCursor cursor_;
		private GameObject placablePrefab_;
		private GameObject previewObject_;

		private Vector3 pressedStartPosition_;

		private bool ShouldScaleFromStartPosition {
			get { return inputDevice_.Action3.IsPressed; }
		}

		private void Update() {
			if (placablePrefab_ == null) {
				return;
			}

			if (inputDevice_.Action3.WasPressed) {
				pressedStartPosition_ = this.transform.position;
			}

			if (inputDevice_.Action3.WasReleased) {
				dynamicArenaData_.SerializeObject(placablePrefab_, this.transform.position, Quaternion.identity, this.transform.localScale);
				this.transform.localScale = Vector3.one;
				RefreshPositionAndScale();
			}
		}

		private void RefreshPositionAndScale() {
			// snap onto grid - assume preview object is 1x1 for now
			Vector3 cursorSnappedPosition = SnapPosition(cursor_.transform.position);
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
		}

		private void RefreshPreviewObjectValidity() {
			if (previewObject_ == null) {
				return;
			}

			Vector3 halfExtents = this.transform.localScale / 2.0f;
			halfExtents = halfExtents.SetX(halfExtents.x - 0.1f);
			halfExtents = halfExtents.SetZ(halfExtents.z - 0.1f);

			bool hit = Physics.BoxCast(this.transform.position + Vector3.up, halfExtents: halfExtents, direction: -Vector3.up, orientation: Quaternion.identity, maxDistance: Mathf.Infinity, layerMask: InGameConstants.PlatformsLayerMask);
			foreach (var renderer in previewObject_.GetComponentsInChildren<Renderer>()) {
				renderer.material.color = hit ? Color.red : Color.green;
			}
		}

		private Vector3 SnapPosition(Vector3 position) {
			Vector3 newPosition = position;
			newPosition = newPosition.SetY(0.0f);
			int clampedX = MathUtil.Clamp((int)newPosition.x, (int)-LevelEditorConstants.kArenaHalfWidth + 1, (int)LevelEditorConstants.kArenaHalfWidth - 1);
			int clampedZ = MathUtil.Clamp((int)newPosition.z, (int)-LevelEditorConstants.kArenaHalfHeight + 1, (int)LevelEditorConstants.kArenaHalfHeight - 1);

			float newX = clampedX;
			float newZ = clampedZ;
			if (clampedX != 0) {
				newX += (clampedX > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newX = cursor_.transform.position.x >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}
			if (clampedZ != 0) {
				newZ += (clampedZ > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newZ = cursor_.transform.position.z >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}

			newPosition = newPosition.SetX(newX);
			newPosition = newPosition.SetZ(newZ);

			return newPosition;
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
	}
}