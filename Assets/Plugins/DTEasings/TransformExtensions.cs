using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT;

namespace DTEasings {
	public static class TransformExtensions {
		public static void Shake(this Transform transform, float magnitude, float duration, EaseType easeType = EaseType.QuadraticEaseOut) {
			if (coroutineMap_.ContainsKey(transform)) {
				coroutineMap_[transform].Cancel();
				coroutineMap_.Remove(transform);
				transform.position = originalPositionMap_[transform];
			}

			originalPositionMap_[transform] = transform.position;
			coroutineMap_[transform] = CoroutineWrapper.StartCoroutine(ShakeCoroutine(transform, magnitude, duration, easeType));
		}

		// PRAGMA MARK - Internal
		private static readonly WaitForEndOfFrame kWaitForEndOfFrame = new WaitForEndOfFrame();
		private static readonly Dictionary<Transform, CoroutineWrapper> coroutineMap_ = new Dictionary<Transform, CoroutineWrapper>();
		private static readonly Dictionary<Transform, Vector3> originalPositionMap_ = new Dictionary<Transform, Vector3>();

		private static IEnumerator ShakeCoroutine(Transform transform, float magnitude, float duration, EaseType easeType) {
			Vector3 offset;
			for (float time = 0.0f; time < duration; time += Time.deltaTime) {
				float currentMagnitude = Mathf.Lerp(magnitude, 0.0f, Easings.Interpolate(time / duration, easeType));
				Vector3 transformedRight = transform.rotation * Vector3.right;
				Vector3 transformedUp = transform.rotation * Vector3.up;

				offset = (transformedRight * UnityEngine.Random.Range(-currentMagnitude, currentMagnitude))
						+ (transformedUp * UnityEngine.Random.Range(-currentMagnitude, currentMagnitude));

				transform.position += offset;
				yield return kWaitForEndOfFrame;
				transform.position -= offset;
			}

			coroutineMap_.Remove(transform);
			transform.position = originalPositionMap_[transform];
		}
	}
}
