using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.Battle.Stats {
	public static class StatsManager {
		// PRAGMA MARK - Public Interface
		public static IEnumerable<Stat> GetStatsFor(Player player) {
			return statsMap_.GetValueOrDefault(player).Values;
		}

		public static TStat GetStatFor<TStat>(Player player) where TStat : Stat {
			return (TStat)statsMap_.GetValueOrDefault(player).GetValueOrDefault(typeof(TStat));
		}

		public static void ClearAllStats() {
			foreach (var statTypeMap in statsMap_.Values) {
				foreach (var stat in statTypeMap.Values) {
					stat.Clear();
				}
			}
		}


		// PRAGMA MARK - Internal
		private static readonly Dictionary<Player, Dictionary<Type, Stat>> statsMap_ = new Dictionary<Player, Dictionary<Type, Stat>>();

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			RegisteredPlayers.OnPlayerAdded += HandlePlayerAdded;
		}

		private static void HandlePlayerAdded(Player player) {
			statsMap_[player] = new Dictionary<Type, Stat>() {
				{ typeof(ShotsFiredStat), new ShotsFiredStat(player) },
				{ typeof(AccuracyStat), new AccuracyStat(player) },
				{ typeof(DashHitsStat), new DashHitsStat(player) },
				{ typeof(ReflectsStat), new ReflectsStat(player) },
			};
		}
	}
}