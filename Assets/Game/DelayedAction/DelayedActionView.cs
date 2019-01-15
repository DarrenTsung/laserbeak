using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTObjectPoolManager;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Transitions;

namespace DT.Game {
	public class DelayedActionView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			percentageHeld_ = 0.0f;
			fillImage_.fillAmount = 0.0f;
		}

		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			inputs_ = null;
			finishedCallback_ = null;
		}


		// PRAGMA MARK - Public Interface
		public void AnimateOutAndRecycle() {
			inputs_ = null;
			finishedCallback_ = null;
			transition_.AnimateOut(() => {
				ObjectPoolManager.Recycle(this.gameObject);
			});
		}

		public void Init(string actionText, ActionType actionType, Action finishedCallback, bool animate = true) {
			actionType_ = actionType;
			finishedCallback_ = finishedCallback;

			actionText_.Text = actionText;
			actionIconOutlet_.Init(actionType);

			if (animate) {
				transition_.AnimateIn();
			}
		}

		public void SetActivePredicate(Predicate activePredicate) {
			activePredicate_ = activePredicate;
		}

		public void SetInputs(IEnumerable<IInputWrapper> inputs) {
			inputs_ = inputs;
		}



		// PRAGMA MARK - Internal
		private const float kHoldTime = 0.7f;
		private const float kDecayMultiplier = 2.0f;

		[Header("Outlets")]
		[SerializeField]
		protected ActionIconOutlet actionIconOutlet_;
		[SerializeField]
		protected Image fillImage_;
		[SerializeField]
		protected TextOutlet actionText_;

		[Header("Read-only")]
		[SerializeField, ReadOnly]
		private float percentageHeld_ = 0.0f;
		[SerializeField, ReadOnly]
		private ActionType actionType_;

		private Action finishedCallback_;
		private Predicate activePredicate_;
		private IEnumerable<IInputWrapper> inputs_ = null;

		private Transition transition_;

		private void Awake() {
			transition_ = new Transition(this.gameObject);
		}

		private void Update() {
			bool isActive = true;
			if (activePredicate_ != null) {
				isActive = activePredicate_.Invoke();
			}
			bool held = isActive && actionType_.IsHeld(inputs_);
			float percentageDelta = Time.deltaTime / kHoldTime;
			percentageHeld_ = Mathf.Clamp01(percentageHeld_ + ((held) ? percentageDelta : (-percentageDelta * kDecayMultiplier)));

			if (finishedCallback_ == null) {
				percentageHeld_ = 1.0f;
			}

			fillImage_.fillAmount = percentageHeld_;

			if (percentageHeld_ >= 1.0f) {
				if (finishedCallback_ != null) {
					finishedCallback_.Invoke();
					finishedCallback_ = null;
				}
			}
		}
	}
}