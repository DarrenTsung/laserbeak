using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class SpawnPlayersOnCreation : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			HashSet<PlayerSpawnPoint> chosenSpawnPoints = new HashSet<PlayerSpawnPoint>();

			PlayerSpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPoint>();

			foreach (InputDevice inputDevice in InputManager.Devices) {
				PlayerSpawnPoint selectedSpawnPoint = spawnPoints.Random();
				if (chosenSpawnPoints.Contains(selectedSpawnPoint) && !spawnPoints.All(chosenSpawnPoints.Contains)) {
					continue;
				}

				BattlePlayer player = ObjectPoolManager.Create<BattlePlayer>(playerPrefab_, parent: this.gameObject);
				player.transform.position = selectedSpawnPoint.transform.position;
				player.SetInput(inputDevice);

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
	}
}