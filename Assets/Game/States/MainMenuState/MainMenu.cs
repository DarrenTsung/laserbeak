using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;

using DT.Game.GameModes;

namespace DT.Game.MainMenu {
	public class MainMenu : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Public Interface
		public void SetPlayHandler(Action playHandler) {
			playHandler_ = playHandler;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			RefreshDemoMode();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject demoModeContainer_;

		private Action playHandler_;

		private void Update() {
			if (InputUtil.WasAnyMainButtonPressed()) {
				HandlePlayPressed();
			}

			if (InputUtil.WasAnyCommandButtonPressed()) {
				// TODO (darren): better place for this?
				GameConstants.Instance.DemoMode = !GameConstants.Instance.DemoMode;
				RefreshDemoMode();
				GameModeShowedInstructionsCache.ResetShowedInstructionsCache();
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

		private void RefreshDemoMode() {
			demoModeContainer_.SetActive(GameConstants.Instance.DemoMode);
		}
	}
}