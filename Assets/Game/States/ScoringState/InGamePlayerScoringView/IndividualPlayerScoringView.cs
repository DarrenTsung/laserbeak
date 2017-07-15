using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
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
			RefreshAttributeText(animate: false);

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

			this.StopAllCoroutines();
			startScoreBar_ = null;
			player_ = null;
		}


		// PRAGMA MARK - Internal
		private const float kDampingRatio = 0.35f;
		private const float kDuration = 0.4f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;
		[SerializeField]
		private GameObject scoresContainer_;

		[Space]
		[SerializeField]
		private GameObject attributeContainer_;
		[SerializeField]
		private TextOutlet attributeTextOutlet_;

		private Player player_;

		private ScoreBarView startScoreBar_;

		private void HandlePlayerScoresChanged() {
			RefreshAttributeText(animate: true);
			RefreshScoreBar(animate: true);
		}

		private void RefreshScoreBar(bool animate) {
			int playerScore = PlayerScores.GetScoreFor(player_);
			startScoreBar_.SetScoreCount(playerScore, player_.Skin.BodyColor, animate);
		}

		private void RefreshAttributeText(bool animate) {
			string attribute = null;
			int playerScore = PlayerScores.GetScoreFor(player_);
			if (playerScore > 0) {
				if (PlayerScores.Winner == player_) {
					attribute = "WINNER!";
				} else if (PlayerScores.GetRankFor(player_) == 1) {
					attribute = "LEADER!";
				}
			}

			attributeTextOutlet_.Text = attribute;
			attributeTextOutlet_.Color = player_.Skin.BodyColor;

			bool showing = !string.IsNullOrEmpty(attribute);
			float endScale = showing ? 1.0f : 0.0f;
			float currentScale = attributeContainer_.transform.localScale.x;
			if (endScale == currentScale) {
				return;
			}

			if (animate) {
				this.StopAllCoroutines();

				this.DoSpringFor(kDuration, kDampingRatio, (float p) => {
					float scale = Mathf.LerpUnclamped(currentScale, endScale, p);
					attributeContainer_.transform.localScale = Vector3.one * scale;
				});
			} else {
				attributeContainer_.transform.localScale = Vector3.one * endScale;
			}
		}
	}
}