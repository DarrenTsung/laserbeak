using System;
using System.Collections;
using UnityEngine;

namespace DTAnimatorStateMachine {
	public class DTStateMachineBehaviour<TStateMachine> : StateMachineBehaviour, IStateBehaviour<TStateMachine> {
		// PRAGMA MARK - IStateBehaviour<TStateMachine> Implementation
		public void InitializeWithContext(Animator animator, TStateMachine stateMachine) {
			stateMachine_ = stateMachine;
			OnInitialized();
		}


		// PRAGMA MARK - StateMachineBehaviour Lifecycle
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (active_) {
				return;
			}

			OnStateEntered();
			active_ = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (!active_) {
				return;
			}

			active_ = false;
			OnStateExited();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			OnStateUpdated();
		}


		// PRAGMA MARK - Internal
		protected TStateMachine StateMachine_ {
			get { return stateMachine_; }
		}

		private TStateMachine stateMachine_;
		private bool active_ = false;

		private void OnDisable() {
			if (!active_) {
				return;
			}

			OnStateExited();
			active_ = false;
		}

		protected virtual void OnInitialized() {
			// stub
		}

		protected virtual void OnStateEntered() {
			// stub
		}

		protected virtual void OnStateExited() {
			// stub
		}

		protected virtual void OnStateUpdated() {
			// stub
		}
	}
}