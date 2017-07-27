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

			// NOTE (darren): do calculations in XZ space because height doesn't matter in PHASERBEAK
			Vector2 laserDirection = laser.transform.forward.Vector2XZValue();
			Vector2 laserToSelfVector = (StateMachine_.Player.transform.position - laser.transform.position).Vector2XZValue();

			float angleDistance = Vector2.Angle(laserDirection, laserToSelfVector);
			if (angleDistance > 30.0f) {
				return;
			}

			StateMachine_.Dash(GenerateValidDashDirection());
		}

		private Vector3 GenerateValidDashDirection() {
			float predictedDashDistance = AIUtil.GetRandomPredictedDashDistance();
			Vector3 playerPosition = StateMachine_.Player.transform.position;

			Vector3 lastDashDirection = Vector3.zero;
			for (int i = 0; i < GameConstants.Instance.AIPositionRetries; i++) {
				Vector3 dashDirection = UnityEngine.Random.insideUnitCircle.normalized;
				Vector3 dashEndPosition = playerPosition + (dashDirection * predictedDashDistance);

				lastDashDirection = dashDirection;

				if (!AIUtil.IsXZPositionOnPlatform(dashEndPosition) || AIUtil.DoesWallExistBetweenXZPoints(playerPosition, dashEndPosition)) {
					continue;
				}

				return dashDirection;
			}

			Debug.LogWarning("Using invalid dash direction!", context: StateMachine_.Player);
			return lastDashDirection;
		}
	}
}