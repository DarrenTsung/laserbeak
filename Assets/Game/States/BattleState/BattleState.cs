using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Pausing;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private GameMode previousGameMode_ = null;
		private GameMode currentGameMode_ = null;

		private PauseController pauseController_;

		protected override void OnInitialized() {
			GameNotifications.OnGameWon.AddListener(() => {
				// reset previous game mode
				previousGameMode_ = null;
			});
		}

		protected override void OnStateEntered() {
			// cleanup in-case
			PlayerSpawner.CleanupAllPlayers();
			CleanupCurrentGameMode();

			// TODO (darren): filtering based on options will be here
			do {
				if (previousGameMode_ == null) {
					currentGameMode_ = GameConstants.Instance.GameModes.First();
				} else {
					currentGameMode_ = GameConstants.Instance.GameModes.Random();
				}
			} while (previousGameMode_ == currentGameMode_);

			currentGameMode_.LoadArena();
			currentGameMode_.ShowIntroductionIfNecessary(() => {
				currentGameMode_.Activate(FinishBattle);
				previousGameMode_ = currentGameMode_;

				GameModeIntroView.OnIntroFinished += HandleIntroFinished;

				InGamePlayerCollectionView.Show();
				InGamePlayerHUDEffect.CreateForAllPlayers();
			});
		}

		protected override void OnStateExited() {
			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;

			CleanupPauseController();
			CleanupCurrentGameMode();

			InGamePlayerCollectionView.Hide();
		}

		private void FinishBattle() {
			StateMachine_.HandleBattleFinished();
		}

		private void CleanupCurrentGameMode() {
			if (currentGameMode_ != null) {
				currentGameMode_.Cleanup();
				currentGameMode_ = null;
			}
		}

		private void GoToTitleScreen() {
			StateMachine_.GoToMainMenu();
		}

		private void HandleIntroFinished() {
			CleanupPauseController();
			pauseController_ = new PauseController(skipCallback: FinishBattle, restartCallback: GoToTitleScreen);
		}

		private void CleanupPauseController() {
			if (pauseController_ != null) {
				pauseController_.Dispose();
				pauseController_ = null;
			}
		}
	}
}