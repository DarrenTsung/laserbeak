using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

			SetAlpha(0.0f);
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
		private BattlePlayer battlePlayer_;
		private CoroutineWrapper animateCoroutine_;
		private RecyclablePrefab recycablePrefab_;

		private void SetAlpha(float alpha) {
			foreach (Material material in battlePlayer_.BodyRenderers.Select(r => r.material)) {
				material.SetColor("_DiffuseColor", battlePlayer_.Skin.BodyColor.WithAlpha(alpha));
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
				SetAlpha(1.0f);
				battlePlayer_ = null;
			}

			if (recycablePrefab_ != null) {
				recycablePrefab_.OnCleanup -= HandleCleanup;
				recycablePrefab_ = null;
			}
		}
	}
}