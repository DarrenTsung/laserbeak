using System;
using UnityEngine;

namespace DTAnimatorStateMachine {
	public static class StateBehaviourExtensions {
		public static void ConfigureAllStateBehaviours<TStateMachine>(this TStateMachine stateMachine, Animator animator) {
			StateMachineBehaviour[] behaviours = animator.GetBehaviours<StateMachineBehaviour>();
			foreach (StateMachineBehaviour behaviour in behaviours) {
				IStateBehaviour<TStateMachine> configurableState = behaviour as IStateBehaviour<TStateMachine>;

				if (configurableState == null) {
					continue;
				}

				configurableState.InitializeWithContext(animator, stateMachine);
			}
		}

		public static void ConfigureAllStateBehaviours<TStateMachine, U>(this TStateMachine stateMachine, Animator animator, U context) {
			StateMachineBehaviour[] behaviours = animator.GetBehaviours<StateMachineBehaviour>();
			foreach (StateMachineBehaviour behaviour in behaviours) {
				IStateBehaviour<TStateMachine, U> configurableState = behaviour as IStateBehaviour<TStateMachine, U>;

				if (configurableState == null) {
					continue;
				}

				configurableState.InitializeWithContext(animator, stateMachine, context);
			}
		}
	}
}