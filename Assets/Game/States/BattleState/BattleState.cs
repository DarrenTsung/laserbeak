using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Lasers;
using DT.Game.Battle.Pausing;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Static
		public static event Action OnBattleStateEntered = delegate {};
		public static GameMode QueuedGameMode = null;

		public static void SkipAndLoadGameMode(GameMode gameMode) {
			if (onSkipBattle_ == null) {
				// can't skip to game mode if no ability to skip
				return;
			}

			QueuedGameMode = gameMode;
			onSkipBattle_.Invoke();
		}


		private static Action onSkipBattle_ = null;


		// PRAGMA MARK - Internal
		private GameMode currentGameMode_ = null;

		private PauseController pauseController_;
		private ActionHintAccumulator reflectActionAccumulator_;

		protected override void OnInitialized() {
			reflectActionAccumulator_ = new ActionHintAccumulator(HintKey.ReflectAction,
										new EventRouter<Laser, BattlePlayer>(GameNotifications.OnBattlePlayerReflectLaser)
											.WithPredicate((laser, battlePlayer) => BattlePlayerUtil.IsHuman(battlePlayer)));
		}

		protected override void OnStateEntered() {
			OnBattleStateEntered.Invoke();

			// cleanup in-case
			PlayerSpawner.CleanupAllPlayers();
			CleanupCurrentGameMode();

			InGameConstants.BattlePlayerPartsFade = false;
			reflectActionAccumulator_.BeginAccumulating();

			// TODO (darren): filtering based on options will be here
			if (QueuedGameMode != null) {
				currentGameMode_ = QueuedGameMode;
				QueuedGameMode = null;
			} else {
				var unlockedModes = GameModesProgression.FilterByUnlocked(GameConstants.Instance.GameModes);
				currentGameMode_ = GameModesPlayedTracker.FilterByLeastPlayed(unlockedModes).ToArray().Random();
			}

			currentGameMode_.LoadArena(() => {
				currentGameMode_.ShowInstructionsIfNecessary(() => {
					currentGameMode_.Activate(FinishBattle);

					GameModeIntroView.OnIntroFinished += HandleIntroFinished;

					InGamePlayerHUDEffect.CreateForAllPlayers();
				});
			});

			onSkipBattle_ = FinishBattle;
		}

		protected override void OnStateExited() {
			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;

			CleanupPauseController();
			CleanupCurrentGameMode();

			GameModeIntroView.Cleanup();
			reflectActionAccumulator_.EndAccumulating();

			InGamePlayerCollectionView.Hide();
			InGamePlayerHUDEffect.CleanupAllEffects();

			onSkipBattle_ = null;
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