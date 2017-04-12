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
		[Header("Properties")]
		[ReadOnly]
		public Vector2 MovementVector;

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
		[SerializeField, ReadOnly]
		private bool dashPressed_ = false;
	}
}