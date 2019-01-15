using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;

namespace DT.Game.Battle {
	[Serializable]
	[CreateAssetMenu(fileName = "ArenaConfig", menuName = "Game/ArenaConfig")]
	public class ArenaConfig : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public GameObject Prefab {
			get { return prefab_; }
		}

		public void SaveDynamicArenaDataJson(string json) {
			dynamicArenaDataJson_ = null;
			dynamicArenaDataJsonText_ = json;
		}

		public string GetDynamicArenaDataJson() {
			if (dynamicArenaDataJson_ == null) {
				return dynamicArenaDataJsonText_;
			} else {
				return dynamicArenaDataJson_.text;
			}
		}

		public GameObject CreateArena(GameObject parent) {
			if (DynamicArenaData_ == null) {
				return ObjectPoolManager.Create(prefab_, parent);
			} else {
				var view = ObjectPoolManager.Create<DynamicArenaView>(GamePrefabs.Instance.InGameDynamicArenaPrefab, parent);
				view.Init(DynamicArenaData_, prefab_);
				return view.gameObject;
			}
		}

		// PRAGMA MARK - Internal
		[SerializeField, DTValidator.Optional]
		private GameObject prefab_;
		[SerializeField, DTValidator.Optional]
		private TextAsset dynamicArenaDataJson_;
		[SerializeField, DTValidator.Optional]
		private string dynamicArenaDataJsonText_;

		[NonSerialized]
		private DynamicArenaData dynamicArenaData_ = null;

		private DynamicArenaData DynamicArenaData_ {
			get { return dynamicArenaData_ ?? (dynamicArenaData_ = JsonUtility.FromJson<DynamicArenaData>(GetDynamicArenaDataJson())); }
		}
	}
}