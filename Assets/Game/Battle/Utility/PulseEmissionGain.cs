using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	// TODO (darren): multiple scripts of these are setting emissionGain to same thing, should fix
	public class PulseEmissionGain : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Renderer renderer_;

		[Header("Properties")]
		[SerializeField]
		private float emissionGainMin_ = 0.3f;
		[SerializeField]
		private float emissionGainMax_ = 1.0f;

		[Space]
		[SerializeField]
		private float speedMultiplier_ = 1.0f;

		[SerializeField, ReadOnly]
		private float time_ = 0.0f;

		private void Update() {
			time_ += Time.deltaTime * speedMultiplier_;

			float amplitude = emissionGainMax_ - emissionGainMin_;
			float emissionGain = emissionGainMin_ + (Mathf.Sin(time_) * amplitude);
			renderer_.material.SetFloat("_EmissionGain", emissionGain);
		}
	}
}