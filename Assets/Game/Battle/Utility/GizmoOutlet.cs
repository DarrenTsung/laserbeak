using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class GizmoOutlet : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SetSphere(string key, Vector3 position, float radius) {
			sphereMap_[key] = Tuple.Create(position, radius);
		}

		public void SetLineTarget(string key, Vector3 position) {
			lineMap_[key] = position;
		}


		// PRAGMA MARK - Internal
		private readonly Dictionary<string, Tuple<Vector3, float>> sphereMap_ = new Dictionary<string, Tuple<Vector3, float>>();
		private readonly Dictionary<string, Vector3> lineMap_ = new Dictionary<string, Vector3>();

		private void OnDrawGizmos() {
			foreach (Tuple<Vector3, float> sphereData in sphereMap_.Values) {
				Gizmos.DrawWireSphere(sphereData.Item1, sphereData.Item2);
			}

			foreach (Vector3 lineEndPoint in lineMap_.Values) {
				Gizmos.DrawLine(this.transform.position, lineEndPoint);
			}
		}
	}
}