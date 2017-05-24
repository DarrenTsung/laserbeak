using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEditor;
using UnityEngine;

using DTCommandPalette;

namespace DT.Game.Battle {
	public class ArenaCommandLoader : AssetCommandLoader<ArenaConfig> {
		// PRAGMA MARK - Static
		[MethodCommand]
		private static void LoadArena() {
			var commandManager = new CommandManager();
			commandManager.AddLoader(new ArenaCommandLoader());

			CommandPaletteWindow.InitializeWindow("Load Arena..", commandManager, clearInput: true);
		}


		// PRAGMA MARK - Internal
		protected override void HandleAssetExecuted(ArenaConfig asset) {
			if (!Application.isPlaying) {
				Debug.LogWarning("Can't load arena unless application is running!");
				return;
			}

			ArenaManager.Instance.LoadArena(asset);
		}
	}
}