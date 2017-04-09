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

namespace DT.Game.Battle.Player {
	public class BattlePlayerPart : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			foreach (var collider in colliders_) {
				collider.enabled = true;
			}

			CoroutineWrapper.DoAfterDelay(3.2f, () => {
				foreach (var collider in colliders_) {
					collider.enabled = false;
				}
			});
		}


		// PRAGMA MARK - Internal
		private Collider[] colliders_;

		private void Awake() {
			colliders_ = this.GetComponentsInChildren<Collider>();
		}
	}
}