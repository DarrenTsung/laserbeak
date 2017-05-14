using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.LevelEditor {
	[Serializable]
	public class DynamicArenaData {
		public event Action OnDataDirty = delegate {};

		public IList<DynamicArenaObjectData> Objects {
			get { return objects_; }
		}

		public IList<DynamicArenaWallData> Walls {
			get { return walls_; }
		}

		public void SerializeObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale) {
			var objectData = new DynamicArenaObjectData();
			objectData.PrefabName = prefab.name;
			objectData.Position = position;
			objectData.Rotation = rotation;
			objectData.LocalScale = localScale;
			objects_.Add(objectData);
			OnDataDirty.Invoke();
		}

		public void SerializeWall(GameObject prefab, Vector3 position, IEnumerable<Vector3> vertexLocalPositions) {
			var wallData = new DynamicArenaWallData();
			wallData.PrefabName = prefab.name;
			wallData.Position = position;
			wallData.VertexLocalPositions = vertexLocalPositions.ToArray();
			walls_.Add(wallData);
			OnDataDirty.Invoke();
		}

		public void ReloadFromSerialized(string serialized) {
			JsonUtility.FromJsonOverwrite(serialized, this);
			OnDataDirty.Invoke();
		}

		public string Serialize() {
			return JsonUtility.ToJson(this);
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private List<DynamicArenaObjectData> objects_ = new List<DynamicArenaObjectData>();
		[SerializeField]
		private List<DynamicArenaWallData> walls_ = new List<DynamicArenaWallData>();
	}

	[Serializable]
	public class DynamicArenaObjectData {
		public string PrefabName;
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 LocalScale;
	}

	[Serializable]
	public class DynamicArenaWallData {
		public string PrefabName;
		public Vector3 Position;
		public Vector3[] VertexLocalPositions;
	}
}