using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Player;
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


		// PRAGMA MARK - Internal
		private BattlePlayerSkin skin_;
		private string nickname_;

		private bool IsValidNickname(string nickname) {
			return nickname.Length == 3;
		}
	}
}