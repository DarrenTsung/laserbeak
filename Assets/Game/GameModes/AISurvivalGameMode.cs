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
	[CreateAssetMenu(fileName = "AISurvivalGameMode", menuName = "Game/Modes/AISurvivalGameMode")]
	public class AISurvivalGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override void Cleanup() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
			InGameTimer.CleanupAndHide();
			AISpawner.ShouldRespawn = false;
		}


		// PRAGMA MARK - Internal
		private const int kSurviveTime = 15;

		[Header("Outlets")]
		[SerializeField]
		private ArenaConfig[] arenas_;

		protected override string DisplayTitle {
			get { return "AI SURVIVAL"; }
		}

		protected override void Activate() {
			ArenaManager.Instance.LoadArena(arenas_.Random());

			PlayerSpawner.SpawnAllPlayers();
			BattlePlayerTeams.DeclareTeam(PlayerSpawner.AllSpawnedBattlePlayers);

			AISpawner.ShouldRespawn = true;
			AISpawner.SpawnAIPlayers();
			BattlePlayerTeams.DeclareTeam(AISpawner.AllSpawnedBattlePlayers);

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>(RegisteredPlayers.AllPlayers.Select(p => GameModeIntroView.Icon.Player));
			icons.Add(GameModeIntroView.Icon.Swords);
			icons.Add(GameModeIntroView.Icon.Skull);
			icons.Add(GameModeIntroView.Icon.Skull);
			icons.Add(GameModeIntroView.Icon.Skull);
			icons.Add(GameModeIntroView.Icon.Skull);
			icons.Add(GameModeIntroView.Icon.Skull);
			icons.Add(GameModeIntroView.Icon.Skull);

			GameModeIntroView.Show(DisplayTitle, icons);
			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 1) {
				return;
			}

			AISpawner.ShouldRespawn = false;
			foreach (BattlePlayer battlePlayer in AISpawner.AllSpawnedBattlePlayers.ToArray()) {
				CoroutineWrapper.DoAfterDelay(UnityEngine.Random.Range(0.0f, 0.8f), () => {
					battlePlayer.Health.Kill();
				});
			}

			Finish();
			foreach (Player player in PlayerSpawner.AllSpawnedPlayers) {
				PlayerScores.IncrementPendingScoreFor(player);
			}
		}
	}
}