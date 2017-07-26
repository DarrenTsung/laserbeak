using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AICelebrateState : DTStateMachineBehaviour<AIStateMachine> {
		// PRAGMA MARK - Internal
		private const float kCluckChance = 0.8f;

		private IAIMovementAction movementAction_ = new AIMoveRandomlyAction();
		private CoroutineWrapper coroutine_;

		protected override void OnStateEntered() {
			movementAction_.Init(StateMachine_);

			CoroutineWrapper.DoAfterDelay(StateMachine_.AIConfiguration.RandomReactionTime(), () => {
				StateMachine_.InputState.LaserPressed = false;
				if (UnityEngine.Random.Range(0.0f, 1.0f) <= kCluckChance) {
					coroutine_ = CoroutineWrapper.StartCoroutine(DoCluckCoroutine());
				}
			});

		}

		protected override void OnStateExited() {
			movementAction_.Dispose();

			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}
		}

		private IEnumerator DoCluckCoroutine() {
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 0.8f));

			int cluckCount = UnityEngine.Random.Range(1, 7);
			for (int i = 0; i < cluckCount; i++) {
				yield return Cluck();
				yield return new WaitForSeconds(StateMachine_.AIConfiguration.RandomReactionTime());
			}
		}

		private IEnumerator Cluck() {
			StateMachine_.InputState.LaserPressed = true;
			yield return null;
			StateMachine_.InputState.LaserPressed = false;
		}
	}
}