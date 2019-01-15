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

		public void AnimateLoadRandomArena(Action callback) {
			ArenaConfig config = arenas_.Random();
			AnimateLoadArena(config, callback);
		}

		public void AnimateLoadArena(ArenaConfig arenaConfig, Action callback) {
			if (animating_) {
				TriggerQueuedCallback();
				queuedAnimatedArenaLoad_ = arenaConfig;
				queuedCallback_ = callback;
				return;
			}

			Action wrappedCallback = () => {
				animating_ = false;
				if (callback != null) {
					callback.Invoke();
				}

				if (queuedAnimatedArenaLoad_ != null) {
					ArenaConfig queuedArena = queuedAnimatedArenaLoad_;
					queuedAnimatedArenaLoad_ = null;
					AnimateLoadArena(queuedArena, TriggerQueuedCallback);
				}
			};

			animating_ = true;
			if (loadedArena_ != null) {
				loadedArenaBackdrop_.AnimateOut();
				loadedArena_.AnimateOut(() => {
					CleanupLoadedArena();
					CreateArena(arenaConfig, animate: true, callback: wrappedCallback);
				});
			} else {
				CleanupLoadedArena();
				CreateArena(arenaConfig, animate: true, callback: wrappedCallback);
			}
		}

		public void CleanupLoadedArena() {
			if (loadedArena_ != null) {
				ObjectPoolManager.Recycle(loadedArena_.GameObject);
				loadedArena_.Dispose();
				loadedArena_ = null;
			}

			if (loadedArenaBackdrop_ != null) {
				loadedArenaBackdrop_.Dispose();
				loadedArenaBackdrop_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private ArenaConfig[] arenas_;
		[SerializeField]
		private GameObject backdropContainer_;

		private Arena loadedArena_;
		private IArenaBackdrop loadedArenaBackdrop_;

		private bool animating_;

		private ArenaConfig queuedAnimatedArenaLoad_;
		private Action queuedCallback_;

		private void TriggerQueuedCallback() {
			if (queuedCallback_ != null) {
				queuedCallback_.Invoke();
				queuedCallback_ = null;
			}
		}

		private void CreateArena(ArenaConfig arenaConfig, bool animate, Action callback = null) {
			GameObject arenaObject = arenaConfig.CreateArena(parent: this.gameObject);
			loadedArena_ = new Arena(arenaObject);

			if (animate) {
				loadedArenaBackdrop_ = new RandomizedArenaBackdrop(backdropContainer_, arenaObject);
				loadedArena_.AnimateIn(callback);
			} else {
				if (callback != null) {
					callback.Invoke();
				}
			}
		}
	}
}