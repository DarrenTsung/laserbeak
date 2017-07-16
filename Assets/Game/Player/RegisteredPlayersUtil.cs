using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;

namespace DT.Game.Players {
	public static class RegisteredPlayersUtil {
		// PRAGMA MARK - Static Public Interface
		public static BattlePlayerSkin GetBestRandomSkin() {
			BattlePlayerSkin[] skins = GameConstants.Instance.PlayerSkins;

			BattlePlayerSkin chosenSkin = skins.Random();
			// NOTE (darren): could do a better random here..
			while (SkinAlreadyInUse(chosenSkin) && !skins.All(SkinAlreadyInUse)) {
				chosenSkin = skins.Random();
			}

			return chosenSkin;
		}

		public static void RegisterAIPlayers(int count) {
			for (int i = 1; i <= count; i++) {
				Player player = new Player(null);
				player.Nickname = "AI-" + i;
				player.Skin = GetBestRandomSkin();

				RegisteredPlayers.Add(player);
			}
		}

		public static void RegisterDebugHumanPlayers() {
			int i = 1;
			foreach (InputDevice device in InputManager.Devices) {
				Player player = new Player(device);
				player.Nickname = "DEBUG-" + i;
				player.Skin = GetBestRandomSkin();

				RegisteredPlayers.Add(player);
				i++;
			}
		}

		public static void UnregisterAIPlayers() {
			foreach (Player player in RegisteredPlayers.AllPlayers.ToList()) {
				if (player.IsAI) {
					RegisteredPlayers.Remove(player);
				}
			}
		}


		// PRAGMA MARK - Static Internal
		private static bool SkinAlreadyInUse(BattlePlayerSkin skin) {
			return RegisteredPlayers.AllPlayers.Any(p => p.Skin == skin);
		}
	}
}