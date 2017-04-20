using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public static class InputUtil {
		// PRAGMA MARK - Static Public Interface
		public static bool IsAnyMainButtonPressed() {
			foreach (InputDevice device in InputManager.Devices) {
				InputControl control = device.GetControl(InputControlType.Action1);
				if (control.IsPressed) {
					return true;
				}
			}

			// TODO (darren): custom profiles binding later
			if (Input.GetKeyDown(KeyCode.Space)) {
				return true;
			}

			return false;
		}
	}
}