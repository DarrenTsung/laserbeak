using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Player;

namespace DT.Game.Battle {
	public class SpawnPlayersOnCreation : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			HashSet<PlayerSpawnPoint> chosenSpawnPoints = new HashSet<PlayerSpawnPoint>();
			HashSet<BattlePlayerSkin> chosenSkins = new HashSet<BattlePlayerSkin>();

			PlayerSpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPoint>();

			foreach (InputDevice inputDevice in InputManager.Devices) {
				PlayerSpawnPoint selectedSpawnPoint = spawnPoints.Random();
				if (chosenSpawnPoints.Contains(selectedSpawnPoint) && !spawnPoints.All(chosenSpawnPoints.Contains)) {
					continue;
				}

				BattlePlayerSkin chosenSkin = playerSkins_.Random();
				// NOTE (darren): could do a better random here..
				while (chosenSkins.Contains(chosenSkin) && !playerSkins_.All(chosenSkins.Contains)) {
					chosenSkin = playerSkins_.Random();
				}

				BattlePlayer player = ObjectPoolManager.Create<BattlePlayer>(playerPrefab_, selectedSpawnPoint.transform.position, Quaternion.identity, parent: this.gameObject);
				player.Init(new InputDeviceDelegate(inputDevice), chosenSkin);

				chosenSkins.Add(chosenSkin);
				chosenSpawnPoints.Add(selectedSpawnPoint);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
		  this.transform.RecycleAllChildren();
		}

		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject playerPrefab_;

		[SerializeField]
		private BattlePlayerSkin[] playerSkins_;
	}
}