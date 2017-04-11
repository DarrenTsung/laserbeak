using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputController : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void DisableInput() {
			foreach (var component in playerInputComponents_) {
				component.Enabled = false;
			}
		}

		public void EnableInput() {
			foreach (var component in playerInputComponents_) {
				component.Enabled = true;
			}
		}

		public void InitInput(BattlePlayer player, IInputDelegate inputDelegate) {
			foreach (var component in playerInputComponents_) {
				component.Init(player, this, inputDelegate);
			}
			EnableInput();
		}

		public void RegisterAnimatedMovement(CoroutineWrapper movementCoroutine) {
			CancelAnyAnimatedMovements();
			movementCoroutine_ = movementCoroutine;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			DisableInput();
			CancelAnyAnimatedMovements();
		}


		// PRAGMA MARK - Internal
		private BattlePlayerInputComponent[] playerInputComponents_;

		private CoroutineWrapper movementCoroutine_;

		private void Awake() {
			playerInputComponents_ = this.GetComponentsInChildren<BattlePlayerInputComponent>();
		}

		private void CancelAnyAnimatedMovements() {
			if (movementCoroutine_ != null) {
				movementCoroutine_.Cancel();
				movementCoroutine_ = null;
			}
		}
	}
}