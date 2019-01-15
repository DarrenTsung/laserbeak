using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIIdleState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Static
		public static void SetShouldCheckDashAttackPredicate(Predicate<BattlePlayer> shouldCheckDashAttackPredicate) {
			shouldCheckDashAttackPredicate_ = shouldCheckDashAttackPredicate;
		}

		public static void ClearShouldCheckDashAttackPredicate() {
			shouldCheckDashAttackPredicate_ = null;
		}


		private static Predicate<BattlePlayer> shouldCheckDashAttackPredicate_;
		private static bool ShouldCheckDashAttack(BattlePlayer battlePlayer) {
			if (shouldCheckDashAttackPredicate_ == null) {
				return true;
			}

			return shouldCheckDashAttackPredicate_.Invoke(battlePlayer);
		}


		// PRAGMA MARK - Internal
		private const float kCheckAttackMinDelay = 0.3f;
		private const float kCheckAttackMaxDelay = 1.5f;

		private const float kDashAggroDistance = 6.0f;

		private IAIMovementAction movementAction_ = new AIMoveRandomlyAction();
		private CoroutineWrapper coroutine_;
		private CoroutineWrapper checkDashAttackCoroutine_;

		private float GetRandomCheckAttackDelay() {
			return UnityEngine.Random.Range(kCheckAttackMinDelay, kCheckAttackMaxDelay);
		}

		protected override void OnStateEntered() {
			movementAction_.Init(StateMachine_);

			checkDashAttackCoroutine_ = CoroutineWrapper.StartCoroutine(CheckDashAttack());
		}

		protected override void OnStateExited() {
			movementAction_.Dispose();

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

		private IEnumerator CheckDashAttack() {
			while (true) {
				yield return new WaitForSeconds(GetRandomCheckAttackDelay());

				if (!ShouldCheckDashAttack(StateMachine_.Player)) {
					continue;
				}

				BattlePlayer target = BattlePlayerUtil.GetClosestEnemyPlayerFor(StateMachine_.Player, whereCondition: (otherPlayer) => {
					return !AIUtil.DoesWallExistBetweenXZPoints(StateMachine_.Player.transform.position, otherPlayer.transform.position);
				});
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