using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.LevelEditor {
	[Serializable]
	public class DynamicArenaData : ISerializationCallbackReceiver {
		public event Action OnDataDirty = delegate {};

		public IList<DynamicArenaObjectData> Objects {
			get { return objects_; }
		}

		public IList<DynamicArenaWallData> Walls {
			get { return walls_; }
		}

		public Vector3[] PlayerSpawnPoints {
			get {
				if (playerSpawnPoints_ == null) {
					playerSpawnPoints_ = new Vector3[4];
				}

				if (playerSpawnPoints_.Length != 4) {
					Array.Resize(ref playerSpawnPoints_, 4);
				}

				return playerSpawnPoints_;
			}
		}

		public int SerializeObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale) {
			var objectData = new DynamicArenaObjectData();
			objectData.PrefabName = prefab.name;
			objectData.Position = position;
			objectData.Rotation = rotation;
			objectData.LocalScale = localScale;
			objectData.UniqueId = objects_.Count + 1;
			objects_.Add(objectData);
			OnDataDirty.Invoke();
			return objectData.UniqueId;
		}

		public int SerializeWall(GameObject prefab, Vector3 position, IEnumerable<Vector3> vertexLocalPositions) {
			var wallData = new DynamicArenaWallData();
			wallData.PrefabName = prefab.name;
			wallData.Position = position;
			wallData.VertexLocalPositions = vertexLocalPositions.ToArray();
			wallData.UniqueId = kWallIndexOffset + walls_.Count + 1;
			walls_.Add(wallData);
			OnDataDirty.Invoke();
			return wallData.UniqueId;
		}

		public void SerializeWaveAttribute(int uniqueId, int waveId) {
			var waveData = new WaveAttributeData();
			waveData.LinkedUniqueId = uniqueId;
			waveData.WaveId = waveId;
			waveAttributes_.Add(waveData);
			OnDataDirty.Invoke();
		}

		public void SerializePlayerSpawnPoint(int index, Vector3 position) {
			if (index < 0 || index >= 4) {
				Debug.LogError("Invalid index: " + index + " when trying to serialize player spawn point!");
				return;
			}

			PlayerSpawnPoints[index] = position;
			OnDataDirty.Invoke();
		}

		public void RemoveObject(DynamicArenaObjectData obj) {
			bool removed = objects_.Remove(obj);
			if (!removed) {
				Debug.LogWarning("Could not remove obj: " + obj + " because not in objects_!");
				return;
			}
			OnDataDirty.Invoke();
		}

		public void ReloadFromSerialized(string serialized) {
			JsonUtility.FromJsonOverwrite(serialized, this);
			OnDataDirty.Invoke();
		}

		public string Serialize() {
			return JsonUtility.ToJson(this);
		}

		public List<AttributeData> GetAttributesFor(int uniqueId) {
			if (uniqueIdToAttributeMap_ == null) {
				uniqueIdToAttributeMap_ = new Dictionary<int, List<AttributeData>>();
				foreach (var attributeData in AllAttributes_) {
					List<AttributeData> attributeDatas = uniqueIdToAttributeMap_.GetAndCreateIfNotFound(attributeData.LinkedUniqueId);
					attributeDatas.Add(attributeData);
				}
			}

			return uniqueIdToAttributeMap_.GetValueOrDefault(uniqueId);
		}

		public DynamicArenaData() {
			OnDataDirty += HandleDataDirty;
		}


		// PRAGMA MARK - ISerializationCallbackReceiver Implementation
		void ISerializationCallbackReceiver.OnBeforeSerialize() {}

		void ISerializationCallbackReceiver.OnAfterDeserialize() {
			// if any of objects_ / walls_ are missing uniqueId, reserialize them based on ordering
			int objectIndex = 1;
			foreach (var objectData in objects_) {
				if (objectData.UniqueId <= 0) {
					objectData.UniqueId = objectIndex;
				}
				objectIndex++;
			}

			int wallIndex = 1;
			foreach (var wallData in walls_) {
				if (wallData.UniqueId <= 0) {
					wallData.UniqueId = kWallIndexOffset + wallIndex;
				}
				wallIndex++;
			}
		}


		// PRAGMA MARK - Internal
		private const int kWallIndexOffset = 1000;

		[Header("Properties")]
		[SerializeField]
		private List<DynamicArenaObjectData> objects_ = new List<DynamicArenaObjectData>();
		[SerializeField]
		private List<DynamicArenaWallData> walls_ = new List<DynamicArenaWallData>();
		[SerializeField]
		private Vector3[] playerSpawnPoints_ = null;

		[Space]
		[SerializeField]
		private List<WaveAttributeData> waveAttributes_ = new List<WaveAttributeData>();


		[NonSerialized]
		private Dictionary<int, List<AttributeData>> uniqueIdToAttributeMap_ = null;

		private IEnumerable<AttributeData> AllAttributes_ {
			get { return waveAttributes_.Cast<AttributeData>(); }
		}

		private void HandleDataDirty() {
			uniqueIdToAttributeMap_ = null;
		}
	}

	[Serializable]
	public class DynamicArenaObjectData {
		public string PrefabName;
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 LocalScale;

		public int UniqueId = 0;
	}

	[Serializable]
	public class DynamicArenaWallData {
		public string PrefabName;
		public Vector3 Position;
		public Vector3[] VertexLocalPositions;

		public int UniqueId = 0;
	}
}