using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerParts : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			float segmentSize = 1.0f / kSegments;
			float halfSegmentSize = segmentSize / 2.0f;
			for (float x = -0.5f + halfSegmentSize; x <= 0.5f; x += segmentSize) {
				for (float y = -0.5f + halfSegmentSize; y <= 0.5f; y += segmentSize) {
					for (float z = -0.5f + halfSegmentSize; z <= 0.5f; z += segmentSize) {
						GameObject partObject = ObjectPoolManager.Create(partPrefab_, parent: this.gameObject);
						partObject.transform.localScale = new Vector3(segmentSize, segmentSize, segmentSize);
						partObject.transform.localPosition = new Vector3(x, y, z);
					}
				}
			}

			CoroutineWrapper.DoAfterDelay(4.0f, () => {
				ObjectPoolManager.Recycle(this);
			});
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			this.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		private const int kSegments = 3;

		[Header("Outlets")]
		[SerializeField]
		private GameObject partPrefab_;
	}
}