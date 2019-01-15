using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class TransitionAlpha : TransitionUI<float> {
		// PRAGMA MARK - Internal
		[Header("Alpha Outlets")]
		[SerializeField, DTValidator.Optional]
		private Image image_;
		[SerializeField, DTValidator.Optional]
		private CanvasGroup canvasGroup_;


		[Header("Alpha Properties")]
		[SerializeField]
		private float minAlpha_ = 0.0f;
		[SerializeField]
		private float maxAlpha_ = 1.0f;

		protected override float GetInValue() { return maxAlpha_; }
		protected override float GetOutValue() { return minAlpha_; }

		protected override float GetCurrentValue() { return GetAlpha(); }
		protected override void SetCurrentValue(float value) { SetAlpha(value); }

		private void SetAlpha(float alpha) {
			if (image_ != null) {
				image_.color = image_.color.WithAlpha(alpha);
			}

			if (canvasGroup_ != null) {
				canvasGroup_.alpha = alpha;
			}
		}

		private float GetAlpha() {
			if (image_ != null) {
				return image_.color.a;
			}

			if (canvasGroup_ != null) {
				return canvasGroup_.alpha;
			}

			Debug.LogWarning("No outlet set - cannot GetAlpha!");
			return 0.0f;
		}
	}
}