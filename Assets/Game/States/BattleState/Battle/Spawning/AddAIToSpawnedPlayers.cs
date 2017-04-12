using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Player;

namespace DT.Game.Battle.AI {
	[RequireComponent(typeof(SpawnDummyPlayerIndefinitely))]
	public class AddAIToSpawnedPlayers : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject aiPrefab_;

		[SerializeField]
		private AIConfiguration aiConfiguration_;

		private void Awake() {
			var dummySpawner = this.GetRequiredComponent<SpawnDummyPlayerIndefinitely>();
			dummySpawner.OnPlayerSpawned += HandlePlayerSpawned;
		}

		private void HandlePlayerSpawned(BattlePlayer player) {
			AIStateMachine ai = ObjectPoolManager.Create<AIStateMachine>(aiPrefab_, parent: player.gameObject);
			ai.Init(player, aiConfiguration_);
		}
	}
}