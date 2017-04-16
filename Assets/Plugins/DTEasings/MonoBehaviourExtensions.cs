using System;
using System.Collections;
using UnityEngine;

using DT;

namespace DTEasings {
	public static class MonoBehaviourExtensions {
		// PRAGMA MARK - Static
		public static Coroutine DoEaseFor(this MonoBehaviour m, float duration, EaseType easeType, Action<float> lerpCallback, Action finishedCallback = null) {
			return m.DoLerpFor(duration, (p) => lerpCallback.Invoke(Easings.Interpolate(p, easeType)), finishedCallback);
		}

		private static Coroutine DoLerpFor(this MonoBehaviour m, float duration, Action<float> lerpCallback, Action finishedCallback = null) {
			return m.StartCoroutine(DoLerpCoroutine(duration, lerpCallback, finishedCallback));
		}

		private static IEnumerator DoLerpCoroutine(float duration, Action<float> lerpCallback, Action finishedCallback) {
			for (float time = 0.0f; time <= duration; time += Time.deltaTime) {
				lerpCallback.Invoke(time / duration);
				yield return null;
			}

			lerpCallback.Invoke(1.0f);

			if (finishedCallback != null) {
				finishedCallback.Invoke();
			}
		}

		public static Coroutine DoSpringFor(this MonoBehaviour m, float approximateDuration, float dampingRatio, Action<float> lerpCallback, Action finishedCallback = null) {
			return m.StartCoroutine(CoroutineWrapper.DoSpringForCoroutine(approximateDuration, dampingRatio, lerpCallback, finishedCallback));
		}
	}
}