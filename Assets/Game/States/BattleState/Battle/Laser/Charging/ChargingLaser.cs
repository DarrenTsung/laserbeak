using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class ChargingLaser : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void UpdateWithPercentage(float percentage) {
			this.transform.localScale = new Vector3(percentage, percentage, percentage);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			this.transform.localScale = Vector3.zero;
		}


		// PRAGMA MARK - Internal
		// TODO (darren): add particle effects etc.
	}
}