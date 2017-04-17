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
		public override void Cleanup() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private int numberOfTeams_ = 2;

		private HashSet<Player>[] teams_;

		protected override void Activate() {
			ArenaManager.Instance.LoadRandomArena();
			PlayerSpawner.SpawnAllPlayers();

			teams_ = new HashSet<Player>[numberOfTeams_];

			int playerCount = RegisteredPlayers.AllPlayers.Count;

			int playersPerTeam = (int)(playerCount / (float)numberOfTeams_);
			int extraPlayers = playerCount % numberOfTeams_;

			int[] teamMap = new int[playerCount];
			int teamMapCounter = 0;
			for (int n = 0; n < numberOfTeams_; n++) {
				for (int p = 0; p < playersPerTeam; p++) {
					teamMap[teamMapCounter] = n;
					teamMapCounter++;
				}

				if (extraPlayers > 0) {
					teamMap[teamMapCounter] = n;
					teamMapCounter++;
					extraPlayers--;
				}
			}

			for (int i = 0; i < playerCount; i++) {
				Player player = RegisteredPlayers.AllPlayers[i];

				int teamIndex = teamMap[i];
				if (teams_[teamIndex] == null) {
					teams_[teamIndex] = new HashSet<Player>();
				}
				teams_[teamIndex].Add(player);
			}

			foreach (HashSet<Player> team in teams_) {
				BattlePlayerTeams.DeclareTeam(team.Select(p => PlayerSpawner.GetBattlePlayerFor(p)));
			}

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			for (int i = 0; i < teams_.Length - 1; i++) {
				icons.AddRange(teams_[i].Select(p => GameModeIntroView.Icon.Player));
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.AddRange(teams_[teams_.Length - 1].Select(p => GameModeIntroView.Icon.Player));

			GameModeIntroView.Show("TEAM BATTLE", icons);

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
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