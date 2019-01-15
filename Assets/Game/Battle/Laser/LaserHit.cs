using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class LaserHit : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void SetMaterial(Material laserMaterial) {
			Color color = laserMaterial.GetColor("_EmissionColor");

			light_.color = color;
			mistParticleSystem_.SetStartColor(color.WithAlpha(0.4f));
			particleSystemRenderer_.sharedMaterial = laserMaterial;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			coroutine_ = CoroutineWrapper.DoEaseFor(duration_, EaseType.CubicEaseOut, (float percentage) => {
				light_.intensity = Mathf.Lerp(kLightIntensity, 0.0f, percentage);
			}, () => {
				ObjectPoolManager.Recycle(this);
			});
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private const float kLightIntensity = 2.0f;

		[Header("Outlets")]
		[SerializeField]
		private Light light_;

		[SerializeField]
		private ParticleSystem mistParticleSystem_;

		[SerializeField]
		private ParticleSystemRenderer particleSystemRenderer_;

		[Header("Properties")]
		[SerializeField]
		private float duration_ = 1.0f;

		private CoroutineWrapper coroutine_;
	}
}