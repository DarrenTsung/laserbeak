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
		public enum WallType {
			Reflect,
			Barrier,
		}

		public Vector3[] VertexLocalPositions {
			get { return vertexLocalPositions_; }
		}

		public void SetVertexLocalPositions(Vector3[] vertexLocalPositions) {
			this.gameObject.RecycleAllChildren();

			GameObject prefab = null;
			switch (wallType_) {
				case WallType.Reflect:
					prefab = GamePrefabs.Instance.WallSegmentReflectPrefab;
					break;
				default:
				case WallType.Barrier:
					prefab = GamePrefabs.Instance.WallSegmentBarrierPrefab;
					break;
			}

			for (int i = 0; i < vertexLocalPositions.Length - 1; i++) {
				Vector3 aPoint = vertexLocalPositions[i];
				Vector3 bPoint = vertexLocalPositions[i + 1];
				if (aPoint == bPoint) {
					bPoint = aPoint + (Vector3.right * 0.05f);
				}

				Vector3 vector = bPoint - aPoint;
				Quaternion rotation = Quaternion.LookRotation(vector);

				GameObject wallSegment = ObjectPoolManager.Create(prefab, aPoint, rotation, parent: this.gameObject);
				wallSegment.transform.localScale = new Vector3(1.0f, 1.0f, vector.magnitude);
			}

			vertexLocalPositions_ = vertexLocalPositions;
		}

		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			SetVertexLocalPositions(serializedVertexLocalPositions_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Vector3[] serializedVertexLocalPositions_;

		[Header("Properties")]
		[SerializeField]
		private WallType wallType_ = WallType.Reflect;

		private Vector3[] vertexLocalPositions_;

		private void OnDrawGizmos() {
			if (serializedVertexLocalPositions_ == null) {
				return;
			}

			for (int i = 0; i < serializedVertexLocalPositions_.Length - 1; i++) {
				Vector3 aPoint = serializedVertexLocalPositions_[i] + this.transform.position;
				Vector3 bPoint = serializedVertexLocalPositions_[i + 1] + this.transform.position;

				for (float y = 0; y <= 1.5f; y += 0.25f) {
					Vector3 raisedAPoint = aPoint.SetY(y);
					Vector3 raisedBPoint = bPoint.SetY(y);
					Gizmos.DrawLine(raisedAPoint, raisedBPoint);
				}
			}
		}
	}
}