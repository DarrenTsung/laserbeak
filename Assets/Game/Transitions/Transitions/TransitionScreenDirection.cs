using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class TransitionScreenDirection : TransitionUI<Vector2> {
		// PRAGMA MARK - Static
		private static readonly Dictionary<RectTransform, Vector2> cachedInPositions_ = new Dictionary<RectTransform, Vector2>();
		private static Vector2 InPositionFor(RectTransform rectTransform) {
			return cachedInPositions_.GetOrCreateCached(rectTransform, (rt) => rt.anchoredPosition);
		}


		// PRAGMA MARK - Public Interface
		public void SetInDirection(Direction direction) {
			inDirection_ = direction;
		}

		public void SetOutDirection(Direction direction) {
			outDirection_ = direction;
		}


		// PRAGMA MARK - Internal
		[Header("ScreenDirection Properties")]
		[SerializeField]
		private Direction inDirection_ = Direction.UP;
		[SerializeField]
		private Direction outDirection_ = Direction.UP;

		private Canvas canvas_;

		protected override Vector2 GetInValue() { return InPositionFor(RectTransform_); }
		protected override Vector2 GetOutValue() {
			Direction direction = (CurrentTransitionType_ == TransitionType.In) ? inDirection_ : outDirection_;
			return GetInValue() + Vector2.Scale(direction.Vector2Value(), Canvas_.pixelRect.size);
		}

		protected override Vector2 GetCurrentValue() { return GetAnchoredPosition(); }
		protected override void SetCurrentValue(Vector2 value) { SetAnchoredPosition(value); }

		private Vector2 GetAnchoredPosition() {
			return RectTransform_.anchoredPosition;
		}

		private void SetAnchoredPosition(Vector2 anchoredPosition) {
			RectTransform_.anchoredPosition = anchoredPosition;
		}

		private Canvas Canvas_ {
			get { return canvas_ ?? (canvas_ = this.GetComponentInParent<Canvas>()); }
		}
	}
}