using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Lasers;
using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.Battle.Stats {
	public class AccuracyStat : Stat {
		// PRAGMA MARK - Public Interface
		public override string DisplayName {
			get { return "ACCURACY"; }
		}

		public override string DisplayValue {
			get {
				if (recordedShots_ == 0) {
					return "N/A";
				}

				return string.Format("{0}%", (Accuracy * 100).ToString("0"));
			}
		}

		public AccuracyStat(Player player) : base(player) {
			GameNotifications.OnBattlePlayerLaserHit.AddListener(HandleBattlePlayerLaserHit);
			GameNotifications.OnBattlePlayerReflectLaser.AddListener(HandleBattlePlayerReflectLaser);
			GameNotifications.OnBattlePlayerShootLaser.AddListener(HandleBattlePlayerShootLaser);
		}

		public override IList<StatAward> GetQualifiedAwards() {
			var awards = new List<StatAward>();

			IEnumerable<Player> otherPlayers = RegisteredPlayers.AllPlayers.Where(p => p != Player_);
			if (otherPlayers.All(p => StatsManager.GetStatFor<AccuracyStat>(p).Accuracy < Accuracy)) {
				awards.Add(new StatAward(this, "LIKELY AN <b>AIM BOT</b>"));
			}

			if (otherPlayers.All(p => StatsManager.GetStatFor<AccuracyStat>(p).Accuracy > Accuracy)) {
				awards.Add(new StatAward(this, "DEFINITELY NOT AN <b>AIM BOT</b>"));
			}

			return awards;
		}

		public override void Clear() {
			recordedShots_ = 0;
			recordedHits_ = 0;
		}


		// PRAGMA MARK - Internal
		private int recordedShots_ = 0;
		private int recordedHits_ = 0;

		private float Accuracy {
			get {
				if (recordedShots_ == 0) {
					return 0;
				}

				return recordedHits_ / (float)recordedShots_;
			}
		}

		private void HandleBattlePlayerShootLaser(BattlePlayer battlePlayer) {
			if (battlePlayer != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			recordedShots_++;
		}

		// someone else reflecting your laser counts towards hit
		private void HandleBattlePlayerReflectLaser(Laser laser, BattlePlayer battlePlayer) {
			RecordHitIfLaserMeetsRequirements(laser);
		}

		// someone else getting hit by your laser counts towards hit
		private void HandleBattlePlayerLaserHit(Laser laser, BattlePlayer battlePlayerHit) {
			RecordHitIfLaserMeetsRequirements(laser);
		}

		private void RecordHitIfLaserMeetsRequirements(Laser laser) {
			// if reflected then doesn't count
			if (laser.BattlePlayerSources.Count > 1) {
				return;
			}

			if (laser.BattlePlayer != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			recordedHits_++;
		}
	}
}