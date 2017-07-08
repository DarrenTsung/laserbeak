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
	public class TransitionLocalPosition : TransitionBase, ITransition {
		// PRAGMA MARK - ITransition Implementation
		public override void Animate(TransitionType transitionType, float delay, Action<ITransition> callback) {
			Vector3 startLocalPosition = (transitionType == TransitionType.In) ? outLocalPosition_ : inLocalPosition_;
			Vector3 endLocalPosition = (transitionType == TransitionType.In) ? inLocalPosition_ : outLocalPosition_;

			EaseType easeType = (transitionType == TransitionType.In) ? inEaseType_ : outEaseType_;

			SetLocalPosition(startLocalPosition);
			CoroutineWrapper.DoAfterDelay(delay, () => {
				CoroutineWrapper.DoEaseFor(Duration_, easeType, (float p) => {
					SetLocalPosition(Vector3.Lerp(startLocalPosition, endLocalPosition, p));
				}, () => {
					callback.Invoke(this);
				});
			});
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Transform transform_;

		[Header("Properties")]
		[SerializeField]
		private Vector3 inLocalPosition_ = Vector3.zero;
		[SerializeField]
		private Vector3 outLocalPosition_ = Vector3.zero;

		[Space]
		[SerializeField]
		private EaseType inEaseType_ = EaseType.QuadraticEaseOut;
		[SerializeField]
		private EaseType outEaseType_ = EaseType.QuadraticEaseIn;

		private void SetLocalPosition(Vector3 localPosition) {
			if (transform_ != null) {
				transform_.localPosition = localPosition;
			}
		}
	}
}