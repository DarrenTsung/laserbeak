using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTEasings;

namespace DT.Game.Transitions {
	public class TransitionLocalScale : TransitionBase, ITransition {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Transform transform_;

		[Header("Properties")]
		[SerializeField]
		private Vector3 inLocalScale_ = Vector3.one;
		[SerializeField]
		private Vector3 outLocalScale_ = Vector3.one;

		protected override void Refresh(TransitionType transitionType, float percentage) {
			Vector3 startLocalScale = (transitionType == TransitionType.In) ? outLocalScale_ : inLocalScale_;
			Vector3 endLocalScale = (transitionType == TransitionType.In) ? inLocalScale_ : outLocalScale_;

			SetLocalScale(Vector3.Lerp(startLocalScale, endLocalScale, percentage));
		}

		private void SetLocalScale(Vector3 localScale) {
			if (transform_ != null) {
				transform_.localScale = localScale;
			}
		}
	}
}