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
	public class GameConstants : Singleton<GameConstants> {
		// PRAGMA MARK - Public Interface
		public GameObject PlayerPrefab {
			get { return playerPrefab_; }
		}

		public GameMode[] GameModes {
			get { return gameModes_; }
		}

		public void ConfigureWithSubstitutePlayerAI(BattlePlayer player) {
			AIStateMachine ai = ObjectPoolManager.Create<AIStateMachine>(aiPrefab_, parent: player.gameObject);
			ai.Init(player, substitutePlayerAIConfiguration_);
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject playerPrefab_;

		[Space]
		[SerializeField]
		private GameMode[] gameModes_;

		[Space]
		[SerializeField]
		private GameObject aiPrefab_;

		[SerializeField]
		private AIConfiguration substitutePlayerAIConfiguration_;

	}
}