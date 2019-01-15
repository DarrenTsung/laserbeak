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
	[CreateAssetMenu(fileName = "LaserSurvivalBattleGameMode", menuName = "Game/Modes/LaserSurvivalGameMode")]
	public class LaserSurvivalBattleGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return "LASER DEATH WHEEL"; }
		}

		public override int Id {
			get { return GameMode.GetIdFor<LaserSurvivalBattleGameMode>(); }
		}


		// PRAGMA MARK - Internal
		protected override void Activate() {
			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.RemoveLast();

			BattlePlayerHealth.LaserDamage = 0;
			InGameConstants.ShowShields = false;
			InGameConstants.AllowChargingLasers = false;

			GameModeIntroView.Show(DisplayTitle, icons);

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		protected override void CleanupInternal() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;

			BattlePlayerHealth.LaserDamage = 1;
			InGameConstants.ShowShields = true;
			InGameConstants.AllowChargingLasers = true;
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