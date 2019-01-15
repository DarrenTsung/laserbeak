using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using InControl;

namespace DT.Game {
	public static class ActionIcons {
		// PRAGMA MARK - Public Interface
		public static event Action OnInputTypeChanged = delegate {};

		public static void Populate(ActionType actionType, GameObject container) {
			inputHandlerMap_.GetRequiredValueOrDefault(currentInputType_).GetRequiredValueOrDefault(actionType).Populate(container);
		}

		public static void RegisterHandler(InputType inputType, IActionIconHandler handler) {
			inputHandlerMap_.GetAndCreateIfNotFound(inputType).SetAndWarnIfReplacing(handler.ActionType, handler);
		}


		// PRAGMA MARK - Internal
		private static Dictionary<InputType, Dictionary<ActionType, IActionIconHandler>> inputHandlerMap_ = new Dictionary<InputType, Dictionary<ActionType, IActionIconHandler>>();
		private static InputType currentInputType_ = InputType.XBoxController;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			RefreshInputType();
			InputManager.OnDeviceAttached += HandleDeviceChanged;
			InputManager.OnDeviceDetached += HandleDeviceChanged;
		}

		private static void HandleDeviceChanged(InputDevice device) {
			RefreshInputType();
		}

		private static void RefreshInputType() {
			if (InputManager.Devices == null) {
				Debug.LogWarning("No input manager present - possible sandbox scene!");
				return;
			}

			bool deviceConnected = InputManager.Devices.Where(d => d.IsAttached).Count() > 0;
			SetCurrentInputType(deviceConnected ? InputType.XBoxController : InputType.Keyboard);
		}

		private static void SetCurrentInputType(InputType inputType) {
			if (currentInputType_ == inputType) {
				return;
			}

			currentInputType_ = inputType;
			OnInputTypeChanged.Invoke();
		}
	}
}