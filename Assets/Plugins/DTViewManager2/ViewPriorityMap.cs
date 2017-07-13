using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTViewManager.Internal;

namespace DTViewManager {
	public class ViewPriorityMap {
		// PRAGMA MARK - Public Interface
		public ViewPriorityMap() { }

		public ViewPriorityMap(int defaultPriority) {
			defaultPriority_ = defaultPriority;
		}

		public int PriorityForPrefabName(string prefabName) {
			return priorityMap_.SafeGet(prefabName.ToLower(), defaultValue: defaultPriority_);
		}

		public void SetPriorityForPrefabName(string prefabName, int priority) {
			priorityMap_[prefabName.ToLower()] = priority;
		}

		public int DefaultPriority { get { return defaultPriority_; } }


		// PRAGMA MARK - Internal
		private Dictionary<string, int> priorityMap_ = new Dictionary<string, int>();
		private int defaultPriority_ = 100;
	}
}