using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTEasings;
using DTObjectPoolManager;

using DT.Game.Transitions;

namespace DT.Game.Battle.Players {
	public class TransitionBattlePlayerEmission : TransitionBase, ITransition {
		// PRAGMA MARK - ITransition Implementation
		public override void Refresh(TransitionType transitionType, float percentage) {
			float startWhiteBalance = (transitionType == TransitionType.In) ? outWhiteBalance_ : inWhiteBalance_;
			float endWhiteBalance = (transitionType == TransitionType.In) ? inWhiteBalance_ : outWhiteBalance_;

			SetWhiteBalancePercentage(Mathf.Lerp(startWhiteBalance, endWhiteBalance, percentage));
		}


		// PRAGMA MARK - Internal
		private const float kEmissionWhiteBalance = 0.2f;

		[Header("BattlePlayerEmission Outlets")]
		[SerializeField]
		private BattlePlayer battlePlayer_;

		[Header("Alpha Properties")]
		[SerializeField]
		private float inWhiteBalance_ = 0.0f;
		[SerializeField]
		private float outWhiteBalance_ = 1.0f;

		private void SetWhiteBalancePercentage(float whiteBalancePercentage) {
			if (battlePlayer_.Skin == null) {
				// TODO (darren): support for editor preview if necessary
				return;
			}

			float whiteBalance = whiteBalancePercentage * kEmissionWhiteBalance;
			battlePlayer_.Skin.OpaqueBodyMaterial.SetColor("_EmissionColor", new Color(whiteBalance, whiteBalance, whiteBalance));
		}
	}
}