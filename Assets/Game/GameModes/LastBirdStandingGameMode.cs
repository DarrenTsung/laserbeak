using System;
using System.Collections;
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
	[CreateAssetMenu(fileName = "LastBirdStandingGameMode", menuName = "Game/Modes/LastBirdStandingGameMode")]
	public class LastBirdStandingGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override void Cleanup() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;
		}


		// PRAGMA MARK - Internal
		// TODO (darren): make this tied to the intro game state UI
		private const float kInitialInputDelay = 1.5f;

		protected override void Activate() {
			ArenaManager.Instance.LoadRandomArena();
			PlayerSpawner.SpawnAllPlayers();

			foreach (BattlePlayer battlePlayer in PlayerSpawner.AllSpawnedBattlePlayers) {
				battlePlayer.InputController.DisableInput(BattlePlayerInputController.PriorityKey.GameMode);
			}

			CoroutineWrapper.DoAfterDelay(kInitialInputDelay, () => {
				foreach (BattlePlayer battlePlayer in PlayerSpawner.AllSpawnedBattlePlayers) {
					battlePlayer.InputController.ClearInput(BattlePlayerInputController.PriorityKey.GameMode);
				}
			});

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 1) {
				return;
			}

			Finish();
			foreach (Player player in PlayerSpawner.AllSpawnedPlayers) {
				PlayerScores.IncrementPendingScoreFor(player);
			}
		}
	}
}