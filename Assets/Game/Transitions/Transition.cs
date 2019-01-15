using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class Transition {
		// PRAGMA MARK - Public Interface
		public static Action<Transition> OnTransitionCreated = delegate {};

		public int TransitionCount {
			get { return Transitions_.Length; }
		}

		public bool Animating {
			get { return animating_; }
		}

		public Transition(GameObject gameObject) {
			gameObject_ = gameObject;
			OnTransitionCreated.Invoke(this);
		}

		public Transition SetDynamic(bool dynamicTransitions) {
			dynamicTransitions_ = dynamicTransitions;
			return this;
		}

		public Transition SetOffsetDelay(float offsetDelay) {
			offsetDelay_ = offsetDelay;
			return this;
		}

		public Transition SetShuffledOrder(bool shuffledOrder) {
			shuffledOrder_ = shuffledOrder;
			return this;
		}

		public void AnimateIn(Action callback = null, bool instant = false) {
			Canvas.ForceUpdateCanvases();
			Animate(TransitionType.In, callback, instant);
		}

		public void AnimateOut(Action callback = null, bool instant = false) {
			Animate(TransitionType.Out, callback, instant);
		}

		public void Animate(TransitionType transitionType, Action callback, bool instant) {
			if (instant) {
				foreach (ITransition transition in Transitions_) {
					transition.Refresh(transitionType, 1.0f);
				}

				if (callback != null) {
					callback.Invoke();
				}
				return;
			}

			if (animating_) {
				Debug.LogWarning("Transition - animating before previous animation was finished!");
				FinishTransitioning();
			}

			if (dynamicTransitions_) {
				transitions_ = null;
			}

			animating_ = true;
			transitionsFinishedCallback_ = callback;
			transitionsComplete_.Clear();

			if (Transitions_.Length > 0) {
				IEnumerable<ITransition> orderedTransitions = transitionType == TransitionType.In ? Transitions_ : Transitions_.ListReverse();
				if (shuffledOrder_) {
					orderedTransitions = Transitions_.OrderBy(a => Guid.NewGuid());
				}

				int index = 0;
				foreach (ITransition transition in orderedTransitions) {
					transition.Animate(transitionType, delay: index * offsetDelay_, callback: HandleTransitionComplete);
					index++;
				}
			} else {
				FinishTransitioning();
			}
		}


		// PRAGMA MARK - Internal
		private ITransition[] transitions_;

		private readonly HashSet<ITransition> transitionsComplete_ = new HashSet<ITransition>();

		private Action transitionsFinishedCallback_;
		private GameObject gameObject_;

		private bool animating_ = false;
		private bool dynamicTransitions_ = false;
		private float offsetDelay_;
		private bool shuffledOrder_ = false;

		private ITransition[] Transitions_ {
			get { return transitions_ ?? (transitions_ = gameObject_.GetComponentsInChildren<ITransition>()); }
		}

		private void HandleTransitionComplete(ITransition transition) {
			transitionsComplete_.Add(transition);

			if (transitionsComplete_.Count < Transitions_.Length) {
				return;
			}

			bool allTransitionsFinished = Transitions_.All(t => transitionsComplete_.Contains(t));
			if (!allTransitionsFinished) {
				return;
			}

			FinishTransitioning();
		}

		private void FinishTransitioning() {
			if (!animating_) {
				Debug.LogWarning("Callback to transition complete when not animating.. investigate this!");
			}
			animating_ = false;

			if (transitionsFinishedCallback_ != null) {
				// NOTE (darren): why the convoluted logic here? In case Animate() is called
				// as a result of the callback we don't want to null the new transitionsFinishedCallback_
				Action finishedCallback = transitionsFinishedCallback_;
				transitionsFinishedCallback_ = null;
				finishedCallback.Invoke();
			}
		}
	}
}