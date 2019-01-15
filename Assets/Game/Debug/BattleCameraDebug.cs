using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game {
	public static class BattleCameraDebug {
		// PRAGMA MARK - Public Interface
		public static void FocusOnPlayer(int playerNumber) {
			// passed in 1-indexed
			int playerIndex = playerNumber - 1;

			if (playerIndex < 0 || playerIndex >= RegisteredPlayers.AllPlayers.Count) {
				return;
			}

			Player player = RegisteredPlayers.AllPlayers[playerIndex];
			BattleCamera.Instance.SetTransformsOfInterest(PlayerSpawner.AllSpawnedBattlePlayers.Where(bp => PlayerSpawner.GetPlayerFor(bp) == player).Select(bp => bp.transform).ToArray(), debug: true);
		}


		// PRAGMA MARK - Internal
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		private static void HandleUpdate() {
			if (Input.GetKeyDown(KeyCode.Alpha0)) {
				BattleCamera.Instance.ClearTransformsOfInterest();
			} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
				FocusOnPlayer(1);
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				FocusOnPlayer(2);
			} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				FocusOnPlayer(3);
			} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
				FocusOnPlayer(4);
			}
		}
	}
}