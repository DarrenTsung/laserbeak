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
	public static class AISpawner {
		// PRAGMA MARK - Static Public Interface
		public static void SpawnAIPlayers() {
			foreach (AISpawnPoint aiSpawnPoint in ArenaManager.Instance.LoadedArena.AISpawnPoints) {
				SpawnAIPlayerFor(aiSpawnPoint);
			}
		}

		public static void CleanupAllPlayers() {
			// ToArray() since we are still listening to OnCleanup which will modify dictionary
			foreach (BattlePlayer battlePlayer in spawnPointMap_.Values.ToArray()) {
				ObjectPoolManager.Recycle(battlePlayer);
			}
			spawnPointMap_.Clear();
		}

		public static IEnumerable<BattlePlayer> AllSpawnedBattlePlayers {
			get { return spawnPointMap_.Values; }
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

		private static readonly Dictionary<AISpawnPoint, BattlePlayer> spawnPointMap_ = new Dictionary<AISpawnPoint, BattlePlayer>();
		private static readonly HashSet<CoroutineWrapper> respawnCoroutines_ = new HashSet<CoroutineWrapper>();

		private static bool shouldRespawn_ = false;

		private static void SpawnAIPlayerFor(AISpawnPoint spawnPoint) {
			if (AIPlayerExistsFor(spawnPoint)) {
				Debug.LogWarning("Could not spawn AI player for: " + spawnPoint);
				return;
			}

			BattlePlayer battlePlayer = ObjectPoolManager.Create<BattlePlayer>(GamePrefabs.Instance.PlayerPrefab, spawnPoint.transform.position, Quaternion.identity, parent: ArenaManager.Instance.LoadedArena.GameObject);
			// spawn player with substitute AI
			GameConstants.Instance.ConfigureWithSubstitutePlayerAI(battlePlayer);
			battlePlayer.SetSkin(GameConstants.Instance.EnemySkin);

			RecyclablePrefab recyclablePrefab = battlePlayer.GetComponent<RecyclablePrefab>();
			Action<RecyclablePrefab> onCleanupCallback = null;
			onCleanupCallback = (RecyclablePrefab unused) => {
				recyclablePrefab.OnCleanup -= onCleanupCallback;
				RemovePlayer(spawnPoint);
			};
			recyclablePrefab.OnCleanup += onCleanupCallback;

			spawnPointMap_[spawnPoint] = battlePlayer;
		}

		private static bool AIPlayerExistsFor(AISpawnPoint spawnPoint) {
			return spawnPointMap_.ContainsKey(spawnPoint);
		}

		private static void RemovePlayer(AISpawnPoint spawnPoint) {
			spawnPointMap_.Remove(spawnPoint);

			if (ShouldRespawn) {
				respawnCoroutines_.Add(CoroutineWrapper.DoAfterDelay(kRespawnDelay, () => {
					SpawnAIPlayerFor(spawnPoint);
				}));
			}
		}
	}
}