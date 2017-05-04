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
	public class AnalyticsGameModeStatsTracker : IAnalyticsTracker {
		// PRAGMA MARK - Public Interface
		public AnalyticsGameModeStatsTracker() {
			GameMode.OnActivate += HandleGameModeActivated;
			GameMode.OnFinish += HandleGameModeFinished;

			GameModeIntroView.OnIntroFinished += HandleIntroFinished;
		}


		// PRAGMA MARK - IAnalyticsTracker Implementation
		void IAnalyticsTracker.Dispose() {
			GameMode.OnActivate -= HandleGameModeActivated;
			GameMode.OnFinish -= HandleGameModeFinished;

			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;
		}


		// PRAGMA MARK - Internal
		private GameMode lastActivatedGameMode_ = null;
		private float lastActivatedTime_ = 0.0f;
		private float lastIntroFinishedTime_ = 0.0f;

		private void HandleGameModeActivated(GameMode mode) {
			lastActivatedGameMode_ = mode;
			lastActivatedTime_ = Time.unscaledTime;
		}

		private void HandleIntroFinished() {
			lastIntroFinishedTime_ = Time.unscaledTime;
		}

		private void HandleGameModeFinished(GameMode mode) {
			if (mode != lastActivatedGameMode_) {
				Debug.LogWarning("AnalyticsGameModeStatsTracker - mode does not match mode that just finished, ignoring event..");
				return;
			}

			if (lastIntroFinishedTime_ < lastActivatedTime_) {
				Debug.LogWarning("AnalyticsGameModeStatsTracker - intro finished before last activated time! Intro should always be recorded after game mode activated! Ignoring event..");
				return;
			}

			float gameTimeInSeconds = Time.unscaledTime - lastIntroFinishedTime_;
			if (gameTimeInSeconds <= 0.0f) {
				Debug.LogWarning("AnalyticsGameModeStatsTracker - gameTimeInSeconds is negative or 0.0f, ignoring event..");
				return;
			}

			Analytics.CustomEvent("GameMode", new Dictionary<string, object>
			{
				{ "Type", mode.GetType().Name },
				{ "GameTimeInSeconds", gameTimeInSeconds },
			});
		}
	}
}