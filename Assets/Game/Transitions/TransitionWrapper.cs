using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class TransitionWrapper {
		// PRAGMA MARK - Public Interface
		public static Action<TransitionWrapper> OnTransitionWrapperCreated = delegate {};

		public int TransitionCount {
			get { return Transitions_.Length; }
		}

		public TransitionWrapper(GameObject gameObject) {
			gameObject_ = gameObject;
			OnTransitionWrapperCreated.Invoke(this);
		}

		public TransitionWrapper SetDynamic(bool dynamicTransitions) {
			dynamicTransitions_ = dynamicTransitions;
			return this;
		}

		public TransitionWrapper SetOffsetDelay(float offsetDelay) {
			offsetDelay_ = offsetDelay;
			return this;
		}

		public TransitionWrapper SetShuffledOrder(bool shuffledOrder) {
			shuffledOrder_ = shuffledOrder;
			return this;
		}

		public void AnimateIn(Action callback = null) {
			Canvas.ForceUpdateCanvases();
			Animate(TransitionType.In, callback);
		}

		public void AnimateOut(Action callback = null) {
			Animate(TransitionType.Out, callback);
		}

		public void Animate(TransitionType transitionType, Action callback) {
			if (animating_) {
				Debug.LogWarning("TransitionWrapper - animating before previous animation was finished!");
				return;
			}

			if (dynamicTransitions_) {
				transitions_ = null;
			}

			animating_ = true;
			transitionsFinishedCallback_ = callback;
			transitionsComplete_.Clear();

			if (Transitions_.Length > 0) {
				IEnumerable<ITransition> orderedTransitions = Transitions_;
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
				transitionsFinishedCallback_.Invoke();
				transitionsFinishedCallback_ = null;
			}
		}
	}
}