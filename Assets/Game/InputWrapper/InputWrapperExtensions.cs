using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InControl;

namespace DT.Game {
	public static class InputWrapperExtensions {
		public static InputDevice GetInputDevice(this IInputWrapper input) {
			var inputWrapperDevice = input as InputWrapperDevice;
			if (inputWrapperDevice == null) {
				return null;
			}

			return inputWrapperDevice.InputDevice;
		}
	}
}