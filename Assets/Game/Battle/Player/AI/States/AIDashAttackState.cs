using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIDashAttackState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Internal
		private float predictedDashDistance_;
		private BattlePlayer target_;

		protected override void OnStateEntered() {
			predictedDashDistance_ = AIUtil.GetRandomPredictedDashDistance();

			target_ = BattlePlayerUtil.GetClosestEnemyPlayerFor(StateMachine_.Player);
		}

		protected override void OnStateExited() {
			target_ = null;
		}

		protected override void OnStateUpdated() {
			if (target_ == null || !BattlePlayer.ActivePlayers.Contains(target_)) {
				StateMachine_.SwitchState(AIStateMachine.State.Idle);
				return;
			}

			Vector2 playerToTargetVector = BattlePlayerUtil.XZVectorFromTo(StateMachine_.Player, target_);

			Vector2 targetDirection = playerToTargetVector.normalized;
			if (playerToTargetVector.magnitude <= predictedDashDistance_) {
				StateMachine_.Dash(targetDirection);
			} else {
				StateMachine_.InputState.LerpMovementVectorTowards(targetDirection);
			}
		}
	}
}