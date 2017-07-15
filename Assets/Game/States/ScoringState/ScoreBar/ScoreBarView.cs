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
	public class ScoreBarView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void SetScoreCount(int scoreCount, Color playerColor, bool animate) {
			image_.color = playerColor;

			float endHeight = scoreCount * kScoreHeight;
			if (Height_ == endHeight) {
				return;
			}

			if (animate) {
				AnimateTo(endHeight);
			} else {
				Height_ = endHeight;
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.StopAllCoroutines();
			layoutElement_.preferredHeight = 0.0f;
		}


		// PRAGMA MARK - Internal
		private const float kScoreHeight = 76.0f;

		private const float kDampingRatio = 0.35f;
		private const float kDuration = 0.4f;

		[Header("Outlets")]
		[SerializeField]
		private LayoutElement layoutElement_;
		[SerializeField]
		private Image image_;

		private float Height_ {
			get { return layoutElement_.preferredHeight; }
			set { layoutElement_.preferredHeight = value; }
		}

		private void AnimateTo(float endHeight) {
			this.StopAllCoroutines();

			float startHeight = Height_;
			this.DoSpringFor(kDuration, kDampingRatio, (float p) => {
				Height_ = Mathf.LerpUnclamped(startHeight, endHeight, p);
			});
		}
	}
}