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
	public class TransitionBattlePlayerEmission : TransitionBase<float> {
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

		protected override float GetInValue() { return inWhiteBalance_; }
		protected override float GetOutValue() { return outWhiteBalance_; }

		protected override float GetCurrentValue() { return GetWhiteBalance(); }
		protected override void SetCurrentValue(float value) { SetWhiteBalancePercentage(value); }

		private readonly Material[] skinMaterials_ = new Material[1];

		private void SetWhiteBalancePercentage(float whiteBalancePercentage) {
			foreach (var bodyMaterial in GetBodyMaterials()) {
				float whiteBalance = whiteBalancePercentage * kEmissionWhiteBalance;
				bodyMaterial.SetColor("_EmissionColor", new Color(whiteBalance, whiteBalance, whiteBalance));
			}
		}

		private float GetWhiteBalance() {
			foreach (var bodyMaterial in GetBodyMaterials()) {
				Color c = bodyMaterial.GetColor("_EmissionColor");
				return c.r / kEmissionWhiteBalance;
			}

			return 0.0f;
		}

		private IEnumerable<Material> GetBodyMaterials() {
			Renderer bodyRenderer = battlePlayer_.BodyRenderers.First();
			IEnumerable<Material> bodyMaterials = null;
			// NOTE (darren): detect bodyRenderer.material != OpaqueBodyMaterial for ghost mode
			if (battlePlayer_.Skin != null && battlePlayer_.Skin.OpaqueBodyMaterial == bodyRenderer.sharedMaterial) {
				skinMaterials_[0] = battlePlayer_.Skin.OpaqueBodyMaterial;
				bodyMaterials = skinMaterials_;
			} else {
				bodyMaterials = battlePlayer_.BodyRenderers.Select(r => r.material);
			}
			return bodyMaterials;
		}
	}
}