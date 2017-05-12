using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputChargedLaser : BattlePlayerInputComponent {
		// PRAGMA MARK - Public Interface
		public static event Action<BattlePlayer> OnFullyCharged = delegate {};

		public event Action OnFullCharge = delegate {};

		public bool FullyCharged {
			get { return chargedTime_ >= kChargeTime; }
		}


		// PRAGMA MARK - Internal
		private const float kChargeTime = 0.35f;

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
		private FullyChargedParticle fullyChargedParticle_ = null;

		protected override void Cleanup() {
			chargedTime_ = 0.0f;
			chargingLaserContainer_.transform.RecycleAllChildren();
			chargingLaser_ = null;
			fullyChargedParticle_ = null;
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

			if (InputDelegate_.LaserPressed && InGameConstants.IsAllowedToChargeLasers(Player_)) {
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
				chargingLaser_.SetColor(Player_.Skin.LaserColor, Player_.Skin.LaserMaterial);
			}

			if (chargingLaser_ != null) {
				chargingLaser_.UpdateWithPercentage(percentCharged);
				if (!Mathf.Approximately(previousPercentCharged, 1.0f) && Mathf.Approximately(percentCharged, 1.0f)) {
					fullyChargedParticle_ = ObjectPoolManager.Create<FullyChargedParticle>(fullyChargedParticlePrefab_, parent: chargingLaserContainer_);
					fullyChargedParticle_.SetColor(Player_.Skin.LaserColor);

					OnFullyCharged.Invoke(Player_);
					OnFullCharge.Invoke();
				}
			}

			UpdateWeightModification();
		}

		private void ShootLaser() {
			Laser laser = ObjectPoolManager.Create<Laser>(laserPrefab_, position: chargingLaserContainer_.transform.position, rotation: chargingLaserContainer_.transform.rotation, parent: BattleRecyclables.Instance);
			laser.Init(Player_);
			Recoil();
			DisperseFullyChargedParticle();
		}

		private void DisperseFullyChargedParticle() {
			if (fullyChargedParticle_ != null) {
				fullyChargedParticle_.Disperse();
				fullyChargedParticle_ = null;
			}
		}

		private void Recoil() {
			Vector3 endPosition = Player_.Rigidbody.position - (kRecoilDistance * this.transform.forward);
			Controller_.MoveTo(Player_, endPosition, kRecoilDuration, EaseType.CubicEaseOut);
		}
	}
}