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
	public class PlayerSpawnPointPlacer : BasePlacer {
		// PRAGMA MARK - Internal
		private int playerIndex_;

		private Vector3 CursorSnappedPosition_ {
			get { return LevelEditorUtil.SnapToGridCenter(LevelEditor_.Cursor.transform.position); }
		}

		private void Update() {
			if (PlacablePrefab_ == null) {
				return;
			}

			if (InputDevice_.Action3.WasReleased) {
				DynamicArenaData_.SerializePlayerSpawnPoint(playerIndex_, this.transform.position);
				UndoHistory_.RecordState();
				Refresh(ignoreCheck: true);
			}
		}

		protected override void Initialize() {
			playerIndex_ = PlacablePrefab_.GetComponent<LevelEditorPlayerSpawnPoint>().PlayerIndex;
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

			if (this.transform.position == CursorSnappedPosition_) {
				return;
			}

			this.transform.position = CursorSnappedPosition_;
		}
	}
}