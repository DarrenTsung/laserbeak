using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputDeviceDelegate : IBattlePlayerInputDelegate {
		// PRAGMA MARK - Public Interface
		public BattlePlayerInputDeviceDelegate(IInputWrapper input) {
			input_ = input;
		}


		// PRAGMA MARK - IBattlePlayerInputDelegate Implementation
		Vector2 IBattlePlayerInputDelegate.MovementVector {
			get { return input_.MovementVector; }
		}

		bool IBattlePlayerInputDelegate.DashPressed {
			get { return input_.PositiveWasPressed; }
		}

		bool IBattlePlayerInputDelegate.LaserPressed {
			get { return input_.LaserIsPressed; }
		}


		// PRAGMA MARK - Internal
		private IInputWrapper input_;
	}
}