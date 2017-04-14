using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public class GameConstants : Singleton<GameConstants> {
		// PRAGMA MARK - Public Interface
		public GameObject PlayerPrefab {
			get { return playerPrefab_; }
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject playerPrefab_;
	}
}