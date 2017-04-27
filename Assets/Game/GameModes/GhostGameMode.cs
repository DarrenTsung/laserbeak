using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Lasers;
using DT.Game.Battle.Players;
using DT.Game.GameModes.Ghost;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes {
	[CreateAssetMenu(fileName = "GhostGameMode", menuName = "Game/Modes/GhostGameMode")]
	public class GhostGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override void Cleanup() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
			BattlePlayerInputDash.OnPlayerDash -= HandlePlayerDash;
			Laser.OnPlayerShoot -= HandlePlayerShoot;
			CleanupGhostModeAddOns();
		}


		// PRAGMA MARK - Internal
		private const float kAlphaLevel = 0.55f;
		private const float kAlphaDashDuration = 1.0f;
		private const float kAlphaShootDuration = 1.5f;

		private readonly Dictionary<BattlePlayer, GhostModePlayerAddOn> ghostModeAddOns_ = new Dictionary<BattlePlayer, GhostModePlayerAddOn>();

		protected override void Activate() {
			CleanupGhostModeAddOns();
			ArenaManager.Instance.LoadRandomArena();
			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.RemoveLast();

			GameModeIntroView.Show("GHOST MODE", icons);

			foreach (BattlePlayer battlePlayer in PlayerSpawner.AllSpawnedBattlePlayers) {
				ghostModeAddOns_[battlePlayer] = new GhostModePlayerAddOn(battlePlayer);
			}

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
			BattlePlayerInputDash.OnPlayerDash += HandlePlayerDash;
			Laser.OnPlayerShoot += HandlePlayerShoot;
		}

		private void HandlePlayerDash(BattlePlayer battlePlayer) {
			ghostModeAddOns_[battlePlayer].AnimateAlpha(kAlphaLevel, kAlphaDashDuration);
		}

		private void HandlePlayerShoot(BattlePlayer battlePlayer) {
			foreach (BattlePlayer p in PlayerSpawner.AllSpawnedBattlePlayers) {
				ghostModeAddOns_[p].AnimateAlpha(kAlphaLevel, kAlphaShootDuration);
			}
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

		private void CleanupGhostModeAddOns() {
			foreach (var addOn in ghostModeAddOns_.Values) {
				addOn.Dispose();
			}
			ghostModeAddOns_.Clear();
		}
	}
}