using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTObjectPoolManager {
	public static class GameObjectExtensions {
		public static void RecycleAllChildren(this GameObject g, bool worldPositionStays = false) {
			g.transform.RecycleAllChildren(worldPositionStays);
		}
	}
}
