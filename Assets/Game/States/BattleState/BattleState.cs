using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private GameMode previousGameMode_ = null;
		private GameMode currentGameMode_ = null;

		protected override void OnStateEntered() {
			// cleanup in-case
			PlayerSpawner.CleanupAllPlayers();
			CleanupCurrentGameMode();

			InGamePlayerCollectionView.Show();

			// TODO (darren): filtering based on options will be here
			do {
				if (previousGameMode_ == null) {
					currentGameMode_ = GameConstants.Instance.GameModes.First();
				} else {
					currentGameMode_ = GameConstants.Instance.GameModes.Random();
				}
			} while (previousGameMode_ == currentGameMode_);
			currentGameMode_.Activate(HandleGameModeFinished);
			previousGameMode_ = currentGameMode_;

			InGamePlayerHUDEffect.CreateForAllPlayers();
		}

		protected override void OnStateExited() {
			CleanupCurrentGameMode();

			InGamePlayerCollectionView.Hide();
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