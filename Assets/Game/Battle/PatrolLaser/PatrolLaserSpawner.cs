using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class PatrolLaserSpawner : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			patrolLaser_ = ObjectPoolManager.Create<PatrolLaser>(GamePrefabs.Instance.PatrolLaserPrefab, parent: this.gameObject);
			patrolLaser_.Init(facingDirection_, worldPatrolPoints_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (patrolLaser_ != null) {
				ObjectPoolManager.Recycle(patrolLaser_);
				patrolLaser_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private Direction facingDirection_;
		[SerializeField]
		private Vector3[] worldPatrolPoints_;

		private PatrolLaser patrolLaser_;

		#if UNITY_EDITOR
		private void OnDrawGizmos() {
			for (int i = 0; i < worldPatrolPoints_.Length; i++) {
				Gizmos.DrawCube(worldPatrolPoints_[i], size: Vector3.one * 0.4f);
			}
		}

		private void OnDrawGizmosSelected() {
			for (int i = 0; i < worldPatrolPoints_.Length; i++) {
				Gizmos.DrawCube(worldPatrolPoints_[i], size: Vector3.one * 0.4f);

				GUIStyle handlesStyle = new GUIStyle();
				handlesStyle.normal.textColor = Color.green;
				UnityEditor.Handles.Label(worldPatrolPoints_[i], string.Format("Patrol: {0}", i), handlesStyle);
			}
		}
		#endif
	}
}