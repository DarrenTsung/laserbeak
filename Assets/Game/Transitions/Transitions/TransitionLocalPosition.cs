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
	public class TransitionLocalPosition : TransitionBase<Vector3> {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Transform transform_;

		[Header("Properties")]
		[SerializeField]
		private Vector3 inLocalPosition_ = Vector3.zero;
		[SerializeField]
		private Vector3 outLocalPosition_ = Vector3.zero;

		protected override Vector3 GetInValue() { return inLocalPosition_; }
		protected override Vector3 GetOutValue() { return outLocalPosition_; }

		protected override Vector3 GetCurrentValue() { return GetLocalPosition(); }
		protected override void SetCurrentValue(Vector3 value) { SetLocalPosition(value); }

		private void SetLocalPosition(Vector3 localPosition) {
			transform_.localPosition = localPosition;
		}

		private Vector3 GetLocalPosition() {
			return transform_.localPosition;
		}
	}
}