using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public static class BattleRecyclables {
		// PRAGMA MARK - Static Public Interface
		public static GameObject Instance {
			get {
				if (instance_ == null) {
					instance_ = new GameObject("BattleRecyclables");
				}
				return instance_;
			}
		}

		public static void Clear() {
			Instance.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Static Internal
		private static GameObject instance_;
	}
}