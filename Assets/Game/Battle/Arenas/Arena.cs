using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using DT.Game.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class Arena : IDisposable {
		// PRAGMA MARK - Public Interface
		public GameObject GameObject {
			get {
				if (disposed_) {
					Debug.LogError("Cannot access properties of disposed Arena!");
					return null;
				}

				return gameObject_;
			}
		}

		public ReadOnlyCollection<PlayerSpawnPoint> PlayerSpawnPoints {
			get {
				if (disposed_) {
					Debug.LogError("Cannot access properties of disposed Arena!");
					return null;
				}

				return playerSpawnPoints_;
			}
		}

		public ReadOnlyCollection<AISpawnPoint> AISpawnPoints {
			get {
				if (disposed_) {
					Debug.LogError("Cannot access properties of disposed Arena!");
					return null;
				}

				return aiSpawnPoints_;
			}
		}

		public Arena(GameObject arenaObject) {
			gameObject_ = arenaObject;

			List<PlayerSpawnPoint> spawnPoints = arenaObject.GetComponentsInChildren<PlayerSpawnPoint>().ToList();
			playerSpawnPoints_ = new ReadOnlyCollection<PlayerSpawnPoint>(spawnPoints);
			aiSpawnPoints_ = new ReadOnlyCollection<AISpawnPoint>(arenaObject.GetComponentsInChildren<AISpawnPoint>());
		}

		public void Dispose() {
			disposed_ = true;
		}


		// PRAGMA MARK - Internal
		private readonly ReadOnlyCollection<PlayerSpawnPoint> playerSpawnPoints_;
		private readonly ReadOnlyCollection<AISpawnPoint> aiSpawnPoints_;
		private readonly GameObject gameObject_ = null;

		private bool disposed_ = false;
	}
}