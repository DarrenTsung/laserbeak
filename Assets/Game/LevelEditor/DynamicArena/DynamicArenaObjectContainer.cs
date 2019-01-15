using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.LevelEditor {
	public class DynamicArenaObjectContainer : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public GameObject Init(GameObject prefab) {
			return ObjectPoolManager.Create(prefab, parent: this.gameObject);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();
		}
	}
}