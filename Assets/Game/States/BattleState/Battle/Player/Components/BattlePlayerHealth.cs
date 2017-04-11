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

namespace DT.Game.Battle.Player {
	public class BattlePlayerHealth : BattlePlayerComponent, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			health_ = kBaseHealth;
		}


		// PRAGMA MARK - Internal
		private const int kBaseHealth = 2;

		private const float kExplosionForce = 600.0f;
		private const float kExplosionRadius = 6.0f;

		private const float kEmissionDuration = 0.15f;
		private const float kEmissionWhiteBalance = 0.2f;

		private const float kDamageKnockbackDuration = 0.3f;
		private const float kDamageKnockbackDistance = 2.5f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject playerPartsPrefab_;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private int health_;

		private void OnCollisionEnter(Collision collision) {
			Laser laser = collision.gameObject.GetComponent<Laser>();
			if (laser == null) {
				Debug.LogWarning("BattlePlayerHealth - unexpected collision: " + collision.gameObject.FullName());
				return;
			}

			Vector3 forward = laser.transform.forward;

			TakeDamage(forward);
			ObjectPoolManager.Recycle(laser.gameObject);
		}

		private void TakeDamage(Vector3 forward) {
			health_--;

			if (health_ <= 0) {
				GameObject playerParts = ObjectPoolManager.Create(playerPartsPrefab_, position: this.transform.position);

				Vector3 explosionPosition = this.transform.position - (forward.normalized * kExplosionRadius / 4.0f);
				foreach (Rigidbody rigidbody in playerParts.GetComponentsInChildren<Rigidbody>()) {
					float distance = (rigidbody.position - explosionPosition).magnitude;
					float explosionForce = Mathf.Clamp(1.0f - (distance / kExplosionForce), 0.0f, 1.0f);
					explosionForce *= UnityEngine.Random.Range(0.1f, 1.3f);
					rigidbody.AddExplosionForce(explosionForce * kExplosionForce, explosionPosition, kExplosionRadius, upwardsModifier: 1.0f);
				}

				foreach (Renderer renderer in playerParts.GetComponentsInChildren<Renderer>()) {
					renderer.material = Player_.Skin.BodyMaterial;
				}

				AnimateDamageEmissionFor(GetEmissiveMaterialsFor(playerParts));

				ObjectPoolManager.Recycle(this);
			} else {
				AnimateDamageEmissionFor(GetEmissiveMaterialsFor(Player_.gameObject));
				Knockback(forward);
			}
		}

		private void AnimateDamageEmissionFor(Material[] materials) {
			CoroutineWrapper.DoEaseFor(kEmissionDuration, EaseType.QuadraticEaseOut, (float percentage) => {
				float inversePercentage = 1.0f - percentage;
				float whiteBalance = inversePercentage * kEmissionWhiteBalance;
				foreach (Material material in materials) {
					material.SetColor("_EmissionColor", new Color(whiteBalance, whiteBalance, whiteBalance));
				}
			});
		}

		private Material[] GetEmissiveMaterialsFor(GameObject gameObject) {
			return gameObject.GetComponentsInChildren<Renderer>()
							.SelectMany(r => r.materials)
							.Where(m => m.HasProperty("_EmissionColor"))
							.ToArray();
		}

		private void Knockback(Vector3 forward) {
			Vector3 endPosition = Player_.Rigidbody.position + (kDamageKnockbackDistance * forward);
			Player_.InputController.MoveTo(Player_, endPosition, kDamageKnockbackDuration, EaseType.CubicEaseOut);
		}
	}
}