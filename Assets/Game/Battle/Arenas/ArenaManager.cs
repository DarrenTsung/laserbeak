using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;

namespace DT.Game.Battle {
	public class ArenaManager : Singleton<ArenaManager> {
		// PRAGMA MARK - Public Interface
		public Arena LoadedArena {
			get { return loadedArena_; }
		}

		public void LoadRandomArena() {
			ArenaConfig config = arenas_.Random();
			LoadArena(config);
		}

		public void LoadArena(ArenaConfig arenaConfig) {
			CleanupLoadedArena();
			loadedArena_ = new Arena(arenaConfig.CreateArena(parent: this.gameObject));
		}

		public void CleanupLoadedArena() {
			if (loadedArena_ != null) {
				ObjectPoolManager.Recycle(loadedArena_.GameObject);
				loadedArena_.Dispose();
				loadedArena_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private ArenaConfig[] arenas_;

		private Arena loadedArena_;
	}
}