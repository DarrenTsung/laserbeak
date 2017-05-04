using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.GameModes;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Pausing {
	public class PauseController : IDisposable {
		// PRAGMA MARK - Public Interface
		public PauseController(Action restartCallback) {
			restartCallback_ = restartCallback;

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}


		// PRAGMA MARK - IDisposable Implementation
		public void Dispose() {
			ExitPause();
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
		}


		// PRAGMA MARK - Internal
		private Action exitPauseCallback_;
		private Action restartCallback_;

		private InGamePauseView view_;

		private void HandleUpdate() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				if (player.IsAI) {
					continue;
				}

				if (InputUtil.WasCommandPressedFor(player.InputDevice)) {
					PauseWith(player);
					return;
				}
			}
		}

		private void PauseWith(Player player) {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;

			view_ = ObjectPoolManager.CreateView<InGamePauseView>(GamePrefabs.Instance.InGamePauseViewPrefab);
			view_.Init(player, Resume, restartCallback_);

			foreach (BattlePlayer battlePlayer in UnityEngine.Object.FindObjectsOfType<BattlePlayer>()) {
				battlePlayer.InputController.DisableInput(BattlePlayerInputController.PriorityKey.Paused);
			}
		}

		private void Resume() {
			ExitPause();
			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		private void ExitPause() {
			if (view_ != null) {
				ObjectPoolManager.Recycle(view_);
				view_ = null;
			}

			foreach (BattlePlayer battlePlayer in UnityEngine.Object.FindObjectsOfType<BattlePlayer>()) {
				battlePlayer.InputController.ClearInput(BattlePlayerInputController.PriorityKey.Paused);
			}
		}
	}
}