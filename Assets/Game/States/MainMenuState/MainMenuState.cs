using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;

namespace DT.Game.MainMenu {
	[RequireComponent(typeof(Animator))]
	public class MainMenuState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		protected override void OnStateEntered() {
			// stub
		}

		protected override void OnStateExited() {
			// stub
		}
	}
}