using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Transitions;

namespace DT.Game {
	// static class to configure all Transitions
	public static class TransitionConfiguration {
		[RuntimeInitializeOnLoadMethod]
		public static void Initialize() {
			Transition.OnTransitionCreated += InitializeTransition;
		}

		private static void InitializeTransition(Transition transition) {
			transition.SetOffsetDelay(GameConstants.Instance.UIOffsetDelay);
		}
	}
}