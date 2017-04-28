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
	public class DisappearingPlatform : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		[MethodCommandAttribute]
		public void Disappear() {
			Flicker();
			this.DoAfterDelay(kFirstFlickerDelay, () => {
				Flicker();
				this.DoAfterDelay(kSecondFlickerDelay, () => {
					Flicker();
					this.DoAfterDelay(kThirdFlickerDelay, () => {
						renderer_.enabled = false;
						this.DoAfterDelay(GameConstants.Instance.ColliderDisappearDelay, () => {
							collider_.enabled = false;
						});
					});
				});
			});
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			renderer_.enabled = true;
			renderer_.material.SetColor("_DiffuseColor", originalColor_);
			collider_.enabled = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			collider_.enabled = false;
		}


		// PRAGMA MARK - Internal
		private const float kFlickerDuration = 0.2f;
		private const float kFlickerAlpha = 0.5f;

		private const float kFirstFlickerDelay = 0.5f;
		private const float kSecondFlickerDelay = 0.3f;
		private const float kThirdFlickerDelay = 0.2f;

		[Header("Outlets")]
		[SerializeField]
		private Collider collider_;

		[SerializeField]
		private Renderer renderer_;

		private Color originalColor_;

		private void Awake() {
			originalColor_ = this.renderer_.material.GetColor("_DiffuseColor");
		}

		private void Flicker() {
			Color endPulseColor = originalColor_.WithAlpha(kFlickerAlpha);
			this.DoEaseFor(kFlickerDuration / 2.0f, EaseType.QuadraticEaseOut, (float p) => {
				renderer_.material.SetColor("_DiffuseColor", Color.Lerp(originalColor_, endPulseColor, p));
			}, () => {
				this.DoEaseFor(kFlickerDuration / 2.0f, EaseType.QuadraticEaseOut, (float p) => {
					renderer_.material.SetColor("_DiffuseColor", Color.Lerp(originalColor_, endPulseColor, 1.0f - p));
				});
			});
		}
	}
}