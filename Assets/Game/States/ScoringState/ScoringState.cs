using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Scoring {
	public class ScoringState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kShowDelay = 0.5f;

		protected override void OnStateEntered() {
			CoroutineWrapper.DoAfterDelay(kShowDelay, () => {
				InGamePlayerScoringView.Show(HandleScoringFinished);
			});
		}

		protected override void OnStateExited() {
			// stub
		}

		private void HandleScoringFinished() {
			StateMachine_.StartBattle();
		}
	}
}