using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputController : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public enum PriorityKey {
			Internal = 0,
			Movement = 1,
			Death = 10,
		}

		public void DisableInput(PriorityKey key) {
			priorityKeyEnabledMap_[(int)key] = false;
			RefreshEnabledStatus();
		}

		public void EnableInput(PriorityKey key) {
			priorityKeyEnabledMap_[(int)key] = true;
			RefreshEnabledStatus();
		}

		public void InitInput(BattlePlayer player, IInputDelegate inputDelegate) {
			foreach (var component in playerInputComponents_) {
				component.Init(player, this, inputDelegate);
			}
			EnableInput(PriorityKey.Internal);
		}

		public void RegisterAnimatedMovement(CoroutineWrapper movementCoroutine) {
			CancelAnyAnimatedMovements();
			movementCoroutine_ = movementCoroutine;
		}

		public void CancelAnyAnimatedMovements() {
			if (movementCoroutine_ != null) {
				movementCoroutine_.Cancel();
				movementCoroutine_ = null;
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			priorityKeyEnabledMap_.Clear();

			DisableInput(PriorityKey.Internal);
			CancelAnyAnimatedMovements();
		}


		// PRAGMA MARK - Internal
		private BattlePlayerInputComponent[] playerInputComponents_;
		private readonly Dictionary<int, bool> priorityKeyEnabledMap_ = new Dictionary<int, bool>();

		private CoroutineWrapper movementCoroutine_;

		private void Awake() {
			playerInputComponents_ = this.GetComponentsInChildren<BattlePlayerInputComponent>();
		}

		private void RefreshEnabledStatus() {
			bool enabled = priorityKeyEnabledMap_.Max(kvp => kvp.Key).Value;
			foreach (var component in playerInputComponents_) {
				component.Enabled = enabled;
			}
		}
	}
}