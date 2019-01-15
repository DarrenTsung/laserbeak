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

		[Header("Properties")]
		[SerializeField]
		private int frameLag_ = 0;
		[SerializeField]
		private bool useSharedMaterial_ = true;
		[SerializeField]
		private bool noSmearForward_ = true;

		[Space]
		[SerializeField]
		private Renderer renderer_;


		Material smearMaterial_ = null;
		private bool enabled_;
		private bool materialHasBeenSet_ = false;

		private Material RendererMaterial_ {
			get { return useSharedMaterial_ ? renderer_.sharedMaterial : renderer_.material; }
		}

		private void Awake() {
			smearMaterial_ = RendererMaterial_;
			HandleNewSmearMaterial();
		}

		private bool CheckMaterialHasBeenSet() {
			if (smearMaterial_ != RendererMaterial_) {
				smearMaterial_ = RendererMaterial_;
				HandleNewSmearMaterial();
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
			smearMaterial_.SetVector("_ForwardDirection", this.transform.forward);
			recentPositions_.Enqueue(this.transform.position);
		}

		private void HandleNewSmearMaterial() {
			if (noSmearForward_) {
				smearMaterial_.EnableKeyword("NO_SMEAR_FORWARD");
			} else {
				smearMaterial_.DisableKeyword("NO_SMEAR_FORWARD");
			}
		}
	}
}