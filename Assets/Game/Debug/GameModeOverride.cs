using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTDebugMenu;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game {
	public static class GameModeOverride {
		// PRAGMA MARK - Public Interface
		public static void RegisterPopup(GenericInspector inspector) {
			if (GameConstants.Instance == null) {
				Debug.LogWarning("No GameConstants - not registering popup (possible sandbox)!");
				return;
			}

			var setGameModePopupItems = new List<PopupItemConfig>() {
				new PopupItemConfig("NO OVERRIDE", () => SetGameModeOverride(null))
			};

			foreach (var gameMode in GameConstants.Instance.GameModes) {
				var currentGameMode = gameMode;
				setGameModePopupItems.Add(new PopupItemConfig(gameMode.DisplayTitle, () => SetGameModeOverride(currentGameMode)));
			}

			Func<int> startIndexDelegate = () => {
				int startIndex = 0;
				if (overrideGameMode_ != null) {
					startIndex = Array.IndexOf(GameConstants.Instance.GameModes, overrideGameMode_) + 1;
				}
				return startIndex;
			};

			inspector.RegisterPopup("Override Game Mode", startIndexDelegate, itemConfigs: setGameModePopupItems.ToArray());
		}

		// PRAGMA MARK - Internal
		private static GameMode overrideGameMode_ = null;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			BattleState.OnBattleStateEntered += InjectOverrideGameMode;
		}

		private static void SetGameModeOverride(GameMode gameMode) {
			overrideGameMode_ = gameMode;
			BattleState.SkipAndLoadGameMode(gameMode);
		}

		private static void InjectOverrideGameMode() {
			if (overrideGameMode_ == null) {
				return;
			}

			BattleState.QueuedGameMode = overrideGameMode_;
		}
	}
}