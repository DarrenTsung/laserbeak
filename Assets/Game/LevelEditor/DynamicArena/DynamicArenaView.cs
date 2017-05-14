using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

using DT.Game.Battle.Walls;

namespace DT.Game.LevelEditor {
	public class DynamicArenaView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(DynamicArenaData dynamicArenaData) {
			dynamicArenaData_ = dynamicArenaData;
			dynamicArenaData_.OnDataDirty += HandleDataDirty;
			HandleDataDirty();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (dynamicArenaData_ != null) {
				dynamicArenaData_.OnDataDirty -= HandleDataDirty;
				dynamicArenaData_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private DynamicArenaData dynamicArenaData_;

		private void HandleDataDirty() {
			this.gameObject.RecycleAllChildren();

			foreach (var objectData in dynamicArenaData_.Objects) {
				GameObject prefab = FindRequiredPrefabFor(objectData.PrefabName);
				if (prefab == null) {
					continue;
				}

				var container = ObjectPoolManager.Create<DynamicArenaObjectContainer>(GamePrefabs.Instance.DynamicArenaObjectContainer, position: objectData.Position, rotation: objectData.Rotation, parent: this.gameObject);
				container.transform.localScale = objectData.LocalScale;
				container.Init(prefab);
			}

			foreach (var wallData in dynamicArenaData_.Walls) {
				GameObject prefab = FindRequiredPrefabFor(wallData.PrefabName);
				if (prefab == null) {
					continue;
				}

				var wall = ObjectPoolManager.Create<Wall>(prefab, position: wallData.Position, rotation: Quaternion.identity, parent: this.gameObject);
				wall.SetVertexLocalPositions(wallData.VertexLocalPositions);
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