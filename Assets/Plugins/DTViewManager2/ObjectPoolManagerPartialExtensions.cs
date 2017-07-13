using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager.Internal;
using DTViewManager;

namespace DTObjectPoolManager {
	public partial class ObjectPoolManager : Singleton<ObjectPoolManager> {
		// PRAGMA MARK - Static
		public static T CreateView<T>(string prefabName = null, bool worldPositionStays = false, ViewManager viewManager = null) where T : MonoBehaviour {
			if (prefabName == null) {
				prefabName = typeof(T).Name;
			}

			GameObject viewObject = CreateView(prefabName, worldPositionStays, viewManager);
			return viewObject.GetRequiredComponent<T>();
		}

		public static GameObject CreateView(string prefabName, bool worldPositionStays = false, ViewManager viewManager = null) {
			viewManager = viewManager ?? ViewManagerLocator.Main;

			GameObject viewObject = ObjectPoolManager.Create(prefabName, null, worldPositionStays);
			viewManager.AttachView(viewObject);
			return viewObject;
		}

		public static T CreateView<T>(GameObject prefab, bool worldPositionStays = false, ViewManager viewManager = null) where T : MonoBehaviour {
			GameObject viewObject = CreateView(prefab, worldPositionStays, viewManager);
			return viewObject.GetRequiredComponent<T>();
		}

		public static GameObject CreateView(GameObject prefab, bool worldPositionStays = false, ViewManager viewManager = null) {
			viewManager = viewManager ?? ViewManagerLocator.Main;

			GameObject viewObject = ObjectPoolManager.Create(prefab, null, worldPositionStays);
			viewManager.AttachView(viewObject);
			return viewObject;
		}
	}
}