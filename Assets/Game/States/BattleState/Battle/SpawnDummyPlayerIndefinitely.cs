using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Player;

namespace DT.Game.Battle {
	public class SpawnDummyPlayerIndefinitely : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			SpawnDummyPlayer();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			CleanupDummyPlayer(recycle: true);
		}

		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject playerPrefab_;

		private RecyclablePrefab dummyPlayerRecyclable_;

		private void CleanupDummyPlayer(bool recycle) {
			if (dummyPlayerRecyclable_ != null) {
				if (recycle) {
					ObjectPoolManager.Recycle(dummyPlayerRecyclable_);
				}

				dummyPlayerRecyclable_.OnCleanup -= RespawnDummyPlayer;
				dummyPlayerRecyclable_ = null;
			}
		}

		private void SpawnDummyPlayer() {
			GameObject playerObject = ObjectPoolManager.Create(playerPrefab_, parent: this.gameObject, position: this.transform.position);
			dummyPlayerRecyclable_ = playerObject.GetComponentInChildren<RecyclablePrefab>();
			dummyPlayerRecyclable_.OnCleanup += RespawnDummyPlayer;
		}

		private void RespawnDummyPlayer(RecyclablePrefab unused) {
			CleanupDummyPlayer(recycle: false);
			CoroutineWrapper.DoAfterDelay(3.0f, () => {
				SpawnDummyPlayer();
			});
		}
	}
}