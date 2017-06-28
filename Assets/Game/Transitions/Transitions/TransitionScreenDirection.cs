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
			Vector2 outPosition = InPosition_ + Vector2.Scale(direction_.Vector2Value(), canvas.pixelRect.size);

			Vector2 startPosition = (this.Type == TransitionType.In) ? outPosition : InPosition_;
			Vector2 endPosition = (this.Type == TransitionType.In) ? InPosition_ : outPosition;

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

		private Vector2? inPosition_ = null;
		private Vector2 InPosition_ {
			get { return (inPosition_ ?? (inPosition_ = RectTransform_.anchoredPosition)).Value; }
		}
	}
}