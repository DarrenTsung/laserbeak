using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public static class PrefabNameRouter {
		// PRAGMA MARK - Static
		private static Dictionary<string, string> prefabNameMapping_ = new Dictionary<string, string>();

		public static void RegisterRouting(string oldPrefabName, string newPrefabName) {
			if (HasRoutingForPrefabName(oldPrefabName)) {
				Debug.LogWarning(string.Format("PrefabNameRouter.RegisterRouting: Overriding previously routed old prefab name ({0})!", oldPrefabName));
			}

			prefabNameMapping_[oldPrefabName] = newPrefabName;
		}

		public static bool HasRoutingForPrefabName(string prefabName) {
			return prefabNameMapping_.ContainsKey(prefabName);
		}

		public static string RoutedPrefabName(string prefabName) {
			if (HasRoutingForPrefabName(prefabName)) {
				return prefabNameMapping_[prefabName];
			}

			return prefabName;
		}
	}
}
