using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle {
	public class PlayerSpawnPoint : MonoBehaviour {
		// PRAGMA MARK - Internal
		private void OnDrawGizmos() {
			Gizmos.DrawWireCube(this.transform.position, size: Vector3.one);
		}
	}
}