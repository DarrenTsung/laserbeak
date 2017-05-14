using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Walls;

namespace DT.Game.LevelEditor {
	public class WallPlacer : BasePlacer {
		// PRAGMA MARK - Internal
		private Wall wall_;
		private bool placingWall_ = false;

		private Wall PreviewWall_ {
			get { return wall_ ?? (wall_ = PreviewObject_.GetComponent<Wall>()); }
		}

		private Vector3 CursorSnappedPosition_ {
			get { return SnapPosition(LevelEditor_.Cursor.transform.position); }
		}

		private void Update() {
			if (PlacablePrefab_ == null) {
				return;
			}

			if (InputDevice_.Action3.WasReleased) {
				if (placingWall_) {
					DynamicArenaData_.SerializeWall(PlacablePrefab_, this.transform.position, PreviewWall_.VertexLocalPositions);
					UndoHistory_.RecordState();
					placingWall_ = false;

					PreviewWall_.SetVertexLocalPositions(new Vector3[] { Vector3.zero, Vector3.zero });
				} else {
					placingWall_ = true;
				}

				Refresh(ignoreCheck: true);
			}
		}

		protected override void Initialize() {
			placingWall_ = false;
			PreviewWall_.SetVertexLocalPositions(new Vector3[] { Vector3.zero, Vector3.zero });
		}

		protected override void HandleCusorMoved() {
			Refresh();
		}

		protected override void HandlePreviewObjectCreated() {
			Refresh();
		}

		private void Refresh(bool ignoreCheck = false) {
			// Race Condition - Cursor moves before Update() above is hit - need to prevent this until after Update is run
			if (!ignoreCheck && InputDevice_.Action3.WasReleased) {
				return;
			}

			if (placingWall_) {
				Vector3 vertexLocalPosition = CursorSnappedPosition_ - this.transform.position;
				PreviewWall_.SetVertexLocalPositions(new Vector3[] { Vector3.zero, vertexLocalPosition });
			} else {
				if (this.transform.position == CursorSnappedPosition_) {
					return;
				}

				this.transform.position = CursorSnappedPosition_;
			}
		}

		// snap onto vertices
		private Vector3 SnapPosition(Vector3 position) {
			Vector3 newPosition = position;

			newPosition = newPosition.SetY(0.0f);
			newPosition = newPosition.SetX(Mathf.Round(newPosition.x));
			newPosition = newPosition.SetZ(Mathf.Round(newPosition.z));

			return newPosition;
		}
	}
}