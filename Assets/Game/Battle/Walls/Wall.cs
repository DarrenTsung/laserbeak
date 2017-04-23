using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Walls {
	public class Wall : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			for (int i = 0; i < vertexLocalPositions_.Length - 1; i++) {
				Vector3 aPoint = vertexLocalPositions_[i] + this.transform.position;
				Vector3 bPoint = vertexLocalPositions_[i + 1] + this.transform.position;

				if (aPoint == bPoint) {
					Debug.LogWarning("Should not have contiguous wall segments at same position :<");
					continue;
				}

				Vector3 vector = bPoint - aPoint;

				Quaternion rotation = Quaternion.LookRotation(vector);
				GameObject wallSegment = ObjectPoolManager.Create(GamePrefabs.Instance.WallSegmentPrefab, aPoint, rotation, parent: this.gameObject);
				wallSegment.transform.localScale = new Vector3(1.0f, 1.0f, vector.magnitude);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Vector3[] vertexLocalPositions_;

		private void OnDrawGizmos() {
			for (int i = 0; i < vertexLocalPositions_.Length - 1; i++) {
				Vector3 aPoint = vertexLocalPositions_[i] + this.transform.position;
				Vector3 bPoint = vertexLocalPositions_[i + 1] + this.transform.position;

				for (float y = 0; y <= 1.5f; y += 0.25f) {
					Vector3 raisedAPoint = aPoint.SetY(y);
					Vector3 raisedBPoint = bPoint.SetY(y);
					Gizmos.DrawLine(raisedAPoint, raisedBPoint);
				}
			}
		}
	}
}