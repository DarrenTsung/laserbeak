using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIIdleState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Internal
		private const float kCheckAttackMinDelay = 0.3f;
		private const float kCheckAttackMaxDelay = 1.5f;

		private const float kDashAggroDistance = 6.0f;

		private IAIMovementAction movementAction_;
		private CoroutineWrapper coroutine_;
		private CoroutineWrapper checkDashAttackCoroutine_;

		private float GetRandomCheckAttackDelay() {
			return UnityEngine.Random.Range(kCheckAttackMinDelay, kCheckAttackMaxDelay);
		}

		protected override void OnStateEntered() {
			movementAction_ = new AIMoveRandomlyAction(StateMachine_);

			checkDashAttackCoroutine_ = CoroutineWrapper.StartCoroutine(CheckDashAttack());
		}

		protected override void OnStateExited() {
			if (movementAction_ != null) {
				movementAction_.Dispose();
				movementAction_ = null;
			}

			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}

			if (checkDashAttackCoroutine_ != null) {
				checkDashAttackCoroutine_.Cancel();
				checkDashAttackCoroutine_ = null;
			}
		}

		protected override void OnStateUpdated() {
			if (coroutine_ == null && InGameConstants.IsAllowedToChargeLasers(StateMachine_.Player)) {
				coroutine_ = CoroutineWrapper.DoAfterDelay(GetRandomCheckAttackDelay(), () => {
					StateMachine_.SwitchState(AIStateMachine.State.Attack);
				});
			}

		}

		protected IEnumerator CheckDashAttack() {
			while (true) {
				yield return new WaitForSeconds(GetRandomCheckAttackDelay());

				BattlePlayer target = BattlePlayerUtil.GetClosestEnemyPlayerFor(StateMachine_.Player);
				if (target == null) {
					continue;
				}

				Vector2 playerToTargetVector = BattlePlayerUtil.XZVectorFromTo(StateMachine_.Player, target);

				if (playerToTargetVector.magnitude <= kDashAggroDistance) {
					StateMachine_.SwitchState(AIStateMachine.State.DashAttack);
				}
			}
		}
	}
}