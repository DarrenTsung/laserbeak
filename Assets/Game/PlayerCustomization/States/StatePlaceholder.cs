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
	public class StatePlaceholder : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public StatePlaceholder(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState, string text)
								: base(player, container, moveToNextState, moveToPreviousState) {
			Container_.GetComponentInChildren<TMP_Text>().text = text;
		}

		public override void Update() {
			if (InputUtil.WasPositivePressedFor(Player_.InputDevice)) {
				MoveToNextState();
			}

			if (InputUtil.WasNegativePressedFor(Player_.InputDevice)) {
				MoveToPreviousState();
			}
		}

		public override void Cleanup() {
			Container_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		protected override void Init() {
			var view = ObjectPoolManager.Create(GamePrefabs.Instance.PlayerReadyView, parent: Container_);
			view.GetComponentInChildren<TMP_Text>().text = "";
			view.GetComponentInChildren<Image>().enabled = false;
		}
	}
}