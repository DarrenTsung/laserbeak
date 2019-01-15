using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public abstract class TransitionBase<T> : MonoBehaviour, ITransition {
		// PRAGMA MARK - ITransition Implementation
		float ITransition.Duration {
			get { return duration_; }
		}

		float ITransition.BaseDelay {
			get { return baseDelay_; }
		}

		void ITransition.Animate(TransitionType transitionType, float delay, Action<ITransition> callback) {
			CurrentTransitionType_ = transitionType;

			EaseType easeType = (transitionType == TransitionType.In) ? inEaseType_ : outEaseType_;
			T startValue = (transitionType == TransitionType.In) ? GetOutValue() : GetInValue();

			if (currentCoroutine_ != null) {
				currentCoroutine_.Cancel();
				currentCoroutine_ = null;

				// If interrupted - start from current value
				startValue = GetCurrentValue();
			}

			T endValue = (transitionType == TransitionType.In) ? GetInValue() : GetOutValue();

			SetValue(startValue, endValue, 0.0f);
			currentCoroutine_ = CoroutineWrapper.DoAfterDelay(baseDelay_ + delay, () => {
				currentCoroutine_ = CoroutineWrapper.DoEaseFor(duration_, easeType, (float p) => {
					SetValue(startValue, endValue, p);
				}, () => {
					currentCoroutine_ = null;
					callback.Invoke(this);
				});
			});
		}

		// NOTE (darren): this function doesn't take into account interrupted transitions
		// and is used instant (no animation) or visualization / mock purposes in editor
		void ITransition.Refresh(TransitionType transitionType, float percentage) {
			T startValue = (transitionType == TransitionType.In) ? GetOutValue() : GetInValue();
			T endValue = (transitionType == TransitionType.In) ? GetInValue() : GetOutValue();
			SetValue(startValue, endValue, percentage);
		}


		// PRAGMA MARK - Internal
		[Header("Base Properties")]
		[SerializeField]
		private float duration_ = 1.0f;
		[SerializeField]
		private float baseDelay_ = 0.0f;

		[Space]
		[SerializeField]
		private EaseType inEaseType_ = EaseType.QuadraticEaseOut;
		[SerializeField]
		private EaseType outEaseType_ = EaseType.QuadraticEaseIn;

		private CoroutineWrapper currentCoroutine_;

		protected TransitionType CurrentTransitionType_ {
			get; private set;
		}

		protected abstract T GetInValue();
		protected abstract T GetOutValue();

		protected abstract T GetCurrentValue();
		protected abstract void SetCurrentValue(T value);

		private interface ILerpConverter<V> {
			V Lerp(V startValue, V endValue, float percentage);
		}

		private class LerpConverter : ILerpConverter<float>, ILerpConverter<Vector2>, ILerpConverter<Vector3> {
			// PRAGMA MARK - ILerpConverter<float> Implementation
			float ILerpConverter<float>.Lerp(float startValue, float endValue, float percentage) {
				return Mathf.Lerp(startValue, endValue, percentage);
			}


			// PRAGMA MARK - ILerpConverter<Vector2> Implementation
			Vector2 ILerpConverter<Vector2>.Lerp(Vector2 startValue, Vector2 endValue, float percentage) {
				return Vector2.Lerp(startValue, endValue, percentage);
			}


			// PRAGMA MARK - ILerpConverter<Vector3> Implementation
			Vector3 ILerpConverter<Vector3>.Lerp(Vector3 startValue, Vector3 endValue, float percentage) {
				return Vector3.Lerp(startValue, endValue, percentage);
			}
		}

		private ILerpConverter<T> lerpConverter_ = new LerpConverter() as ILerpConverter<T>;

		private void SetValue(T startValue, T endValue, float percentage) {
			SetCurrentValue(lerpConverter_.Lerp(startValue, endValue, percentage));
		}
	}
}