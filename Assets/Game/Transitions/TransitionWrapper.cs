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

		public void Animate(TransitionType type, Action callback) {
			if (animating_) {
				Debug.LogWarning("TransitionWrapper - animating before previous animation was finished!");
				return;
			}

			if (dynamicTransitions_) {
				transitionTypeMap_.Clear();
			}

			animating_ = true;
			transitionsFinishedCallback_ = callback;
			transitionType_ = type;
			transitionsComplete_.Clear();

			List<ITransition> transitions = GetTransitionsFor(type);
			IEnumerable<ITransition> orderedTransitions = transitions;
			if (shuffledOrder_) {
				orderedTransitions = transitions.OrderBy(a => Guid.NewGuid());
			}

			if (transitions.Count > 0) {
				int index = 0;
				foreach (ITransition transition in orderedTransitions) {
					transition.Animate(delay: index * offsetDelay_, callback: HandleTransitionComplete);
					index++;
				}
			} else {
				FinishTransitioning();
			}
		}


		// PRAGMA MARK - Internal
		private ITransition[] transitions_;

		private readonly HashSet<ITransition> transitionsComplete_ = new HashSet<ITransition>();
		private readonly Dictionary<TransitionType, List<ITransition>> transitionTypeMap_ = new Dictionary<TransitionType, List<ITransition>>();

		private Action transitionsFinishedCallback_;
		private TransitionType transitionType_;
		private GameObject gameObject_;

		private bool animating_ = false;
		private bool dynamicTransitions_ = false;
		private float offsetDelay_;
		private bool shuffledOrder_ = false;

		private ITransition[] Transitions_ {
			get { return transitions_ ?? (transitions_ = gameObject_.GetComponentsInChildren<ITransition>()); }
		}

		private List<ITransition> GetTransitionsFor(TransitionType type) {
			if (!transitionTypeMap_.ContainsKey(type)) {
				transitionTypeMap_[type] = new List<ITransition>(Transitions_.Where(t => t.Type == type));
			}

			return transitionTypeMap_[type];
		}

		private void HandleTransitionComplete(ITransition transition) {
			transitionsComplete_.Add(transition);

			List<ITransition> transitions = GetTransitionsFor(transitionType_);
			if (transitionsComplete_.Count < transitions.Count) {
				return;
			}

			bool allTransitionsFinished = transitions.All(t => transitionsComplete_.Contains(t));
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