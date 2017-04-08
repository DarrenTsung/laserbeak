using System;
using UnityEngine;

namespace DTAnimatorStateMachine {
	public interface IStateBehaviour<TStateMachine> {
		void InitializeWithContext(Animator animator, TStateMachine stateMachine);
	}

	public interface IStateBehaviour<TStateMachine, U> {
		void InitializeWithContext(Animator animator, TStateMachine stateMachine, U context);
	}
}