using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.Battle {
	public static class PlayerSpawner {
		// PRAGMA MARK - Static Public Interface
		public static void SpawnAllPlayers() {
			HashSet<PlayerSpawnPoint> chosenSpawnPoints = new HashSet<PlayerSpawnPoint>();
			PlayerSpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPoint>();

			foreach (Player player in RegisteredPlayers.AllPlayers) {
				PlayerSpawnPoint selectedSpawnPoint = spawnPoints.Random();
				if (chosenSpawnPoints.Contains(selectedSpawnPoint) && !spawnPoints.All(chosenSpawnPoints.Contains)) {
					continue;
				}

				chosenSpawnPoints.Add(selectedSpawnPoint);
				SpawnPlayerFor(player, selectedSpawnPoint);
			}
		}

		public static void CleanupAllPlayers() {
			// ToArray() since we are still listening to OnCleanup which will modify dictionary
			foreach (BattlePlayer battlePlayer in playerMap_.Values.ToArray()) {
				ObjectPoolManager.Recycle(battlePlayer);
			}
			playerMap_.Clear();
		}


		// PRAGMA MARK - Internal
		private static readonly Dictionary<Player, BattlePlayer> playerMap_ = new Dictionary<Player, BattlePlayer>();

		private static void SpawnPlayerFor(Player player, PlayerSpawnPoint spawnPoint) {
			if (PlayerExistsFor(player)) {
				Debug.LogWarning("Could not spawn player for: " + player);
				return;
			}

			BattlePlayer battlePlayer = ObjectPoolManager.Create<BattlePlayer>(GameConstants.Instance.PlayerPrefab, spawnPoint.transform.position, Quaternion.identity);
			battlePlayer.Init(new InputDeviceDelegate(player.InputDevice), player.Skin);

			RecyclablePrefab recyclablePrefab = battlePlayer.GetComponent<RecyclablePrefab>();
			Action<RecyclablePrefab> onCleanupCallback = null;
			onCleanupCallback = (RecyclablePrefab unused) => {
				recyclablePrefab.OnCleanup -= onCleanupCallback;
				CleanupPlayerFor(player);
			};
			recyclablePrefab.OnCleanup += onCleanupCallback;

			playerMap_[player] = battlePlayer;
		}

		private static bool PlayerExistsFor(Player player) {
			return playerMap_.ContainsKey(player);
		}

		private static void CleanupPlayerFor(Player player) {
			playerMap_.Remove(player);
		}
	}
}