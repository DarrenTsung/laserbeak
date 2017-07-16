using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIDashState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Internal
		private CoroutineWrapper coroutine_;

		protected override void OnStateEntered() {
			Vector2 dashDirection = StateMachine_.DashDirection;
			StateMachine_.InputState.LerpMovementVectorTo(dashDirection, () => {
				StateMachine_.InputState.Dash();

				coroutine_ = CoroutineWrapper.DoAfterDelay(StateMachine_.AIConfiguration.RandomReactionTime(), () => {
					StateMachine_.SwitchState(AIStateMachine.State.Idle);
				});
			});
		}

		protected override void OnStateExited() {
			StateMachine_.InputState.CancelTargetMovementVector();
			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}
		}
	}
}