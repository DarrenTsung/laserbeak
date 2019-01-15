using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.AI {
	public class AIInputState : MonoBehaviour, IBattlePlayerInputDelegate {
		// PRAGMA MARK - Public Interface
		public Vector2 MovementVector {
			get { return movementVector_; }
		}

		public void LerpMovementVectorTowards(Vector2 vector, bool checkTargetMovementVectorExists = true) {
			// NOTE (darren): don't want to mix calls to the callback / every frame methods
			if (checkTargetMovementVectorExists && targetMovementVector_ != null) {
				Debug.LogWarning("Lerping movement vector while targetMovementVector_ exists - probably shouldn't mix this!");
			}
			movementVector_ = Vector2.Lerp(movementVector_, vector.normalized, kMovementVectorLerpSpeed * Time.deltaTime);
		}

		public void CancelTargetMovementVector() {
			targetMovementVector_ = null;
			reachedTargetCallback_ = null;
		}

		public void LerpMovementVectorTo(Vector2 vector, Action callback) {
			targetMovementVector_ = vector;
			reachedTargetCallback_ = callback;
		}

		[Header("Properties")]
		[ReadOnly]
		public bool LaserPressed;

		public void Dash() {
			dashPressed_ = true;
			CoroutineWrapper.DoAfterFrame(() => {
				dashPressed_ = false;
			});
		}


		// PRAGMA MARK - IBattlePlayerInputDelegate Implementation
		Vector2 IBattlePlayerInputDelegate.MovementVector {
			get { return MovementVector; }
		}

		bool IBattlePlayerInputDelegate.DashPressed {
			get { return dashPressed_; }
		}

		bool IBattlePlayerInputDelegate.LaserPressed {
			get { return LaserPressed; }
		}


		// PRAGMA MARK - Internal
		private const float kMovementVectorLerpSpeed = 10.0f;

		[SerializeField, ReadOnly]
		private bool dashPressed_ = false;

		[SerializeField, ReadOnly]
		public Vector2 movementVector_;

		private Vector2? targetMovementVector_;
		private Action reachedTargetCallback_;

		private void Update() {
			if (targetMovementVector_ != null) {
				Vector2 targetMovementVector = targetMovementVector_.Value;
				LerpMovementVectorTowards(targetMovementVector, checkTargetMovementVectorExists: false);
				if ((movementVector_ - targetMovementVector).magnitude < 0.1f) {
					if (reachedTargetCallback_ != null) {
						reachedTargetCallback_.Invoke();
					}
					targetMovementVector_ = null;
				}
			}
		}
	}
}