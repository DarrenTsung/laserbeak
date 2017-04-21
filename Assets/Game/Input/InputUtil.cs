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
		public static bool WasAnyMainButtonPressed() {
			foreach (InputDevice device in InputManager.Devices) {
				if (WasPositivePressedFor(device)) {
					return true;
				}
			}

			// TODO (darren): custom profiles binding later
			if (Input.GetKeyDown(KeyCode.Space)) {
				return true;
			}

			return false;
		}

		public static bool WasPositivePressedFor(InputDevice device) {
			return device.GetControl(InputControlType.Action1).WasPressed;
		}

		public static bool WasNegativePressedFor(InputDevice device) {
			return device.GetControl(InputControlType.Action2).WasPressed;
		}
	}
}