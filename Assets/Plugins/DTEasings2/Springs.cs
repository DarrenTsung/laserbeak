using System;
using UnityEngine;

namespace DTEasings {
	public static class Springs {
		/// <summary>
		/// uses the implicit euler method. slower, but always stable.
		/// see http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
		/// </summary>
		/// <returns>The spring.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="targetValue">Target value.</param>
		/// <param name="velocity">Velocity by reference. Be sure to reset it to 0 if changing the targetValue between calls</param>
		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.</param>
		/// <param name="angularFrequency">An angular frequency of 2pi (radians per second) means the oscillation completes one
		/// full period over one second, i.e. 1Hz. should be less than 35 or so to remain stable</param>
		public static float StableSpring(float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency) {
			var f = 1f + 2f * Time.deltaTime * dampingRatio * angularFrequency;
			var oo = angularFrequency * angularFrequency;
			var hoo = Time.deltaTime * oo;
			var hhoo = Time.deltaTime * hoo;
			var detInv = 1.0f / (f + hhoo);
			var detX = f * currentValue + Time.deltaTime * velocity + hhoo * targetValue;
			var detV = velocity + hoo * (targetValue - currentValue);

			currentValue = detX * detInv;
			velocity = detV * detInv;

			return currentValue;
		}
	}
}
