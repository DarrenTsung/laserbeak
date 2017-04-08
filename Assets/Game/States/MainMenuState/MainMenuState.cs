using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.MainMenu {
	public class MainMenuState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject mainMenuPrefab_;

		private MainMenu mainMenu_;

		protected override void OnStateEntered() {
			ObjectPoolManager.CreateView(mainMenuPrefab_);
		}

		protected override void OnStateExited() {
			if (mainMenu_ != null) {
				ObjectPoolManager.Recycle(mainMenu_);
				mainMenu_ = null;
			}
		}
	}
}