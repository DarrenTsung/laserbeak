using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;

namespace DT.Game.Battle.Lasers {
	public class Laser : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Public Interface
		public float SpeedMultiplier {
			get; set;
		}

		public BattlePlayer BattlePlayer {
			get { return battlePlayerSources_.LastOrDefault(); }
		}

		public void ChangeBattlePlayerSource(BattlePlayer battlePlayer) {
			battlePlayer.GetComponent<RecyclablePrefab>().OnCleanup += HandleCleanup;
			battlePlayerSources_.Add(battlePlayer);
		}

		public void Init(BattlePlayer battlePlayer) {
			ChangeBattlePlayerSource(battlePlayer);
			AudioConstants.Instance.LaserShoot.PlaySFX(volumeScale: 0.33f);
			BattleCamera.Shake(0.14f);

			Color laserColor = battlePlayer.Skin.LaserColor;
			laserRenderer_.material.SetColor("_EmissionColor", laserColor);
			laserRenderer_.material.SetColor("_DiffuseColor", laserColor);
			light_.color = laserColor;
		}

		public void HandleHit(bool destroy = true) {
			LaserHit laserHit = ObjectPoolManager.Create<LaserHit>(laserHitParticlePrefab_, this.transform.position, this.transform.rotation, parent: BattleRecyclables.Instance);
			laserHit.SetMaterial(laserRenderer_.material);
			if (destroy) {
				ObjectPoolManager.Recycle(this.gameObject);
			}
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			SpeedMultiplier = 1.0f;
			battlePlayerSources_.Clear();
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

		private readonly List<BattlePlayer> battlePlayerSources_ = new List<BattlePlayer>();
		private Rigidbody rigidbody_;

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();
		}

		private void FixedUpdate() {
			rigidbody_.velocity = Vector3.zero;
			rigidbody_.angularVelocity = Vector3.zero;

			CurveLaserTowardPlayers();

			Vector3 deltaWorldPosition = this.transform.forward * kLaserSpeed * SpeedMultiplier * Time.fixedDeltaTime;
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

		private void HandleCleanup(RecyclablePrefab prefab) {
			prefab.OnCleanup -= HandleCleanup;

			BattlePlayer battlePlayer = prefab.GetComponent<BattlePlayer>();
			battlePlayerSources_.Remove(battlePlayer);
		}
	}
}