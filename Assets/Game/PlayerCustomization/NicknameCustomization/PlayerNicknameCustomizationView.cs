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
		// PRAGMA MARK - Public Interface
		public void Init(Player player, Action onFinishCustomization) {
			player_ = player;
			onFinishCustomization_ = onFinishCustomization;

			nicknameOutlet_.Text = Nickname_;
		}

		public void HandleOkButtonPressed() {
			if (string.IsNullOrEmpty(Nickname_)) {
				player_.Nickname = string.Format("P{0}", player_.Index() + 1);
			} else {
				player_.Nickname = Nickname_;
			}

			onFinishCustomization_.Invoke();
		}

		public void HandleDeleteButtonPressed() {
			string oldNickname = Nickname_;
			if (oldNickname.Length <= 0) {
				return;
			}

			Nickname_ = oldNickname.Substring(0, oldNickname.Length - 1);
		}

		public void HandleKeypadSelected(KeypadSelectable keypad) {
			char newChar = keypad.CharactersToChoose.GetWrapped(keypadIndex_);
			if (keypadIndex_ != 0) {
				HandleDeleteButtonPressed();
			}

			if (Nickname_.Length >= kCharacterLimit) {
				return;
			}

			Nickname_ = Nickname_ + newChar;
			keypadIndex_++;
			keypadDelay_ = kKeypadDelay;
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
			get { return PlayerPrefs.GetString(player_.Index() + "Nickname_", defaultValue: ""); }
			set {
				string newNickname = value.ToUpper();

				PlayerPrefs.SetString(player_.Index() + "Nickname_", newNickname);
				nicknameOutlet_.Text = newNickname;
			}
		}

		private void Update() {
			keypadDelay_ -= Time.deltaTime;
			if (keypadDelay_ <= 0.0f) {
				keypadIndex_ = 0;
			}
		}

		private void HandleSelectorMoved() {
			keypadIndex_ = 0;
		}
	}
}