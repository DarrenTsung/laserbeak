using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Scoring {
	public class ScoreBubbleView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SetFilled(bool filled, bool animate) {
			Vector3 endScale = filled ? Vector3.one : Vector3.zero;
			if (animate) {
				// animated score bubbles become gold
				image_.color = kAnimatedColor;
				AnimateTo(endScale);
			} else {
				// non-animated score bubbles become white
				image_.color = Color.white;
				scaledObject_.transform.localScale = endScale;
			}
		}


		// PRAGMA MARK - Internal
		private const float kDampingRatio = 0.15f;
		private const float kDuration = 0.4f;

		private static readonly Color kAnimatedColor = ColorUtil.HexStringToColor("#ffce3a");

		[Header("Outlets")]
		[SerializeField]
		private GameObject scaledObject_;

		[SerializeField]
		private Image image_;

		private void AnimateTo(Vector3 endScale) {
			this.StopAllCoroutines();

			Vector3 startScale = scaledObject_.transform.localScale;
			this.DoSpringFor(kDuration, kDampingRatio, (float p) => {
				scaledObject_.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, p);
			});
		}
	}
}