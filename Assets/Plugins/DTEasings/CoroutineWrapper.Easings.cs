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
	}
}