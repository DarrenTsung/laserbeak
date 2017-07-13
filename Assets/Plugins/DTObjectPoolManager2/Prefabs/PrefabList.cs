using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public static class PrefabList {
		private static Dictionary<string, GameObject> prefabMap_ = new Dictionary<string, GameObject>();

		static PrefabList() {
			GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("");
			foreach (GameObject g in loadedPrefabs) {
				prefabMap_[g.name.ToLower()] = g;
			}
		}

		public static GameObject PrefabForName(string name) {
			string lowercaseName = name.ToLower();

			if (!prefabMap_.ContainsKey(lowercaseName)) {
				Debug.LogError("PrefabForName: invalid prefab name: (" + name + "), not in list!");
				return null;
			}

			return prefabMap_[lowercaseName];
		}
	}
}