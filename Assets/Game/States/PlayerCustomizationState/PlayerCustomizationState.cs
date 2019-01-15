using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Battle.Stats;
using DT.Game.Players;

namespace DT.Game.PlayerCustomization {
	public class PlayerCustomizationState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Static
		public static bool LobbyArenaLoaded {
			get; private set;
		}


		// PRAGMA MARK - Internal
		private CornerDelayedActionView delayedBackToMainMenuView_;

		protected override void OnStateEntered() {
			LobbyArenaLoaded = false;

			RegisteredPlayersUtil.UnregisterAIPlayers();
			RegisteredPlayers.BeginPlayerRegistration();
			PlayerCustomizationView.Show(Continue);
			InGameConstants.BattlePlayerPartsFade = true;

			PlayerSpawner.ShouldRespawn = true;

			delayedBackToMainMenuView_ = CornerDelayedActionView.Show("BACK TO MAIN MENU", CornerPoint.TopLeft, ActionType.Negative, GoBack);

			// in case where no players to customize - continue
			if (RegisteredPlayers.AllPlayers.Count <= 0) {
				Continue();
				return;
			} else {
				ArenaManager.Instance.AnimateLoadArena(GameConstants.Instance.PlayerCustomizationLobbyArena, callback: () => {
					LobbyArenaLoaded = true;
				});
			}
		}

		protected override void OnStateExited() {
			BattleRecyclables.Clear();
			PlayerSpawner.CleanupAllPlayers();
			InGameConstants.BattlePlayerPartsFade = false;

			PlayerSpawner.ShouldRespawn = false;
			LobbyArenaLoaded = false;

			PlayerCustomizationView.Hide();
			RegisteredPlayers.FinishPlayerRegistration();
			StatsManager.ClearAllStats();

			if (delayedBackToMainMenuView_ != null) {
				delayedBackToMainMenuView_.AnimateOutAndRecycle();
				delayedBackToMainMenuView_ = null;
			}
		}

		private void Continue() {
			// TODO (darren): do this is a different way later
			// when we have actual AI selection (customization)
			if (InGameConstants.FillWithAI) {
				int missingPlayersCount = Math.Max(0, GameConstants.Instance.PlayersToFill - RegisteredPlayers.AllPlayers.Count);
				RegisteredPlayersUtil.RegisterAIPlayers(missingPlayersCount);
			}

			StateMachine_.Continue();
		}

		private void GoBack() {
			StateMachine_.GoToMainMenu();
		}
	}
}