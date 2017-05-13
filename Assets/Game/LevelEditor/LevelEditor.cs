using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class LevelEditor : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public LevelEditorCursor Cursor {
			get { return cursor_; }
		}

		public ObjectPlacer ObjectPlacer {
			get { return objectPlacer_; }
		}

		public void Init(InputDevice inputDevice, Action exitCallback) {
			cursorContextMenu_ = new CursorContextMenu(inputDevice, this);
			levelEditorMenu_ = new LevelEditorMenu(inputDevice, this, exitCallback, SaveDataToEditor);

			dynamicArenaData_ = new DynamicArenaData();
			undoHistory_ = new UndoHistory(dynamicArenaData_, inputDevice);

			dynamicArenaView_.Init(dynamicArenaData_);

			cursor_ = ObjectPoolManager.Create<LevelEditorCursor>(GamePrefabs.Instance.LevelEditorCursorPrefab, parent: this.gameObject);
			cursor_.Init(inputDevice);

			objectPlacer_ = ObjectPoolManager.Create<ObjectPlacer>(GamePrefabs.Instance.ObjectPlacerPrefab, parent: this.gameObject);
			objectPlacer_.Init(dynamicArenaData_, undoHistory_, inputDevice, cursor_);
			objectPlacer_.SetObjectToPlace(GamePrefabs.Instance.LevelEditorObjects.FirstOrDefault());
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (cursor_ != null) {
				ObjectPoolManager.Recycle(cursor_);
				cursor_ = null;
			}

			if (objectPlacer_ != null) {
				ObjectPoolManager.Recycle(objectPlacer_);
				objectPlacer_ = null;
			}

			if (undoHistory_ != null) {
				undoHistory_.Dispose();
				undoHistory_ = null;
			}

			if (levelEditorMenu_ != null) {
				levelEditorMenu_.Dispose();
				levelEditorMenu_ = null;
			}

			if (cursorContextMenu_ != null) {
				cursorContextMenu_.Dispose();
				cursorContextMenu_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private DynamicArenaView dynamicArenaView_;

		private LevelEditorCursor cursor_;
		private ObjectPlacer objectPlacer_;
		private UndoHistory undoHistory_;
		private LevelEditorMenu levelEditorMenu_;
		private CursorContextMenu cursorContextMenu_;

		private DynamicArenaData dynamicArenaData_ = new DynamicArenaData();

		private void SaveDataToEditor() {
			string directoryPath = Path.Combine(Application.dataPath, "CustomLevels");
			Directory.CreateDirectory(directoryPath);

			string[] filenames = Directory.GetFiles(directoryPath);
			int index = 1;

			string filename = "";
			do {
				filename = string.Format("CustomLevel{0}.txt", index);
				index++;
			} while (filenames.Any(f => f == filename));

			File.WriteAllText(Path.Combine(directoryPath, filename), dynamicArenaData_.Serialize());
		}
	}
}