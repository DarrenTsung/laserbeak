using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class TransitionWrapper {
		// PRAGMA MARK - Public Interface
		public TransitionWrapper(GameObject gameObject) {
			gameObject_ = gameObject;
		}

		public void AnimateIn(Action callback = null) {
			Animate(TransitionType.In, callback);
		}

		public void AnimateOut(Action callback = null) {
			Animate(TransitionType.Out, callback);
		}

		public void Animate(TransitionType type, Action callback) {
			if (transitionsFinishedCallback_ != null) {
				Debug.LogWarning("TransitionWrapper - animating before previous animation was finished!");
			}

			transitionsFinishedCallback_ = callback;
			transitionType_ = type;
			transitionsComplete_.Clear();
			foreach (ITransition transition in GetTransitionsFor(type)) {
				transition.Animate(HandleTransitionComplete);
			}
		}


		// PRAGMA MARK - Internal
		private ITransition[] transitions_;

		private readonly HashSet<ITransition> transitionsComplete_ = new HashSet<ITransition>();
		private readonly Dictionary<TransitionType, HashSet<ITransition>> transitionTypeMap_ = new Dictionary<TransitionType, HashSet<ITransition>>();

		private Action transitionsFinishedCallback_;
		private TransitionType transitionType_;
		private GameObject gameObject_;

		private ITransition[] Transitions_ {
			get { return transitions_ ?? (transitions_ = gameObject_.GetComponentsInChildren<ITransition>()); }
		}

		private HashSet<ITransition> GetTransitionsFor(TransitionType type) {
			if (!transitionTypeMap_.ContainsKey(type)) {
				transitionTypeMap_[type] = new HashSet<ITransition>(Transitions_.Where(t => t.Type == type));
			}

			return transitionTypeMap_[type];
		}

		private void HandleTransitionComplete(ITransition transition) {
			transitionsComplete_.Add(transition);

			HashSet<ITransition> transitions = GetTransitionsFor(transitionType_);
			if (transitionsComplete_.Count < transitions.Count) {
				return;
			}

			bool allTransitionsFinished = transitions.All(t => transitionsComplete_.Contains(t));
			if (!allTransitionsFinished) {
				return;
			}

			if (transitionsFinishedCallback_ != null) {
				transitionsFinishedCallback_.Invoke();
				transitionsFinishedCallback_ = null;
			}
		}
	}
}