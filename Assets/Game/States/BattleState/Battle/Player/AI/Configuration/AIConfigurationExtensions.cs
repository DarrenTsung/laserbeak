using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;

namespace DT.Game.Battle.AI {
	public static class AIConfigurationExtensions {
		// PRAGMA MARK - Public Interface
		public static float RandomReactionTime(this AIConfiguration configuration) {
			float baseReactionTime = Mathf.Lerp(kMaxReactionTime, kMinReactionTime, configuration.SkillLevel);
			float deviatedReactionTime = MathUtil.SampleGaussian(baseReactionTime, kReactionTimeStandardDeviation);
			// TODO (darren): instead of clamping this we should use a skewed normal distribution instead
			return Mathf.Clamp(deviatedReactionTime, kMinReactionTime, kMaxReactionTime);
		}

		public static float AccuracyInDegrees(this AIConfiguration configuration) {
			return Mathf.Lerp(kMaxAccuracyInDegrees, kMinAccuracyInDegrees, configuration.SkillLevel);
		}


		// PRAGMA MARK - Internal
		// Minimum reaction time with visual stimulus - 0.25f seconds
		private const float kMinReactionTime = 0.25f;
		private const float kMaxReactionTime = 1.25f;

		private const float kReactionTimeStandardDeviation = 0.05f;

		private const float kMinAccuracyInDegrees = 0.0f;
		private const float kMaxAccuracyInDegrees = 45.0f;
	}
}