using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.ScrollableMenuPopups;

namespace DT.Game.LevelEditor {
	public class CursorContextMenu {
		// PRAGMA MARK - Public Interface
		public CursorContextMenu(InputDevice inputDevice, LevelEditor levelEditor) {
			inputDevice_ = inputDevice;
			levelEditor_ = levelEditor;

			populators_ = new IScrollableMenuPopulator[] {
				new SpawnPointContextPopulator(inputDevice, levelEditor_),
				new LevelObjectContextPopulator(levelEditor_),
			};

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		public void Dispose() {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
			CleanupContextMenu();

			foreach (var populator in populators_) {
				populator.Dispose();
			}
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;

		private ScrollableMenuPopup contextMenu_;

		// no selected item populators
		private IScrollableMenuPopulator[] populators_;

		private void HandleUpdate() {
			if (inputDevice_.Action4.WasPressed) {
				CleanupContextMenu();
				contextMenu_ = ScrollableMenuPopup.Create(inputDevice_, populators_.SelectMany(p => p.GetItems()));
				contextMenu_.GetComponent<RecyclablePrefab>().OnCleanup += ClearContextMenuReferences;
			}
		}

		private void CleanupContextMenu() {
			if (contextMenu_ != null) {
				ObjectPoolManager.Recycle(contextMenu_);
				contextMenu_ = null;
			}
		}

		private void ClearContextMenuReferences(RecyclablePrefab unused) {
			contextMenu_.GetComponent<RecyclablePrefab>().OnCleanup -= ClearContextMenuReferences;
			contextMenu_ = null;
		}
	}
}