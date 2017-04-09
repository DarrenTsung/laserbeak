using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class RecycleAfterTime : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			CoroutineWrapper.DoAfterDelay(duration_, () => {
				ObjectPoolManager.Recycle(this);
			});
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private float duration_ = 1.0f;
	}
}