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

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.LevelSelect {
	public static class CoopLevelManager {
		// PRAGMA MARK - Public Interface
		public static bool IsPlayingCoop {
			get { return currentLevelConfig_ != null; }
		}

		public static CoopLevelConfig CurrentLevelConfig {
			get { return currentLevelConfig_; }
		}

		// hmm
		public static void ExitCoop() {
			currentLevelConfig_ = null;
		}

		public static void SetCurrentLevelConfig(CoopLevelConfig config) {
			if (currentLevelConfig_ != null) {
				Debug.LogWarning("Cannot set current level when currently playing coop!");
				return;
			}

			currentLevelConfig_ = config;
		}



		// PRAGMA MARK - Internal
		private static CoopLevelConfig currentLevelConfig_;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			BattleState.OnBattleStateEntered += HandleBattleStateEntered;
		}

		private static void HandleBattleStateEntered() {
			if (!IsPlayingCoop || currentLevelConfig_ == null) {
				return;
			}

			var coopGameMode = ScriptableObject.CreateInstance<CoopLevelGameMode>();
			coopGameMode.Init(currentLevelConfig_);
			BattleState.QueuedGameMode = coopGameMode;
		}
	}
}