using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public abstract class TransitionUI<T> : TransitionBase<T>, ITransition {
		// PRAGMA MARK - Internal
		private RectTransform rectTransform_;
		protected RectTransform RectTransform_ {
			get { return rectTransform_ ?? (rectTransform_ = this.gameObject.GetComponent<RectTransform>()); }
		}
	}
}