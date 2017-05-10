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
	public class ReflectsStat : Stat {
		// PRAGMA MARK - Public Interface
		public override string DisplayName {
			get { return "REFLECTS"; }
		}

		public override string DisplayValue {
			get { return recordedReflects_.ToString(); }
		}

		public ReflectsStat(Player player) : base(player) {
			GameNotifications.OnBattlePlayerReflectLaser.AddListener(HandleBattlePlayerReflectLaser);
		}

		public override IList<StatAward> GetQualifiedAwards() {
			var awards = new List<StatAward>();

			IEnumerable<Player> otherPlayers = RegisteredPlayers.AllPlayers.Where(p => p != Player_);
			if (otherPlayers.All(p => StatsManager.GetStatFor<ReflectsStat>(p).recordedReflects_ < recordedReflects_)) {
				awards.Add(new StatAward(this, "PING PONG <b>CHAMPION</b>"));
			}

			return awards;
		}

		public override void Clear() {
			recordedReflects_ = 0;
		}


		// PRAGMA MARK - Internal
		private int recordedReflects_ = 0;

		private void HandleBattlePlayerReflectLaser(Laser laser, BattlePlayer battlePlayerThatReflected) {
			if (battlePlayerThatReflected != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			recordedReflects_++;
		}
	}
}