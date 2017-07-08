using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class TransitionScreenDirection : TransitionUI, ITransition {
		// PRAGMA MARK - Static
		private static readonly Dictionary<RectTransform, Vector2> cachedInPositions_ = new Dictionary<RectTransform, Vector2>();
		private static Vector2 InPositionFor(RectTransform rectTransform) {
			return cachedInPositions_.GetOrCreateCached(rectTransform, (rt) => rt.anchoredPosition);
		}


		// PRAGMA MARK - ITransition Implementation
		public override void Animate(TransitionType transitionType, float delay, Action<ITransition> callback) {
			Canvas canvas = this.GetComponentInParent<Canvas>();

			Direction direction = (transitionType == TransitionType.In) ? inDirection_ : outDirection_;

			Vector2 inPosition = InPositionFor(RectTransform_);
			Vector2 outPosition = inPosition + Vector2.Scale(direction.Vector2Value(), canvas.pixelRect.size);

			Vector2 startPosition = (transitionType == TransitionType.In) ? outPosition : inPosition;
			Vector2 endPosition = (transitionType == TransitionType.In) ? inPosition : outPosition;

			EaseType easeType = (transitionType == TransitionType.In) ? inEaseType_ : outEaseType_;

			RectTransform_.anchoredPosition = startPosition;
			CoroutineWrapper.DoAfterDelay(delay, () => {
				CoroutineWrapper.DoEaseFor(Duration_, easeType, (float p) => {
					RectTransform_.anchoredPosition = Vector2.Lerp(startPosition, endPosition, p);
				}, () => {
					callback.Invoke(this);
				});
			});
		}


		// PRAGMA MARK - Internal
		[Header("ScreenDirection Properties")]
		[SerializeField]
		private Direction inDirection_ = Direction.UP;
		[SerializeField]
		private Direction outDirection_ = Direction.UP;

		[Space]
		[SerializeField]
		private EaseType inEaseType_ = EaseType.QuadraticEaseOut;
		[SerializeField]
		private EaseType outEaseType_ = EaseType.QuadraticEaseIn;
	}
}