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