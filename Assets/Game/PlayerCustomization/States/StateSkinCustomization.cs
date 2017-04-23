using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.PlayerCustomization.Skin;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.States {
	public class StateSkinCustomization : IndividualPlayerCustomizationState {
		// PRAGMA MARK - Static
		private static readonly Dictionary<Player, bool> selectingSkinMap_ = new Dictionary<Player, bool>();

		public static event Action OnSelectedSkinsDirty = delegate {};

		public static bool IsSkinSelected(BattlePlayerSkin skin) {
			return RegisteredPlayers.AllPlayers.Where(p => IsPlayerNotSelectingSkin(p)).Any(p => p.Skin == skin);
		}

		private static bool IsPlayerNotSelectingSkin(Player player) {
			return selectingSkinMap_.GetValueOrDefault(player, defaultValue: false) == false;
		}


		// PRAGMA MARK - Public Interface
		public StateSkinCustomization(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState)
						: base(player, container, moveToNextState, moveToPreviousState) {}

		public override void Update() {
			if (InputUtil.WasNegativePressedFor(Player_.InputDevice)) {
				MoveToPreviousState();
			}
		}

		public override void Cleanup() {
			if (selectionView_ != null) {
				selectionView_.OnSelectableHover -= HandleSelectableHover;
				selectionView_.OnSelectableSelected -= HandleSelectableSelected;
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}

			// we're not selecting skins right now
			selectingSkinMap_[Player_] = false;
			OnSelectedSkinsDirty.Invoke();
		}


		// PRAGMA MARK - Internal
		private ElementSelectionView selectionView_;

		protected override void Init() {
			GameObject skinSelectionContainer = GameObjectUtil.FindRequired("SkinSelectionContainer");
			SkinSelectable currentSelectable = skinSelectionContainer.GetComponentsInChildren<SkinSelectable>().First(s => s.Skin == Player_.Skin);

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(Player_, skinSelectionContainer, startSelectable: currentSelectable);
			selectionView_.OnSelectableHover += HandleSelectableHover;
			selectionView_.OnSelectableSelected += HandleSelectableSelected;

			// we're selecting skins right now
			selectingSkinMap_[Player_] = true;
			OnSelectedSkinsDirty.Invoke();
		}

		private void HandleSelectableHover(ISelectable selectable) {
			Player_.Skin = (selectable as SkinSelectable).Skin;
		}

		private void HandleSelectableSelected(ISelectable selectable) {
			BattlePlayerSkin skin = (selectable as SkinSelectable).Skin;
			// if any player has already selected this skin, we cannot select it
			// if all skins are selected then we ignore this check
			if (IsSkinSelected(skin) && !GameConstants.Instance.PlayerSkins.All(s => IsSkinSelected(s))) {
				AudioConstants.Instance.Negative.PlaySFX();
				return;
			}

			Player_.Skin = skin;
			MoveToNextState();
		}
	}
}