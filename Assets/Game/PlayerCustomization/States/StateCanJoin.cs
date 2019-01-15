using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.States {
	public class StateCanJoin : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public StateCanJoin(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState)
						: base(player, container, moveToNextState, moveToPreviousState) {}

		public override void Update() {
			if (Player_.Input.PositiveWasPressed) {
				GameNotifications.OnPlayerJoinedGame.Invoke(Player_);
				MoveToNextState();
			}
		}

		public override void Cleanup() {
			Container_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		protected override void Init() {
			if (string.IsNullOrEmpty(Player_.Nickname)) {
				Player_.Nickname = RegisteredPlayers.GetDefaultNicknameFor(Player_);
			}
			Player_.Skin = null;

			ObjectPoolManager.Create(GamePrefabs.Instance.CanJoinViewPrefab, parent: Container_);
		}
	}
}