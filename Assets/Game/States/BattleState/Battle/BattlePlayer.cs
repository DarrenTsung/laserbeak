using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	[RequireComponent(typeof(Rigidbody))]
	public class BattlePlayer : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SetInput(InputDevice inputDevice) {
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - Internal
		private const float kPlayerSpeed = 4.8f;

		private Rigidbody rigidbody_;
		private InputDevice inputDevice_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
		}

		private void FixedUpdate() {
			Vector2 deltaPosition = inputDevice_.LeftStick.Value * Time.fixedDeltaTime * kPlayerSpeed;
			// convert 2D coord -> 3D
			Vector3 deltaWorldPosition = new Vector3(deltaPosition.x, 0.0f, deltaPosition.y);

			rigidbody_.MovePosition(rigidbody_.position + deltaWorldPosition);

			// snap rotation if input is not (0, 0)
			if (deltaWorldPosition.magnitude > Mathf.Epsilon) {
				rigidbody_.MoveRotation(Quaternion.LookRotation(deltaWorldPosition));
			}
		}
	}
}