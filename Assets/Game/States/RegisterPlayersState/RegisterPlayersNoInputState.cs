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

				player.Skin = GetBestSkin();

				RegisteredPlayers.Add(player);
			}

			StateMachine_.StartBattle();
		}

		protected override void OnStateExited() {
		}

		private bool SkinAlreadyInUse(BattlePlayerSkin skin) {
			return RegisteredPlayers.AllPlayers.Any(p => p.Skin == skin);
		}

		private BattlePlayerSkin GetBestSkin() {
			BattlePlayerSkin chosenSkin = playerSkins_.Random();
			// NOTE (darren): could do a better random here..
			while (SkinAlreadyInUse(chosenSkin) && !playerSkins_.All(SkinAlreadyInUse)) {
				chosenSkin = playerSkins_.Random();
			}

			return chosenSkin;
		}
	}
}