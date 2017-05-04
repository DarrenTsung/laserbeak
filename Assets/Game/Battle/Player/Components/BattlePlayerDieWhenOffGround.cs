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
	public class BattlePlayerDieWhenOffGround : BattlePlayerComponent, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void PauseCheckingDeath() {
			checkDeath_ = false;
		}

		public void ResumeCheckingDeath() {
			checkDeath_ = true;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			dustParticleSystem_.SetEmissionRateOverDistance(kDustParticleEmissionRate);
			enabled_ = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			enabled_ = false;
			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}
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
		private CoroutineWrapper coroutine_;
		private bool checkDeath_ = true;

		private void Awake() {
			kLayerMask = LayerMask.GetMask("Platforms");
		}

		private void OnDrawGizmos() {
			Gizmos.DrawWireSphere(GetRaycastPosition() + Vector3.up, 0.1f);
		}

		private void FixedUpdate() {
			if (!enabled_) {
				return;
			}

			int resultCount = Physics.RaycastNonAlloc(new Ray(GetRaycastPosition(), -Vector3.up), results_, maxDistance: kPenetrationLength, layerMask: kLayerMask);
			if (checkDeath_ && resultCount <= 0) {
				enabled_ = false;
				Player_.InputController.DisableInput(BattlePlayerInputController.PriorityKey.OffGround);
				Player_.Rigidbody.constraints = RigidbodyConstraints.None;
				Player_.Rigidbody.drag = 0.0f;
				dustParticleSystem_.SetEmissionRateOverDistance(0.0f);
				coroutine_ = CoroutineWrapper.DoAfterDelay(kDeathDelay, () => {
					Player_.Health.Kill();
				});
			}
		}

		private Vector3 GetRaycastPosition() {
			return this.transform.position - Vector3.ClampMagnitude(Player_.Rigidbody.velocity, 0.2f);
		}
	}
}