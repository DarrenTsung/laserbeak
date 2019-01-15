using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public class InputWrapperDevice : IInputWrapper {
		// PRAGMA MARK - Public Interface
		public InputDevice InputDevice {
			get { return inputDevice_; }
		}

		public InputWrapperDevice(InputDevice inputDevice) {
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - IInputWrapper Implementation
		bool IInputWrapper.CommandWasPressed {
			get { return inputDevice_.Command.WasPressed; }
		}

		bool IInputWrapper.CommandIsPressed {
			get { return inputDevice_.Command.IsPressed; }
		}

		Vector2 IInputWrapper.MovementVector {
			get { return inputDevice_.LeftStick.Value; }
		}

		bool IInputWrapper.LaserIsPressed {
			get { return inputDevice_.Action3.IsPressed; }
		}

		bool IInputWrapper.PositiveWasPressed {
			get { return inputDevice_.Action1.WasPressed; }
		}

		bool IInputWrapper.PositiveIsPressed {
			get { return inputDevice_.Action1.IsPressed; }
		}

		bool IInputWrapper.NegativeWasPressed {
			get { return inputDevice_.Action2.WasPressed; }
		}

		bool IInputWrapper.NegativeIsPressed {
			get { return inputDevice_.Action2.IsPressed; }
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
	}
}