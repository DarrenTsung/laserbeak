using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class InputDeviceDelegate : IInputDelegate {
		// PRAGMA MARK - Public Interface
		public InputDeviceDelegate(InputDevice inputDevice) {
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - IInputDelegate Implementation
		Vector2 IInputDelegate.MovementVector {
			get { return inputDevice_.LeftStick.Value; }
		}

		bool IInputDelegate.DashPressed {
			get { return inputDevice_.Action1.WasPressed; }
		}

		bool IInputDelegate.LaserPressed {
			get { return inputDevice_.Action3.IsPressed; }
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
	}
}