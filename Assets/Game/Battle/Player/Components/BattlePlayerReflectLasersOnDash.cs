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
	public class BattlePlayerReflectLasersOnDash : BattlePlayerComponent, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			reflect_ = false;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			CancelReflectCoroutine();
		}


		// PRAGMA MARK - Internal
		// only the first XX% of the dash time actually reflects the laser
		private const float kDashPercentageReflect = 0.66f;

		[Header("Outlets")]
		[SerializeField]
		private BattlePlayerInputDash dashInput_;

		private CoroutineWrapper reflectCoroutine_;
		private bool reflect_ = false;

		private void Awake() {
			dashInput_.OnDash += HandleDash;
		}

		private void OnTriggerEnter(Collider collider) {
			if (!reflect_) {
				return;
			}

			Laser laser = collider.gameObject.GetComponentInParent<Laser>();
			if (laser == null) {
				return;
			}

			// reflect laser back to original shooter
			laser.transform.LookAt(laser.BattlePlayer.transform);

			laser.SpeedMultiplier *= 1.3f;
			AudioConstants.Instance.LaserShoot.PlaySFX(volumeScale: 0.5f);
			laser.HandleHit(destroy: false);
			laser.ChangeBattlePlayerSource(Player_);
		}

		private void HandleDash() {
			float reflectTime = kDashPercentageReflect * BattlePlayerInputDash.kDashDuration;
			SetReflectiveFor(reflectTime);
		}

		private void SetReflectiveFor(float time) {
			CancelReflectCoroutine();

			reflect_ = true;
			Player_.Health.HandleCollisions = false;

			reflectCoroutine_ = CoroutineWrapper.DoAfterDelay(time, () => {
				reflect_ = false;
				Player_.Health.HandleCollisions = true;
			});
		}

		private void CancelReflectCoroutine() {
			if (reflectCoroutine_ != null) {
				reflectCoroutine_.Cancel();
				reflectCoroutine_ = null;
			}
		}
	}
}