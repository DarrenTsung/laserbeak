using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputBeakControl : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		protected override void Cleanup() {
		}

		private void Update() {
			if (!Enabled) {
				Player_.Animator.SetBool("BeakOpen", false);
				return;
			}

			Player_.Animator.SetBool("BeakOpen", InputDelegate_.LaserPressed);
		}
	}
}