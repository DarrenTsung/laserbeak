using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes {
	[CreateAssetMenu(fileName = "TeamBattleGameMode", menuName = "Game/Modes/TeamBattleGameMode")]
	public class TeamBattleGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return "ANGELS VS DEMONS"; }
		}

		public override int Id {
			get { return GameMode.GetIdFor<TeamBattleGameMode>(); }
		}


		// PRAGMA MARK - Internal
		private const int kNumberOfTeams = 2;

		[Header("Outlets")]
		[SerializeField]
		private GameObject[] accessoryPrefabs_;

		private HashSet<Player>[] teams_;

		protected override void Activate() {
			PlayerSpawner.SpawnAllPlayers();

			// Sort the players from lowest score to highest score
			List<Player> players = RegisteredPlayers.AllPlayers.ToList();
			players.Sort((playerA, playerB) => {
				return PlayerScores.GetScoreFor(playerA).CompareTo(PlayerScores.GetScoreFor(playerB));
			});

			teams_ = new HashSet<Player>[kNumberOfTeams];
			if (players.Count == 4 && kNumberOfTeams == 2) {
				teams_[0] = new HashSet<Player>() { players[0], players[3] };
				teams_[1] = new HashSet<Player>() { players[1], players[2] };
			} else {
				Debug.LogWarning("Assumptions failed when creating teams - random teams!");

				players.Shuffle();

				int teamIndex = 0;
				foreach (Player player in players) {
					if (teams_[teamIndex] == null) {
						teams_[teamIndex] = new HashSet<Player>();
					}

					teams_[teamIndex].Add(player);

					teamIndex = MathUtil.Wrap(teamIndex + 1, 0, kNumberOfTeams);
				}
			}

			// set override color for players
			for (int i = 0; i < teams_.Length; i++) {
				HashSet<Player> team = teams_[i];

				foreach (Player player in team) {
					BattlePlayer battlePlayer = PlayerSpawner.GetBattlePlayerFor(player);
					ObjectPoolManager.Create(accessoryPrefabs_[i], parent: battlePlayer.AccessoriesContainer);
				}
			}

			foreach (HashSet<Player> team in teams_) {
				BattlePlayerTeams.DeclareTeam(team.Select(p => PlayerSpawner.GetBattlePlayerFor(p)));
			}

			List<int> playerOrdering = new List<int>();
			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			for (int i = 0; i < teams_.Length - 1; i++) {
				playerOrdering.AddRange(teams_[i].Select(p => RegisteredPlayers.AllPlayers.IndexOf(p)));
				icons.AddRange(teams_[i].Select(p => GameModeIntroView.Icon.Player));
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			playerOrdering.AddRange(teams_[teams_.Length - 1].Select(p => RegisteredPlayers.AllPlayers.IndexOf(p)));
			icons.AddRange(teams_[teams_.Length - 1].Select(p => GameModeIntroView.Icon.Player));

			GameModeIntroView.Show(DisplayTitle, icons, playerOrdering);

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		protected override void CleanupInternal() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
		}

		private void HandleSpawnedPlayerRemoved() {
			int? teamIndexLeft = null;
			for (int i = 0; i < teams_.Length; i++) {
				HashSet<Player> team  = teams_[i];
				bool teamAlive = team.Any(p => PlayerSpawner.IsAlive(p));
				if (teamAlive) {
					// if multiple teams still alive
					if (teamIndexLeft != null) {
						return;
					}

					teamIndexLeft = i;
				}
			}

			Finish();
			// possible that no teams are left alive
			if (teamIndexLeft != null) {
				foreach (Player player in teams_[(int)teamIndexLeft].Where(p => PlayerSpawner.IsAlive(p))) {
					PlayerScores.IncrementPendingScoreFor(player);
				}
			}
		}
	}
}