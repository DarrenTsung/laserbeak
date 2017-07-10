using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.AI;
using DT.Game.Battle.Players;
using DT.Game.GameModes;

namespace DT.Game {
	public class GameConstants : Singleton<GameConstants> {
		// PRAGMA MARK - Public Interface
		public GameMode[] GameModes {
			get { return gameModes_; }
		}

		public void ConfigureWithSubstitutePlayerAI(BattlePlayer player) {
			AIStateMachine ai = ObjectPoolManager.Create<AIStateMachine>(GamePrefabs.Instance.AIPrefab, parent: player.gameObject);
			ai.Init(player, substitutePlayerAIConfiguration_);
		}

		[Header("Basic")]
		public int ScoreToWin = 5;
		public int PlayersToFill = 4;
		public bool FillWithAI = false;
		public bool DemoMode = false;

		[Space]
		public float UIOffsetDelay = 0.1f;

		[Space]
		public float ColliderDisappearDelay = 0.2f;

		[Header("Player")]
		public BattlePlayerSkin[] PlayerSkins;
		public BattlePlayerSkin EnemySkin;

		[Space]
		public float PlayerShieldAlphaMin = 0.2f;

		[Header("Materials")]
		public Material PlayerOpaqueMaterial;
		public Material PlayerTransparentMaterial;

		public Material EyeMaterial;
		public Material BeakMaterial;

		public Material LaserMaterial;

		[Header("Battle")]
		public Vector3 PlayerFocusOffset = new Vector3(0.0f, 4.0f, 0.0f);
		public ArenaConfig PlayerCustomizationLobbyArena;


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private GameMode[] gameModes_;

		[SerializeField]
		private AIConfiguration substitutePlayerAIConfiguration_;
	}
}