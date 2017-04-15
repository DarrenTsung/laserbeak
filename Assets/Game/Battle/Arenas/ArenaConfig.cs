using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	[CreateAssetMenu(fileName = "ArenaConfig", menuName = "Game/ArenaConfig")]
	public class ArenaConfig : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public GameObject Prefab {
			get { return prefab_; }
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject prefab_;
	}
}