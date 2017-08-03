using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputMovement : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kPlayerSpeed = 5.3f;

		private void FixedUpdate() {
			if (!Enabled) {
				return;
			}

			Vector2 speedVector = InputDelegate_.MovementVector * kPlayerSpeed * Player_.WeightedRatio();
			Vector3 speedWorldVector = speedVector.Vector3XZValue();

			if (InGameConstants.AllowBattlePlayerMovement) {
				Player_.Rigidbody.velocity = speedWorldVector;
			} else {
				Player_.Rigidbody.velocity = Vector3.zero;
			}
			Player_.Rigidbody.angularVelocity = Vector3.zero;

			// snap rotation if input is not (0, 0)
			if (speedWorldVector.magnitude > Mathf.Epsilon) {
				Vector3 speedWorldDirection = speedWorldVector.normalized;
				if (speedWorldDirection == Vector3.zero) {
					return;
				}

				Player_.Rigidbody.MoveRotation(Quaternion.LookRotation(speedWorldDirection));
			}
		}
	}
}