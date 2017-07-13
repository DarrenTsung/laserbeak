using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Pausing;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

using DTCommandPalette;

namespace DT.Game.Battle {
	public static class BattleStateCommands {
		// PRAGMA MARK - Static
		[DTCommandPalette.MethodCommand]
		public static void LoadGameMode() {
			var commandManager = new CommandManager();
			foreach (GameMode gameMode in GameConstants.Instance.GameModes) {
				commandManager.AddCommand(new GenericCommand(gameMode.DisplayTitle, () => {
					BattleState.SkipAndLoadGameMode(gameMode);
				}));
			}

			CommandPaletteWindow.InitializeWindow("Load Game Mode..", commandManager, clearInput: true);
		}
	}
}