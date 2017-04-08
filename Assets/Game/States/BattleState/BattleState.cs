using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle {
	public class BattleState : DTStateMachineBehaviour<GameStateMachine> {
		// PRAGMA MARK - Internal
		[SerializeField]
		private GameObject battlePrefab_;

		private GameObject battleObject_;

		protected override void OnStateEntered() {
			battleObject_ = ObjectPoolManager.Create(battlePrefab_);
		}

		protected override void OnStateExited() {
			if (battleObject_ != null) {
				ObjectPoolManager.Recycle(battleObject_);
				battleObject_ = null;
			}
		}
	}
}