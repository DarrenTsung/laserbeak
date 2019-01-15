using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;

namespace DT.Game.Scoring {
	public class ScoringState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kShowDelay = 1.7f;
		private const float kResetCameraDelay = 0.7f;

		private ActionHintAccumulator celebrateActionAccumulator_;

		protected override void OnInitialized() {
			celebrateActionAccumulator_ = new ActionHintAccumulator(HintKey.CelebrateAction,
											new EventRouter<BattlePlayer>(GameNotifications.OnBattlePlayerCelebrated)
												.WithPredicate(battlePlayer => BattlePlayerUtil.IsHuman(battlePlayer)));
		}

		protected override void OnStateEntered() {
			if (!PlayerScores.HasPendingScores) {
				HandleScoringFinished();
				return;
			}

			InGameConstants.AllowChargingLasers = false;
			InGameConstants.EnableQuacking = true;

			GameModesProgressionNextUnlockView.ShowIfPossible();
			ActionHintTrackerUtil.ShowIfNecessary(HintKey.CelebrateAction, HintKey.ReflectAction);
			celebrateActionAccumulator_.BeginAccumulating();

			if (InGameConstants.ZoomInOnSurvivors) {
				BattleCamera.Instance.SetSurvivingPlayersAsTransformsOfInterest();
			}

			if (InGameConstants.ShowScoringView) {
				CoroutineWrapper.DoAfterDelay(kShowDelay, () => {
					InGamePlayerScoringView.Show(HandleScoringFinished);
				});
			} else {
				CoroutineWrapper.DoAfterDelay(kShowDelay + 2.0f, () => {
					HandleScoringFinished();
				});
			}
		}

		protected override void OnStateExited() {
			// cleanup battle here
			BattlePlayerTeams.ClearTeams();
			BattleRecyclables.Clear();
			PlayerSpawner.CleanupAllPlayers();
			AISpawner.CleanupAllPlayers();

			InGameConstants.AllowChargingLasers = true;
			InGameConstants.EnableQuacking = false;

			celebrateActionAccumulator_.EndAccumulating();

			Hint.Hide();
		}

		private void HandleScoringFinished() {
			BattleCamera.Instance.ClearTransformsOfInterest();
			BattlePlayerPart.RemoveCollidersFromAll();

			CoroutineWrapper.DoAfterDelay(kResetCameraDelay, () => {
				if (PlayerScores.HasWinner) {
					StateMachine_.HandleGameFinished();
				} else {
					StateMachine_.Continue();
				}
			});
		}
	}
}