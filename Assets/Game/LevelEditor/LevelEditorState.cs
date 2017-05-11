using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class LevelEditorState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private LevelEditor levelEditor_;

		protected override void OnStateEntered() {
			InputDevice inputDevice = InputManager.Devices.First();

			levelEditor_ = ObjectPoolManager.Create<LevelEditor>(GamePrefabs.Instance.LevelEditorPrefab);
			levelEditor_.Init(inputDevice);
		}

		protected override void OnStateExited() {
			if (levelEditor_ != null) {
				ObjectPoolManager.Recycle(levelEditor_);
				levelEditor_ = null;
			}
		}
	}
}