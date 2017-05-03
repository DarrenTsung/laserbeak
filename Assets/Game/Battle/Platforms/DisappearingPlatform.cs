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
			renderer_.material.SetColor("_Color", originalColor_);
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

		private static readonly Color kFlickerColor = ColorUtil.HexStringToColor("#34b8c4");

		[Header("Outlets")]
		[SerializeField]
		private Collider collider_;

		[SerializeField]
		private Renderer renderer_;

		private Color originalColor_;

		private void Awake() {
			originalColor_ = this.renderer_.material.GetColor("_Color");
		}

		private void Flicker() {
			this.DoEaseFor(kFlickerDuration / 2.0f, EaseType.QuadraticEaseOut, (float p) => {
				renderer_.material.SetColor("_Color", Color.Lerp(originalColor_, kFlickerColor, p));
			}, () => {
				this.DoEaseFor(kFlickerDuration / 2.0f, EaseType.QuadraticEaseOut, (float p) => {
					renderer_.material.SetColor("_Color", Color.Lerp(originalColor_, kFlickerColor, 1.0f - p));
				});
			});
		}
	}
}