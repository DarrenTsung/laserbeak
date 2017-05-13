using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class LevelEditorMenu {
		// PRAGMA MARK - Public Interface
		public LevelEditorMenu(InputDevice inputDevice, LevelEditor levelEditor, Action exitCallback, Action saveCallback) {
			inputDevice_ = inputDevice;
			levelEditor_ = levelEditor;
			exitCallback_ = exitCallback;
			saveCallback_ = saveCallback;

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		public void Dispose() {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
			HideMenu();
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;
		private Action exitCallback_;
		private Action saveCallback_;

		private void HandleUpdate() {
			if (inputDevice_.Command.WasPressed && !MenuView.Showing) {
				levelEditor_.Cursor.SetLockedInPlace(true);
				MenuView.Show(inputDevice_, "LEVEL EDITOR", new Dictionary<string, Action>() {
					{ "BACK TO EDITING", HideMenu },
					{ "SAVE", () => {
						saveCallback_.Invoke();
						HideMenu();
					} },
					{ "EXIT EDITING", () => exitCallback_.Invoke() },
				});
			}
		}

		private void HideMenu() {
			MenuView.Hide();
			levelEditor_.Cursor.SetLockedInPlace(false);
		}
	}
}