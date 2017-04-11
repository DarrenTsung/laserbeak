using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;

namespace DT.Game {
	[RequireComponent(typeof(Animator))]
	public class GameStateMachine : MonoBehaviour {
		public void StartBattle() {
			animator_.SetTrigger("StartBattle");
		}

		public void GoToMainMenu() {
			animator_.SetTrigger("GoToMainMenu");
		}


		// PRAGMA MARK - Internal
		private Animator animator_;

		private void Awake() {
			animator_ = this.GetRequiredComponent<Animator>();
			this.ConfigureAllStateBehaviours(animator_);
		}
	}
}