using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.MainMenu {
	public class MainMenuState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject mainMenuPrefab_;

		private MainMenu mainMenu_;

		protected override void OnStateEntered() {
			ArenaManager.Instance.LoadRandomArena();

			RegisteredPlayers.Clear();
			RegisteredPlayersUtil.RegisterAIPlayers(4);
			PlayerSpawner.ShouldRespawn = true;
			PlayerSpawner.SpawnAllPlayers();

			mainMenu_ = ObjectPoolManager.CreateView<MainMenu>(mainMenuPrefab_);
			mainMenu_.SetPlayHandler(() => StateMachine_.Continue());
		}

		protected override void OnStateExited() {
			RegisteredPlayers.Clear();
			PlayerSpawner.ShouldRespawn = false;
			PlayerSpawner.CleanupAllPlayers();
			BattleRecyclables.Clear();

			if (mainMenu_ != null) {
				ObjectPoolManager.Recycle(mainMenu_);
				mainMenu_ = null;
			}
		}
	}
}