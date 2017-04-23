using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes.Tag {
	public class TagExplosive : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(BattlePlayer player) {
			player_ = player;
			timeLeft_ = kExplosiveTime;
			ResetPulseTimer();
			enabled_ = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			enabled_ = false;
		}


		// PRAGMA MARK - Internal
		private const float kExplosiveTime = 5.0f;

		private const float kPulseTime = 0.08f;
		private const float kMaxPulseFrequency = 0.6f;

		[Header("Outlets")]
		[SerializeField]
		private Renderer renderer_;

		[Header("Properties")]
		[SerializeField]
		private Color startPulseColor_;
		[SerializeField]
		private Color endPulseColor_;

		[Header("Read-Only")]
		[SerializeField, ReadOnly]
		private float timeLeft_;

		[SerializeField, ReadOnly]
		private float pulseTimer_;

		private BattlePlayer player_;
		private bool enabled_ = true;

		private void Update() {
			if (!enabled_) {
				return;
			}

			timeLeft_ -= Time.deltaTime;
			if (timeLeft_ <= 0.0f) {
				player_.Health.Kill();
				ObjectPoolManager.Recycle(this);
				return;
			}

			pulseTimer_ -= Time.deltaTime;
			if (pulseTimer_ <= 0.0f) {
				Pulse();
				ResetPulseTimer();
			}
		}

		private void Pulse() {
			AudioConstants.Instance.ExplosiveTimerBeep.PlaySFX(randomPitchRange: 0.02f);
			this.DoLerpFor(kPulseTime / 2.0f, (float p) => {
				renderer_.material.SetColor("_EmissionColor", Color.Lerp(startPulseColor_, endPulseColor_, p));
			}, () => {
				this.DoLerpFor(kPulseTime / 2.0f, (float p) => {
					renderer_.material.SetColor("_EmissionColor", Color.Lerp(startPulseColor_, endPulseColor_, 1.0f - p));
				});
			});
		}

		private void ResetPulseTimer() {
			pulseTimer_ = Mathf.Lerp(kPulseTime, kMaxPulseFrequency, Easings.QuadraticEaseIn(timeLeft_ / kExplosiveTime));
		}
	}
}