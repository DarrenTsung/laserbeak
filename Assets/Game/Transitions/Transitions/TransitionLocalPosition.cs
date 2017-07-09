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
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Transform transform_;

		[Header("Properties")]
		[SerializeField]
		private Vector3 inLocalPosition_ = Vector3.zero;
		[SerializeField]
		private Vector3 outLocalPosition_ = Vector3.zero;

		protected override void Refresh(TransitionType transitionType, float percentage) {
			Vector3 startLocalPosition = (transitionType == TransitionType.In) ? outLocalPosition_ : inLocalPosition_;
			Vector3 endLocalPosition = (transitionType == TransitionType.In) ? inLocalPosition_ : outLocalPosition_;

			SetLocalPosition(Vector3.Lerp(startLocalPosition, endLocalPosition, percentage));
		}

		private void SetLocalPosition(Vector3 localPosition) {
			if (transform_ != null) {
				transform_.localPosition = localPosition;
			}
		}
	}
}