using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class FullyChargedParticle : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void SetColor(Color color) {
			renderer_.color = color;
		}

		public void Disperse() {
			if (dispersing_) {
				return;
			}

			dispersing_ = true;
			CancelCoroutines();
			coroutines_.Add(CoroutineWrapper.DoEaseFor(kOutDuration, EaseType.CubicEaseOut, (float percentage) => {
				float scale = Mathf.Lerp(kHeldScale, kFinalScale, percentage);
				this.transform.localScale = new Vector3(scale, scale, scale);
			}));
			coroutines_.Add(CoroutineWrapper.DoEaseFor(kOutDuration, EaseType.CubicEaseIn, (float percentage) => {
				renderer_.material.SetFloat("_DissolveFactor", percentage * kDissolveMaxCap);

				float emissionGain = Mathf.Lerp(kMaxEmissionGain, 0.0f, percentage);
				renderer_.material.SetFloat("_EmissionGain", emissionGain);
			}, () => {
				ObjectPoolManager.Recycle(this);
			}));
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			CancelCoroutines();
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			dispersing_ = false;

			CancelCoroutines();
			renderer_.material.SetFloat("_DissolveFactor", 0.0f);
			coroutines_.Add(CoroutineWrapper.DoEaseFor(kInDuration * 0.6f, EaseType.CubicEaseOut, (float percentage) => {
				float scale = Mathf.Lerp(0.0f, kFinalScale, percentage);
				this.transform.localScale = new Vector3(scale, scale, scale);

				float emissionGain = Mathf.Lerp(0.0f, kMaxEmissionGain, percentage);
				renderer_.material.SetFloat("_EmissionGain", emissionGain);
			}, () => {
				coroutines_.Add(CoroutineWrapper.DoEaseFor(kInDuration * 0.4f, EaseType.CubicEaseOut, (float percentage2) => {
					float newScale = Mathf.Lerp(kFinalScale, kHeldScale, percentage2);
					this.transform.localScale = new Vector3(newScale, newScale, newScale);

					float newEmissionGain = Mathf.Lerp(kMaxEmissionGain, kHeldEmissionGain, percentage2);
					renderer_.material.SetFloat("_EmissionGain", newEmissionGain);
				}, () => {
					CancelCoroutines();
					StartHeldLooping();
				}));
			}));
		}


		// PRAGMA MARK - Internal
		private const float kInDuration = 0.50f;
		private const float kHelpLoopDuration = 0.50f;
		private const float kOutDuration = 0.70f;

		private const float kHeldScale = 0.27f;
		private const float kHeldMaxScale = 0.32f;
		private const float kFinalScale = 0.45f;

		private const float kHeldEmissionGain = 0.35f;
		private const float kHeldMaxEmissionGain = 0.4f;
		private const float kMaxEmissionGain = 0.55f;

		// because texture finishes dissolving at 0.75, scaling lerp here
		private const float kDissolveMaxCap = 0.75f;

		[Header("Outlets")]
		[SerializeField]
		private SpriteRenderer renderer_;

		private readonly List<CoroutineWrapper> coroutines_ = new List<CoroutineWrapper>();

		private bool dispersing_ = false;

		private void StartHeldLooping() {
			coroutines_.Add(CoroutineWrapper.DoEaseFor(kHelpLoopDuration / 2.0f, EaseType.CubicEaseInOut, (float percentage) => {
				float scale = Mathf.Lerp(kHeldScale, kHeldMaxScale, percentage);
				this.transform.localScale = new Vector3(scale, scale, scale);

				float emissionGain = Mathf.Lerp(kHeldEmissionGain, kHeldMaxEmissionGain, percentage);
				renderer_.material.SetFloat("_EmissionGain", emissionGain);
			}, () => {
				coroutines_.Add(CoroutineWrapper.DoEaseFor(kHelpLoopDuration / 2.0f, EaseType.CubicEaseInOut, (float percentage) => {
					float scale = Mathf.Lerp(kHeldMaxScale, kHeldScale, percentage);
					this.transform.localScale = new Vector3(scale, scale, scale);

					float emissionGain = Mathf.Lerp(kHeldMaxEmissionGain, kHeldEmissionGain, percentage);
					renderer_.material.SetFloat("_EmissionGain", emissionGain);
				}, () => {
					// loop continuously
					CancelCoroutines();
					StartHeldLooping();
				}));
			}));
		}

		private void CancelCoroutines() {
			foreach (CoroutineWrapper coroutine in coroutines_) {
				coroutine.Cancel();
			}
			coroutines_.Clear();
		}
	}
}