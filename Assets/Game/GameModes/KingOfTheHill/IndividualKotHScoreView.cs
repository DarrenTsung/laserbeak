using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.GameModes.KingOfTheHill {
	public class IndividualKotHScoreView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void Init(Player player, IKotHScoreSource scoreSource) {
			player_ = player;

			Color playerColor = player_.Skin.BodyColor;
			bar_.color = playerColor;
			nicknameText_.Color = playerColor;

			nicknameText_.Text = player.Nickname;

			scoreSource_ = scoreSource;
			scoreSource_.OnAnyScoreChanged += RefreshBar;
			RefreshBar();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Image bar_;

		[SerializeField]
		private TextOutlet nicknameText_;

		private RectTransform barTransform_;
		private Player player_;
		private IKotHScoreSource scoreSource_;

		private void Awake() {
			barTransform_ = bar_.GetRequiredComponent<RectTransform>();
		}

		private void RefreshBar() {
			float p = scoreSource_.GetPercentageScoreFor(player_);

			barTransform_.anchorMax = barTransform_.anchorMax.SetY(p);
			barTransform_.offsetMax = barTransform_.offsetMax.SetY(0.0f);
		}
	}
}