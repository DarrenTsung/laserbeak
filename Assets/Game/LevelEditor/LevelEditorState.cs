using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;

namespace DT.Game.LevelEditor {
	public class LevelEditorState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private LevelEditor levelEditor_;

		protected override void OnStateEntered() {
			ArenaManager.Instance.CleanupLoadedArena();

			InputDevice inputDevice = InputManager.Devices.First();

			levelEditor_ = ObjectPoolManager.Create<LevelEditor>(GamePrefabs.Instance.LevelEditorPrefab);
			levelEditor_.Init(inputDevice, ExitToMainMenu);
		}

		protected override void OnStateExited() {
			if (levelEditor_ != null) {
				ObjectPoolManager.Recycle(levelEditor_);
				levelEditor_ = null;
			}
		}

		private void ExitToMainMenu() {
			StateMachine_.GoToMainMenu();
		}
	}
}