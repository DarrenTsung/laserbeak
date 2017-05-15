using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;

namespace DT.Game.Battle {
	[CreateAssetMenu(fileName = "ArenaConfig", menuName = "Game/ArenaConfig")]
	public class ArenaConfig : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public GameObject CreateArena(GameObject parent) {
			if (DynamicArenaData_ == null) {
				return ObjectPoolManager.Create(prefab_, parent);
			} else {
				var view = ObjectPoolManager.Create<DynamicArenaView>(GamePrefabs.Instance.InGameDynamicArenaPrefab, parent);
				view.Init(DynamicArenaData_);
				return view.gameObject;
			}
		}

		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject prefab_;
		[SerializeField]
		private TextAsset dynamicArenaDataJson_;

		[NonSerialized]
		private DynamicArenaData dynamicArenaData_ = null;

		private DynamicArenaData DynamicArenaData_ {
			get {
				if (dynamicArenaDataJson_ == null) {
					return null;
				}

				return dynamicArenaData_ ?? (dynamicArenaData_ = JsonUtility.FromJson<DynamicArenaData>(dynamicArenaDataJson_.text));
			}
		}
	}
}