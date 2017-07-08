using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.MainMenu {
	public class MainMenuState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject mainMenuPrefab_;

		private MainMenu mainMenu_;

		protected override void OnStateEntered() {
			BattleCamera.SetDepthOfFieldEnabled(true);
			RegisteredPlayers.Clear();
			ArenaManager.Instance.AnimateLoadRandomArena(() => {
				RegisteredPlayersUtil.RegisterAIPlayers(4);
				PlayerSpawner.ShouldRespawn = true;
				PlayerSpawner.SpawnAllPlayers();
			});

			mainMenu_ = ObjectPoolManager.CreateView<MainMenu>(mainMenuPrefab_);
			mainMenu_.Init(battleHandler: GoToPlayerCustomization, levelEditorHandler: GoToLevelEditor);
		}

		private void GoToPlayerCustomization() {
			mainMenu_.AnimateOut(StateMachine_.GoToPlayerCustomization);
		}

		private void GoToLevelEditor() {
			mainMenu_.AnimateOut(StateMachine_.GoToLevelEditor);
		}

		protected override void OnStateExited() {
			BattleCamera.SetDepthOfFieldEnabled(false, animate: true);
			PlayerScores.Clear();
			RegisteredPlayers.Clear();
			PlayerSpawner.ShouldRespawn = false;
			PlayerSpawner.CleanupAllPlayers();

			if (mainMenu_ != null) {
				ObjectPoolManager.Recycle(mainMenu_);
				mainMenu_ = null;
			}
		}
	}
}