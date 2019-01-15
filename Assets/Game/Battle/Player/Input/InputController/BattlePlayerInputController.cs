using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputController : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		private static readonly HashSet<Type> kAlwaysOnInputTypes = new HashSet<Type>() {
			typeof(BattlePlayerInputBeakControl),
		};


		// PRAGMA MARK - Public Interface
		public enum PriorityKey {
			Internal = 0,
			Movement = 1,
			OffGround = 10,
			Paused = 20,
			GameMode = 21,
		}

		public void DisableInput(PriorityKey key) {
			priorityKeyEnabledMap_[(int)key] = false;
			RefreshEnabledStatus();
		}

		public void ClearInput(PriorityKey key) {
			priorityKeyEnabledMap_.Remove((int)key);
			RefreshEnabledStatus();
		}

		public void EnableInput(PriorityKey key) {
			priorityKeyEnabledMap_[(int)key] = true;
			RefreshEnabledStatus();
		}

		public void InitInput(BattlePlayer player, IBattlePlayerInputDelegate inputDelegate) {
			foreach (var component in playerInputComponents_) {
				component.Init(player, this, inputDelegate);
			}
			EnableInput(PriorityKey.Internal);
		}

		public void RegisterAnimatedMovement(CoroutineWrapper movementCoroutine) {
			CancelAnyAnimatedMovements();
			movementCoroutine_ = movementCoroutine;
			dieWhenOffGround_.PauseCheckingDeath();
		}

		public void CancelAnyAnimatedMovements() {
			if (movementCoroutine_ != null) {
				movementCoroutine_.Cancel();
				movementCoroutine_ = null;
			}

			if (resumeCheckingDeathCoroutine_ != null) {
				StopCoroutine(resumeCheckingDeathCoroutine_);
				resumeCheckingDeathCoroutine_ = null;
			}

			resumeCheckingDeathCoroutine_ = this.DoAfterDelay(kResumeCheckingOffGroundDelay, () => {
				dieWhenOffGround_.ResumeCheckingDeath();
				resumeCheckingDeathCoroutine_ = null;
			});
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			priorityKeyEnabledMap_.Clear();

			DisableInput(PriorityKey.Internal);
			CancelAnyAnimatedMovements();
		}


		// PRAGMA MARK - Internal
		private const float kResumeCheckingOffGroundDelay = 0.15f;

		private BattlePlayerInputComponent[] playerInputComponents_;
		private readonly Dictionary<int, bool> priorityKeyEnabledMap_ = new Dictionary<int, bool>();

		private CoroutineWrapper movementCoroutine_;
		private BattlePlayerDieWhenOffGround dieWhenOffGround_;
		private Coroutine resumeCheckingDeathCoroutine_;

		private readonly HashSet<BattlePlayerInputComponent> componentsToKeepOn_ = new HashSet<BattlePlayerInputComponent>();

		private void Awake() {
			playerInputComponents_ = this.GetComponentsInChildren<BattlePlayerInputComponent>();
			dieWhenOffGround_ = this.GetComponentInChildren<BattlePlayerDieWhenOffGround>();

			foreach (var component in playerInputComponents_) {
				if (kAlwaysOnInputTypes.Contains(component.GetType())) {
					componentsToKeepOn_.Add(component);
				}
			}
		}

		[SerializeField]
		private bool debug_ = false;
		private void Update() {
			if (!debug_) {
				return;
			}

			foreach (var kvp in priorityKeyEnabledMap_) {
				Debug.Log("kvp.Key: " + kvp.Key);
				Debug.Log("kvp.Value: " + kvp.Value);
			}
		}

		private void RefreshEnabledStatus() {
			bool enabled = priorityKeyEnabledMap_.MaxBy(kvp => kvp.Key).Value;
			foreach (var component in playerInputComponents_) {
				if (componentsToKeepOn_.Contains(component)) {
					component.Enabled = true;
				} else {
					component.Enabled = enabled;
				}
			}
		}
	}
}