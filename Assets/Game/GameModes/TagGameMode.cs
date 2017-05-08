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
using DT.Game.GameModes.Tag;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes {
	[CreateAssetMenu(fileName = "TagGameMode", menuName = "Game/Modes/TagGameMode")]
	public class TagGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return "HOT POTATO - WITH BOMBS"; }
		}

		public override void Cleanup() {
			BattlePlayerHealth.OnBattlePlayerHit -= HandleBattlePlayerHit;
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
			BattlePlayerHealth.LaserDamage = 1;
			BattlePlayerHealth.KnockbackMultiplier = 1.0f;

			foreach (BattlePlayer battlePlayer in PlayerSpawner.AllSpawnedBattlePlayers) {
				var explosive = battlePlayer.GetComponentInChildren<TagExplosive>();
				if (explosive == null) {
					continue;
				}

				ObjectPoolManager.Recycle(explosive);
			}
		}


		// PRAGMA MARK - Internal
		// the person who is "it" - also they will explode if they don't tag anyone soon :)
		private BattlePlayer itPlayer_ = null;
		private BattlePlayer ItPlayer_ {
			get { return itPlayer_; }
		}

		private void SetItPlayer(BattlePlayer battlePlayer, float? timeLeft = null) {
			if (itPlayer_ == battlePlayer) {
				return;
			}

			itPlayer_ = battlePlayer;
			if (itPlayer_ == null) {
				Debug.LogWarning("ItPlayer is null - no explosive going to be set!");
				return;
			}

			var explosive = ObjectPoolManager.Create<TagExplosive>(GamePrefabs.Instance.TagExplosivePrefab, parent: itPlayer_.AccessoriesContainer);
			if (timeLeft == null) {
				explosive.Init(itPlayer_);
			} else {
				explosive.Init(itPlayer_, timeLeft.Value);
			}
		}

		protected override void Activate() {
			ArenaManager.Instance.LoadRandomArena();
			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.RemoveLast();

			BattlePlayerHealth.KnockbackMultiplier = 0.0f;
			BattlePlayerHealth.LaserDamage = 0;

			GameModeIntroView.Show(DisplayTitle, icons, onFinishedCallback: () => {
				SetItPlayer(PlayerSpawner.AllSpawnedBattlePlayers.Random());
			});

			BattlePlayerHealth.OnBattlePlayerHit += HandleBattlePlayerHit;
			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		private void HandleBattlePlayerHit(BattlePlayer playerHit, BattlePlayer laserSourcePlayer) {
			if (laserSourcePlayer == playerHit) {
				return;
			}

			if (laserSourcePlayer != ItPlayer_) {
				return;
			}

			TagExplosive tagExplosive = laserSourcePlayer.GetComponentInChildren<TagExplosive>();
			SetItPlayer(playerHit, tagExplosive.TimeLeft);
			if (tagExplosive != null) {
				ObjectPoolManager.Recycle(tagExplosive);
			} else {
				Debug.LogError("Failed to get TagExplosive from It player, very weird!");
			}
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 1) {
				// if ItPlayer_ is now dead - choose new itPlayer
				if (!PlayerSpawner.AllSpawnedBattlePlayers.Contains(ItPlayer_)) {
					SetItPlayer(PlayerSpawner.AllSpawnedBattlePlayers.Random());
				}
				return;
			}

			Finish();
			foreach (Player player in PlayerSpawner.AllSpawnedPlayers) {
				PlayerScores.IncrementPendingScoreFor(player);
			}
		}
	}
}