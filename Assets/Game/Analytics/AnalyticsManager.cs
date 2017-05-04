using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Pausing;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.GameAnalytics {
	public class AnalyticsManager : MonoBehaviour {
		// PRAGMA MARK - Static
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			GameObject analyticsObject = new GameObject("AnalyticsManager (HiddenSingleton)");
			analyticsObject.AddComponent<AnalyticsManager>();
			GameObject.DontDestroyOnLoad(analyticsObject);
		}


		// PRAGMA MARK - Internal
		private readonly List<IAnalyticsTracker> trackers_ = new List<IAnalyticsTracker>();

		private void OnEnable() {
			trackers_.Add(new AnalyticsGameModeStatsTracker());
		}

		private void OnDisable() {
			foreach (IAnalyticsTracker tracker in trackers_) {
				tracker.Dispose();
			}
			trackers_.Clear();
		}
	}
}