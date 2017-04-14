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
	public class BattlePlayerDieWhenOffGround : BattlePlayerComponent, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			dustParticleSystem_.SetEmissionRateOverDistance(kDustParticleEmissionRate);
			enabled_ = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			enabled_ = false;
		}


		// PRAGMA MARK - Internal
		private const float kPenetrationLength = 0.3f;
		private static int kLayerMask;

		private const float kDeathDelay = 1.5f;

		private const float kDustParticleEmissionRate = 1.0f;

		[Header("Outlets")]
		[SerializeField]
		private ParticleSystem dustParticleSystem_;

		private RaycastHit[] results_ = new RaycastHit[10];
		private bool enabled_ = true;

		private void Awake() {
			kLayerMask = LayerMask.GetMask("Platforms");
		}

		private void FixedUpdate() {
			if (!enabled_) {
				return;
			}

			int resultCount = Physics.RaycastNonAlloc(new Ray(this.transform.position, -Vector3.up), results_, maxDistance: kPenetrationLength, layerMask: kLayerMask);
			if (resultCount <= 0) {
				enabled_ = false;
				Player_.InputController.DisableInput(BattlePlayerInputController.PriorityKey.OffGround);
				Player_.Rigidbody.isKinematic = false;
				dustParticleSystem_.SetEmissionRateOverDistance(0.0f);
				CoroutineWrapper.DoAfterDelay(kDeathDelay, () => {
					// kill self
					Player_.Health.TakeDamage(BattlePlayerHealth.kMaxDamage, Player_.Rigidbody.velocity);
				});
			}
		}
	}
}