using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.PlayerCustomization.Nickname;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.States {
	public class StateNicknameCustomization : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public StateNicknameCustomization(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState)
						: base(player, container, moveToNextState, moveToPreviousState) {}

		public override void Update() {
			if (InputUtil.WasNegativePressedFor(Player_.InputDevice)) {
				MoveToPreviousState();
			}
		}

		public override void Cleanup() {
			Container_.RecycleAllChildren();

			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private ElementSelectionView selectionView_;

		protected override void Init() {
			var nicknameView = ObjectPoolManager.Create<PlayerNicknameCustomizationView>(GamePrefabs.Instance.NicknameCustomizationViewPrefab, parent: Container_);
			nicknameView.Init(Player_, onFinishCustomization: MoveToNextState);

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(Player_, nicknameView.gameObject);
		}
	}
}