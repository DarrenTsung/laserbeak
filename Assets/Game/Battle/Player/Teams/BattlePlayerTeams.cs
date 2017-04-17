using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public static class BattlePlayerTeams {
		// PRAGMA MARK - Static Public Interface
		public static bool AreOnSameTeam(BattlePlayer player, BattlePlayer otherPlayer) {
			// if not on any teams
			if (!teamMap_.ContainsKey(player)) {
				return false;
			}

			return teamMap_[player].Contains(otherPlayer);
		}

		public static void DeclareTeam(IEnumerable<BattlePlayer> playerTeam) {
			HashSet<BattlePlayer> playerSet = new HashSet<BattlePlayer>(playerTeam);
			foreach (BattlePlayer battlePlayer in playerTeam) {
				if (teamMap_.ContainsKey(battlePlayer)) {
					// if already part of team - lets just error for now
					// since we need to maintain consistency of team references
					Debug.LogError("Unsupported inconsistency edge case - cannot declare a team for a player that already has a team right now!");
				}

				teamMap_[battlePlayer] = playerSet;
			}
		}

		public static void ClearTeams() {
			teamMap_.Clear();
		}


		// PRAGMA MARK - Internal
		private static readonly Dictionary<BattlePlayer, HashSet<BattlePlayer>> teamMap_ = new Dictionary<BattlePlayer, HashSet<BattlePlayer>>();
	}
}