using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIAttackState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Internal
		private const float kNearDistance = 5.0f;

		private BattlePlayerInputChargedLaser chargedLaserComponent_;
		private BattlePlayerInputChargedLaser ChargedLaserComponent_ {
			get {
				if (chargedLaserComponent_ == null) {
					chargedLaserComponent_ = StateMachine_.Player.GetComponentInChildren<BattlePlayerInputChargedLaser>();
				}
				return chargedLaserComponent_;
			}
		}

		protected override void OnStateEntered() {
			StateMachine_.InputState.LaserPressed = true;
			ChargedLaserComponent_.OnFullCharge += HandleFullyChargedLaser;
		}

		protected override void OnStateExited() {
			// stub
		}

		protected override void OnStateUpdated() {
			HeadTowardsClosestEnemyPlayer();
		}

		private void HeadTowardsClosestEnemyPlayer() {
			if (StateMachine_.Player == null) {
				return;
			}

			BattlePlayer closestEnemyPlayer = BattlePlayer.ActivePlayers.Where(p => p != StateMachine_.Player).Min(p => (p.transform.position - StateMachine_.Player.transform.position).magnitude);
			if (closestEnemyPlayer == null) {
				StateMachine_.InputState.MovementVector = Vector2.zero;
				return;
			}

			Vector3 distance = closestEnemyPlayer.transform.position - StateMachine_.Player.transform.position;
			Vector2 xzDirection = new Vector2(distance.x, distance.z);
			if (xzDirection.magnitude <= kNearDistance) {
				Quaternion rotation = StateMachine_.Player.transform.rotation;
				Quaternion rotationToTarget = Quaternion.LookRotation(distance);

				// if accurate enough then don't move anymore
				float angleToTarget = Quaternion.Angle(rotation, rotationToTarget);
				if (angleToTarget < StateMachine_.AIConfiguration.AccuracyInDegrees()) {
					// don't move if already near player position
					StateMachine_.InputState.MovementVector = Vector2.zero;
					return;
				}
			}

			StateMachine_.InputState.MovementVector = xzDirection.normalized;
		}

		private void HandleFullyChargedLaser() {
			CoroutineWrapper.DoAfterDelay(StateMachine_.AIConfiguration.RandomReactionTime(), () => {
				StateMachine_.InputState.LaserPressed = false;
				// TODO (darren): leave state properly
				CoroutineWrapper.DoAfterDelay(StateMachine_.AIConfiguration.RandomReactionTime(), () => {
					OnStateEntered();
				});
			});
		}
	}
}