using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class BobUpAndDown : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private float bobAmplitude_ = 0.3f;
		[SerializeField]
		private float bobFrequencyMultiplier_ = 1.0f;

		[SerializeField, ReadOnly]
		private float time_ = 0.0f;

		private Vector3 startLocalPosition_;

		private void Awake() {
			startLocalPosition_ = this.transform.localPosition;
		}

		private void Update() {
			time_ += Time.deltaTime * bobFrequencyMultiplier_;
			this.transform.localPosition = startLocalPosition_ + (Mathf.Sin(time_) * bobAmplitude_ * Vector3.up);
		}
	}
}