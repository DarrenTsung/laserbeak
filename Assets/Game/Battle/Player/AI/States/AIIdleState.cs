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
		private const float kCheckAttackMaxDelay = 1.0f;

		private IAIMovementAction movementAction_;
		private CoroutineWrapper coroutine_;

		protected override void OnStateEntered() {
			movementAction_ = new AIMoveRandomlyAction(StateMachine_);

			if (InGameConstants.AllowChargingLasers) {
				coroutine_ = CoroutineWrapper.DoAfterDelay(UnityEngine.Random.Range(kCheckAttackMinDelay, kCheckAttackMaxDelay), () => {
					StateMachine_.SwitchState(AIStateMachine.State.Attack);
				});
			}
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
		}
	}
}