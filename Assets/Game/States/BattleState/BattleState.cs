using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject battlePrefab_;

		private GameObject battleObject_;

		protected override void OnStateEntered() {
			battleObject_ = ObjectPoolManager.Create(battlePrefab_);
		}

		protected override void OnStateExited() {
			if (battleObject_ != null) {
				ObjectPoolManager.Recycle(battleObject_);
				battleObject_ = null;
			}
		}

		protected override void OnStateUpdated() {
			if (InputManager.Devices.Any(d => d.CommandWasPressed)) {
				StateMachine_.GoToMainMenu();
			}
		}
	}
}