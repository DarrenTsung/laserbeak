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

		private const float kEmissionDuration = 0.3f;

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

			TakeDamage();
			ObjectPoolManager.Recycle(laser.gameObject);
		}

		private void TakeDamage() {
			health_--;

			if (health_ <= 0) {
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