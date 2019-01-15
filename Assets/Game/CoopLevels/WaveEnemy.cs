using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Battle;
using DT.Game.Battle.Lasers;
using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;

namespace DT.Game.LevelSelect {
	public class WaveEnemy : MonoBehaviour, IWaveElement, IRecycleSetupSubscriber, IRecycleCleanupSubscriber, ILaserCollisionDelegate {
		// PRAGMA MARK - IWaveElement Implementation
		void IWaveElement.Spawn(Action removalCallback) {
			this.gameObject.SetActive(true);
			removalCallback_ = removalCallback;

			Laser.RegisterLaserTarget(this.transform);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			this.gameObject.SetActive(false);

			Laser.UnregisterLaserTarget(this.transform);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (removalCallback_ != null) {
				removalCallback_.Invoke();
				removalCallback_ = null;
			}
		}


		// PRAGMA MARK - ILaserCollisionDelegate Implementation
		void ILaserCollisionDelegate.HandleLaserHit(Laser laser) {
			Vector3 forceVector = (laser.transform.position - this.transform.position).normalized;

			laser.HandleHit(destroy: true);
			foreach (var destroyDelegate in this.GetComponentsInChildren<IWaveElementDestroyDelegate>()) {
				destroyDelegate.HandleDestruction(forceVector);
			}
		}


		// PRAGMA MARK - Internal
		private Action removalCallback_;
	}
}