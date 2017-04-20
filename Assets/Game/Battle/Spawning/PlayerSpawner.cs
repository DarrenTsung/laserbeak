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
		public static event Action OnSpawnedPlayerRemoved = delegate {};

		public static void SpawnAllPlayers() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				SpawnPlayer(player);
			}
		}

		public static void CleanupAllPlayers() {
			// ToArray() since we are still listening to OnCleanup which will modify dictionary
			foreach (BattlePlayer battlePlayer in playerMap_.Values.ToArray()) {
				ObjectPoolManager.Recycle(battlePlayer);
			}
			playerMap_.Clear();
		}

		public static bool IsAlive(Player player) {
			return PlayerExistsFor(player);
		}

		public static IEnumerable<Player> AllSpawnedPlayers {
			get { return playerMap_.Keys; }
		}

		public static IEnumerable<BattlePlayer> AllSpawnedBattlePlayers {
			get { return playerMap_.Values; }
		}

		public static BattlePlayer GetBattlePlayerFor(Player player) {
			return playerMap_.GetValueOrDefault(player);
		}

		public static bool ShouldRespawn {
			get { return shouldRespawn_; }
			set {
				if (shouldRespawn_ == value) {
					return;
				}

				shouldRespawn_ = value;
				if (!shouldRespawn_) {
					// cleanup any respawning coroutines
					foreach (CoroutineWrapper coroutine in respawnCoroutines_) {
						coroutine.Cancel();
					}
					respawnCoroutines_.Clear();
				}
			}
		}


		// PRAGMA MARK - Internal
		private const float kRespawnDelay = 2.0f;

		private static readonly Dictionary<Player, BattlePlayer> playerMap_ = new Dictionary<Player, BattlePlayer>();
		private static readonly HashSet<CoroutineWrapper> respawnCoroutines_ = new HashSet<CoroutineWrapper>();

		private static bool shouldRespawn_ = false;

		private static void SpawnPlayer(Player player) {
			IList<PlayerSpawnPoint> spawnPoints = ArenaManager.Instance.LoadedArena.PlayerSpawnPoints;
			PlayerSpawnPoint spawnPoint = spawnPoints.GetClamped(RegisteredPlayers.AllPlayers.IndexOf(player));

			if (PlayerExistsFor(player)) {
				Debug.LogWarning("Could not spawn player for: " + player);
				return;
			}

			BattlePlayer battlePlayer = ObjectPoolManager.Create<BattlePlayer>(GamePrefabs.Instance.PlayerPrefab, spawnPoint.transform.position, Quaternion.identity, parent: ArenaManager.Instance.LoadedArena.GameObject);
			if (player.InputDevice != null) {
				battlePlayer.Init(new InputDeviceDelegate(player.InputDevice), player.Skin);
			} else {
				// spawn player with substitute AI
				GameConstants.Instance.ConfigureWithSubstitutePlayerAI(battlePlayer);
				battlePlayer.SetSkin(player.Skin);
			}

			RecyclablePrefab recyclablePrefab = battlePlayer.GetComponent<RecyclablePrefab>();
			Action<RecyclablePrefab> onCleanupCallback = null;
			onCleanupCallback = (RecyclablePrefab unused) => {
				recyclablePrefab.OnCleanup -= onCleanupCallback;
				RemovePlayer(player);
			};
			recyclablePrefab.OnCleanup += onCleanupCallback;

			playerMap_[player] = battlePlayer;
		}

		private static bool PlayerExistsFor(Player player) {
			return playerMap_.ContainsKey(player);
		}

		private static void RemovePlayer(Player player) {
			playerMap_.Remove(player);
			OnSpawnedPlayerRemoved.Invoke();

			if (ShouldRespawn) {
				respawnCoroutines_.Add(CoroutineWrapper.DoAfterDelay(kRespawnDelay, () => {
					SpawnPlayer(player);
				}));
			}
		}
	}
}