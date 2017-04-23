using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.PlayerCustomization.States;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.Skin {
	public class SkinSelectable : MonoBehaviour, ISelectable, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(BattlePlayerSkin skin) {
			skin_ = skin;
			thumbnailImage_.sprite = skin_.ThumbnailSprite;

			StateSkinCustomization.OnSelectedSkinsDirty += RefreshSelected;
			RefreshSelected();
		}

		public BattlePlayerSkin Skin {
			get { return skin_; }
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			// do nothing - used as marker only
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			StateSkinCustomization.OnSelectedSkinsDirty -= RefreshSelected;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Image thumbnailImage_;

		private BattlePlayerSkin skin_;

		private void RefreshSelected() {
			float alpha = StateSkinCustomization.IsSkinSelected(skin_) ? 0.5f : 1.0f;
			thumbnailImage_.color = Color.white.WithAlpha(alpha);
		}
	}
}