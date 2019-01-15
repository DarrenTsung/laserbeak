using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTViewManager;

using DT.Game.Audio;
using DT.Game.Hints;

namespace DT.Game {
	[RequireComponent(typeof(Animator))]
	public class GameStateMachine : MonoBehaviour {
		// PRAGMA MARK - Static
		public static event Action OnBattleFinished = delegate {};

		[MethodCommand]
		private static void SkipBattle() {
			UnityEngine.Object.FindObjectOfType<GameStateMachine>().HandleBattleFinished();
		}


		// PRAGMA MARK - Public Interface
		public void GoToMainMenu() {
			animator_.SetTrigger("GoToMainMenu");
		}

		public void GoToPlayerCustomization() {
			animator_.SetTrigger("GoToPlayerCustomization");
		}

		public void GoToLevelEditor() {
			animator_.SetTrigger("GoToLevelEditor");
		}

		public void GoToLevelSelect() {
			animator_.SetTrigger("GoToLevelSelect");
		}

		public void HandleBattleFinished() {
			animator_.SetTrigger("BattleFinished");
			OnBattleFinished.Invoke();
		}

		public void RestartBattle() {
			animator_.SetTrigger("RestartBattle");
		}

		public void Continue() {
			animator_.SetTrigger("Continue");
		}

		public void HandleGameFinished() {
			animator_.SetTrigger("GameFinished");
		}


		// PRAGMA MARK - Internal
		private Animator animator_;

		private void Awake() {
			Cursor.visible = false;
			Application.targetFrameRate = 60;

			animator_ = this.GetRequiredComponent<Animator>();
			this.ConfigureAllStateBehaviours(animator_);

			AudioManager.Instance.PlayBGM(AudioConstants.Instance.BackgroundMusic);
		}
	}
}