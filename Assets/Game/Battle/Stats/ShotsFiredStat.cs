using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.Battle.Stats {
	public class ShotsFiredStat : Stat {
		// PRAGMA MARK - Public Interface
		public override string DisplayName {
			get { return "SHOTS"; }
		}

		public override string DisplayValue {
			get { return recordedShots_.ToString(); }
		}

		public ShotsFiredStat(Player player) : base(player) {
			GameNotifications.OnBattlePlayerShootLaser.AddListener(HandleBattlePlayerShootLaser);
		}

		public override IList<StatAward> GetQualifiedAwards() {
			var awards = new List<StatAward>();

			IEnumerable<Player> otherPlayers = RegisteredPlayers.AllPlayers.Where(p => p != Player_);
			if (otherPlayers.All(p => StatsManager.GetStatFor<ShotsFiredStat>(p).recordedShots_ < recordedShots_)) {
				awards.Add(new StatAward(this, "MOST <b>TRIGGER HAPPY</b>"));
			}

			if (otherPlayers.All(p => StatsManager.GetStatFor<ShotsFiredStat>(p).recordedShots_ > recordedShots_)) {
				awards.Add(new StatAward(this, "<b>PACIFIST</b>"));
			}

			return awards;
		}

		public override void Clear() {
			recordedShots_ = 0;
		}


		// PRAGMA MARK - Internal
		private int recordedShots_ = 0;

		private void HandleBattlePlayerShootLaser(BattlePlayer battlePlayer) {
			if (battlePlayer != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			recordedShots_++;
		}
	}
}