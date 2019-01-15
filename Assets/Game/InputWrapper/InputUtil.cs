using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public static class InputUtil {
		// PRAGMA MARK - Public Interface
		public static IEnumerable<IInputWrapper> AllInputs {
			get { return inputWrappers_; }
		}


		// PRAGMA MARK - Internal
		private static readonly List<IInputWrapper> inputWrappers_ = new List<IInputWrapper>();

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			RefreshInputWrappers();
			InputManager.OnDeviceAttached += HandleDeviceChanged;
			InputManager.OnDeviceDetached += HandleDeviceChanged;
		}

		private static void HandleDeviceChanged(InputDevice _) {
			RefreshInputWrappers();
		}

		private static void RefreshInputWrappers() {
			inputWrappers_.Clear();

			if (InputManager.Devices == null) {
				Debug.LogWarning("No InputManager - possible sandbox scene!");
				return;
			}

			if (InputManager.Devices.Count <= 0) {
				inputWrappers_.Add(new InputWrapperKeyboard());
			}

			foreach (var deviceWrapper in InputManager.Devices.Select(inputDevice => new InputWrapperDevice(inputDevice))) {
				inputWrappers_.Add(deviceWrapper);
			}
		}
	}
}