using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerPart : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		public static void RemoveCollidersFromAll() {
			RemoveCollidersEvent.Invoke();
		}

		private static event Action RemoveCollidersEvent = delegate {};


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			RemoveCollidersEvent += RemoveColliders;
			foreach (var collider in colliders_) {
				collider.enabled = true;
			}

			if (InGameConstants.BattlePlayerPartsFade) {
				CoroutineWrapper.DoAfterDelay(GameConstants.Instance.BattlePlayerPartFadeDuration * 0.8f, RemoveColliders);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			RemoveCollidersEvent -= RemoveColliders;
			rigidbody_.velocity = Vector3.zero;
		}


		// PRAGMA MARK - Internal
		private Rigidbody rigidbody_;
		private Collider[] colliders_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
			colliders_ = this.GetComponentsInChildren<Collider>();
		}

		private void RemoveColliders() {
			foreach (var collider in colliders_) {
				collider.enabled = false;
			}
		}
	}
}