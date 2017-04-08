using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTObjectPoolManager {
	public static class MonoBehaviourExtensions {
		public static void RecycleAllChildren(this MonoBehaviour m, bool worldPositionStays = false) {
			m.gameObject.RecycleAllChildren(worldPositionStays);
		}
	}
}
