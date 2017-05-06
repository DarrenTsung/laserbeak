using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.InstructionPopups;

namespace DT.Game.GameModes {
	public class GameModeShowedInstructionsCache {
		// PRAGMA MARK - Static
		public static void ResetShowedInstructionsCache() {
			cache_ = null;
			PlayerPrefs.DeleteKey("GameModeShowedInstructionsCache");
		}

		public static bool HasShowedInstructionsFor(GameMode mode) {
			return Cache_.ShowedDisplayNames.Contains(mode.DisplayTitle);
		}

		public static void MarkInstructionsAsShownFor(GameMode mode) {
			Cache_.ShowedDisplayNames.Add(mode.DisplayTitle);
			PlayerPrefs.SetString("GameModeShowedInstructionsCache", JsonUtility.ToJson(Cache_));
		}


		private static GameModeShowedInstructionsCache cache_;
		private static GameModeShowedInstructionsCache Cache_ {
			get {
				if (cache_ == null) {
					string cacheString = PlayerPrefs.GetString("GameModeShowedInstructionsCache", defaultValue: "");
					cache_ = JsonUtility.FromJson<GameModeShowedInstructionsCache>(cacheString);

					if (cache_ == null) {
						cache_ = new GameModeShowedInstructionsCache();
					}
				}

				return cache_;
			}
		}



		// PRAGMA MARK - Public Interface
		public HashSet<string> ShowedDisplayNames = new HashSet<string>();
	}
}