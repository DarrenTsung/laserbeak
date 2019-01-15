using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Transitions;

namespace DT.Game.GameModes {
	public class GameModesProgressionNextUnlockView : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void ShowIfPossible() {
			if (!InGameConstants.ShowNextGameModeUnlockView) {
				return;
			}

			if (GameModesProgression.RecentlyUnlockedGameMode == null && !GameModesProgression.HasLockedGameModes()) {
				return;
			}

			ObjectPoolManager.CreateView(GamePrefabs.Instance.GameModesProgressionNextUnlockViewPrefab);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			if (GameModesProgression.RecentlyUnlockedGameMode != null) {
				text_.Text = "Unlocked New Game Mode!";
			} else {
				int playsUntilNextUnlock = GameModesProgression.PlaysUntilNextUnlock();
				if (playsUntilNextUnlock > 1) {
					text_.Text = string.Format("New Game Mode in <b>{0}</b> games!", playsUntilNextUnlock);
				} else {
					text_.Text = string.Format("New Game Mode in <b>{0}</b> game!", playsUntilNextUnlock);
				}
			}

			transition_.AnimateIn(() => {
				this.DoAfterDelay(kDuration, () => {
					transition_.AnimateOut(() => {
						ObjectPoolManager.Recycle(this);
					});
				});
			});
		}


		// PRAGMA MARK - Internal
		private const float kDuration = 4.0f;

		[Header("Outlets")]
		[SerializeField]
		private TextOutlet text_;

		private Transition transition_;

		private void Awake() {
			transition_ = new Transition(this.gameObject);
		}
	}
}