using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using DT.Game.Players;
using DT.Game.Transitions;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes.KingOfTheHill;

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

		public KingOfTheHillArea KingOfTheHillArea {
			get {
				if (disposed_) {
					Debug.LogError("Cannot access properties of disposed Arena!");
					return null;
				}

				return kingOfTheHillArea_;
			}
		}

		public Arena(GameObject arenaObject) {
			gameObject_ = arenaObject;

			List<PlayerSpawnPoint> spawnPoints = arenaObject.GetComponentsInChildren<PlayerSpawnPoint>().ToList();
			playerSpawnPoints_ = new ReadOnlyCollection<PlayerSpawnPoint>(spawnPoints);
			aiSpawnPoints_ = new ReadOnlyCollection<AISpawnPoint>(arenaObject.GetComponentsInChildren<AISpawnPoint>());

			kingOfTheHillArea_ = gameObject_.GetComponentInChildren<KingOfTheHillArea>();
		}

		public void Dispose() {
			disposed_ = true;
		}

		public void AnimateIn(Action callback) {
			TransitionWrapper_.AnimateIn(callback);
		}

		public void AnimateOut(Action callback) {
			TransitionWrapper_.AnimateOut(callback);
		}


		// PRAGMA MARK - Internal
		private const float kArenaAnimateTime = 0.3f;

		private readonly ReadOnlyCollection<PlayerSpawnPoint> playerSpawnPoints_;
		private readonly ReadOnlyCollection<AISpawnPoint> aiSpawnPoints_;
		private readonly GameObject gameObject_ = null;
		private readonly KingOfTheHillArea kingOfTheHillArea_ = null;

		private bool disposed_ = false;

		private TransitionWrapper transitionWrapper_;
		private TransitionWrapper TransitionWrapper_ {
			get {
				if (transitionWrapper_ == null) {
					transitionWrapper_ = new TransitionWrapper(gameObject_).SetShuffledOrder(true);
					transitionWrapper_.SetOffsetDelay(kArenaAnimateTime / transitionWrapper_.TransitionCount);
				}
				return transitionWrapper_;
			}
		}
	}
}