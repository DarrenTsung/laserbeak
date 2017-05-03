using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.PlayerCustomization {
	public class PlayerCustomizationState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		protected override void OnStateEntered() {
			RegisteredPlayers.Clear();
			RegisteredPlayers.BeginPlayerRegistration();
			PlayerCustomizationView.Show(Continue);

			// in case where no players to customize - continue
			if (RegisteredPlayers.AllPlayers.Count <= 0) {
				Continue();
				return;
			}
		}

		protected override void OnStateExited() {
			PlayerCustomizationView.Hide();
			RegisteredPlayers.FinishPlayerRegistration();
		}

		private void Continue() {
			// TODO (darren): do this is a different way later
			// when we have actual AI selection (customization)
			int missingPlayersCount = Math.Max(0, GameConstants.Instance.PlayersToFill - RegisteredPlayers.AllPlayers.Count);
			RegisteredPlayersUtil.RegisterAIPlayers(missingPlayersCount);

			StateMachine_.Continue();
		}
	}
}