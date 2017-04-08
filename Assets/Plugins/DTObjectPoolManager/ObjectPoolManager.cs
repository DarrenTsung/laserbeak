using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public partial class ObjectPoolManager : Singleton<ObjectPoolManager> {
		// PRAGMA MARK - Static
		public static event Action<GameObject> OnGameObjectCreated = delegate { };

		public static T Create<T>(string prefabName = null, GameObject parent = null, bool worldPositionStays = false) where T : UnityEngine.Component {
			if (prefabName == null) {
				prefabName = typeof(T).Name;
			}

			GameObject instantiatedPrefab = Create(prefabName, parent, worldPositionStays);
			return instantiatedPrefab.GetRequiredComponent<T>();
		}

		public static GameObject Create(string prefabName, GameObject parent = null, bool worldPositionStays = false) {
			return ObjectPoolManager.Instance.CreateInternal(prefabName, parent, worldPositionStays);
		}

		public static GameObject Create(GameObject prefab, GameObject parent = null, bool worldPositionStays = false) {
			return ObjectPoolManager.Instance.CreateInternal(prefab.name, parent, worldPositionStays, (prefabName) => prefab);
		}

		public static void Recycle(MonoBehaviour usedObject, bool worldPositionStays = false) {
			Recycle(usedObject.gameObject, worldPositionStays);
		}

		public static void Recycle(GameObject usedObject, bool worldPositionStays = false) {
			ObjectPoolManager.Instance.RecycleInternal(usedObject, worldPositionStays);
		}


		// PRAGMA MARK - Internal
		private HashSet<GameObject> objectsBeingCleanedUp_ = new HashSet<GameObject>();
		private Dictionary<string, Stack<GameObject>> objectPools_ = new Dictionary<string, Stack<GameObject>>();

		private GameObject CreateInternal(string prefabName, GameObject parent = null, bool worldPositionStays = false, Func<string, GameObject> prefabProvider = null) {
			prefabName = prefabName.ToLower();

			GameObject instantiatedPrefab = GetGameObjectForPrefabName(prefabName, prefabProvider);

			if (parent != null) {
				instantiatedPrefab.transform.SetParent(parent.transform, worldPositionStays);
			}

			RecyclablePrefab recycleData = instantiatedPrefab.GetOrAddComponent<RecyclablePrefab>();
			recycleData.Setup();

			return instantiatedPrefab;
		}

		private void RecycleInternal(GameObject usedObject, bool worldPositionStays = false) {
			if (usedObject == null) {
				Debug.LogWarning("Recycle: called on null object!");
				return;
			}

			RecyclablePrefab recycleData = usedObject.GetComponent<RecyclablePrefab>();
			if (recycleData == null) {
				Debug.LogWarning("Recycle: usedObject - (" + usedObject + ") does not have RecyclablePrefab script!");
				// Because the recycle lifecycle wasn't set up properly, just destroy this object instead of recycling
				GameObject.Destroy(usedObject);
				return;
			}

			if (objectsBeingCleanedUp_.Contains(usedObject)) {
				return;
			}

			objectsBeingCleanedUp_.Add(usedObject);
			recycleData.Cleanup();
			usedObject.transform.SetParent(this.transform, worldPositionStays);
			this.DoAfterFrame(() => {
				this.DoAfterFrame(() => {
					usedObject.SetActive(false);

					Stack<GameObject> recycledObjects = ObjectPoolForPrefabName(recycleData.prefabName);
					recycledObjects.Push(usedObject);

					objectsBeingCleanedUp_.Remove(usedObject);
				});
			});
		}

		private Stack<GameObject> ObjectPoolForPrefabName(string prefabName) {
			return objectPools_.GetAndCreateIfNotFound(prefabName);
		}

		private GameObject GetGameObjectForPrefabName(string prefabName, Func<string, GameObject> prefabProvider) {
			Stack<GameObject> recycledObjects = ObjectPoolForPrefabName(prefabName);

			// try to find a recycled object that is usable
			while (recycledObjects.Count > 0) {
				GameObject recycledObj = recycledObjects.Pop();
				if (objectsBeingCleanedUp_.Contains(recycledObj)) {
					Debug.LogError("ObjectPoolManager - instantiating object that is being recycled (did you forget to clear references to recycled objects?)");
				}

				if (recycledObj != null) {
					if (!ValidateRecycledObject(recycledObj, prefabName)) {
						return null;
					}

					recycledObj.SetActive(true);
					return recycledObj;
				}
			}

			// if no recycled object is found, instantiate one
			if (prefabProvider == null) {
				prefabProvider = PrefabList.PrefabForName;
			}
			GameObject prefab = prefabProvider.Invoke(prefabName);
			if (prefab == null) {
				return null;
			}

			GameObject instantiatedPrefab = GameObject.Instantiate(prefab);

			RecyclablePrefab recycleData = instantiatedPrefab.GetOrAddComponent<RecyclablePrefab>();
			recycleData.prefabName = prefabName;

			OnGameObjectCreated.Invoke(instantiatedPrefab);
			return instantiatedPrefab;
		}

		private bool ValidateRecycledObject(GameObject recycledObject, string prefabName) {
			if (recycledObject.activeSelf) {
				Debug.LogError("ValidateRecycledObject: recycled object: (" + recycledObject + ") is still active, is someone else using it?");
				return false;
			}

			RecyclablePrefab recycleData = recycledObject.GetComponent<RecyclablePrefab>();
			if (recycleData == null) {
				Debug.LogError("ValidateRecycledObject: recycled object: (" + recycledObject + ") doesn't have a recyclable prefab script!");
				return false;
			}

			if (recycleData.prefabName != prefabName) {
				Debug.LogError("ValidateRecycledObject: recycled object: (" + recycledObject + ") doesn't match prefab name: " + prefabName + "!");
				return false;
			}

			return true;
		}
	}
}