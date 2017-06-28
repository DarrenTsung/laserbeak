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
		public void InitWith(Player player, bool enableNudge = false) {
			player_ = player;
			player_.OnNicknameChanged += HandleNicknameChanged;
			player_.OnSkinChanged += HandleSkinChanged;

			enableNudge_ = enableNudge;

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

			enableNudge_ = false;
		}


		// PRAGMA MARK - Internal
		private const float kNudgeDistance = 5.0f;
		private const float kNudgeSpeed = 2.0f;

		[Header("Outlets")]
		[SerializeField]
		private Image image_;
		[SerializeField]
		private TextOutlet nicknameText_;
		[SerializeField]
		private RectTransform profileTransform_;

		private Player player_;
		private bool enableNudge_ = false;

		private void Update() {
			if (enableNudge_) {
				UpdateProfileNudge();
			}
		}

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

		private void UpdateProfileNudge() {
			if (player_.InputDevice == null) {
				return;
			}

			float oldX = profileTransform_.anchoredPosition.x;
			float targetX = player_.InputDevice.LeftStick.X * kNudgeDistance;
			float newX = Mathf.Lerp(oldX, targetX, kNudgeSpeed);
			profileTransform_.anchoredPosition = new Vector2(newX, 0.0f);
		}
	}
}