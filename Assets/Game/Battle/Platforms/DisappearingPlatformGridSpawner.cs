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
	public class DisappearingPlatformGridSpawner : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			for (float x = -gridHalfWidth_; x <= gridHalfWidth_; x++) {
				float worldX = x * kEdgeSize;
				for (float y = -gridHalfHeight_; y <= gridHalfHeight_; y++) {
					float worldY = y * kEdgeSize;
					GameObject platform = ObjectPoolManager.Create(GamePrefabs.Instance.DisappearingPlatformPrefab, parent: this.gameObject);
					platform.transform.localPosition = new Vector3(worldX, 0.0f, worldY);
				}
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.gameObject.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		private const float kEdgeSize = 3.0f;

		[Header("Properties")]
		[SerializeField]
		private int gridHalfWidth_ = 3;

		[SerializeField]
		private int gridHalfHeight_ = 3;
	}
}