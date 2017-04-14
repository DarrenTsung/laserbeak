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
	public class SpawnPlayers : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
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


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			this.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject playerPrefab_;

		private readonly Dictionary<Player, BattlePlayer> playerMap_ = new Dictionary<Player, BattlePlayer>();

		private void SpawnPlayerFor(Player player, PlayerSpawnPoint spawnPoint) {
			if (PlayerExistsFor(player)) {
				Debug.LogWarning("Could not spawn player for: " + player);
				return;
			}

			BattlePlayer battlePlayer = ObjectPoolManager.Create<BattlePlayer>(playerPrefab_, spawnPoint.transform.position, Quaternion.identity, parent: this.gameObject);
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

		private bool PlayerExistsFor(Player player) {
			return playerMap_.ContainsKey(player);
		}

		private void CleanupPlayerFor(Player player) {
			playerMap_.Remove(player);
		}
	}
}