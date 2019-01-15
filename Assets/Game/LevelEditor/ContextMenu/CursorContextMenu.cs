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
			ScrollableMenuPopup.Hide();

			foreach (var populator in populators_) {
				populator.Dispose();
			}
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;

		// no selected item populators
		private IScrollableMenuPopulator[] populators_;

		private void HandleUpdate() {
			if (inputDevice_.Action4.WasPressed) {
				ScrollableMenuPopup.Show(inputDevice_, populators_.SelectMany(p => p.GetItems()));
			}
		}
	}
}