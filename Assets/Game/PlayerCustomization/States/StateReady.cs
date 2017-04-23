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
	public class StateReady : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public StateReady(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState)
						: base(player, container, moveToNextState, moveToPreviousState) {}

		public override void Update() {
			if (InputUtil.WasNegativePressedFor(Player_.InputDevice)) {
				MoveToPreviousState();
			}
		}

		public override void Cleanup() {
			Container_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		protected override void Init() {
			ObjectPoolManager.Create(GamePrefabs.Instance.PlayerReadyView, parent: Container_);
		}
	}
}