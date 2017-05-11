using System;
using System.Collections;
using System.Collections.Generic;
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

		public void SerializeObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale) {
			var objectData = new DynamicArenaObjectData();
			objectData.PrefabName = prefab.name;
			objectData.Position = position;
			objectData.Rotation = rotation;
			objectData.LocalScale = localScale;
			objects_.Add(objectData);
			OnDataDirty.Invoke();
		}


		// PRAGMA MARK - Internal
		private List<DynamicArenaObjectData> objects_ = new List<DynamicArenaObjectData>();
	}

	[SerializeField]
	public class DynamicArenaObjectData {
		public string PrefabName;
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 LocalScale;
	}
}