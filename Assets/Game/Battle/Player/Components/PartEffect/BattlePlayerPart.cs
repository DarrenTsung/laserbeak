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
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			foreach (var collider in colliders_) {
				collider.enabled = true;
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			rigidbody_.velocity = Vector3.zero;
		}


		// PRAGMA MARK - Internal
		private Rigidbody rigidbody_;
		private Collider[] colliders_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
			colliders_ = this.GetComponentsInChildren<Collider>();
		}
	}
}