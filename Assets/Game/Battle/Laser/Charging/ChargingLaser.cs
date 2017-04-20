using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;

namespace DT.Game.Battle.Lasers {
	public class ChargingLaser : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void UpdateWithPercentage(float percentage) {
			this.transform.localScale = new Vector3(percentage, percentage, percentage);
			pointLight_.range = percentage * kLightRange;

			chargingParticleSystem_.SetEmissionRateOverTime(Mathf.Lerp(kMaxParticleRateOverTime, 0.0f, percentage));

			humAudioSource_.volume = Easings.CubicEaseOut(percentage) * kHumMaxVolume;
		}

		public void SetLaserMaterial(Material laserMaterial) {
			laserRenderer_.material = laserMaterial;

			Color laserColor = laserMaterial.GetColor("_EmissionColor");
			pointLight_.color = laserColor;

			chargingParticleSystem_.GetComponent<ParticleSystemRenderer>().material = laserMaterial;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			enabled_ = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			this.transform.localScale = Vector3.zero;
			pointLight_.range = 0.0f;
			chargingParticleSystem_.SetEmissionRateOverTime(0.0f);
			battlePlayer_ = null;

			enabled_ = false;
		}


		// PRAGMA MARK - Internal
		private const float kMaxParticleRateOverTime = 35.0f;
		private const float kLightRange = 4.0f;

		private const float kHumMaxVolume = 0.3f;

		private const float kPitchMaxDelta = 0.40f;
		private const float kPitchAngleChange = 30.0f;

		[Header("Outlets")]
		[SerializeField]
		private Light pointLight_;

		[SerializeField]
		private ParticleSystem chargingParticleSystem_;

		[SerializeField]
		private Renderer laserRenderer_;

		[SerializeField]
		private AudioSource humAudioSource_;

		private Quaternion? previousRotation_ = null;
		private bool enabled_;

		private BattlePlayer battlePlayer_;
		private BattlePlayer BattlePlayer_ {
			get {
				if (battlePlayer_ == null) {
					battlePlayer_ = this.GetComponentInParent<BattlePlayer>();
				}
				return battlePlayer_;
			}
		}

		private void Update() {
			if (!enabled_) {
				return;
			}

			Quaternion currentRotation = BattlePlayer_.transform.localRotation;
			Quaternion previousRotation = currentRotation;
			if (previousRotation_ != null) {
				previousRotation = (Quaternion)previousRotation_;
			}

			float angleDifference = Quaternion.Angle(currentRotation, previousRotation);
			// use sine ease out so needs to be more than just little movement to change pitch
			float p = Easings.SineEaseOut(angleDifference / kPitchAngleChange);
			humAudioSource_.pitch = Mathf.Lerp(1.0f, 1.0f + kPitchMaxDelta, p);

			previousRotation_ = currentRotation;
		}
	}
}