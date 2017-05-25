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
	[CreateAssetMenu(fileName = "TestingGameMode", menuName = "Game/Modes/TestingGameMode")]
	public class TestingGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return "TESTING MODE"; }
		}

		public override int Id {
			get { return 0; }
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
			BattlePlayerHealth.KnockbackMultiplier = 0.1f;

			GameModeIntroView.Show(DisplayTitle, icons);

			foreach (Player player in RegisteredPlayers.AllPlayers.Where(p => p.IsAI)) {
				BattlePlayer battlePlayer = PlayerSpawner.GetBattlePlayerFor(player);
				battlePlayer.Rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
			}

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		protected override void CleanupInternal() {
			BattlePlayerHealth.LaserDamage = 1;
			BattlePlayerHealth.KnockbackMultiplier = 1.0f;
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
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