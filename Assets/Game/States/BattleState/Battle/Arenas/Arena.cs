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

		public Arena(GameObject arenaObject) {
			gameObject_ = arenaObject;
			playerSpawnPoints_ = new ReadOnlyCollection<PlayerSpawnPoint>(arenaObject.GetComponentsInChildren<PlayerSpawnPoint>());
		}

		public void Dispose() {
			disposed_ = true;
		}


		// PRAGMA MARK - Internal
		private readonly ReadOnlyCollection<PlayerSpawnPoint> playerSpawnPoints_;
		private readonly GameObject gameObject_ = null;

		private bool disposed_ = false;
	}
}