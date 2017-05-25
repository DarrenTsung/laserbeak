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
		// PRAGMA MARK - Static
		public static GameMode QueuedGameMode = null;


		// PRAGMA MARK - Internal
		private GameMode currentGameMode_ = null;

		private PauseController pauseController_;

		protected override void OnInitialized() {
			// stub
		}

		protected override void OnStateEntered() {
			// cleanup in-case
			PlayerSpawner.CleanupAllPlayers();
			CleanupCurrentGameMode();

			// TODO (darren): filtering based on options will be here
			if (QueuedGameMode != null) {
				currentGameMode_ = QueuedGameMode;
				QueuedGameMode = null;
			} else {
				var unlockedModes = GameModesProgression.FilterByUnlocked(GameConstants.Instance.GameModes);
				currentGameMode_ = GameModesPlayedTracker.FilterByLeastPlayed(unlockedModes).ToArray().Random();
			}

			currentGameMode_.LoadArena();
			currentGameMode_.ShowInstructionsIfNecessary(() => {
				currentGameMode_.Activate(FinishBattle);

				GameModeIntroView.OnIntroFinished += HandleIntroFinished;

				InGamePlayerCollectionView.Show();
				InGamePlayerHUDEffect.CreateForAllPlayers();
			});
		}

		protected override void OnStateExited() {
			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;

			CleanupPauseController();
			CleanupCurrentGameMode();

			GameModeIntroView.Cleanup();

			InGamePlayerCollectionView.Hide();
			InGamePlayerHUDEffect.CleanupAllEffects();
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