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

namespace DT.Game.Scoring {
	public class IndividualPlayerScoringView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Player player) {
			player_ = player;

			var inGamePlayerView = ObjectPoolManager.Create<InGamePlayerView>(GamePrefabs.Instance.InGamePlayerViewPrefab, parent: playerViewContainer_);
			inGamePlayerView.InitWith(player);

			scoreBubbleViews_ = new ScoreBubbleView[GameConstants.Instance.ScoreToWin];
			for (int i = 0; i < GameConstants.Instance.ScoreToWin; i++) {
				scoreBubbleViews_[i] = ObjectPoolManager.Create<ScoreBubbleView>(GamePrefabs.Instance.ScoreBubbleViewPrefab, parent: scoresContainer_);
			}

			RefreshScoreBubbles(animate: false);

			PlayerScores.OnPlayerScoresChanged += HandlePlayerScoresChanged;
			PlayerScores.OnPlayerWon += HandlePlayerWon;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			wonContainer_.SetActive(false);
			statsContainer_.gameObject.SetActive(false);

			scoresContainer_.gameObject.SetActive(true);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewContainer_.transform.RecycleAllChildren();
			scoresContainer_.transform.RecycleAllChildren();

			PlayerScores.OnPlayerScoresChanged -= HandlePlayerScoresChanged;
			PlayerScores.OnPlayerWon -= HandlePlayerWon;

			player_ = null;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;

		[SerializeField]
		private GameObject scoresContainer_;

		[SerializeField]
		private GameObject wonContainer_;

		[SerializeField]
		private StatsContainer statsContainer_;

		private Player player_;
		private ScoreBubbleView[] scoreBubbleViews_;

		private void HandlePlayerScoresChanged() {
			RefreshScoreBubbles(animate: true);
		}

		private void RefreshScoreBubbles(bool animate) {
			int playerScore = PlayerScores.GetScoreFor(player_);
			for (int i = 0; i < GameConstants.Instance.ScoreToWin; i++) {
				int flippedIndex = GameConstants.Instance.ScoreToWin - 1 - i;
				scoreBubbleViews_[i].SetFilled(flippedIndex < playerScore, animate);
			}
		}

		private void HandlePlayerWon() {
			statsContainer_.Init(player_);
			statsContainer_.gameObject.SetActive(true);
			scoresContainer_.gameObject.SetActive(false);

			if (PlayerScores.Winner != player_) {
				return;
			}

			wonContainer_.SetActive(true);
		}
	}
}