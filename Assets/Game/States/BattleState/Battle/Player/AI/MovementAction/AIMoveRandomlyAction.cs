using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIMoveRandomlyAction : IAIMovementAction {
		// PRAGMA MARK - Public Internal
		public AIMoveRandomlyAction(AIStateMachine stateMachine) {
			stateMachine_ = stateMachine;
			MoveToRandomNearbyPosition();
		}

		public void Dispose() {
			stateMachine_ = null;
			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private const float kNearDistance = 2.0f;
		private const float kIdleStandardDeviation = 0.5f;

		private const float kGoodEnoughDistance = 0.1f;

		private AIStateMachine stateMachine_;
		private CoroutineWrapper coroutine_;

		private void MoveToRandomNearbyPosition() {
			Vector3 nearbyPosition = GenerateRandomNearbyPosition();
			// TODO (darren): validate nearby position with nav-mesh
			coroutine_ = CoroutineWrapper.StartCoroutine(MoveToPosition(nearbyPosition));
		}

		private IEnumerator MoveToPosition(Vector3 position) {
			// NOTE (darren): why do we need to wait a frame?
			// if we generate a position that is too close + we are already
			// there, then we can actually set coroutine_ to the DoAfterDelay
			// which will get overridden by the first coroutine_ when the method returns...
			// therefore we lose the reference to the DoAfterDelay :O
			yield return null;

			Vector2 xzPosition = position.Vector2XZValue();
			while (true) {
				Vector2 currentXZPosition = stateMachine_.Player.transform.position.Vector2XZValue();
				if (Vector2.Distance(currentXZPosition, xzPosition) <= kGoodEnoughDistance) {
					break;
				}

				stateMachine_.InputState.LerpMovementVectorTo(xzPosition - currentXZPosition);
				yield return null;
			}

			stateMachine_.InputState.LerpMovementVectorTo(Vector2.zero);
			coroutine_ = CoroutineWrapper.DoAfterDelay(Math.Max(0.0f, MathUtil.SampleGaussian(0.0f, kIdleStandardDeviation)), () => {
				MoveToRandomNearbyPosition();
			});
		}

		private Vector3 GenerateRandomNearbyPosition() {
			Quaternion randomDirection = Quaternion.Euler(new Vector3(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f));
			return stateMachine_.Player.transform.position + (randomDirection * Vector3.forward * UnityEngine.Random.Range(kGoodEnoughDistance, kNearDistance));
		}
	}
}