using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerHealth : BattlePlayerComponent, IRecycleSetupSubscriber {
		// PRAGMA MARK - Static
		public static float KnockbackMultiplier = 1.0f;
		public static int LaserDamage = 1;

		public static event Action<BattlePlayer, int> OnBattlePlayerDamaged = delegate {};
		public static event Action<BattlePlayer> OnBattlePlayerDied = delegate {};


		// PRAGMA MARK - Public Interface
		public object KnockbackDamageSource {
			get { return knockbackDamageSource_; }
		}

		public void Kill() {
			invulnerable_ = false;
			TakeDamage(kMaxDamage, forward: Vector3.zero);
		}

		public void TakeDamage(int damage, Vector3 forward, object damageSource = null) {
			if (health_ <= 0) {
				return;
			}

			if (invulnerable_) {
				return;
			}

			health_ -= damage;
			OnBattlePlayerDamaged.Invoke(Player_, damage);

			if (health_ <= 0) {
				GameObject playerParts = ObjectPoolManager.Create(playerPartsPrefab_, this.transform.position, Quaternion.identity, parent: BattleRecyclables.Instance);

				// NOTE (darren): remove any negative y component from damage forward vector
				// since gravity + explosive downwards force looks crazy
				Vector3 explosionForward = forward.SetY(Mathf.Max(forward.y, 0.0f));
				Vector3 explosionPosition = this.transform.position - (explosionForward.normalized * kExplosionRadius / 4.0f);
				foreach (Rigidbody rigidbody in playerParts.GetComponentsInChildren<Rigidbody>()) {
					float distance = (rigidbody.position - explosionPosition).magnitude;
					float explosionForce = Mathf.Clamp(1.0f - (distance / kExplosionForce), 0.0f, 1.0f);
					explosionForce *= UnityEngine.Random.Range(0.1f, 1.3f);
					rigidbody.AddExplosionForce(explosionForce * kExplosionForce, explosionPosition, kExplosionRadius, upwardsModifier: 1.0f);
				}

				// Animate single material to batch
				SetEmissiveMaterialsFor(playerParts, Player_.Skin.BodyPartMaterial);

				Material[] partMaterialArray = new Material[] { Player_.Skin.BodyPartMaterial };
				AnimateDamageEmissionFor(partMaterialArray);
				AudioConstants.Instance.PlayerDeath.PlaySFX();
				BattleCamera.Shake(1.0f);
				BattleCamera.StopTimeForKill();
				OnBattlePlayerDied.Invoke(Player_);
				if (damageSource != null) {
					GameNotifications.OnBattlePlayerDiedWithSource.Invoke(Player_, damageSource);
				}

				ObjectPoolManager.Recycle(this);
			} else {
				float multiplier = 1.0f;
				if (damage > 0) {
					AnimateShieldHit(hideShieldAfterwards: health_ <= 1);
					// AnimateDamageEmissionFor(Player_.BodyRenderers.Select(r => r.sharedMaterial));
					multiplier = 0.5f;
				}

				Knockback(forward, damageSource);
				AudioConstants.Instance.PlayerHurt.PlaySFX(volumeScale: multiplier);
				BattleCamera.Shake(0.55f * multiplier);
			}
		}

		public bool HandleCollisions {
			get { return handleCollisions_; }
			set { handleCollisions_ = value; }
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			HandleCollisions = true;
			health_ = kBaseHealth;

			Player_.ShieldRenderer.enabled = true;
		}


		// PRAGMA MARK - Internal
		private const int kMaxDamage = 999;
		private const int kBaseHealth = 2;

		private const float kExplosionForce = 600.0f;
		private const float kExplosionRadius = 6.0f;

		private const float kEmissionDuration = 0.15f;
		private const float kEmissionWhiteBalance = 0.2f;

		private const float kShieldAnimateDuration = 0.3f;
		private const float kShieldAlphaMax = 1.0f;

		private const float kDamageKnockbackDuration = 0.3f;
		private const float kDamageKnockbackDistance = 2.5f;

		private const float kClearLaserHitTime = 0.1f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject playerPartsPrefab_;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private int health_;

		[SerializeField, ReadOnly]
		private bool invulnerable_;

		[SerializeField, ReadOnly]
		private bool handleCollisions_ = true;

		private CoroutineWrapper invulnerableCoroutine_;
		private CoroutineWrapper knockbackClearCoroutine_;
		private object knockbackDamageSource_;

		private readonly HashSet<Laser> lasersHit_ = new HashSet<Laser>();

		private void OnTriggerEnter(Collider collider) {
			if (!HandleCollisions) {
				return;
			}

			Laser laser = collider.gameObject.GetComponentInParent<Laser>();
			if (laser == null) {
				return;
			}

			if (lasersHit_.Contains(laser)) {
				return;
			}

			lasersHit_.Add(laser);
			CoroutineWrapper.DoAfterDelay(kClearLaserHitTime, () => {
				lasersHit_.Remove(laser);
			});

			int damage = LaserDamage;
			// laser only does damage if not on same team
			// HACK (darren): BattlePlayer needs to be refactored in things like Laser / Teams
			// because BattlePlayer can be recycled it's not a good thing to keep track of
			// should instead make a HashCode-like thing
			if (laser.BattlePlayer != null && BattlePlayerTeams.AreOnSameTeam(laser.BattlePlayer, Player_)) {
				damage = 0;
			}

			GameNotifications.OnBattlePlayerLaserHit.Invoke(laser, Player_);

			Vector3 forward = laser.transform.forward;

			TakeDamage(damage, forward, damageSource: laser);
			laser.HandleHit();
		}

		private void AnimateDamageEmissionFor(IEnumerable<Material> materials) {
			CoroutineWrapper.DoEaseFor(kEmissionDuration, EaseType.QuadraticEaseOut, (float percentage) => {
				float inversePercentage = 1.0f - percentage;
				float whiteBalance = inversePercentage * kEmissionWhiteBalance;
				foreach (Material material in materials) {
					material.SetColor("_EmissionColor", new Color(whiteBalance, whiteBalance, whiteBalance));
				}
			});
		}

		private void AnimateShieldHit(bool hideShieldAfterwards = false) {
			Color baseColor = Player_.ShieldRenderer.material.GetColor("_Color");
			CoroutineWrapper.DoEaseFor(kShieldAnimateDuration, EaseType.QuadraticEaseOut, (float percentage) => {
				float inversePercentage = 1.0f - percentage;
				float alpha = Mathf.Lerp(GameConstants.Instance.PlayerShieldAlphaMin, kShieldAlphaMax, inversePercentage);
				Color newColor = baseColor.WithAlpha(alpha);
				Player_.ShieldRenderer.material.SetColor("_Color", newColor);
			}, () => {
				if (hideShieldAfterwards) {
					Player_.ShieldRenderer.enabled = false;
				}
			});
		}

		private void SetEmissiveMaterialsFor(GameObject gameObject, Material material) {
			foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>()) {
				renderer.sharedMaterial = material;
			}
		}

		private void Knockback(Vector3 forward, object damageSource = null) {
			if (knockbackClearCoroutine_ != null) {
				knockbackClearCoroutine_.Cancel();
				knockbackClearCoroutine_ = null;
			}

			knockbackDamageSource_ = damageSource;
			Vector3 endPosition = Player_.Rigidbody.position + (KnockbackMultiplier * kDamageKnockbackDistance * forward);
			Player_.InputController.MoveTo(Player_, endPosition, kDamageKnockbackDuration * KnockbackMultiplier, EaseType.CubicEaseOut, onFinishedCallback: () => {
				// why do we have another delay? Because dashing pauses checking if off ground,
				// we need to persist the source of knockback for a little bit
				knockbackClearCoroutine_ = CoroutineWrapper.DoAfterDelay(0.1f, () => {
					knockbackDamageSource_ = null;
				});
			});
		}

		private void SetInvulnerableFor(float time) {
			if (invulnerableCoroutine_ != null) {
				invulnerableCoroutine_.Cancel();
				invulnerableCoroutine_ = null;
			}

			invulnerable_ = true;
			invulnerableCoroutine_ = CoroutineWrapper.DoAfterDelay(time, () => {
				invulnerable_ = false;
			});
		}
	}
}