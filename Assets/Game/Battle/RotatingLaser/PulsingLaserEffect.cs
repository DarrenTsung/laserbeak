using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class PulsingLaserEffect : MonoBehaviour {
		// PRAGMA MARK - Static
		private const float kOffsetScrollSpeed = -2.0f;
		private const float kEmissionGainPulseAmount = 0.006f;
		private const float kEmissionPulseWavelength = 0.3f;

		private static readonly HashSet<Material> pulseMaterials_ = new HashSet<Material>();
		private static readonly Dictionary<Material, float> materialsStartEmissionGain_ = new Dictionary<Material, float>();
		private static readonly Dictionary<Material, Vector2> materialsStartTextureOffset_ = new Dictionary<Material, Vector2>();
		private static readonly Dictionary<Material, float> materialsPulseAmount_ = new Dictionary<Material, float>();
		private static readonly Dictionary<Material, float> materialsPulseWavelength_ = new Dictionary<Material, float>();

		private static void EnablePulsing() {
			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		private static void HandleUpdate() {
			float offset = Mathf.Repeat(Time.time * kOffsetScrollSpeed, 1.0f);
			var textureOffset = new Vector2(offset, offset);
			foreach (Material m in pulseMaterials_) {
				float pulseWavelength = kEmissionPulseWavelength;
				if (materialsPulseWavelength_.ContainsKey(m)) {
					pulseWavelength = materialsPulseWavelength_[m];
				}

				var emissionGainOffset = MathUtil.EvaluateSine(Time.time, 0.5f, wavelength: pulseWavelength);
				float pulseAmount = kEmissionGainPulseAmount;
				if (materialsPulseAmount_.ContainsKey(m)) {
					pulseAmount = materialsPulseAmount_[m];
				}

				float emissionGain = materialsStartEmissionGain_[m] + (emissionGainOffset * pulseAmount);
				m.SetTextureOffset("_Illum", textureOffset);
				m.SetFloat("_EmissionGain", emissionGain);
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Material material_;

		[Header("Properties")]
		[SerializeField]
		private bool useCustomProperties_ = false;
		[SerializeField]
		private float customPulseAmount_ = kEmissionGainPulseAmount;
		[SerializeField]
		private float customPulseWavelength_ = kEmissionPulseWavelength;

		private void Awake() {
			pulseMaterials_.Add(material_);
			materialsStartEmissionGain_[material_] = material_.GetFloat("_EmissionGain");
			materialsStartTextureOffset_[material_] = material_.GetTextureOffset("_Illum");
			if (useCustomProperties_) {
				materialsPulseAmount_[material_] = customPulseAmount_;
				materialsPulseWavelength_[material_] = customPulseWavelength_;
			}
			EnablePulsing();
		}

		private void OnDestroy() {
			material_.SetFloat("_EmissionGain", materialsStartEmissionGain_[material_]);
			material_.SetTextureOffset("_Illum", materialsStartTextureOffset_[material_]);
		}
	}
}