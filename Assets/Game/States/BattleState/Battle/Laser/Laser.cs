using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Lasers {
	public class Laser : MonoBehaviour {
		// PRAGMA MARK - Internal
		private const float kLaserSpeed = 25.0f;

		// in degrees per second
		private const float kRotationSpeed = 40.0f;
		private const float kRotationMaxAngle = 60.0f;

		private Rigidbody rigidbody_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
		}

		private void FixedUpdate() {
			CurveLaserTowardPlayers();

			Vector3 deltaWorldPosition = this.transform.forward * kLaserSpeed * Time.fixedDeltaTime;
			rigidbody_.MovePosition(rigidbody_.position + deltaWorldPosition);
		}

		private void CurveLaserTowardPlayers() {
			Quaternion minRotation = Quaternion.identity;
			float minDeltaAngle = float.MaxValue;
			foreach (BattlePlayer player in BattlePlayer.ActivePlayers) {
				Vector3 delta = (player.transform.position - this.transform.position).normalized;
				if (delta.magnitude <= Mathf.Epsilon * 2.0f) {
					continue;
				}

				Quaternion rotationToPlayer = Quaternion.LookRotation(delta);
				float deltaAngle = Quaternion.Angle(this.transform.rotation, rotationToPlayer);
				if (deltaAngle < minDeltaAngle) {
					minDeltaAngle = deltaAngle;
					minRotation = rotationToPlayer;
				}
			}

			float rotationMultiplier = Mathf.Clamp(1.0f - Easings.CubicEaseIn(minDeltaAngle / kRotationMaxAngle), 0.0f, 1.0f);
			// in degrees
			float rotationSpeed = kRotationSpeed * rotationMultiplier * Time.fixedDeltaTime;
			float rotationLerpPercentage = Mathf.Clamp(rotationSpeed / minDeltaAngle, 0.0f, 1.0f);
			rigidbody_.MoveRotation(Quaternion.Lerp(this.transform.rotation, minRotation, rotationLerpPercentage));
		}
	}
}