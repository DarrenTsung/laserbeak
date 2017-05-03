using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes.Ghost {
	public class GhostModePlayerAddOn {
		// PRAGMA MARK - Public Interface
		public GhostModePlayerAddOn(BattlePlayer battlePlayer) {
			battlePlayer_ = battlePlayer;
			recycablePrefab_ = battlePlayer_.GetComponentInParent<RecyclablePrefab>();
			recycablePrefab_.OnCleanup += HandleCleanup;

			battlePlayer_.DustParticleSystem.gameObject.SetActive(false);
			SetAlpha(0.0f);
			foreach (Renderer r in battlePlayer_.BodyRenderers) {
				Color diffuseColor = r.material.GetColor("_DiffuseColor");
				r.material = GameConstants.Instance.PlayerTransparentMaterial;
				r.material.SetColor("_DiffuseColor", diffuseColor);
				r.shadowCastingMode = ShadowCastingMode.Off;
			}
		}

		public void Dispose() {
			Cleanup();
		}

		public void AnimateAlpha(float alpha, float duration) {
			CancelAnimation();
			animateCoroutine_ = CoroutineWrapper.DoEaseFor(duration, EaseType.QuinticEaseIn, (float p) => {
				SetAlpha(Mathf.Lerp(alpha, 0.0f, p));
			});
		}


		// PRAGMA MARK - Internal
		private const float kBaseMetallic = 0.11f;

		private BattlePlayer battlePlayer_;
		private CoroutineWrapper animateCoroutine_;
		private RecyclablePrefab recycablePrefab_;

		private void SetAlpha(float alpha) {
			float metallic = Mathf.Lerp(0.0f, kBaseMetallic, alpha);
			foreach (Material material in battlePlayer_.BodyRenderers.Select(r => r.material)) {
				Color diffuseColor = material.GetColor("_DiffuseColor");
				material.SetColor("_DiffuseColor", diffuseColor.WithAlpha(alpha));
				material.SetFloat("_Metallic", metallic);
			}

			foreach (Renderer renderer in battlePlayer_.BodyRenderers) {
				renderer.enabled = !Mathf.Approximately(alpha, 0.0f);
			}
		}

		private void CancelAnimation() {
			if (animateCoroutine_ != null) {
				animateCoroutine_.Cancel();
				animateCoroutine_ = null;
			}
		}

		private void HandleCleanup(RecyclablePrefab unused) {
			Cleanup();
		}

		private void Cleanup() {
			CancelAnimation();

			if (battlePlayer_ != null) {
				battlePlayer_.DustParticleSystem.gameObject.SetActive(true);
				SetAlpha(1.0f);
				foreach (Renderer r in battlePlayer_.BodyRenderers) {
					Color diffuseColor = r.material.GetColor("_DiffuseColor");
					r.material = GameConstants.Instance.PlayerOpaqueMaterial;
					r.material.SetColor("_DiffuseColor", diffuseColor.WithAlpha(1.0f));
					r.material.SetFloat("_Metallic", kBaseMetallic);
					r.shadowCastingMode = ShadowCastingMode.On;
				}
				battlePlayer_ = null;
			}

			if (recycablePrefab_ != null) {
				recycablePrefab_.OnCleanup -= HandleCleanup;
				recycablePrefab_ = null;
			}
		}
	}
}