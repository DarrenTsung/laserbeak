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

namespace DT.Game.RegisterPlayers {
	public class RegisterPlayersNoInputState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private BattlePlayerSkin[] playerSkins_;

		protected override void OnStateEntered() {
			foreach (InputDevice inputDevice in InputManager.Devices) {
				if (RegisteredPlayers.IsInputDeviceAlreadyRegistered(inputDevice)) {
					continue;
				}

				Player player = new Player(inputDevice);

				StringBuilder nicknameBuilder = new StringBuilder();
				nicknameBuilder.Append(CharUtil.RandomUppercaseLetter());
				nicknameBuilder.Append(CharUtil.RandomUppercaseLetter());
				nicknameBuilder.Append(CharUtil.RandomUppercaseLetter());
				player.Nickname = nicknameBuilder.ToString();

				player.Skin = RegisteredPlayersUtil.GetBestRandomSkin();

				RegisteredPlayers.Add(player);
			}

			// TODO (darren): do this is a different way later
			// when we have actual AI selection
			int missingPlayersCount = 4 - RegisteredPlayers.AllPlayers.Count;
			RegisteredPlayersUtil.RegisterAIPlayers(missingPlayersCount);

			StateMachine_.Continue();
		}

		protected override void OnStateExited() {
			// stub
		}
	}
}