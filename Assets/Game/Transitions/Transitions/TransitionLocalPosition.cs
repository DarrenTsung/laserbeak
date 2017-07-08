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
		public override void Animate(float delay, Action<ITransition> callback) {
			Vector3 startLocalPosition = (this.Type == TransitionType.In) ? outLocalPosition_ : inLocalPosition_;
			Vector3 endLocalPosition = (this.Type == TransitionType.In) ? inLocalPosition_ : outLocalPosition_;

			SetLocalPosition(startLocalPosition);
			CoroutineWrapper.DoAfterDelay(delay, () => {
				CoroutineWrapper.DoEaseFor(Duration_, easeType_, (float p) => {
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
		private EaseType easeType_ = EaseType.QuadraticEaseOut;

		private void SetLocalPosition(Vector3 localPosition) {
			if (transform_ != null) {
				transform_.localPosition = localPosition;
			}
		}
	}
}