using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTEasings;

namespace DT.Game.Transitions {
	public class TransitionLocalScale : TransitionBase<Vector3> {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Transform transform_;

		[Header("Properties")]
		[SerializeField]
		private Vector3 inLocalScale_ = Vector3.one;
		[SerializeField]
		private Vector3 outLocalScale_ = Vector3.one;

		protected override Vector3 GetInValue() { return inLocalScale_; }
		protected override Vector3 GetOutValue() { return outLocalScale_; }

		protected override Vector3 GetCurrentValue() { return GetLocalScale(); }
		protected override void SetCurrentValue(Vector3 value) { SetLocalScale(value); }

		private void SetLocalScale(Vector3 localScale) {
			transform_.localScale = localScale;
		}

		private Vector3 GetLocalScale() {
			return transform_.localScale;
		}
	}
}