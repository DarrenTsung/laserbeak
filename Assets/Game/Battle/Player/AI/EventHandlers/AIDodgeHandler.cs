using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIDodgeHandler : AIEventHandler {
		// PRAGMA MARK - Public Interface
		public override void Setup() {
			GameNotifications.OnBattlePlayerShotLaser.AddListener(HandleBattlePlayerShotLaser);
		}

		public override void Cleanup() {
			GameNotifications.OnBattlePlayerShotLaser.RemoveListener(HandleBattlePlayerShotLaser);
		}


		// PRAGMA MARK - Internal
		private void HandleBattlePlayerShotLaser(Laser laser, BattlePlayer battlePlayer) {
			if (battlePlayer == StateMachine_.Player) {
				return;
			}

			// dash randomly!
			StateMachine_.Dash(UnityEngine.Random.onUnitSphere);
		}
	}
}