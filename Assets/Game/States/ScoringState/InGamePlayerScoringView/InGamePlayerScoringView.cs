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
	public class InGamePlayerScoringView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void Show(Action onFinishedCallback) {
			var view = ObjectPoolManager.CreateView<InGamePlayerScoringView>(GamePrefabs.Instance.InGamePlayerScoringView);
			view.Init(onFinishedCallback);
		}


		// PRAGMA MARK - Public Interface
		public void Init(Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var individualPlayerScoringView = ObjectPoolManager.Create<IndividualPlayerScoringView>(GamePrefabs.Instance.IndividualPlayerScoringViewPrefab, parent: scoringViewContainer_);
				individualPlayerScoringView.Init(player);
			}

			// TODO (darren): animate in
			this.DoAfterDelay(kBeforeScoringDelay, () => {
				StartCoroutine(DoScoring());
			});
			hasWinnerContainer_.SetActive(false);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			scoringViewContainer_.transform.RecycleAllChildren();
			hasWinnerContainer_.SetActive(false);
		}


		// PRAGMA MARK - Internal
		private const float kBeforeScoringDelay = 1.0f;
		private const float kBetweenScoringDelay = 1.0f;
		private const float kEndScoringDelay = 2.0f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject scoringViewContainer_;

		[SerializeField]
		private GameObject hasWinnerContainer_;

		private Action onFinishedCallback_;

		private IEnumerator DoScoring() {
			WaitForSeconds betweenScoringDelay = new WaitForSeconds(kBetweenScoringDelay);
			while (PlayerScores.HasPendingScores) {
				AudioConstants.Instance.ScoreAdded.PlaySFX();
				PlayerScores.StepConvertPendingScoresToScores();

				if (!PlayerScores.HasPendingScores || PlayerScores.HasWinner) {
					break;
				}

				yield return betweenScoringDelay;
			}

			if (PlayerScores.HasWinner) {
				GameNotifications.OnGameWon.Invoke();
				hasWinnerContainer_.SetActive(true);
				AudioConstants.Instance.Win.PlaySFX(randomPitchRange: 0.0f);
				// wait for main button press
				while (!InputUtil.WasAnyCommandButtonPressed()) {
					yield return null;
				}
			} else {
				yield return new WaitForSeconds(kEndScoringDelay);
			}

			Finish();
		}

		private void Finish() {
			onFinishedCallback_.Invoke();
			ObjectPoolManager.Recycle(this);
		}
	}
}