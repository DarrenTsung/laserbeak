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
			CleanupLoadedArena();
			ArenaConfig config = arenas_.Random();
			loadedArena_ = new Arena(ObjectPoolManager.Create(config.Prefab, parent: this.gameObject));
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