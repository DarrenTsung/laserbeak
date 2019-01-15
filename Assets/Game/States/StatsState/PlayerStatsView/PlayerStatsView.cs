using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Stats {
	public class PlayerStatsView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static PlayerStatsView Show(Action playAgainCallback, Action goToMainMenuCallback) {
			var view = ObjectPoolManager.CreateView<PlayerStatsView>(GamePrefabs.Instance.PlayerStatsViewPrefab);
			view.Init(playAgainCallback, goToMainMenuCallback);
			return view;
		}


		// PRAGMA MARK - Public Interface
		public void Init(Action playAgainCallback, Action goToMainMenuCallback) {
			playAgainCallback_ = playAgainCallback;
			goToMainMenuCallback_ = goToMainMenuCallback;

			playAgainView_ = CornerDelayedActionView.Show("PLAY ANOTHER ROUND", CornerPoint.BottomRight, ActionType.Positive, () => {
				if (playAgainCallback_ != null) {
					playAgainCallback_.Invoke();
					playAgainCallback_ = null;
				}
			});

			backToMainMenuView_ = CornerDelayedActionView.Show("BACK TO MAIN MENU", CornerPoint.BottomLeft, ActionType.Negative, () => {
				if (goToMainMenuCallback_ != null) {
					goToMainMenuCallback_.Invoke();
					goToMainMenuCallback_ = null;
				}
			});

			unlockedGameModesText_.SetActive(false);
			if (GameModesProgression.HasLockedGameModes()) {
				unlockedGameModesText_.Text = string.Format("<size=33><b>{0}/{1}</b></size> GAME MODES UNLOCKED!", GameModesProgression.FilterByUnlocked(GameConstants.Instance.GameModes).Count(), GameConstants.Instance.GameModes.Length);
				unlockedGameModesText_.SetActive(true);
			}
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var individualPlayerStatsView = ObjectPoolManager.Create<IndividualPlayerStatsView>(GamePrefabs.Instance.IndividualPlayerStatsViewPrefab, parent: playerViewsContainer_);
				individualPlayerStatsView.Init(player);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewsContainer_.transform.RecycleAllChildren();

			if (backToMainMenuView_ != null) {
				backToMainMenuView_.AnimateOutAndRecycle();
				backToMainMenuView_ = null;
			}

			if (playAgainView_ != null) {
				playAgainView_.AnimateOutAndRecycle();
				playAgainView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewsContainer_;
		[SerializeField]
		private TextOutlet unlockedGameModesText_;

		private Action playAgainCallback_;
		private Action goToMainMenuCallback_;

		private CornerDelayedActionView playAgainView_;
		private CornerDelayedActionView backToMainMenuView_;
	}
}