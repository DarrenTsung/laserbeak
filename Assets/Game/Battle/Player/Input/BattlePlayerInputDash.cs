using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputDash : BattlePlayerInputComponent {
		// PRAGMA MARK - Static
		public static int DashDamage = 0;

		public static event Action<BattlePlayer> OnPlayerDash = delegate {};


		// PRAGMA MARK - Public Interface
		public const float kDashDuration = 0.15f;

		public event Action OnDash = delegate {};
		public event Action OnDashCancelled = delegate {};


		// PRAGMA MARK - Internal
		private const float kDashCooldown = 1.0f;

		private const float kDashIntentThreshold = 0.3f;
		private const float kDashDistance = 4.0f;

		[Header("Outlets")]
		[SerializeField]
		private Collider dashCollider_;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private float cooldownTimer_ = 0.0f;

		protected override void Initialize() {
			dashCollider_.enabled = false;
		}

		protected override void Cleanup() {
			this.StopAllCoroutines();
			dashCollider_.enabled = false;
		}

		private void Update() {
			cooldownTimer_ -= Time.deltaTime;

			if (!Enabled) {
				return;
			}

			if (cooldownTimer_ > 0.0f) {
				return;
			}

			if (InputDelegate_.DashPressed && InGameConstants.AllowBattlePlayerMovement) {
				Vector3 direction = InputDelegate_.MovementVector.Vector3XZValue().normalized;
				if (direction.magnitude > kDashIntentThreshold) {
					Dash(direction);
					cooldownTimer_ = kDashCooldown;
				}
			}
		}

		private void OnTriggerEnter(Collider collider) {
			if (Player_ == null) {
				return;
			}

			BattlePlayer battlePlayer = collider.gameObject.GetComponentInParent<BattlePlayer>();
			if (battlePlayer == null) {
				Debug.LogWarning("Dash collider colliding with unknown object: " + collider.gameObject.FullName());
				return;
			}

			if (battlePlayer == Player_) {
				return;
			}

			Vector3 forward = Player_.Rigidbody.velocity;
			forward = forward.normalized;
			battlePlayer.Health.TakeDamage(DashDamage, forward);
			Player_.Health.TakeDamage(0, -forward);

			dashCollider_.enabled = false;
			OnDashCancelled.Invoke();
		}

		private void Dash(Vector3 direction) {
			dashCollider_.enabled = true;
			this.DoAfterDelay(kDashDuration, () => {
				dashCollider_.enabled = false;
			});

			Vector3 endPosition = Player_.Rigidbody.position + (kDashDistance * Player_.WeightedRatio() * direction);
			Controller_.MoveTo(Player_, endPosition, kDashDuration, EaseType.CubicEaseOut);
			AudioConstants.Instance.Dash.PlaySFX();
			OnDash.Invoke();
			OnPlayerDash.Invoke(Player_);
		}
	}
}