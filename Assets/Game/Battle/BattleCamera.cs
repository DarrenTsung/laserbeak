using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PostProcessing;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	[RequireComponent(typeof(Camera))]
	public class BattleCamera : Singleton<BattleCamera> {
		// PRAGMA MARK - Static Public Interface
		public static void Shake(float percentage) {
			if (percentage < 0.0f || percentage > 1.0f) {
				Debug.LogWarning("?? What are you trying to do here?");
			}

			float shakeDuration = kShakeMaxDuration * percentage;
			Instance.transform.Shake(kShakeMaxAmount * percentage, shakeDuration, returnToOriginalPosition: false);
			Instance.AnimateChromaticAberration(intensity: Easings.QuarticEaseIn(percentage) * kAberrationMaxAmount, duration: shakeDuration);
		}

		public static void StopTimeForKill() {
			Instance.StopTimeFor(duration: 0.1f);
		}

		public static void SetDepthOfFieldEnabled(bool enabled, bool animate = false) {
			Instance.SetDepthOfFieldEnabledInternal(enabled, animate);
		}


		// PRAGMA MARK - Public Interface
		public Camera Camera {
			get { return camera_; }
		}

		public void SetSurvivingPlayersAsTransformsOfInterest() {
			survivingPlayersAsInterest_ = true;
		}

		public void ClearTransformsOfInterest() {
			survivingPlayersAsInterest_ = false;
			transformsOfInterest_ = null;
		}


		// PRAGMA MARK - Internal
		private const float kShakeMaxAmount = 0.6f;
		private const float kShakeMaxDuration = 0.7f;

		private const float kAberrationMaxAmount = 0.4f;

		private const float kXRotationMin = 45;
		private const float kXRotationMax = 65;
		// NOTE (darren): base focusVector magnitude is ~25
		private const float kRotationLerpMagnitude = 14.0f;

		// must be in range of 0...0.5f
		private const float kFocusViewportRadius = 0.3f;
		private static readonly Plane kFocusPlane = new Plane(Vector3.up, Vector3.zero);
		private const float kFocusMinimum = 7.0f;

		[Header("Outlets")]
		[SerializeField]
		private PostProcessingProfile postProcessingProfile_;

		[Header("Properties")]
		[SerializeField]
		private float cameraSpeed_ = 1.0f;

		private IEnumerable<Transform> transformsOfInterest_;
		private bool survivingPlayersAsInterest_ = false;

		private Camera camera_;
		private Vector3 initialPosition_;

		private CoroutineWrapper aberrationCoroutine_;
		private CoroutineWrapper timeScaleCoroutine_;
		private CoroutineWrapper depthOfFieldCoroutine_;

		private void Awake() {
			camera_ = this.GetRequiredComponent<Camera>();
			initialPosition_ = this.transform.position;
		}

		private void LateUpdate() {
			if (survivingPlayersAsInterest_) {
				if (PlayerSpawner.AllSpawnedBattlePlayers.Count() > 0) {
					transformsOfInterest_ = PlayerSpawner.AllSpawnedBattlePlayers.Select(bp => bp.transform);
				} else {
					transformsOfInterest_ = null;
				}
			}

			Vector3 targetPosition = initialPosition_;
			if (transformsOfInterest_ != null) {
				targetPosition = GetTargetPositionToHighlightInterest() + GameConstants.Instance.PlayerFocusOffset;
			}

			this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, cameraSpeed_ * Time.deltaTime);

			float p = Mathf.Clamp((GetCurrentFocusVector().magnitude - kFocusMinimum) / kRotationLerpMagnitude, 0.0f, 1.0f);
			this.transform.rotation = Quaternion.Euler(Mathf.Lerp(kXRotationMin, kXRotationMax, p), 0, 0);
		}

		private Vector3 GetTargetPositionToHighlightInterest() {
			Vector3 averageInterestPosition = transformsOfInterest_.Select(t => t.position).Average();

			Vector3 focusPoint = GetFocusPoint();

			Vector3 initialPositionFocusVector = initialPosition_ - focusPoint;
			Vector3 currentFocusVector = this.transform.position - focusPoint;
			float minimumFocusScale = currentFocusVector.magnitude / kFocusMinimum;

			float maxOutOfRadiusScale = 1.0f;
			Vector2 averageViewportPosition = camera_.WorldToViewportPoint(averageInterestPosition);
			Vector2 averageTranslation = new Vector2(0.5f, 0.5f) - averageViewportPosition;
			foreach (Vector2 transformViewportPosition in transformsOfInterest_.Select(t => ((Vector2)camera_.WorldToViewportPoint(t.position)) + averageTranslation)) {
				Vector2 scaledTransformViewportPosition = (transformViewportPosition - new Vector2(0.5f, 0.5f)) * minimumFocusScale;
				float outOfRadiusScale = scaledTransformViewportPosition.magnitude / kFocusViewportRadius;
				if (outOfRadiusScale > 1.0f && outOfRadiusScale > maxOutOfRadiusScale) {
					maxOutOfRadiusScale = outOfRadiusScale;
				}
			}

			Vector3 newFocusVector = currentFocusVector.normalized * kFocusMinimum * maxOutOfRadiusScale;
			// focus should never go beyond initialPosition
			newFocusVector = Vector3.ClampMagnitude(newFocusVector, initialPositionFocusVector.magnitude);
			return averageInterestPosition + newFocusVector;
		}

		private Vector3 GetCurrentFocusVector() {
			return this.transform.position - GetFocusPoint();
		}

		private Vector3 GetFocusPoint() {
			Ray centerRay = camera_.ViewportPointToRay(new Vector2(0.5f, 0.5f));
			Vector3 currentFocusPoint;
			float distanceAlongRay = 0.0f;
			// if center ray misses plane
			if (kFocusPlane.Raycast(centerRay, out distanceAlongRay) == false) {
				Debug.LogWarning("Focus plane no intersection with center ray!");
				currentFocusPoint = this.transform.position;
			} else {
				currentFocusPoint = centerRay.GetPoint(distanceAlongRay);
			}

			return currentFocusPoint;
		}

		private void AnimateChromaticAberration(float intensity, float duration) {
			if (aberrationCoroutine_ != null) {
				aberrationCoroutine_.Cancel();
				aberrationCoroutine_ = null;
			}

			aberrationCoroutine_ = CoroutineWrapper.DoEaseFor(duration, EaseType.CubicEaseIn, (float p) => {
				ChromaticAberrationModel.Settings settings = postProcessingProfile_.chromaticAberration.settings;
				settings.intensity = Mathf.Lerp(intensity, 0.0f, p);
				postProcessingProfile_.chromaticAberration.settings = settings;
			});
		}

		private void StopTimeFor(float duration) {
			if (timeScaleCoroutine_ != null) {
				timeScaleCoroutine_.Cancel();
				timeScaleCoroutine_ = null;
			}

			Time.timeScale = 0.0f;
			timeScaleCoroutine_ = CoroutineWrapper.DoAfterRealtimeDelay(duration, () => {
				Time.timeScale = 1.0f;
			});
		}

		private const float kMaxFocalLength = 36.0f;
		private const float kMinFocalLength = 0.0f;

		private const float kDepthOfFieldAnimationDuration = 3.0f;

		private void SetDepthOfFieldEnabledInternal(bool enabled, bool animate) {
			if (depthOfFieldCoroutine_ != null) {
				depthOfFieldCoroutine_.Cancel();
				depthOfFieldCoroutine_ = null;
			}

			DepthOfFieldModel.Settings settings = postProcessingProfile_.depthOfField.settings;
			if (!animate) {
				settings.focalLength = enabled ? kMaxFocalLength : kMinFocalLength;
				postProcessingProfile_.depthOfField.settings = settings;
				postProcessingProfile_.depthOfField.enabled = enabled;
			} else {
				postProcessingProfile_.depthOfField.enabled = true;
				float start = enabled ? kMinFocalLength : kMaxFocalLength;
				float end = enabled ? kMaxFocalLength : kMinFocalLength;
				depthOfFieldCoroutine_ = CoroutineWrapper.DoEaseFor(kDepthOfFieldAnimationDuration, EaseType.CubicEaseOut, (float p) => {
					settings.focalLength = Mathf.Lerp(start, end, p);
					postProcessingProfile_.depthOfField.settings = settings;
				}, () => {
					postProcessingProfile_.depthOfField.enabled = enabled;
				});
			}
		}
	}
}