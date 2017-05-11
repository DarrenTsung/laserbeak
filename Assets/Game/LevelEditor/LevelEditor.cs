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
			cursor_ = ObjectPoolManager.Create<LevelEditorCursor>(GamePrefabs.Instance.LevelEditorCursorPrefab, parent: this.gameObject);
			cursor_.Init(inputDevice);

			GameObject previewObject = new GameObject("ObjectPreview");
			previewObject.transform.SetParent(this.transform);
			objectPreview_ = previewObject.AddComponent<ObjectPreview>();
			objectPreview_.Init(cursor_);
			objectPreview_.SetPreviewObject(GamePrefabs.Instance.LevelEditorObjects.FirstOrDefault());
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();
			cursor_ = null;
			objectPreview_ = null;
		}


		// PRAGMA MARK - Internal
		private LevelEditorCursor cursor_;
		private ObjectPreview objectPreview_;
	}
}