using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
			inGamePlayerView.InitWith(player, enableNudge: true);

			scoreBubbleViews_ = new ScoreBubbleView[GameConstants.Instance.ScoreToWin];
			for (int i = 0; i < GameConstants.Instance.ScoreToWin; i++) {
				scoreBubbleViews_[i] = ObjectPoolManager.Create<ScoreBubbleView>(GamePrefabs.Instance.ScoreBubbleViewPrefab, parent: scoresContainer_);
			}

			RefreshScoreBubbles(animate: false);

			PlayerScores.OnPlayerScoresChanged += HandlePlayerScoresChanged;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			scoresContainer_.gameObject.SetActive(true);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewContainer_.transform.RecycleAllChildren();
			scoresContainer_.transform.RecycleAllChildren();

			PlayerScores.OnPlayerScoresChanged -= HandlePlayerScoresChanged;

			player_ = null;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;

		[SerializeField]
		private GameObject scoresContainer_;

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
	}
}