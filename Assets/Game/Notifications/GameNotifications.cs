using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

using DT.Game.Battle.Players;

namespace DT.Game {
	// first - battle player that was hit, second - battle player that shot the laser
	public class BattlePlayerLaserHitEvent : UnityEvent<BattlePlayer, BattlePlayer> {}

	public class BattlePlayerUnityEvent : UnityEvent<BattlePlayer> {}

	public static class GameNotifications {
		// PRAGMA MARK - Static Public Interface
		public static UnityEvent OnGameWon = new UnityEvent();
		public static BattlePlayerLaserHitEvent OnBattlePlayerLaserHit = new BattlePlayerLaserHitEvent();
		public static BattlePlayerUnityEvent OnBattlePlayerShootLaser = new BattlePlayerUnityEvent();
	}
}