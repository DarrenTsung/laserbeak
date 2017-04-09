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
		private const int kBaseHealth = 1;

		private const float kExplosionForce = 500.0f;
		private const float kExplosionRadius = 6.0f;
		private const float kEmissionDuration = 0.3f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject playerPartsPrefab_;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private int health_;

		private Material[] emissiveMaterials_;

		private Material[] EmissiveMaterials_ {
			get {
				if (emissiveMaterials_ == null) {
					emissiveMaterials_ = Player_.GetComponentsInChildren<Renderer>()
												.SelectMany(r => r.materials)
												.Where(m => m.HasProperty("_EmissionColor"))
												.ToArray();
				}
				return emissiveMaterials_;
			}
		}

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
					explosionForce *= UnityEngine.Random.Range(0.1f, 1.2f);
					rigidbody.AddExplosionForce(explosionForce * kExplosionForce, explosionPosition, kExplosionRadius, upwardsModifier: 1.0f);
				}

				ObjectPoolManager.Recycle(this);
			} else {
				AnimateDamageEmission();
			}
		}

		private void AnimateDamageEmission() {
			CoroutineWrapper.DoEaseFor(kEmissionDuration, EaseType.QuadraticEaseOut, (float percentage) => {
				float inversePercentage = 1.0f - percentage;
				foreach (Material material in EmissiveMaterials_) {
					material.SetColor("_EmissionColor", new Color(inversePercentage, inversePercentage, inversePercentage));
				}
			});
		}
	}
}