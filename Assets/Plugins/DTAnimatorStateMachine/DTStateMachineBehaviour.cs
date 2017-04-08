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
			if (this._active) {
				return;
			}

			this.OnStateEntered();
			this._active = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (!this._active) {
				return;
			}

			this._active = false;
			this.OnStateExited();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			this.OnStateUpdated();
		}


		// PRAGMA MARK - Internal
		protected TStateMachine StateMachine_ {
			get { return stateMachine_; }
		}

		private TStateMachine stateMachine_;
		private bool _active = false;

		void OnDisable() {
			if (!this._active) {
				return;
			}

			this.OnStateExited();
			this._active = false;
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