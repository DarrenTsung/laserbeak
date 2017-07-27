using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Transitions;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public static class BattlePlayerUtil {
		// PRAGMA MARK - Static Public Interface
		public static BattlePlayer GetClosestEnemyPlayerFor(BattlePlayer player, Predicate<BattlePlayer> whereCondition = null) {
			BattlePlayer closestEnemyPlayer = BattlePlayer.ActivePlayers.Where(p => p != player)
																		.Where(p => !BattlePlayerTeams.AreOnSameTeam(player, p))
																		.Where(p => whereCondition != null ? whereCondition.Invoke(p) : true)
																		.MinBy(p => (p.transform.position - player.transform.position).magnitude);
			return closestEnemyPlayer;
		}

		public static Vector2 XZVectorFromTo(BattlePlayer playerA, BattlePlayer playerB) {
			return (playerB.transform.position - playerA.transform.position).Vector2XZValue();
		}
	}
}