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
		public override void Cleanup() {
			BattlePlayerHealth.OnBattlePlayerHit -= HandleBattlePlayerHit;
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
			BattlePlayerHealth.LaserDamage = 1;
			BattlePlayerHealth.KnockbackMultiplier = 1.0f;
		}


		// PRAGMA MARK - Internal
		// the person who is "it" - also they will explode if they don't tag anyone soon :)
		private BattlePlayer itPlayer_ = null;
		private BattlePlayer ItPlayer_ {
			get { return itPlayer_; }
			set {
				if (itPlayer_ == value) {
					return;
				}

				itPlayer_ = value;
				if (itPlayer_ != null) {
					var explosive = ObjectPoolManager.Create<TagExplosive>(GamePrefabs.Instance.TagExplosivePrefab, parent: itPlayer_.AccessoriesContainer);
					explosive.Init(itPlayer_);
				}
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

			BattlePlayerHealth.KnockbackMultiplier = 0.2f;
			BattlePlayerHealth.LaserDamage = 0;

			GameModeIntroView.Show("EXPLOSIVE TAG", icons, onFinishedCallback: () => {
				ItPlayer_ = PlayerSpawner.AllSpawnedBattlePlayers.Random();
			});

			BattlePlayerHealth.OnBattlePlayerHit += HandleBattlePlayerHit;
			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		private void HandleBattlePlayerHit(BattlePlayer playerHit, BattlePlayer laserSourcePlayer) {
			if (laserSourcePlayer != ItPlayer_) {
				return;
			}

			ObjectPoolManager.Recycle(laserSourcePlayer.GetComponentInChildren<TagExplosive>());
			ItPlayer_ = playerHit;
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 1) {
				// if ItPlayer_ is now dead - choose new itPlayer
				if (!PlayerSpawner.AllSpawnedBattlePlayers.Contains(ItPlayer_)) {
					ItPlayer_ = PlayerSpawner.AllSpawnedBattlePlayers.Random();
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