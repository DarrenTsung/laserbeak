using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Transitions;

namespace DT.Game.Hints {
	public enum HintKey {
		CelebrateAction,
		ReflectAction,
	}

	public static class HintKeyExtensions {
		public static string GetText(this HintKey hintKey) {
			switch (hintKey) {
				case HintKey.CelebrateAction:
					return "HINT: Tap the laser button rapidly to celebrate your victory!";
				case HintKey.ReflectAction:
					return "HINT: Dash into lasers to reflect them!";
				default:
					return "";
			}
		}
	}
}