using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public static class AIUtil {
		// PRAGMA MARK - Public Interface
		public static bool IsXZPositionOnPlatform(Vector3 position) {
			position = position.SetY(kFootPositionY);
			int hits = Physics.RaycastNonAlloc(new Ray(position, -Vector3.up), kRaycastResults_, maxDistance: kPenetrationLength, layerMask: InGameConstants.PlatformsLayerMask);
			return hits > 0;
		}

		public static bool DoesWallExistBetweenXZPoints(Vector3 pointA, Vector3 pointB) {
			pointA = pointA.SetY(kFootPositionY);
			pointB = pointB.SetY(kFootPositionY);

			Vector3 aToB = pointB - pointA;
			Ray aToBRay = new Ray(pointA, aToB.normalized);
			int hits = Physics.RaycastNonAlloc(aToBRay, kRaycastResults_, maxDistance: aToB.magnitude, layerMask: InGameConstants.PlatformsLayerMask);
			return hits > 0;
		}

		public static float GetRandomPredictedDashDistance() {
			return UnityEngine.Random.Range(kDashPredictionDistanceMin, kDashPredictionDistanceMax);
		}


		// PRAGMA MARK - Internal
		private const float kDashPredictionDistanceMin = 3.5f;
		private const float kDashPredictionDistanceMax = 4.5f;

		private const float kFootPositionY = 0.1f;
		private const float kPenetrationLength = 0.3f;

		private static RaycastHit[] kRaycastResults_ = new RaycastHit[1];
	}
}