using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public abstract class TransitionBase : MonoBehaviour, ITransition {
		// PRAGMA MARK - ITransition Implementation
		float ITransition.Duration {
			get { return duration_; }
		}

		float ITransition.BaseDelay {
			get { return baseDelay_; }
		}

		void ITransition.Animate(TransitionType transitionType, float delay, Action<ITransition> callback) {
			EaseType easeType = (transitionType == TransitionType.In) ? inEaseType_ : outEaseType_;

			Refresh(transitionType, 0.0f);
			CoroutineWrapper.DoAfterDelay(baseDelay_ + delay, () => {
				CoroutineWrapper.DoEaseFor(duration_, easeType, (float p) => {
					Refresh(transitionType, p);
				}, () => {
					callback.Invoke(this);
				});
			});
		}

		// ITransition.Refresh
		public abstract void Refresh(TransitionType transitionType, float percentage);


		// PRAGMA MARK - Internal
		[Header("Base Properties")]
		[SerializeField]
		private float duration_ = 1.0f;
		[SerializeField]
		private float baseDelay_ = 0.0f;

		[Space]
		[SerializeField]
		private EaseType inEaseType_ = EaseType.QuadraticEaseOut;
		[SerializeField]
		private EaseType outEaseType_ = EaseType.QuadraticEaseIn;
	}
}