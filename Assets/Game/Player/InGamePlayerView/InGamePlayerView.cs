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
	public class InGamePlayerView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void InitWith(Player player) {
			nicknameText_.Text = player.Nickname;
			nicknameText_.Color = player.Skin.Color;
			backgroundImage_.color = player.Skin.Color;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Image backgroundImage_;

		[SerializeField]
		private TextOutlet nicknameText_;
	}
}