using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class LevelEditor : MonoBehaviour, IRecycleCleanupSubscriber {
		public void Init(InputDevice inputDevice) {
			dynamicArenaView_.Init(dynamicArenaData_);

			cursor_ = ObjectPoolManager.Create<LevelEditorCursor>(GamePrefabs.Instance.LevelEditorCursorPrefab, parent: this.gameObject);
			cursor_.Init(inputDevice);

			GameObject previewObject = new GameObject("ObjectPlacer");
			previewObject.transform.SetParent(this.transform);
			objectPreview_ = previewObject.AddComponent<ObjectPlacer>();
			objectPreview_.Init(dynamicArenaData_, inputDevice, cursor_);
			objectPreview_.SetObjectToPlace(GamePrefabs.Instance.LevelEditorObjects.FirstOrDefault());
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (cursor_ != null) {
				ObjectPoolManager.Recycle(cursor_);
				cursor_ = null;
			}

			if (objectPreview_ != null) {
				GameObject.Destroy(objectPreview_);
				objectPreview_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private DynamicArenaView dynamicArenaView_;

		private LevelEditorCursor cursor_;
		private ObjectPlacer objectPreview_;

		private DynamicArenaData dynamicArenaData_ = new DynamicArenaData();
	}
}