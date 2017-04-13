using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Player;

namespace DT.Game.Battle {
	public class SpawnPlayers : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			HashSet<PlayerSpawnPoint> chosenSpawnPoints = new HashSet<PlayerSpawnPoint>();

			PlayerSpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPoint>();

			foreach (InputDevice inputDevice in InputManager.Devices) {
				PlayerSpawnPoint selectedSpawnPoint = spawnPoints.Random();
				if (chosenSpawnPoints.Contains(selectedSpawnPoint) && !spawnPoints.All(chosenSpawnPoints.Contains)) {
					continue;
				}

				chosenSpawnPoints.Add(selectedSpawnPoint);

				BattlePlayerSkin chosenSkin = GetBestSkin();
				playerSkinMap_[inputDevice] = chosenSkin;
				SpawnPlayerFor(inputDevice, selectedSpawnPoint, chosenSkin);
			}

			InputManager.OnDeviceDetached += HandleDeviceDetached;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			this.transform.RecycleAllChildren();

			InputManager.OnDeviceDetached -= HandleDeviceDetached;
		}


		// PRAGMA MARK - Internal
		private const float kRespawnCooldown = 1.0f;

		[SerializeField]
		private GameObject playerPrefab_;

		[SerializeField]
		private BattlePlayerSkin[] playerSkins_;

		private readonly Dictionary<InputDevice, BattlePlayer> playerMap_ = new Dictionary<InputDevice, BattlePlayer>();
		private readonly Dictionary<InputDevice, BattlePlayerSkin> playerSkinMap_ = new Dictionary<InputDevice, BattlePlayerSkin>();

		private void Update() {
			foreach (InputDevice inputDevice in InputManager.Devices) {
				if (!inputDevice.Action1.WasPressed) {
					continue;
				}

				if (PlayerExistsFor(inputDevice)) {
					continue;
				}

				SpawnPlayerFor(inputDevice);
			}
		}

		private bool SkinAlreadyInUse(BattlePlayerSkin skin) {
			return playerSkinMap_.Values.Any(s => s == skin);
		}

		private BattlePlayerSkin GetBestSkin() {
			BattlePlayerSkin chosenSkin = playerSkins_.Random();
			// NOTE (darren): could do a better random here..
			while (SkinAlreadyInUse(chosenSkin) && !playerSkins_.All(SkinAlreadyInUse)) {
				chosenSkin = playerSkins_.Random();
			}

			return chosenSkin;
		}

		private void SpawnPlayerFor(InputDevice inputDevice) {
			if (PlayerExistsFor(inputDevice)) {
				return;
			}

			PlayerSpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPoint>();
			BattlePlayerSkin chosenSkin = playerSkinMap_.GetValueOrDefault(inputDevice) ?? GetBestSkin();
			SpawnPlayerFor(inputDevice, spawnPoints.Random(), chosenSkin);
		}

		private void SpawnPlayerFor(InputDevice inputDevice, PlayerSpawnPoint spawnPoint, BattlePlayerSkin skin) {
			BattlePlayer player = ObjectPoolManager.Create<BattlePlayer>(playerPrefab_, spawnPoint.transform.position, Quaternion.identity, parent: this.gameObject);
			player.Init(new InputDeviceDelegate(inputDevice), skin);

			RecyclablePrefab recyclablePrefab = player.GetComponent<RecyclablePrefab>();
			Action<RecyclablePrefab> onCleanupCallback = null;
			onCleanupCallback = (RecyclablePrefab unused) => {
				recyclablePrefab.OnCleanup -= onCleanupCallback;
				CoroutineWrapper.DoAfterDelay(kRespawnCooldown, () => {
					CleanupPlayerFor(inputDevice);
				});
			};
			recyclablePrefab.OnCleanup += onCleanupCallback;

			playerMap_[inputDevice] = player;
		}

		private void HandleDeviceDetached(InputDevice inputDevice) {
			if (!PlayerExistsFor(inputDevice)) {
				return;
			}

			// explode players
			playerMap_[inputDevice].Health.TakeDamage(BattlePlayerHealth.kMaxDamage, forward: Vector3.zero);
			CleanupPlayerFor(inputDevice);
		}

		private bool PlayerExistsFor(InputDevice inputDevice) {
			return playerMap_.ContainsKey(inputDevice);
		}

		private void CleanupPlayerFor(InputDevice inputDevice) {
			playerMap_.Remove(inputDevice);
		}
	}
}