using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public abstract class TransitionBase : MonoBehaviour, ITransition {
		// PRAGMA MARK - ITransition Implementation
		public TransitionType Type {
			get { return transitionType_; }
		}

		public abstract void Animate(float delay, Action<ITransition> callback);


		// PRAGMA MARK - Internal
		[Header("Base Properties")]
		[SerializeField]
		private TransitionType transitionType_;
		[SerializeField]
		private float duration_ = 1.0f;

		protected float Duration_ {
			get { return duration_; }
		}
	}
}