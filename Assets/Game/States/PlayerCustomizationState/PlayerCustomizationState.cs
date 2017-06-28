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
		// PRAGMA MARK - Internal
		protected override void OnStateEntered() {
			RegisteredPlayersUtil.UnregisterAIPlayers();
			RegisteredPlayers.BeginPlayerRegistration();
			PlayerCustomizationView.Show(GoBack, Continue);

			PlayerSpawner.ShouldRespawn = true;

			// in case where no players to customize - continue
			if (RegisteredPlayers.AllPlayers.Count <= 0) {
				Continue();
				return;
			}
		}

		protected override void OnStateExited() {
			BattleRecyclables.Clear();
			PlayerSpawner.CleanupAllPlayers();

			PlayerSpawner.ShouldRespawn = false;

			PlayerCustomizationView.Hide();
			RegisteredPlayers.FinishPlayerRegistration();
			StatsManager.ClearAllStats();
		}

		private void Continue() {
			// TODO (darren): do this is a different way later
			// when we have actual AI selection (customization)
			if (GameConstants.Instance.FillWithAI) {
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