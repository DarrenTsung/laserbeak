using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game {
	public class SmearEffect : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			recentPositions_.Clear();
			SmearMaterial_.SetVector("_PrevPosition", this.transform.position);
			SmearMaterial_.SetVector("_Position", this.transform.position);

			enabled_ = true;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			enabled_ = false;
		}


		// PRAGMA MARK - Internal
		private Queue<Vector3> recentPositions_ = new Queue<Vector3>();

		[SerializeField]
		int frameLag_ = 0;

		[SerializeField]
		private Renderer renderer_;

		Material smearMaterial_ = null;
		private bool enabled_;

		public Material SmearMaterial_ {
			get {
				if (smearMaterial_ == null || smearMaterial_ != renderer_.material) {
					smearMaterial_ = renderer_.material;
				}

				return smearMaterial_;
			}
		}

		private void LateUpdate() {
			if (!enabled_) {
				return;
			}

			if (recentPositions_.Count > frameLag_) {
				Vector3 previousPosition = recentPositions_.Dequeue();
				SmearMaterial_.SetVector("_PrevPosition", previousPosition);
			}

			SmearMaterial_.SetVector("_Position", this.transform.position);
			recentPositions_.Enqueue(this.transform.position);
		}
	}
}