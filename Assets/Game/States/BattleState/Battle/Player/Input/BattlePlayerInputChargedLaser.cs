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

		private const float kPlayerAddedWeight = 0.8f;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject chargingLaserPrefab_;

		[SerializeField]
		private GameObject laserPrefab_;

		[SerializeField]
		private GameObject fullyChargedParticlePrefab_;

		[Header("Outlets")]
		[SerializeField]
		private GameObject chargingLaserContainer_;

		[Header("Properties")]
		[SerializeField, ReadOnly]
		private float chargedTime_ = 0.0f;

		private ChargingLaser chargingLaser_ = null;

		protected override void Cleanup() {
			chargedTime_ = 0.0f;
			chargingLaserContainer_.transform.RecycleAllChildren();
			chargingLaser_ = null;
		}

		private void UpdateWeightModification() {
			if (Player_ != null) {
				Player_.SetWeightModification(this, chargedTime_ > 0.0f ? kPlayerAddedWeight : 0.0f);
			}
		}

		private void Update() {
			if (!Enabled) {
				UpdateWeightModification();
				return;
			}

			if (!InputDelegate_.LaserPressed && chargedTime_ >= kChargeTime) {
				ShootLaser();
				chargedTime_ = 0.0f;
			}

			float previousPercentCharged = chargedTime_ / kChargeTime;

			if (InputDelegate_.LaserPressed) {
				chargedTime_ += Time.deltaTime * kChargeRate;
			} else {
				chargedTime_ -= Time.deltaTime * kDischargeRate;
			}

			chargedTime_ = Mathf.Clamp(chargedTime_, 0.0f, kChargeTime);
			float percentCharged = chargedTime_ / kChargeTime;
			if (percentCharged <= 0.0f && chargingLaser_ != null) {
				ObjectPoolManager.Recycle(chargingLaser_);
				chargingLaser_ = null;
			} else if (percentCharged > 0.0f && chargingLaser_ == null) {
				chargingLaser_ = ObjectPoolManager.Create<ChargingLaser>(chargingLaserPrefab_, parent: chargingLaserContainer_);
				chargingLaser_.SetLaserMaterial(Player_.Skin.LaserMaterial);
			}

			if (chargingLaser_ != null) {
				chargingLaser_.UpdateWithPercentage(percentCharged);
				if (previousPercentCharged != 1.0f && percentCharged == 1.0f) {
					var fullyChargedParticle = ObjectPoolManager.Create<FullyChargedParticle>(fullyChargedParticlePrefab_, parent: chargingLaserContainer_);
					fullyChargedParticle.SetColor(Player_.Skin.LaserMaterial.GetColor("_EmissionColor"));
				}
			}

			UpdateWeightModification();
		}

		private void ShootLaser() {
			Laser laser = ObjectPoolManager.Create<Laser>(laserPrefab_, position: chargingLaserContainer_.transform.position, rotation: chargingLaserContainer_.transform.rotation);
			laser.SetMaterial(Player_.Skin.LaserMaterial);
			Recoil();
		}

		private void Recoil() {
			Vector3 endPosition = Player_.Rigidbody.position - (kRecoilDistance * this.transform.forward);
			Controller_.MoveTo(Player_, endPosition, kRecoilDuration, EaseType.CubicEaseOut);
		}
	}
}