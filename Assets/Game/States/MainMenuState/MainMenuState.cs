using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTDebugMenu;
using DTObjectPoolManager;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Debugs;
using DT.Game.GameModes;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.MainMenu {
	public class MainMenuState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject mainMenuPrefab_;

		private MainMenu mainMenu_;
		private IDynamicGroup dynamicGroup_;

		protected override void OnInitialized() {
			var inspector = GenericInspectorRegistry.Get("PHASERBEAK");
			inspector.BeginDynamic();
			inspector.RegisterHeader("Main Menu State");
			inspector.RegisterButton("Toggle Main Menu Visible", ToggleMainMenuVisible);
			dynamicGroup_ = inspector.EndDynamic();
		}

		protected override void OnStateEntered() {
			dynamicGroup_.Enabled = true;

			#if DEMO
			PHASERBEAKDebug.ResetAllThings();
			#endif

			BattleCamera.SetDepthOfFieldEnabled(true);
			InGameConstants.BattlePlayerPartsFade = true;
			RegisteredPlayers.Clear();

			ArenaManager.Instance.AnimateLoadRandomArena(() => {
				RegisteredPlayersUtil.RegisterAIPlayers(4);
				PlayerSpawner.ShouldRespawn = true;
				PlayerSpawner.SpawnAllPlayers();
			});

			CreateMainMenu();
		}

		private void GoToPlayerCustomization() {
			mainMenu_.AnimateOut(StateMachine_.GoToPlayerCustomization);
		}

		private void GoToLevelEditor() {
			mainMenu_.AnimateOut(StateMachine_.GoToLevelEditor);
		}

		private void GoToLevelSelection() {
			mainMenu_.AnimateOut(StateMachine_.GoToLevelSelect);
		}

		protected override void OnStateExited() {
			dynamicGroup_.Enabled = false;

			BattleCamera.SetDepthOfFieldEnabled(false, animate: true);
			InGameConstants.BattlePlayerPartsFade = false;
			PlayerScores.Clear();
			RegisteredPlayers.Clear();
			PlayerSpawner.ShouldRespawn = false;
			PlayerSpawner.CleanupAllPlayers();

			CleanupMainMenu();
		}

		private void CreateMainMenu() {
			mainMenu_ = ObjectPoolManager.CreateView<MainMenu>(mainMenuPrefab_);
			mainMenu_.Init(battleHandler: GoToPlayerCustomization, levelEditorHandler: GoToLevelEditor, coopHandler: GoToLevelSelection);
		}

		private void CleanupMainMenu() {
			if (mainMenu_ != null) {
				ObjectPoolManager.Recycle(mainMenu_);
				mainMenu_ = null;
			}
		}

		private void ToggleMainMenuVisible() {
			if (mainMenu_ != null) {
				CleanupMainMenu();
				BattleCamera.SetDepthOfFieldEnabled(false);
			} else {
				CreateMainMenu();
				BattleCamera.SetDepthOfFieldEnabled(true);
			}
		}
	}
}