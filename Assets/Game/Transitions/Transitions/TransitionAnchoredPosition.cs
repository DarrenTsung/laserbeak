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
	public class TransitionAnchoredPosition : TransitionUI<Vector2> {
		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private Vector2 inAnchoredPosition_ = Vector3.zero;
		[SerializeField]
		private Vector2 outAnchoredPosition_ = Vector3.zero;

		protected override Vector2 GetInValue() { return inAnchoredPosition_; }
		protected override Vector2 GetOutValue() { return outAnchoredPosition_; }

		protected override Vector2 GetCurrentValue() { return GetAnchoredPosition(); }
		protected override void SetCurrentValue(Vector2 value) { SetAnchoredPosition(value); }

		private void SetAnchoredPosition(Vector3 anchoredPosition) {
			RectTransform_.anchoredPosition = anchoredPosition;
		}

		private Vector2 GetAnchoredPosition() {
			return RectTransform_.anchoredPosition;
		}
	}
}