using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public class Player {
		// PRAGMA MARK - Public Interface
		public event Action OnSkinChanged = delegate {};
		public event Action OnNicknameChanged = delegate {};

		public BattlePlayerSkin Skin {
			get { return skin_; }
			set {
				if (skin_ == value) {
					return;
				}

				skin_ = value;
				OnSkinChanged.Invoke();
			}
		}

		public string Nickname {
			get { return nickname_; }
			set {
				if (!IsValidNickname(value)) {
					Debug.LogWarning("Nickname: " + value + " is not valid!");
					return;
				}

				if (nickname_ == value) {
					return;
				}

				nickname_ = value;
				OnNicknameChanged.Invoke();
			}
		}

		public bool IsAI {
			get { return Input == null; }
		}

		public IInputWrapper Input {
			get { return input_; }
		}

		public Player(IInputWrapper input) {
			input_ = input;
		}

		public override string ToString() {
			return string.Format("{0} - {1}", Nickname, IsAI ? "AI" : "Human");
		}


		// PRAGMA MARK - Internal
		private BattlePlayerSkin skin_ = null;
		private string nickname_ = "";
		private IInputWrapper input_;

		private bool IsValidNickname(string nickname) {
			if (nickname == null) {
				return false;
			}

			return true;
		}
	}
}