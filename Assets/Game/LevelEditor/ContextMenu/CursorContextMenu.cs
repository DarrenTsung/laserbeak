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
				new LevelObjectContextPopulator(levelEditor_),
			};

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		public void Dispose() {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
			HideContextMenu(recycle: true);
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;

		private ScrollableMenuPopup contextMenu_;

		// no selected item populators
		private IScrollableMenuPopulator[] populators_;

		private void HandleUpdate() {
			if (inputDevice_.Action4.WasPressed) {
				if (contextMenu_ != null) {
					HideContextMenu(recycle: true);
				} else {
					contextMenu_ = ScrollableMenuPopup.Create(inputDevice_, populators_.SelectMany(p => p.GetItems()));
					contextMenu_.OnHidden += HandleContextMenuHidden;
					levelEditor_.Cursor.SetLockedInPlace(true);
				}
			}
		}

		private void HandleContextMenuHidden() {
			HideContextMenu();
		}

		private void HideContextMenu(bool recycle = false) {
			if (contextMenu_ != null) {
				contextMenu_.OnHidden -= HandleContextMenuHidden;
				if (recycle) {
					ObjectPoolManager.Recycle(contextMenu_);
				}
				contextMenu_ = null;
			}

			if (levelEditor_.Cursor != null) {
				levelEditor_.Cursor.SetLockedInPlace(false);
			}
		}
	}
}