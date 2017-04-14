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
		protected override void OnStateEntered() {
			PlayerSpawner.SpawnAllPlayers();
		}

		protected override void OnStateExited() {
			PlayerSpawner.CleanupAllPlayers();
		}

		protected override void OnStateUpdated() {
			if (InputManager.Devices.Any(d => d.CommandWasPressed)) {
				StateMachine_.GoToMainMenu();
			}
		}
	}
}