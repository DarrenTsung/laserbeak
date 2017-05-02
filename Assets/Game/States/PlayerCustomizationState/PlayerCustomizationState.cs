using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.PlayerCustomization {
	public class PlayerCustomizationState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private const float kMoveOnDelay = 1.0f;

		protected override void OnStateEntered() {
			PlayerCustomizationView.Show(Continue);
		}

		protected override void OnStateExited() {
			PlayerCustomizationView.Hide();
		}

		private void Continue() {
			// TODO (darren): handle players backing out of this delay?
			CoroutineWrapper.DoAfterDelay(kMoveOnDelay, () => {
				StateMachine_.Continue();
			});
		}
	}
}