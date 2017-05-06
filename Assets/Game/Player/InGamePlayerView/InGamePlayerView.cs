using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public class InGamePlayerView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void InitWith(Player player) {
			player_ = player;
			player_.OnNicknameChanged += HandleNicknameChanged;
			player_.OnSkinChanged += HandleSkinChanged;

			HandleNicknameChanged();
			HandleSkinChanged();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (player_ != null) {
				player_.OnNicknameChanged -= HandleNicknameChanged;
				player_.OnSkinChanged -= HandleSkinChanged;
				player_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Image image_;

		[SerializeField]
		private TextOutlet nicknameText_;

		private Player player_;

		private void HandleNicknameChanged() {
			nicknameText_.Text = player_.Nickname;
		}

		private void HandleSkinChanged() {
			if (player_.Skin != null) {
				nicknameText_.Color = player_.Skin.BodyColor;
				image_.sprite = player_.Skin.ThumbnailSprite;
				image_.color = Color.white;
			} else {
				nicknameText_.Color = Color.clear;
				image_.color = Color.clear;
				image_.sprite = null;
			}
		}
	}
}