using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

			Instance.transform.Shake(kShakeMaxAmount * percentage, kShakeMaxDuration * percentage, returnToOriginalPosition: false);
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

		private const float kXRotationMin = 45;
		private const float kXRotationMax = 65;
		// NOTE (darren): base focusVector magnitude is ~25
		private const float kRotationLerpMagnitude = 14.0f;

		// must be in range of 0...0.5f
		private const float kFocusViewportRadius = 0.3f;
		private static readonly Plane kFocusPlane = new Plane(Vector3.up, Vector3.zero);
		private const float kFocusMinimum = 7.0f;

		[Header("Properties")]
		[SerializeField]
		private float cameraSpeed_ = 1.0f;

		private IEnumerable<Transform> transformsOfInterest_;
		private bool survivingPlayersAsInterest_ = false;

		private Camera camera_;
		private Vector3 initialPosition_;

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
				targetPosition = GetTargetPositionToHighlightInterest();
			}

			this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, cameraSpeed_ * Time.deltaTime);

			float p = Mathf.Clamp((GetCurrentFocusVector().magnitude - kFocusMinimum) / kRotationLerpMagnitude, 0.0f, 1.0f);
			this.transform.rotation = Quaternion.Euler(Mathf.Lerp(kXRotationMin, kXRotationMax, p), 0, 0);
		}

		private Vector3 GetTargetPositionToHighlightInterest() {
			Vector3 averageInterestPosition = transformsOfInterest_.Select(t => t.position).Average();

			Vector3 currentFocusVector = GetCurrentFocusVector();
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
			return averageInterestPosition + newFocusVector;
		}

		private Vector3 GetCurrentFocusVector() {
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

			return this.transform.position - currentFocusPoint;
		}
	}
}