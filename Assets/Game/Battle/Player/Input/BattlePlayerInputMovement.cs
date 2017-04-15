using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputMovement : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kPlayerSpeed = 4.8f;

		private void FixedUpdate() {
			if (!Enabled) {
				return;
			}

			Vector2 deltaPosition = InputDelegate_.MovementVector * Time.fixedDeltaTime * kPlayerSpeed * Player_.WeightedRatio();
			Vector3 deltaWorldPosition = deltaPosition.Vector3XZValue();

			Player_.Rigidbody.MovePosition(Player_.Rigidbody.position + deltaWorldPosition);

			// snap rotation if input is not (0, 0)
			if (deltaWorldPosition.magnitude > Mathf.Epsilon) {
				Player_.Rigidbody.MoveRotation(Quaternion.LookRotation(deltaWorldPosition.normalized));
			}
		}
	}
}