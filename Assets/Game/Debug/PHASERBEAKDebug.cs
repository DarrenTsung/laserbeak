using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTDebugMenu;
using DTFPSView;

using DT.Game.Battle;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.Debugs {
	public static class PHASERBEAKDebug {
		// PRAGMA MARK - Internal
		public static void ResetAllThings() {
			GameModeShowedInstructionsCache.ResetShowedInstructionsCache();
			GameModesPlayedTracker.Reset();
			GameModesProgression.Reset();
			foreach (var tracker in ActionHintTrackerRegistry.AllTrackers) {
				tracker.ResetTracking();
			}
		}
	}
}