using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.LevelSelect {
	public class LevelSelectState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		private CornerDelayedActionView delayedBackToMainMenuView_;
		private LevelSelectView view_;

		protected override void OnInitialized() {
			// TODO (darren): queue up an ArenaGameMode in Battle
			// TODO (darren): different route for scoring screen after Battle
		}

		protected override void OnStateEntered() {
			view_ = ObjectPoolManager.CreateView<LevelSelectView>(GamePrefabs.Instance.LevelSelectViewPrefab);
			view_.Init(HandleLevelSelected);

			delayedBackToMainMenuView_ = CornerDelayedActionView.Show("BACK TO MAIN MENU", CornerPoint.TopLeft, ActionType.Negative, GoBack);
			delayedBackToMainMenuView_.SetInputs(InputUtil.AllInputs);
		}

		protected override void OnStateExited() {
			if (view_ != null) {
				view_.AnimateOutAndRecycle();
				view_ = null;
			}

			if (delayedBackToMainMenuView_ != null) {
				delayedBackToMainMenuView_.AnimateOutAndRecycle();
				delayedBackToMainMenuView_ = null;
			}
		}

		private void GoBack() {
			StateMachine_.GoToMainMenu();
		}

		private void HandleLevelSelected(CoopLevelConfig config) {
			CoopLevelManager.SetCurrentLevelConfig(config);
			StateMachine_.GoToPlayerCustomization();
		}
	}
}