using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;

namespace DT.Game.MainMenu {
	public class MainMenu : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SetPlayHandler(Action playHandler) {
			playHandler_ = playHandler;
		}


		// PRAGMA MARK - Internal
		private Action playHandler_;

		private void Update() {
			if (InputUtil.IsAnyMainButtonPressed()) {
				HandlePlayPressed();
			}
		}

		private void HandlePlayPressed() {
			if (playHandler_ == null) {
				return;
			}

			AudioConstants.Instance.UIBeep.PlaySFX();
			playHandler_.Invoke();
			// avoid invoke handler twice
			playHandler_ = null;
		}
	}
}