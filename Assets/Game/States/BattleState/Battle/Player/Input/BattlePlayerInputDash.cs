using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputDash : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kDashCooldown = 2.0f;

		private const float kDashIntentThreshold = 0.3f;

		private const float kDashDuration = 0.15f;
		private const float kDashDistance = 4.0f;

		[Header("Outlets")]
		[SerializeField]
		private Rigidbody rigidbody_;

		[SerializeField]
		private InputControlType controlType_ = InputControlType.Action1;

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

			InputControl control = InputDevice_.GetControl(controlType_);
			if (control.WasPressed) {
				Vector3 direction = InputDevice_.LeftStick.Value.Vector3XZValue().normalized;
				if (direction.magnitude > kDashIntentThreshold) {
					Dash(direction);
					cooldownTimer_ = kDashCooldown;
				}
			}
		}

		private void Dash(Vector3 direction) {
			Controller_.DisableInput();
			Vector3 startPosition = rigidbody_.position;
			Vector3 endPosition = rigidbody_.position + (kDashDistance * direction);
			CoroutineWrapper.DoLerpFor(kDashDuration, (float p) => {
				rigidbody_.MovePosition(Vector3.Lerp(startPosition, endPosition, p));
			}, () => {
				Controller_.EnableInput();
			});
		}
	}
}