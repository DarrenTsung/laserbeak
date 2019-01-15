using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class RotatingLaser : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			enabled_ = false;
			rotationDirection_ = RotationDirection.CLOCKWISE;
			GameModeIntroView.OnIntroFinishedPossibleMock += HandleIntroFinished;
			Reset();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			GameModeIntroView.OnIntroFinishedPossibleMock -= HandleIntroFinished;
		}


		// PRAGMA MARK - Internal
		private const float kBaseRotationSpeed = 20.0f;
		private const float kMaxRotationSpeedTime = 20.0f; // 20 seconds
		private const float kMaxRotationSpeed = 50.0f;

		private const float kSwitchTimeMin_ = 5.0f;
		private const float kSwitchTimeMax_ = 7.0f;

		private const float kSwitchTimeShift_ = 4.0f;

		private const float kFakeoutChance = 0.3f;

		private const float kSwitchPulseEmissionGain = 0.7f;
		private const float kSwitchWarningEmissionGain = 0.45f;
		private const float kSwitchNormalEmissionGain = 0.3f;

		private const int kSafeSwitches = 2;
		private const float kSwitchWarningDuration = 1.0f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject rotationContainer_;
		[SerializeField]
		private Renderer switchWarningRenderer_;

		[Header("Read-Only")]
		[SerializeField]
		private RotationDirection rotationDirection_;
		[SerializeField]
		private float rotationSpeed_;
		[SerializeField]
		private float timeAlive_ = 0.0f;

		[Space]
		[SerializeField]
		private float switchTimer_;

		private bool enabled_ = true;
		private int safeSwitchesLeft_ = 0;

		private void Awake() {
			Reset();
		}

		private void HandleIntroFinished() {
			GameModeIntroView.OnIntroFinishedPossibleMock -= HandleIntroFinished;

			SetEmissionGain(kSwitchWarningEmissionGain);
			this.DoAfterDelay(kSwitchWarningDuration, () => {
				SetEmissionGain(kSwitchNormalEmissionGain);
				enabled_ = true;
			});
		}

		private void Update() {
			if (!enabled_) {
				return;
			}

			timeAlive_ += Time.deltaTime;
			rotationSpeed_ = Mathf.Lerp(kBaseRotationSpeed, kMaxRotationSpeed, timeAlive_ / kMaxRotationSpeedTime);

			switchTimer_ -= Time.deltaTime;
			if (switchTimer_ <= kSwitchWarningDuration) {
				SetEmissionGain(kSwitchWarningEmissionGain);
			} else {
				SetEmissionGain(kSwitchNormalEmissionGain);
			}

			if (switchTimer_ <= 0.0f) {
				bool fakeout = RandomUtil.RandomChance(kFakeoutChance);

				if (safeSwitchesLeft_ > 0) {
					fakeout = false;
					safeSwitchesLeft_--;
				}

				if (!fakeout) {
					this.DoEaseFor(0.08f, EaseType.QuadraticEaseOut, (p) => {
						float emissionGain = Mathf.Lerp(kSwitchWarningEmissionGain, kSwitchPulseEmissionGain, p);
						SetEmissionGain(emissionGain);
					}, () => {
						this.DoEaseFor(0.08f, EaseType.QuadraticEaseOut, (p) => {
							float emissionGain = Mathf.Lerp(kSwitchPulseEmissionGain, kSwitchNormalEmissionGain, p);
							SetEmissionGain(emissionGain);
						});
					});
					rotationDirection_ = rotationDirection_.Flipped();
				}
				ResetSwitchTimer();
			}

			float rotationAmount = rotationSpeed_ * Time.deltaTime * rotationDirection_.FloatValue();
			rotationContainer_.transform.rotation = rotationContainer_.transform.rotation * Quaternion.Euler(0.0f, rotationAmount, 0.0f);
		}

		private void ResetSwitchTimer() {
			float switchShift = kSwitchTimeShift_ * Mathf.Clamp(timeAlive_ / kMaxRotationSpeedTime, 0.0f, 1.0f);
			float min = kSwitchTimeMin_ - switchShift;
			float max = kSwitchTimeMax_ - switchShift;
			switchTimer_ = UnityEngine.Random.Range(min, max);
		}

		private void Reset() {
			ResetSwitchTimer();
			rotationSpeed_ = kBaseRotationSpeed;
			timeAlive_ = 0.0f;
			safeSwitchesLeft_ = kSafeSwitches;
			rotationContainer_.transform.rotation = Quaternion.identity;
		}

		private void SetEmissionGain(float emissionGain) {
			switchWarningRenderer_.material.SetFloat("_EmissionGain", emissionGain);
		}
	}
}