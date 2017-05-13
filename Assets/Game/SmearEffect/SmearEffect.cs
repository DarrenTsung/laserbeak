using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game {
	public class SmearEffect : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			recentPositions_.Clear();

			if (CheckMaterialHasBeenSet()) {
				smearMaterial_.SetVector("_PrevPosition", this.transform.position);
				smearMaterial_.SetVector("_Position", this.transform.position);
			}

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
		private bool materialHasBeenSet_ = false;

		private void Awake() {
			smearMaterial_ = renderer_.sharedMaterial;
		}

		private bool CheckMaterialHasBeenSet() {
			if (smearMaterial_ != renderer_.sharedMaterial) {
				smearMaterial_ = renderer_.sharedMaterial;
				materialHasBeenSet_ = true;
			}

			return materialHasBeenSet_;
		}

		private void LateUpdate() {
			if (!enabled_) {
				return;
			}

			if (!CheckMaterialHasBeenSet()) {
				return;
			}

			if (recentPositions_.Count > frameLag_) {
				Vector3 previousPosition = recentPositions_.Dequeue();
				smearMaterial_.SetVector("_PrevPosition", previousPosition);
			} else {
				smearMaterial_.SetVector("_PrevPosition", this.transform.position);
			}

			smearMaterial_.SetVector("_Position", this.transform.position);
			recentPositions_.Enqueue(this.transform.position);
		}
	}
}