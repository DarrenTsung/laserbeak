using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

using DT.Game.Battle.Lasers;
using DT.Game.Battle.Players;

namespace DT.Game {
	public class BattlePlayerLaserEvent : UnityEvent<Laser, BattlePlayer> {}
	public class BattlePlayerEvent : UnityEvent<BattlePlayer> {}
	public class BattlePlayerObjectEvent : UnityEvent<BattlePlayer, Object> {}

	// first is battle player was hit, second was battle player that hit the other player
	public class BattlePlayerDashHitEvent : UnityEvent<BattlePlayer, BattlePlayer> {}

	public static class GameNotifications {
		// PRAGMA MARK - Static Public Interface
		public static UnityEvent OnGameWon = new UnityEvent();
		public static BattlePlayerLaserEvent OnBattlePlayerReflectLaser = new BattlePlayerLaserEvent();
		public static BattlePlayerLaserEvent OnBattlePlayerLaserHit = new BattlePlayerLaserEvent();
		public static BattlePlayerLaserEvent OnBattlePlayerShotLaser = new BattlePlayerLaserEvent();
		public static BattlePlayerDashHitEvent OnBattlePlayerDashHit = new BattlePlayerDashHitEvent();
		public static BattlePlayerObjectEvent OnBattlePlayerDiedWithSource = new BattlePlayerObjectEvent();
		public static BattlePlayerEvent OnBattlePlayerFellOffGround = new BattlePlayerEvent();
	}
}