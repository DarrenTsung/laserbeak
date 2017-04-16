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

		public static void Clear() {
			pendingScoreMap_.Clear();
			scoreMap_.Clear();
		}

		public static void IncrementPendingScoreFor(Player player) {
			pendingScoreMap_[player] = GetPendingScore(player) + 1;
		}

		public static bool HasPendingScores {
			get { return pendingScoreMap_.Values.Any(score => score > 0); }
		}

		public static void StepConvertPendingScoresToScores() {
			foreach (Player player in pendingScoreMap_.Keys.ToArray()) {
				int pendingScore = GetPendingScore(player);
				if (pendingScore <= 0) {
					continue;
				}

				pendingScoreMap_[player] = pendingScore - 1;
				scoreMap_[player] = GetScoreFor(player) + 1;
				OnPlayerScoresChanged.Invoke();
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