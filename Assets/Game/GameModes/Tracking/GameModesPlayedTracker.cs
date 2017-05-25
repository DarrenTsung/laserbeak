using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.GameModes {
	public static class GameModesPlayedTracker {
		[Serializable]
		private class GameModesPlayedData {
			public void IncrementCountFor(int gameModeId) {
				PlayedMap_.Increment(gameModeId);

				dataPoints_ = PlayedMap_.Select(kvp => new GameModesPlayedDataPoint(kvp.Key, kvp.Value)).ToArray();
			}

			public int GetCountFor(int gameModeId) {
				return PlayedMap_.GetValue(gameModeId);
			}

			[SerializeField]
			private GameModesPlayedDataPoint[] dataPoints_;

			[NonSerialized]
			private CountMap<int> playedMap_ = null;
			private CountMap<int> PlayedMap_ {
				get {
					if (playedMap_ == null) {
						playedMap_ = new CountMap<int>();
						if (dataPoints_ != null) {
							foreach (var dataPoint in dataPoints_) {
								playedMap_[dataPoint.GameModeId] = dataPoint.Count;
							}
						}
					}

					return playedMap_;
				}
			}
		}

		[Serializable]
		private class GameModesPlayedDataPoint {
			public int GameModeId;
			public int Count;

			public GameModesPlayedDataPoint(int gameModeId, int count) {
				GameModeId = gameModeId;
				Count = count;
			}
		}


		public static int GetPlayedCountFor(GameMode gameMode) {
			return PlayedData_.GetCountFor(gameMode.Id);
		}

		private static GameModesPlayedData playedData_ = null;
		private static GameModesPlayedData PlayedData_ {
			get { return playedData_ ?? (playedData_ = JsonUtility.FromJson<GameModesPlayedData>(PlayerPrefs.GetString("GameModesPlayedTracker::PlayedData"))) ?? (playedData_ = new GameModesPlayedData()); }
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			GameMode.OnActivate += HandleGameModeActivated;
		}

		[MethodCommand]
		private static void LogGameModePlayedTracker() {
			foreach (var gameMode in GameConstants.Instance.GameModes) {
				Debug.Log("Game mode: " + gameMode.DisplayTitle + " has been played " + GetPlayedCountFor(gameMode) + " times!");
			}
		}

		private static void HandleGameModeActivated(GameMode gameMode) {
			PlayedData_.IncrementCountFor(gameMode.Id);
			SavePlayedData();
		}

		private static void SavePlayedData() {
			PlayerPrefs.SetString("GameModesPlayedTracker::PlayedData", JsonUtility.ToJson(PlayedData_));
		}
	}
}