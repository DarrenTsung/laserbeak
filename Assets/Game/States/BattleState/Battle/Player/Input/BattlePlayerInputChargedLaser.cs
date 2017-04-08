using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputChargedLaser : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kChargeTime = 0.85f;

		private const float kRecoilDistance = 3.0f;
		private const float kRecoilDuration = 0.4f;

		private const float kChargeRate = 1.0f;
		private const float kDischargeRate = 2.0f;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject chargingLaserPrefab_;

		[SerializeField]
		private GameObject laserPrefab_;

		[Header("Outlets")]
		[SerializeField]
		private Rigidbody rigidbody_;

		[SerializeField]
		private GameObject chargingLaserContainer_;

		[Header("Properties")]
		[SerializeField]
		private InputControlType controlType_ = InputControlType.Action3;

		[SerializeField, ReadOnly]
		private float chargedTime_ = 0.0f;

		private ChargingLaser chargingLaser_ = null;

		private void Update() {
			if (!Enabled) {
				return;
			}

			InputControl control = InputDevice_.GetControl(controlType_);
			if (control.IsPressed) {
				chargedTime_ += Time.deltaTime * kChargeRate;
			} else {
				chargedTime_ -= Time.deltaTime * kDischargeRate;
			}

			chargedTime_ = Mathf.Clamp(chargedTime_, 0.0f, kChargeTime);
			float percentCharged = chargedTime_ / kChargeTime;

			if (percentCharged >= 1.0f) {
				ShootLaser();
				chargedTime_ = 0.0f;
				percentCharged = 0.0f;
			}

			if (percentCharged <= 0.0f && chargingLaser_ != null) {
				ObjectPoolManager.Recycle(chargingLaser_);
				chargingLaser_ = null;
			} else if (percentCharged > 0.0f && chargingLaser_ == null) {
				chargingLaser_ = ObjectPoolManager.Create<ChargingLaser>(chargingLaserPrefab_, parent: chargingLaserContainer_);
			}

			if (chargingLaser_ != null) {
				chargingLaser_.UpdateWithPercentage(percentCharged);
			}
		}

		private void ShootLaser() {
			Laser laser = ObjectPoolManager.Create<Laser>(laserPrefab_);
			laser.transform.position = chargingLaserContainer_.transform.position;
			laser.transform.rotation = chargingLaserContainer_.transform.rotation;

			Recoil();
		}

		private void Recoil() {
			Controller_.DisableInput();
			Vector3 startPosition = rigidbody_.position;
			Vector3 endPosition = rigidbody_.position - (kRecoilDistance * this.transform.forward);
			CoroutineWrapper.DoEaseFor(kRecoilDuration, EaseType.CubicEaseOut, (float p) => {
				rigidbody_.MovePosition(Vector3.Lerp(startPosition, endPosition, p));
			}, () => {
				Controller_.EnableInput();
			});
		}
	}
}