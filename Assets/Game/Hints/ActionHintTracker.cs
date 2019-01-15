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
	public class ActionHintTracker {
		// PRAGMA MARK - Public Interface
		public bool ShouldShowHint() {
			return missedActionCount_ >= kThreshold;
		}

		public void ResetTracking() {
			missedActionCount_ = 0;
		}

		public void HandleActionPeriod(bool actionPerformed) {
			if (actionPerformed) {
				missedActionCount_ = 0;
			} else {
				missedActionCount_++;
			}
		}


		// PRAGMA MARK - Internal
		private const int kThreshold = 3;

		private int missedActionCount_ = 0;
	}
}