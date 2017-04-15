using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.AI;
using DT.Game.Battle.Players;
using DT.Game.GameModes;

namespace DT.Game {
	public class GamePrefabs : Singleton<GamePrefabs> {
		// PRAGMA MARK - Public Interface
		public GameObject PlayerPrefab {
			get { return playerPrefab_; }
		}

		public GameObject AIPrefab {
			get { return aiPrefab_; }
		}


		// PRAGMA MARK - Internal
		[Header("Prefabs")]
		[SerializeField]
		private GameObject playerPrefab_;

		[SerializeField]
		private GameObject aiPrefab_;
	}
}