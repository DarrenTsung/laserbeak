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
using DT.Game.Scoring;

namespace DT.Game.Stats {
	public class StatsState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kShowDelay = 0.5f;

		protected override void OnStateEntered() {
			PlayerStatsView.Show(Restart);
		}

		protected override void OnStateExited() {
			PlayerScores.Clear();
		}

		private void Restart() {
			StateMachine_.GoToPlayerCustomization();
		}
	}
}