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
	public static class ActionHintTrackerUtil {
		// PRAGMA MARK - Public Interface
		public static void HandleActionPeriod(HintKey hintKey, bool actionPerformed) {
			var tracker = ActionHintTrackerRegistry.Get(hintKey);
			tracker.HandleActionPeriod(actionPerformed);
		}

		public static void ShowIfNecessary(params HintKey[] hintKeys) {
			if (!InGameConstants.ShowHintsView) {
				return;
			}

			hintKeys.Shuffle();

			foreach (var hintKey in hintKeys) {
				var tracker = ActionHintTrackerRegistry.Get(hintKey);
				if (!tracker.ShouldShowHint()) {
					continue;
				}

				tracker.ResetTracking();
				Hint.Show(hintKey.GetText());
			}
		}
	}
}