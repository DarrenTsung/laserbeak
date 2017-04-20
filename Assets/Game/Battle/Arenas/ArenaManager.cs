using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class ArenaManager : Singleton<ArenaManager> {
		// PRAGMA MARK - Public Interface
		public void LoadRandomArena() {
			ArenaConfig config = arenas_.Random();
			LoadArena(config);
		}

		public void LoadArena(ArenaConfig arenaConfig) {
			CleanupLoadedArena();
			loadedArena_ = new Arena(ObjectPoolManager.Create(arenaConfig.Prefab, parent: this.gameObject));
		}

		public Arena LoadedArena {
			get { return loadedArena_; }
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private ArenaConfig[] arenas_;

		private Arena loadedArena_;

		private void CleanupLoadedArena() {
			if (loadedArena_ != null) {
				ObjectPoolManager.Recycle(loadedArena_.GameObject);
				loadedArena_.Dispose();
				loadedArena_ = null;
			}
		}
	}
}