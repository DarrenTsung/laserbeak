using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Popups {
	public class PopupButtonConfig {
		public string ButtonText;
		public Action Callback;
		public bool DefaultOption;

		public PopupButtonConfig(string buttonText, Action callback, bool defaultOption = false) {
			ButtonText = buttonText;
			Callback = callback;
			DefaultOption = defaultOption;
		}
	}
}