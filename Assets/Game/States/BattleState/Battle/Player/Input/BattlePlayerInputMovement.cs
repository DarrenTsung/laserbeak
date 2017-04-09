using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputMovement : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kPlayerSpeed = 4.8f;

		[SerializeField]
		private Rigidbody rigidbody_;

		private void FixedUpdate() {
			if (!Enabled) {
				return;
			}

			Vector2 deltaPosition = InputDevice_.LeftStick.Value * Time.fixedDeltaTime * kPlayerSpeed * Player_.WeightedRatio();
			Vector3 deltaWorldPosition = deltaPosition.Vector3XZValue();

			rigidbody_.MovePosition(rigidbody_.position + deltaWorldPosition);

			// snap rotation if input is not (0, 0)
			if (deltaWorldPosition.magnitude > Mathf.Epsilon * 2.0f) {
				rigidbody_.MoveRotation(Quaternion.LookRotation(deltaWorldPosition));
			}
		}
	}
}