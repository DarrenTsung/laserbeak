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
using DT.Game.Players;

namespace DT.Game.Scoring {
	public class ScoringState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kShowDelay = 0.5f;

		protected override void OnStateEntered() {
			if (!PlayerScores.HasPendingScores) {
				HandleScoringFinished();
				return;
			}

			InGameConstants.AllowChargingLasers = false;
			InGameConstants.EnableQuacking = true;
			BattleCamera.Instance.SetSurvivingPlayersAsTransformsOfInterest();

			CoroutineWrapper.DoAfterDelay(kShowDelay, () => {
				InGamePlayerScoringView.Show(HandleScoringFinished);
			});
		}

		protected override void OnStateExited() {
			// cleanup battle here
			BattlePlayerTeams.ClearTeams();
			BattleRecyclables.Clear();
			PlayerSpawner.CleanupAllPlayers();
			AISpawner.CleanupAllPlayers();

			InGameConstants.AllowChargingLasers = true;
			InGameConstants.EnableQuacking = false;
			BattleCamera.Instance.ClearTransformsOfInterest();
		}

		private void HandleScoringFinished() {
			if (PlayerScores.HasWinner) {
				PlayerScores.Clear();
				StateMachine_.GoToPlayerCustomization();
			} else {
				StateMachine_.Continue();
			}
		}
	}
}