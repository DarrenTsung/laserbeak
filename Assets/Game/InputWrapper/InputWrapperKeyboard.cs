using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public class InputWrapperKeyboard : IInputWrapper {
		// PRAGMA MARK - Public Interface
		public InputWrapperKeyboard() {
		}


		// PRAGMA MARK - IInputWrapper Implementation
		bool IInputWrapper.CommandWasPressed {
			get { return Input.GetKeyDown(KeyCode.Escape); }
		}

		bool IInputWrapper.CommandIsPressed {
			get { return Input.GetKey(KeyCode.Escape); }
		}

		Vector2 IInputWrapper.MovementVector {
			get { return new Vector2(Input.GetAxis("KeyboardHorizontal"), Input.GetAxis("KeyboardVertical")); }
		}

		bool IInputWrapper.LaserIsPressed {
			get { return Input.GetKey(KeyCode.B); }
		}

		bool IInputWrapper.PositiveWasPressed {
			get { return Input.GetKeyDown(KeyCode.N); }
		}

		bool IInputWrapper.PositiveIsPressed {
			get { return Input.GetKey(KeyCode.N); }
		}

		bool IInputWrapper.NegativeWasPressed {
			get { return Input.GetKeyDown(KeyCode.M); }
		}

		bool IInputWrapper.NegativeIsPressed {
			get { return Input.GetKey(KeyCode.M); }
		}
	}
}