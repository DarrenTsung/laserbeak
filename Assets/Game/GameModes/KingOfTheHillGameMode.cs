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

using DT.Game.GameModes.KingOfTheHill;

namespace DT.Game.GameModes {
	[CreateAssetMenu(fileName = "KingOfTheHillGameMode", menuName = "Game/Modes/KingOfTheHillGameMode")]
	public class KingOfTheHillGameMode : GameMode, IKotHScoreSource {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return "KING OF THE HILL"; }
		}

		public override int Id {
			get { return GameMode.GetIdFor<KingOfTheHillGameMode>(); }
		}


		// PRAGMA MARK - IKotHScoreSource Implementation
		public event Action OnAnyScoreChanged = delegate {};

		public float GetPercentageScoreFor(Player player) {
			return timeSpentInArea_.GetValueOrDefault(player) / kTimeToWin;
		}


		// PRAGMA MARK - Internal
		// seconds
		private const float kTimeToWin = 5.0f;

		private readonly Dictionary<Player, float> timeSpentInArea_ = new Dictionary<Player, float>();
		private KingOfTheHillScoreView scoreView_ = null;

		protected override void Activate() {
			timeSpentInArea_.Clear();

			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
				icons.Add(GameModeIntroView.Icon.Swords);
			}
			icons.RemoveLast();

			GameModeIntroView.Show(DisplayTitle, icons);

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
			PlayerSpawner.ShouldRespawn = true;

			CleanupScoreView();
			scoreView_ = ObjectPoolManager.CreateView<KingOfTheHillScoreView>(GamePrefabs.Instance.KotHScoreViewPrefab);
			scoreView_.Init(this);
		}

		protected override void CleanupInternal() {
			PlayerSpawner.ShouldRespawn = false;
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;

			CleanupScoreView();
		}

		private void CleanupScoreView() {
			if (scoreView_ != null) {
				ObjectPoolManager.Recycle(scoreView_);
				scoreView_ = null;
			}
		}

		private void HandleUpdate() {
			ICollection<BattlePlayer> activePlayersInArea = ArenaManager.Instance.LoadedArena.KingOfTheHillArea.ActivePlayers;
			if (activePlayersInArea.Count <= 0 || activePlayersInArea.Count > 1) {
				return;
			}

			var playerCollectingScore = PlayerSpawner.GetPlayerFor(activePlayersInArea.First());
			timeSpentInArea_[playerCollectingScore] = timeSpentInArea_.GetValueOrDefault(playerCollectingScore) + Time.deltaTime;
			OnAnyScoreChanged.Invoke();
			CheckIfPlayerWon(playerCollectingScore);
		}

		private void CheckIfPlayerWon(Player player) {
			float percentage = GetPercentageScoreFor(player);
			if (percentage >= 1.0f) {
				PlayerScores.IncrementPendingScoreFor(player);
				Finish();
			}
		}
	}
}