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
	public class TransitionAnchoredPosition : TransitionBase, ITransition {
		// PRAGMA MARK - ITransition Implementation
		public override void Refresh(TransitionType transitionType, float percentage) {
			Vector2 startAnchoredPosition = (transitionType == TransitionType.In) ? outAnchoredPosition_ : inAnchoredPosition_;
			Vector2 endAnchoredPosition = (transitionType == TransitionType.In) ? inAnchoredPosition_ : outAnchoredPosition_;

			SetAnchoredPosition(Vector2.Lerp(startAnchoredPosition, endAnchoredPosition, percentage));
		}

		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private RectTransform rectTransform_;

		[Header("Properties")]
		[SerializeField]
		private Vector2 inAnchoredPosition_ = Vector3.zero;
		[SerializeField]
		private Vector2 outAnchoredPosition_ = Vector3.zero;

		private void SetAnchoredPosition(Vector3 anchoredPosition) {
			if (rectTransform_ != null) {
				rectTransform_.anchoredPosition = anchoredPosition;
			} else {
				Debug.LogWarning("Missing rectTransform_ outlet!");
			}
		}
	}
}