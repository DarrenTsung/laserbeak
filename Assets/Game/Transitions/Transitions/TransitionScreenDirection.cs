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
		// PRAGMA MARK - ITransition Implementation
		public override void Animate(Action<ITransition> callback) {
			Canvas canvas = this.GetComponentInParent<Canvas>();
			Vector2 outPosition = Vector2.Scale(direction_.Vector2Value(), canvas.pixelRect.size);
			Vector2 inPosition = Vector2.zero;

			Vector2 startPosition = (this.Type == TransitionType.In) ? outPosition : inPosition;
			Vector2 endPosition = (this.Type == TransitionType.In) ? inPosition : outPosition;
			CoroutineWrapper.DoEaseFor(Duration_, easeType_, (float p) => {
				RectTransform_.anchoredPosition = Vector2.Lerp(startPosition, endPosition, p);
			}, () => {
				callback.Invoke(this);
			});
		}


		// PRAGMA MARK - Internal
		[Header("ScreenDirection Properties")]
		[SerializeField]
		private Direction direction_;
		[SerializeField]
		private EaseType easeType_;
	}
}