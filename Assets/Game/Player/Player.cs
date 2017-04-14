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

		// TODO (darren): will probably need an input device recovery system (if controller gets unplugged)
		// will probably require InputDevice to be public set
		public InputDevice InputDevice {
			get { return inputDevice_; }
		}

		public Player(InputDevice inputDevice) {
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - Internal
		private BattlePlayerSkin skin_ = null;
		private string nickname_ = "";
		private InputDevice inputDevice_;

		private bool IsValidNickname(string nickname) {
			if (nickname == null) {
				return false;
			}

			if (nickname.Length > 3) {
				return false;
			}

			return true;
		}
	}
}