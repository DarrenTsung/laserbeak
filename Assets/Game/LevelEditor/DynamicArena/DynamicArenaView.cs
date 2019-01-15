using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

using DT.Game.Battle.Walls;

namespace DT.Game.LevelEditor {
	public class DynamicArenaView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public event Action OnViewRefreshed = delegate {};

		public void Init(DynamicArenaData dynamicArenaData, GameObject customPrefab) {
			customContainer_.RecycleAllChildren();
			if (customPrefab != null) {
				ObjectPoolManager.Create(customPrefab, parent: customContainer_);
			}

			dynamicArenaData_ = dynamicArenaData;
			dynamicArenaData_.OnDataDirty += HandleDataDirty;
			HandleDataDirty();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			customContainer_.RecycleAllChildren();
			dynamicContainer_.RecycleAllChildren();

			if (dynamicArenaData_ != null) {
				dynamicArenaData_.OnDataDirty -= HandleDataDirty;
				dynamicArenaData_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject dynamicContainer_;
		[SerializeField]
		private GameObject customContainer_;

		[Header("Properties")]
		[SerializeField]
		private bool forLevelEditor_ = true;

		private DynamicArenaData dynamicArenaData_;

		private void HandleDataDirty() {
			// TODO (darren): deserialize attributes to deliver payloads
			dynamicContainer_.RecycleAllChildren();

			foreach (var objectData in dynamicArenaData_.Objects) {
				GameObject prefab = FindRequiredPrefabFor(objectData.PrefabName);
				if (prefab == null) {
					continue;
				}

				var container = ObjectPoolManager.Create<DynamicArenaObjectContainer>(GamePrefabs.Instance.DynamicArenaObjectContainer, position: objectData.Position, rotation: objectData.Rotation, parent: dynamicContainer_);
				container.transform.localScale = objectData.LocalScale;
				GameObject instantiated = container.Init(prefab);

				FillAttributesOf(instantiated, objectData.UniqueId);
			}

			foreach (var wallData in dynamicArenaData_.Walls) {
				GameObject prefab = FindRequiredPrefabFor(wallData.PrefabName);
				if (prefab == null) {
					continue;
				}

				var wall = ObjectPoolManager.Create<Wall>(prefab, position: wallData.Position, rotation: Quaternion.identity, parent: dynamicContainer_);
				wall.SetVertexLocalPositions(wallData.VertexLocalPositions);

				FillAttributesOf(wall.gameObject, wallData.UniqueId);
			}

			if (forLevelEditor_) {
				for (int playerIndex = 0; playerIndex < dynamicArenaData_.PlayerSpawnPoints.Length; playerIndex++) {
					Vector3 position = dynamicArenaData_.PlayerSpawnPoints[playerIndex];
					// Vector3.zero is not valid - hide until valid player spawn point is placed
					if (position == Vector3.zero) {
						continue;
					}

					var spawnPoint = ObjectPoolManager.Create<LevelEditorPlayerSpawnPoint>(GamePrefabs.Instance.LevelEditorPlayerSpawnPointPrefab, position: position, rotation: Quaternion.identity, parent: dynamicContainer_);
					spawnPoint.SetPlayerIndex(playerIndex);
				}
			} else {
				for (int playerIndex = 0; playerIndex < dynamicArenaData_.PlayerSpawnPoints.Length; playerIndex++) {
					Vector3 position = dynamicArenaData_.PlayerSpawnPoints[playerIndex];
					ObjectPoolManager.Create(GamePrefabs.Instance.PlayerSpawnPointPrefab, position: position, rotation: Quaternion.identity, parent: dynamicContainer_);
				}
			}

			OnViewRefreshed.Invoke();
		}

		private void FillAttributesOf(GameObject instantiated, int uniqueId) {
			Dictionary<Type, IAttributeMarker> attributeMarkers = instantiated.GetComponentsInChildren<IAttributeMarker>().ToMapWithKeys(marker => marker.AttributeType);
			List<AttributeData> attributeDatas = dynamicArenaData_.GetAttributesFor(uniqueId);
			if (attributeDatas == null) {
				return;
			}

			foreach (var attributeData in attributeDatas) {
				Type attributeType = attributeData.GetType();
				if (!attributeMarkers.ContainsKey(attributeType)) {
					Debug.LogWarning("Cannot find attribute marker for key: " + attributeType);
					continue;
				}

				attributeMarkers[attributeType].SetAttribute(attributeData);
			}
		}

		private GameObject FindRequiredPrefabFor(string prefabName) {
			GameObject prefab = GamePrefabs.Instance.LevelEditorObjects.FirstOrDefault(p => p.name == prefabName);
			if (prefab == null) {
				Debug.LogWarning("Prefab named: " + prefabName + " not found in the LevelEditorObjects - corrupted dynamic arena data?");
			}

			return prefab;
		}
	}
}