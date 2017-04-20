using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;

using DT.Game.Audio;

namespace DT.Game {
	[RequireComponent(typeof(Animator))]
	public class GameStateMachine : MonoBehaviour {
		public void StartBattle() {
			animator_.SetTrigger("StartBattle");
		}

		public void GoToMainMenu() {
			animator_.SetTrigger("GoToMainMenu");
		}

		public void HandleBattleFinished() {
			animator_.SetTrigger("BattleFinished");
		}


		// PRAGMA MARK - Internal
		private Animator animator_;

		private void Awake() {
			animator_ = this.GetRequiredComponent<Animator>();
			this.ConfigureAllStateBehaviours(animator_);

			AudioManager.Instance.PlayBGM(AudioConstants.Instance.BackgroundMusic);
		}
	}
}