using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class ObjectPreview : MonoBehaviour, IRecycleCleanupSubscriber {
		public void Init(DynamicArenaData dynamicArenaData, InputDevice inputDevice, LevelEditorCursor cursor) {
			dynamicArenaData_ = dynamicArenaData;
			inputDevice_ = inputDevice;

			cursor_ = cursor;
			cursor_.OnMoved += RefreshPosition;

			RefreshPosition();
		}

		public void SetPreviewObject(GameObject prefab) {
			if (prefab == null) {
				Debug.LogWarning("Cannot set preview object of null object!");
				return;
			}

			placablePrefab_ = prefab;
			ObjectPoolManager.Create(prefab, parent: this.gameObject);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();

			if (cursor_ != null) {
				cursor_.OnMoved -= RefreshPosition;
				cursor_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private DynamicArenaData dynamicArenaData_;
		private InputDevice inputDevice_;
		private LevelEditorCursor cursor_;
		private GameObject placablePrefab_;

		private void Update() {
			if (placablePrefab_ == null) {
				return;
			}

			if (inputDevice_.Action3.WasReleased) {
				dynamicArenaData_.SerializeObject(placablePrefab_, this.transform.position, Quaternion.identity, this.transform.localScale);
			}
		}

		private void RefreshPosition() {
			// snap onto grid - assume preview object is 1x1 for now
			Vector3 newPosition = cursor_.transform.position;
			newPosition = newPosition.SetY(0.0f);
			newPosition = newPosition.SetX((int)(newPosition.x + LevelEditorConstants.kHalfGridSize) - LevelEditorConstants.kHalfGridSize);
			newPosition = newPosition.SetZ((int)(newPosition.z + LevelEditorConstants.kHalfGridSize) - LevelEditorConstants.kHalfGridSize);

			this.transform.position = newPosition;
		}
	}
}