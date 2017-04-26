using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes {
	[CreateAssetMenu(fileName = "PlatformingSurvivalGameMode", menuName = "Game/Modes/PlatformingSurvivalGameMode")]
	public class PlatformingSurvivalGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override void Cleanup() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
			BattlePlayerHealth.LaserDamage = 1;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private ArenaConfig[] arenas_;

		protected override void Activate() {
			ArenaManager.Instance.LoadArena(arenas_.Random());
			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.RemoveLast();

			BattlePlayerHealth.LaserDamage = 0;
			GameModeIntroView.Show("SURVIVE - NO PHASER DAMAGE", icons);

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 1) {
				return;
			}

			Finish();
			foreach (Player player in PlayerSpawner.AllSpawnedPlayers) {
				PlayerScores.IncrementPendingScoreFor(player);
			}
		}
	}
}