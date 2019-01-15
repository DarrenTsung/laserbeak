using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerPanicControl : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const float kPanicAverageAngleMovement = 100.0f;
		private const float kPanicEmissionRate = 0.22f;
		private const float kPanicEmissionRateStart = 0.1f;
		private const float kUpdateInterval = 0.5f;

		private const float kDecayRate = 0.2f;

		[Header("Outlets")]
		[SerializeField]
		private ParticleSystem panicParticleSystem_;
		[SerializeField]
		private ParticleSystemRenderer panicParticleSystemRenderer_;

		private Quaternion previousRotation_ = Quaternion.identity;
		private float averageAngleMovement_;
		private float accumulatedAngleMovement_;

		private float timeleft_ = kUpdateInterval;
		private float panicTimeLeft_ = kPanicEmissionRateStart;

		protected override void Initialize() {
			panicParticleSystemRenderer_.sharedMaterial = Player_.Skin.LaserMaterial;
			panicTimeLeft_ = kPanicEmissionRateStart;
		}

		protected override void Cleanup() {
			// stub
		}

		private void Update() {
			var currentRotation = Player_.transform.rotation;
			float angleMovement = Quaternion.Angle(currentRotation, previousRotation_);
			accumulatedAngleMovement_ += angleMovement;

			timeleft_ -= Time.deltaTime;
			if (timeleft_ <= 0.0f) {
				averageAngleMovement_ = accumulatedAngleMovement_ / kUpdateInterval;
				accumulatedAngleMovement_ = 0.0f;
			}

			if (averageAngleMovement_ >= kPanicAverageAngleMovement) {
				panicTimeLeft_ -= Time.deltaTime;
				if (panicTimeLeft_ <= 0.0f) {
					panicTimeLeft_ = kPanicEmissionRate;
					SpawnPanicParticle();
				}
			} else {
				panicTimeLeft_ = Mathf.Min(panicTimeLeft_ + kDecayRate * Time.deltaTime, kPanicEmissionRateStart);
			}

			previousRotation_ = currentRotation;
		}

		private static readonly Vector3 kParticleFacingVector = new Vector3(0.0f, 0.6f, -1.0f).normalized;
		private Vector3 previousOffset_ = Vector3.zero;
		private void UpdatePanicParticleSystem() {
			Vector3 offset = Vector3.zero;
			do {
				offset = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0.0f, UnityEngine.Random.Range(0.0f, -0.3f));
			} while ((offset - previousOffset_).magnitude <= 0.2f);

			panicParticleSystem_.transform.localPosition = offset;

			var mainModule = panicParticleSystem_.main;

			Quaternion rotation = Quaternion.FromToRotation((kParticleFacingVector + (offset.normalized * 0.2f)).normalized, (Camera.main.transform.position - panicParticleSystem_.transform.position).normalized);
			Vector3 eulerAngles = rotation.eulerAngles;
			mainModule.startRotationX = eulerAngles.x * Mathf.Deg2Rad;
			mainModule.startRotationY = eulerAngles.y * Mathf.Deg2Rad;
			mainModule.startRotationZ = eulerAngles.z * Mathf.Deg2Rad;
		}

		private void SpawnPanicParticle() {
			UpdatePanicParticleSystem();
			panicParticleSystem_.Emit(1);
		}
	}
}