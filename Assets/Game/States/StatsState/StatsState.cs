using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Battle.Stats;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.Stats {
	public class StatsState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kShowDelay = 0.5f;

		private PlayerStatsView view_;

		protected override void OnStateEntered() {
			if (RegisteredPlayers.AllPlayers.All(p => p.IsAI)) {
				PlayAgain();
				return;
			}

			view_ = PlayerStatsView.Show(PlayAgain, GoToMainMenu);
		}

		protected override void OnStateExited() {
			if (view_ != null) {
				ObjectPoolManager.Recycle(view_);
				view_ = null;
			}

			PlayerScores.Clear();
			StatsManager.ClearAllStats();
		}

		private void PlayAgain() {
			StateMachine_.GoToPlayerCustomization();
		}

		private void GoToMainMenu() {
			StateMachine_.GoToMainMenu();
		}
	}
}