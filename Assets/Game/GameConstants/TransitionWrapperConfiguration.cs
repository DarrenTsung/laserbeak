using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Transitions;

namespace DT.Game {
	// static class to configure all TransitionWrappers
	public static class TransitionWrapperConfiguration {
		[RuntimeInitializeOnLoadMethod]
		public static void Initialize() {
			TransitionWrapper.OnTransitionWrapperCreated += InitializeTransitionWrapper;
		}

		private static void InitializeTransitionWrapper(TransitionWrapper transitionWrapper) {
			transitionWrapper.SetOffsetDelay(GameConstants.Instance.UIOffsetDelay);
		}
	}
}