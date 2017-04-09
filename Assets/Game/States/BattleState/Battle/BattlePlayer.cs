using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayer : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void InitInput(InputDevice inputDevice) {
			inputController_.InitInput(this, inputDevice);
		}

		public float BaseWeight {
			get { return kBaseWeight; }
		}

		public float Weight {
			get { return kBaseWeight + weightModifications_.Values.Sum(); }
		}

		public void SetWeightModification(object key, float weightModification) {
			weightModifications_[key] = weightModification;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			weightModifications_.Clear();
		}


		// PRAGMA MARK - Internal
		private const float kBaseWeight = 1.0f;

		[Header("Outlets")]
		[SerializeField]
		private BattlePlayerInputController inputController_;


		private readonly Dictionary<object, float> weightModifications_ = new Dictionary<object, float>();
	}
}