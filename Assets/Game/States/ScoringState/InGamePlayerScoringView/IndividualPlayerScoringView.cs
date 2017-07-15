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

			startScoreBar_ = ObjectPoolManager.Create<ScoreBarView>(GamePrefabs.Instance.ScoreBarViewPrefab, parent: scoresContainer_);
			RefreshScoreBar(animate: false);

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

			startScoreBar_ = null;

			player_ = null;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;

		[SerializeField]
		private GameObject scoresContainer_;

		private Player player_;

		private ScoreBarView startScoreBar_;

		private void HandlePlayerScoresChanged() {
			RefreshScoreBar(animate: true);
		}

		private void RefreshScoreBar(bool animate) {
			int playerScore = PlayerScores.GetScoreFor(player_);
			startScoreBar_.SetScoreCount(playerScore, player_.Skin.BodyColor, animate);
		}
	}
}