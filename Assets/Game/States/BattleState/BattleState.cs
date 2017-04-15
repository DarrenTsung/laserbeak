using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private GameMode currentGameMode_ = null;

		protected override void OnStateEntered() {
			// cleanup in-case
			CleanupCurrentGameMode();

			// TODO (darren): filtering based on options will be here
			currentGameMode_ = GameConstants.Instance.GameModes.Random();
			currentGameMode_.Activate(HandleGameModeFinished);
		}

		protected override void OnStateExited() {
			CleanupCurrentGameMode();
		}

		private void HandleGameModeFinished() {
			StateMachine_.HandleBattleFinished();
		}

		private void CleanupCurrentGameMode() {
			if (currentGameMode_ != null) {
				currentGameMode_.Cleanup();
				currentGameMode_ = null;
			}
		}
	}
}