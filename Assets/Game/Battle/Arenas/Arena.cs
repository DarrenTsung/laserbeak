using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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

			PlayerSpawnPoint[] spawnPoints = arenaObject.GetComponentsInChildren<PlayerSpawnPoint>();
			Array.Sort(spawnPoints, (PlayerSpawnPoint a, PlayerSpawnPoint b) => {
				Vector3 aPos = a.transform.position.Floor();
				Vector3 bPos = b.transform.position.Floor();

				if (aPos.z != bPos.z) {
					// higher z -> first
					return bPos.z.CompareTo(aPos.z);
				}

				// lower x -> first
				return aPos.x.CompareTo(bPos.x);
			});
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