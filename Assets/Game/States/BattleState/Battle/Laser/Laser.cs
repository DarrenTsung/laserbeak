using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class Laser : MonoBehaviour {
		// PRAGMA MARK - Internal
		private const float kLaserSpeed = 15.0f;

		private Rigidbody rigidbody_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
		}

		private void FixedUpdate() {
			Vector3 deltaWorldPosition = this.transform.forward * kLaserSpeed * Time.fixedDeltaTime;
			rigidbody_.MovePosition(rigidbody_.position + deltaWorldPosition);
		}
	}
}