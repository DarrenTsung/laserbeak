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
	public class DashHitsStat : Stat {
		// PRAGMA MARK - Public Interface
		public override string DisplayName {
			get { return "DASH HITS"; }
		}

		public override string DisplayValue {
			get { return recordedHits_.ToString(); }
		}

		public DashHitsStat(Player player) : base(player) {
			GameNotifications.OnBattlePlayerDashHit.AddListener(HandleBattlePlayerDashHit);
		}

		public override IList<StatAward> GetQualifiedAwards() {
			var awards = new List<StatAward>();

			IEnumerable<Player> otherPlayers = RegisteredPlayers.AllPlayers.Where(p => p != Player_);
			if (otherPlayers.All(p => StatsManager.GetStatFor<DashHitsStat>(p).recordedHits_ < recordedHits_)) {
				awards.Add(new StatAward(this, "MOST <b>UP IN YOUR GRILL</b>"));
			}

			return awards;
		}

		public override void Clear() {
			recordedHits_ = 0;
		}


		// PRAGMA MARK - Internal
		private int recordedHits_ = 0;

		private void HandleBattlePlayerDashHit(BattlePlayer battlePlayerHit, BattlePlayer battlePlayerSource) {
			if (battlePlayerSource != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			recordedHits_++;
		}
	}
}