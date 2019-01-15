using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.States {
	public class StateReady : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public StateReady(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState)
						: base(player, container, moveToNextState, moveToPreviousState) {}

		public override void Update() {
			if (Player_.Input.NegativeWasPressed) {
				MoveToPreviousState();
			}

			if (spawnPlayerAfterLobbyLoaded_ && PlayerCustomizationState.LobbyArenaLoaded) {
				SpawnPlayer();
				spawnPlayerAfterLobbyLoaded_ = false;
			}
		}

		public override void Cleanup() {
			spawnPlayerAfterLobbyLoaded_ = false;

			Container_.RecycleAllChildren();
			PlayerSpawner.CleanupForPlayer(Player_);
		}


		// PRAGMA MARK - Internal
		private bool spawnPlayerAfterLobbyLoaded_ = false;

		protected override void Init() {
			ObjectPoolManager.Create(GamePrefabs.Instance.PlayerReadyView, parent: Container_);

			if (!PlayerCustomizationState.LobbyArenaLoaded) {
				spawnPlayerAfterLobbyLoaded_ = true;
				return;
			}

			SpawnPlayer();
		}

		private void SpawnPlayer() {
			PlayerSpawner.ForceSpawnPlayer(Player_);
		}
	}
}