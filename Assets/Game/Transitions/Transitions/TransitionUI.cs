using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public abstract class TransitionUI : TransitionBase, ITransition {
		// PRAGMA MARK - Internal
		private RectTransform rectTransform_;
		protected RectTransform RectTransform_ {
			get { return rectTransform_ ?? (rectTransform_ = this.gameObject.GetComponent<RectTransform>()); }
		}
	}
}