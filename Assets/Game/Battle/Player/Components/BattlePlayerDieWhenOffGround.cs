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
		private const float kCheckRadius = 0.33f;
		private const int kCheckSampleCount = 5;

		private const float kPenetrationLength = 0.3f;

		private const float kDeathDelay = 1.5f;

		private const float kDustParticleEmissionRate = 1.5f;

		[Header("Outlets")]
		[SerializeField]
		private ParticleSystem dustParticleSystem_;

		private RaycastHit[] results_ = new RaycastHit[10];
		private bool enabled_ = true;
		private CoroutineWrapper coroutine_;
		private bool checkDeath_ = true;

		private Vector3[] cachedOffsets_;
		private Vector3[] CachedOffsets_ {
			get {
				if (cachedOffsets_ == null) {
					cachedOffsets_ = new Vector3[kCheckSampleCount];
					for (int i = 0; i < kCheckSampleCount; i++) {
						float p = i / (float)kCheckSampleCount;
						Vector3 offset = Quaternion.Euler(0, 360.0f * p, 0) * Vector3.right * kCheckRadius;
						cachedOffsets_[i] = offset;
					}
				}
				return cachedOffsets_;
			}
		}

		private void OnDrawGizmos() {
			if (Application.isPlaying) {
				Color oldColor = Gizmos.color;

				foreach (Vector3 position in GetRaycastPositions()) {
					bool collidesWithGround = CollisionsAtPosition(position) > 0;
					Gizmos.color = collidesWithGround ? Color.green : Color.red;
					Gizmos.DrawWireSphere(position + Vector3.up, 0.05f);
				}

				Gizmos.color = oldColor;
			}
		}

		private void FixedUpdate() {
			if (!enabled_) {
				return;
			}

			int resultCount = GetRaycastPositions().Sum(pos => CollisionsAtPosition(pos));
			if (checkDeath_ && resultCount <= 0) {
				enabled_ = false;

				GameNotifications.OnBattlePlayerFellOffGround.Invoke(Player_);

				Player_.InputController.DisableInput(BattlePlayerInputController.PriorityKey.OffGround);
				Player_.Rigidbody.constraints = RigidbodyConstraints.None;
				Player_.Rigidbody.drag = 0.0f;
				dustParticleSystem_.SetEmissionRateOverDistance(0.0f);
				coroutine_ = CoroutineWrapper.DoAfterDelay(kDeathDelay, () => {
					Player_.Health.Kill();
				});
			}
		}

		private int CollisionsAtPosition(Vector3 position) {
			return Physics.RaycastNonAlloc(new Ray(position, -Vector3.up), results_, maxDistance: kPenetrationLength, layerMask: InGameConstants.PlatformsLayerMask);
		}

		private IEnumerable<Vector3> GetRaycastPositions() {
			foreach (Vector3 offset in CachedOffsets_) {
				yield return this.transform.position + offset;
			}
		}
	}
}