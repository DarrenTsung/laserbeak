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
	public static class PlayerScores {
		// PRAGMA MARK - Static Public Interface
		public static event Action OnPlayerScoresChanged = delegate {};
		public static event Action OnPlayerWon = delegate {};

		public static void Clear() {
			pendingScoreMap_.Clear();
			scoreMap_.Clear();
		}

		public static void IncrementPendingScoreFor(Player player) {
			if (HasWinner) {
				Debug.LogWarning("Shouldn't change pending scores while HasWinner!");
				return;
			}

			pendingScoreMap_[player] = GetPendingScore(player) + 1;
		}

		public static bool HasPendingScores {
			get { return pendingScoreMap_.Values.Any(score => score > 0); }
		}

		public static Player Winner {
			get { return scoreMap_.FirstOrDefault(kvp => kvp.Value >= GameConstants.Instance.ScoreToWin).Key; }
		}

		public static bool HasWinner {
			get { return Winner != null; }
		}

		public static int GetRankFor(Player player) {
			int playerScore = GetScoreFor(player);
			return 1 + RegisteredPlayers.AllPlayers.Where(p => p != player && GetScoreFor(p) > playerScore).Count();
		}

		public static void StepConvertPendingScoresToScores() {
			if (HasWinner) {
				Debug.LogWarning("Shouldn't convert scores while HasWinner!");
				return;
			}

			foreach (Player player in pendingScoreMap_.Keys.ToArray()) {
				int pendingScore = GetPendingScore(player);
				if (pendingScore <= 0) {
					continue;
				}

				pendingScoreMap_[player] = pendingScore - 1;
				scoreMap_[player] = GetScoreFor(player) + 1;
				OnPlayerScoresChanged.Invoke();
			}

			if (HasWinner) {
				OnPlayerWon.Invoke();
			}
		}

		public static int GetScoreFor(Player player) {
			return scoreMap_.GetValueOrDefault(player, defaultValue: 0);
		}


		// PRAGMA MARK - Static Internal
		private static readonly Dictionary<Player, int> scoreMap_ = new Dictionary<Player, int>();
		private static readonly Dictionary<Player, int> pendingScoreMap_ = new Dictionary<Player, int>();

		private static int GetPendingScore(Player player) {
			return pendingScoreMap_.GetValueOrDefault(player, defaultValue: 0);
		}
	}
}