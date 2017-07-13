using System;
using System.Collections;
using UnityEngine;

using DTEasings;

namespace DT {
	public partial class CoroutineWrapper {
		// PRAGMA MARK - Static
		public static CoroutineWrapper DoEaseFor(float duration, EaseType easeType, Action<float> lerpCallback, Action finishedCallback = null) {
			return CoroutineWrapper.DoLerpFor(duration, (p) => lerpCallback.Invoke(Easings.Interpolate(p, easeType)), finishedCallback);
		}

		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.
		public static CoroutineWrapper DoSpringFor(float approximateDuration, float dampingRatio, Action<float> lerpCallback, Action finishedCallback = null) {
			return CoroutineWrapper.StartCoroutine(DoSpringForCoroutine(approximateDuration, dampingRatio, lerpCallback, finishedCallback));
		}

		public static IEnumerator DoSpringForCoroutine(float approximateDuration, float dampingRatio, Action<float> lerpCallback, Action finishedCallback) {
			float angularFrequency = 2.0f * Mathf.PI / approximateDuration;

			float currentValue = 0.0f;
			float velocity = 0.0f;
			while (!Mathf.Approximately(currentValue, 1.0f) || velocity > Mathf.Epsilon) {
				currentValue = Springs.StableSpring(currentValue, 1.0f, ref velocity, dampingRatio, angularFrequency);

				lerpCallback.Invoke(currentValue);
				yield return null;
			}

			lerpCallback.Invoke(1.0f);

			if (finishedCallback != null) {
				finishedCallback.Invoke();
			}
		}
	}
}