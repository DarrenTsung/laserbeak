using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.ElementSelection;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.Nickname {
	public class KeypadSelectable : MonoBehaviour, ISelectable {
		// PRAGMA MARK - Public Interface
		public char[] CharactersToChoose {
			get { return characters_; }
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			view_.HandleKeypadSelected(this);
		}


		// PRAGMA MARK - Internal
		private PlayerNicknameCustomizationView view_;
		private char[] characters_;

		private void Awake() {
			view_ = this.GetComponentInParent<PlayerNicknameCustomizationView>();

			string text = this.GetComponentInChildren<TMP_Text>().text;
			characters_ = text.ToArray();
		}
	}
}