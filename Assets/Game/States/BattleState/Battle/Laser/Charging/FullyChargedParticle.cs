using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class FullyChargedParticle : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Public Interface
		public void SetColor(Color color) {
			renderer_.color = color;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			CoroutineWrapper.DoEaseFor(kDuration, EaseType.CubicEaseOut, (float percentage) => {
				float scale = Mathf.Lerp(0.0f, kFinalScale, percentage);
				this.transform.localScale = new Vector3(scale, scale, scale);
			});
			CoroutineWrapper.DoEaseFor(kDuration, EaseType.CubicEaseIn, (float percentage) => {
				renderer_.material.SetFloat("_DissolveFactor", percentage * kDissolveMaxCap);
				renderer_.material.SetFloat("_EmissionGain", (1.0f - percentage) * kEmissionGain);
			});
		}


		// PRAGMA MARK - Internal
		private const float kDuration = 0.60f;
		private const float kFinalScale = 0.65f;
		private const float kEmissionGain = 0.50f;

		// because texture finishes dissolving at 0.75, scaling lerp here
		private const float kDissolveMaxCap = 0.75f;

		[Header("Outlets")]
		[SerializeField]
		private SpriteRenderer renderer_;
	}
}