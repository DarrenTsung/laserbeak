using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;

namespace DT.Game.GameModes {
	public static class GameModesProgression {
		public static IEnumerable<GameMode> FilterByUnlocked(IEnumerable<GameMode> gameModes) {
			foreach (GameMode mode in gameModes) {
				if (UnlockedData_.IsUnlocked(mode.Id)) {
					yield return mode;
				}
			}
		}

		[Serializable]
		private class GameModesUnlocked {
			public bool IsUnlocked(int gameModeId) {
				// first game mode is always unlocked
				if (gameModeId == GameConstants.Instance.GameModes.First().Id) {
					return true;
				}

				return UnlockedIdsSet_.Contains(gameModeId);
			}

			public void Unlock(int gameModeId) {
				UnlockedIdsSet_.Add(gameModeId);
				unlockedIds_ = UnlockedIdsSet_.ToArray();
			}


			[NonSerialized]
			private HashSet<int> unlockedIdsSet_ = null;
			private HashSet<int> UnlockedIdsSet_ {
				get { return unlockedIdsSet_ ?? (unlockedIdsSet_ = (unlockedIds_ != null) ? new HashSet<int>(unlockedIds_) : new HashSet<int>()); }
			}


			[SerializeField]
			private int[] unlockedIds_;
		}

		private const int kUnlockAfterPlays = 3;

		private static GameModesUnlocked unlockedData_ = null;
		private static GameModesUnlocked UnlockedData_ {
			get { return unlockedData_ ?? (unlockedData_ = JsonUtility.FromJson<GameModesUnlocked>(PlayerPrefs.GetString("GameModeProgression::UnlockedData"))) ?? (unlockedData_ = new GameModesUnlocked()); }
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			GameModesPlayedTracker.OnGameModePlayTracked += HandleGameModePlayTracked;
		}

		[MethodCommand]
		private static void LogGameModeProgression() {
			foreach (var gameMode in GameConstants.Instance.GameModes) {
				Debug.Log("Game mode: " + gameMode.DisplayTitle + " is unlocked: " + UnlockedData_.IsUnlocked(gameMode.Id) + "!");
			}
		}

		private static void HandleGameModePlayTracked(GameMode gameMode) {
			int totalPlayCount = GameConstants.Instance.GameModes.Sum(mode => GameModesPlayedTracker.GetPlayedCountFor(mode));
			bool shouldUnlock = totalPlayCount >= kUnlockAfterPlays;
			if (!shouldUnlock) {
				return;
			}

			var lockedGameModes = GameConstants.Instance.GameModes.Where(mode => !UnlockedData_.IsUnlocked(mode.Id)).ToArray();
			if (lockedGameModes.Length <= 0) {
				return;
			}

			GameMode unlockedMode = lockedGameModes.Random();
			Unlock(unlockedMode);
			GameModesPlayedTracker.Reset();
			BattleState.QueuedGameMode = unlockedMode;
		}

		private static void Unlock(GameMode gameMode) {
			UnlockedData_.Unlock(gameMode.Id);
			SaveUnlockedData();
		}

		private static void SaveUnlockedData() {
			PlayerPrefs.SetString("GameModeProgression::UnlockedData", JsonUtility.ToJson(UnlockedData_));
		}
	}
}