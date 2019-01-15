using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Walls;

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

			CheckCollisionWithPlayer(collider);
			CheckCollisionWithWall(collider);
		}

		private void CheckCollisionWithWall(Collider collider) {
			Wall wall = collider.gameObject.GetComponentInParent<Wall>();
			if (wall == null) {
				return;
			}

			Player_.Health.Knockback(forward: -collider.transform.right);

			dashCollider_.enabled = false;
			OnDashCancelled.Invoke();
		}

		private void CheckCollisionWithPlayer(Collider collider) {
			BattlePlayer battlePlayer = collider.gameObject.GetComponentInParent<BattlePlayer>();
			if (battlePlayer == null) {
				return;
			}

			if (battlePlayer == Player_) {
				return;
			}

			Vector3 forward = Player_.Rigidbody.velocity;
			forward = forward.normalized;
			battlePlayer.Health.TakeDamage(DashDamage, forward, damageSource: Player_);
			Player_.Health.Knockback(-forward);

			GameNotifications.OnBattlePlayerDashHit.Invoke(battlePlayer, Player_);

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