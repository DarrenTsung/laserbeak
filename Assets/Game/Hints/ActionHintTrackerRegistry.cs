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
	public static class ActionHintTrackerRegistry {
		// PRAGMA MARK - Public Interface
		public static IEnumerable<ActionHintTracker> AllTrackers {
			get { return actionHintTrackerMap_.Values; }
		}

		public static ActionHintTracker Get(HintKey hintKey) {
			return actionHintTrackerMap_.GetAndCreateIfNotFound(hintKey);
		}


		// PRAGMA MARK - Internal
		private static readonly Dictionary<HintKey, ActionHintTracker> actionHintTrackerMap_ = new Dictionary<HintKey, ActionHintTracker>();
	}
}