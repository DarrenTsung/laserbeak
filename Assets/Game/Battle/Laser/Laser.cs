using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;

namespace DT.Game.Battle.Lasers {
	public class Laser : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public BattlePlayer BattlePlayer {
			get { return battlePlayer_; }
		}

		public void Init(BattlePlayer battlePlayer) {
			battlePlayer_ = battlePlayer;
			AudioConstants.Instance.LaserShoot.PlaySFX(volumeScale: 0.4f);
			BattleCamera.Shake(0.14f);
		}

		public void SetMaterial(Material material) {
			laserRenderer_.material = material;
			light_.color = material.GetColor("_EmissionColor");
		}

		public void HandleHit() {
			LaserHit laserHit = ObjectPoolManager.Create<LaserHit>(laserHitParticlePrefab_, this.transform.position, this.transform.rotation, parent: BattleRecyclables.Instance);
			laserHit.SetMaterial(laserRenderer_.material);
		}


		// PRAGMA MARK - Internal
		private const float kLaserSpeed = 25.0f;

		// in degrees per second
		private const float kRotationSpeed = 40.0f;
		private const float kRotationMaxAngle = 60.0f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject laserHitParticlePrefab_;

		[SerializeField]
		private Light light_;

		[SerializeField]
		private Renderer laserRenderer_;

		private BattlePlayer battlePlayer_;
		private Rigidbody rigidbody_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
		}

		private void FixedUpdate() {
			rigidbody_.velocity = Vector3.zero;
			rigidbody_.angularVelocity = Vector3.zero;

			CurveLaserTowardPlayers();

			Vector3 deltaWorldPosition = this.transform.forward * kLaserSpeed * Time.fixedDeltaTime;
			rigidbody_.MovePosition(rigidbody_.position + deltaWorldPosition);
		}

		private void CurveLaserTowardPlayers() {
			Quaternion minRotation = Quaternion.identity;
			float minDeltaAngle = float.MaxValue;
			foreach (BattlePlayer player in BattlePlayer.ActivePlayers) {
				Vector3 delta = (player.transform.position - this.transform.position).normalized;
				if (delta.magnitude <= Mathf.Epsilon) {
					continue;
				}

				Quaternion rotationToPlayer = Quaternion.LookRotation(delta.normalized);
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