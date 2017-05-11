using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	[Serializable]
	public class LevelEditor : MonoBehaviour, IRecycleCleanupSubscriber {
		public void Init(InputDevice inputDevice) {
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			inputDevice_ = null;
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
	}
}