using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public static class BattleCamera {
		// PRAGMA MARK - Static Public Interface
		public static Camera Instance {
			get; set;
		}

		public static void Shake(float percentage) {
			if (percentage < 0.0f || percentage > 1.0f) {
				Debug.LogWarning("?? What are you trying to do here?");
			}

			Instance.transform.Shake(kShakeMaxAmount * percentage, kShakeMaxDuration * percentage);
		}


		// PRAGMA MARK - Internal
		private const float kShakeMaxAmount = 0.6f;
		private const float kShakeMaxDuration = 0.7f;
	}
}