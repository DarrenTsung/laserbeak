using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.AI {
	public class AIInputState : MonoBehaviour, IInputDelegate {
		// PRAGMA MARK - Public Interface
		public Vector2 MovementVector {
			get { return movementVector_; }
		}

		public void LerpMovementVectorTo(Vector2 vector) {
			movementVector_ = Vector2.Lerp(movementVector_, vector.normalized, kMovementVectorLerpSpeed * Time.deltaTime);
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


		// PRAGMA MARK - IInputDelegate Implementation
		Vector2 IInputDelegate.MovementVector {
			get { return MovementVector; }
		}

		bool IInputDelegate.DashPressed {
			get { return dashPressed_; }
		}

		bool IInputDelegate.LaserPressed {
			get { return LaserPressed; }
		}


		// PRAGMA MARK - Internal
		private const float kMovementVectorLerpSpeed = 2.0f;

		[SerializeField, ReadOnly]
		private bool dashPressed_ = false;

		[SerializeField, ReadOnly]
		public Vector2 movementVector_;
	}
}