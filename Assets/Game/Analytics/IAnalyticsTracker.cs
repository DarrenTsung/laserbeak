using System;
using System.Collections;
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
	public interface IAnalyticsTracker {
		void Dispose();
	}
}