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
	public class SuicidesStat : Stat {
		// PRAGMA MARK - Public Interface
		public override string DisplayName {
			get { return "SUICIDES"; }
		}

		public override string DisplayValue {
			get { return TotalSuicides.ToString(); }
		}

		public SuicidesStat(Player player) : base(player) {
			GameNotifications.OnBattlePlayerDiedWithSource.AddListener(HandleBattlePlayerDiedWithSource);
			GameNotifications.OnBattlePlayerFellOffGround.AddListener(HandleBattlePlayerFellOffGround);
		}

		public override IList<StatAward> GetQualifiedAwards() {
			var awards = new List<StatAward>();

			IEnumerable<Player> otherPlayers = RegisteredPlayers.AllPlayers.Where(p => p != Player_);
			if (otherPlayers.All(p => StatsManager.GetStatFor<SuicidesStat>(p).TotalSuicides < TotalSuicides)) {
				if (recordedSuicidesByLaser_ > recordedSuicidesByFalling_) {
					awards.Add(new StatAward(this, "BEST AT <b>SUICIDING</b>"));
				} else {
					awards.Add(new StatAward(this, "GOOD AT <b>FALLING OFF EDGES</b>"));
				}
			}

			if (otherPlayers.All(p => StatsManager.GetStatFor<SuicidesStat>(p).TotalSuicides > TotalSuicides)) {
				awards.Add(new StatAward(this, "BEST AT <b>NOT SUICIDING</b>"));
			}

			return awards;
		}

		public override void Clear() {
			recordedSuicidesByLaser_ = 0;
			recordedSuicidesByFalling_ = 0;
		}


		// PRAGMA MARK - Internal
		private int recordedSuicidesByLaser_ = 0;
		private int recordedSuicidesByFalling_ = 0;

		private int TotalSuicides {
			get { return recordedSuicidesByFalling_ + recordedSuicidesByLaser_; }
		}

		private void HandleBattlePlayerFellOffGround(BattlePlayer battlePlayer) {
			if (battlePlayer != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			Debug.Log("BATTLE PLAYER FELL OFF THE GROUND!");
			Debug.Log("battlePlayer.Health.KnockbackDamageSource: " + battlePlayer.Health.KnockbackDamageSource);
			if (battlePlayer.Health.KnockbackDamageSource != null) {
				// need to check for my own laser hitting me + pushing me off the edge
				if (battlePlayer.Health.KnockbackDamageSource is Laser) {
					Laser knockbackLaser = battlePlayer.Health.KnockbackDamageSource as Laser;
					Debug.Log("LASER PUSHED");

					// if laser source is not self - then someone else pushed me off the edge
					if (knockbackLaser.BattlePlayer != battlePlayer) {
						return;
					}
				} else if (battlePlayer.Health.KnockbackDamageSource is BattlePlayer) {
					BattlePlayer knockbackPlayer = battlePlayer.Health.KnockbackDamageSource as BattlePlayer;
					Debug.Log("PLAYER PUSHED");

					if (knockbackPlayer == battlePlayer) {
						Debug.LogWarning("How did battlePlayer knockback themselves?? Investigate please.");
					}

					// knockbacked by another player - does not count as suicide
					return;
				}

				recordedSuicidesByLaser_++;
			} else {
				// classic suicide - just walked / dashed off the edge
				recordedSuicidesByFalling_++;
			}
		}

		private void HandleBattlePlayerDiedWithSource(BattlePlayer battlePlayer, object damageSource) {
			if (battlePlayer != PlayerSpawner.GetBattlePlayerFor(Player_)) {
				return;
			}

			Laser laser = damageSource as Laser;
			if (laser == null) {
				return;
			}

			// if source is not self - then it is not a suicide
			if (laser.BattlePlayer != battlePlayer) {
				return;
			}

			recordedSuicidesByLaser_++;
		}
	}
}