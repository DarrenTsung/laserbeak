using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	[RequireComponent(typeof(Image))]
	public class RotatingCircleEffect : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			segmentLength_ = UnityEngine.Random.Range(kSegmentLengthMin, kSegmentLengthMax);
			segmentLengthChange_ = UnityEngine.Random.Range(-kSegmentLengthChange, kSegmentLengthChange);
			angleOffset_ = UnityEngine.Random.Range(kAngleOffsetMin, kAngleOffsetMax);
			angleOffsetChange_ = UnityEngine.Random.Range(-kAngleOffsetChange, kAngleOffsetChange);

			Animate();
		}


		// PRAGMA MARK - Internal
		private const float kSegmentLengthMin = 0.3f;
		private const float kSegmentLengthMax = 0.6f;

		private const float kAngleOffsetMin = 0.25f;
		private const float kAngleOffsetMax = 0.40f;

		private const float kSegmentLengthChange = 0.16f;
		private const float kAngleOffsetChange = 0.3f;

		private const float kAnimateDuration = 3.5f;
		private const float kAnimateColorDuration = 0.6f;

		[Header("Read-Only Properties")]
		[SerializeField, ReadOnly]
		private float segmentLength_;
		[SerializeField, ReadOnly]
		private float segmentLengthChange_;
		[SerializeField, ReadOnly]
		private float angleOffset_;
		[SerializeField, ReadOnly]
		private float angleOffsetChange_;

		private Image image_;

		private void Awake() {
			image_ = this.GetRequiredComponent<Image>();
			image_.material = Material.Instantiate(image_.material);
		}

		private void Animate() {
			image_.material.SetFloat("_SegmentLength", segmentLength_);
			image_.material.SetFloat("_AngleOffset", angleOffset_);

			// animate color in
			CoroutineWrapper.DoEaseFor(kAnimateColorDuration, EaseType.CubicEaseInOut, (float p) => {
				image_.color = ColorUtil.LerpWhiteBlack(1.0f - p);
			});

			// animate color out
			CoroutineWrapper.DoAfterDelay(kAnimateDuration - kAnimateColorDuration, () => {
				CoroutineWrapper.DoEaseFor(kAnimateColorDuration, EaseType.CubicEaseInOut, (float p) => {
					image_.color = ColorUtil.LerpWhiteBlack(p);
				});
			});

			CoroutineWrapper.DoEaseFor(kAnimateDuration, EaseType.CubicEaseOut, (float p) => {
				image_.material.SetFloat("_SegmentLength", segmentLength_ + (p * segmentLengthChange_));
				image_.material.SetFloat("_AngleOffset", angleOffset_ + (p * angleOffsetChange_));
			});
		}
	}
}