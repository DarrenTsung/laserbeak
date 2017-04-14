using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputDash : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kDashCooldown = 2.0f;

		private const float kDashIntentThreshold = 0.3f;

		private const float kDashDuration = 0.15f;
		private const float kDashDistance = 4.0f;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private float cooldownTimer_ = 0.0f;

		private void Update() {
			cooldownTimer_ -= Time.deltaTime;

			if (!Enabled) {
				return;
			}

			if (cooldownTimer_ > 0.0f) {
				return;
			}

			if (InputDelegate_.DashPressed) {
				Vector3 direction = InputDelegate_.MovementVector.Vector3XZValue().normalized;
				if (direction.magnitude > kDashIntentThreshold) {
					Dash(direction);
					cooldownTimer_ = kDashCooldown;
				}
			}
		}

		private void Dash(Vector3 direction) {
			Vector3 endPosition = Player_.Rigidbody.position + (kDashDistance * Player_.WeightedRatio() * direction);
			Controller_.MoveTo(Player_, endPosition, kDashDuration, EaseType.CubicEaseOut);
		}
	}
}