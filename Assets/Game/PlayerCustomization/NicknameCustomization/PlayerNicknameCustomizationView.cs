using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.Nickname {
	public class PlayerNicknameCustomizationView : MonoBehaviour, ISelectionViewDelegate, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		private static readonly Dictionary<Player, string> cachedNicknames_ = new Dictionary<Player, string>();


		// PRAGMA MARK - Public Interface
		public void Init(Player player, Action onFinishCustomization) {
			player_ = player;
			onFinishCustomization_ = onFinishCustomization;

			RefreshNicknameText();
		}

		public void HandleOkButtonPressed() {
			if (string.IsNullOrEmpty(Nickname_)) {
				player_.Nickname = RegisteredPlayers.GetDefaultNicknameFor(player_);
			} else {
				player_.Nickname = Nickname_;
			}

			onFinishCustomization_.Invoke();
			AudioConstants.Instance.UIBeep.PlaySFX();
		}

		public void HandleDeleteButtonPressed() {
			keypadIndex_ = 0;
			RemoveLastCharacterFromNickname();
			AudioConstants.Instance.UIBeep.PlaySFX();
		}

		public void HandleKeypadSelected(KeypadSelectable keypad) {
			char newChar = keypad.CharactersToChoose.GetWrapped(keypadIndex_);
			if (keypadIndex_ != 0) {
				RemoveLastCharacterFromNickname();
			}

			if (Nickname_.Length >= kCharacterLimit) {
				AudioConstants.Instance.Negative.PlaySFX();
				return;
			}

			keypadIndex_++;
			keypadDelay_ = kKeypadDelay;

			Nickname_ = Nickname_ + newChar;
			AudioConstants.Instance.UIBeep.PlaySFX();
		}


		// PRAGMA MARK - ISelectionViewDelegate Implementation
		void ISelectionViewDelegate.HandleSelectionView(ElementSelectionView selectionView) {
			selectionView_ = selectionView;
			selectionView_.OnSelectorMoved += HandleSelectorMoved;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectionView_ != null) {
				selectionView_.OnSelectorMoved -= HandleSelectorMoved;
				selectionView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		// after X seconds -> will start new character again
		private const float kKeypadDelay = 1.0f;
		private const int kCharacterLimit = 6;

		[SerializeField]
		private TextOutlet nicknameOutlet_;

		private Action onFinishCustomization_;
		private Player player_;

		private ElementSelectionView selectionView_;

		private int keypadIndex_ = 0;
		private float keypadDelay_ = 0.0f;

		private string Nickname_ {
			get { return cachedNicknames_.GetValueOrDefault(player_, defaultValue: RegisteredPlayers.GetDefaultNicknameFor(player_)); }
			set {
				string newNickname = value.ToUpper();

				cachedNicknames_[player_] = newNickname;
				RefreshNicknameText();
			}
		}

		private void Update() {
			keypadDelay_ -= Time.deltaTime;
			if (keypadDelay_ <= 0.0f) {
				keypadIndex_ = 0;
				RefreshNicknameText();
			}
		}

		private void HandleSelectorMoved() {
			keypadIndex_ = 0;
			RefreshNicknameText();
		}

		private void RefreshNicknameText() {
			if (keypadIndex_ > 0 && Nickname_.Length > 0) {
				// cursor on current space
				nicknameOutlet_.Text = string.Format("{0}<u>{1}</u>", Nickname_.Substring(0, Nickname_.Length - 1), Nickname_.Last());
			} else if (Nickname_.Length < kCharacterLimit) {
				// cursor on next space
				nicknameOutlet_.Text = string.Format("{0}_", Nickname_);
			} else {
				// no cursor because character limit
				nicknameOutlet_.Text = Nickname_;
			}
		}

		private void RemoveLastCharacterFromNickname() {
			string oldNickname = Nickname_;
			if (oldNickname.Length <= 0) {
				return;
			}

			Nickname_ = oldNickname.Substring(0, oldNickname.Length - 1);
		}
	}
}