using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	[RequireComponent(typeof(Animator))]
	public class AIStateMachine : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public enum State {
			Attack,
		}

		public AIInputState InputState {
			get { return inputState_; }
		}

		public BattlePlayer Player {
			get { return player_; }
		}

		public AIConfiguration AIConfiguration {
			get { return configuration_; }
		}

		public void Init(BattlePlayer player, AIConfiguration configuration) {
			player_ = player;
			configuration_ = configuration;

			player_.SetInputDelegate(InputState);
			playerRecyclable_ = player.GetComponentInChildren<RecyclablePrefab>();
			playerRecyclable_.OnCleanup += RecycleSelf;
		}

		public void SwitchState(State state) {
			animator_.CrossFade(Enum.GetName(typeof(State), state), transitionDuration: 0.0f);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			this.ConfigureAllStateBehaviours(animator_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			player_ = null;
			configuration_ = null;

			if (playerRecyclable_ != null) {
				playerRecyclable_.OnCleanup -= RecycleSelf;
				playerRecyclable_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField, ReadOnly]
		private AIConfiguration configuration_;

		private BattlePlayer player_;
		private Animator animator_;
		private AIInputState inputState_;

		private RecyclablePrefab playerRecyclable_;

		private void Awake() {
			inputState_ = this.GetRequiredComponent<AIInputState>();
			animator_ = this.GetRequiredComponent<Animator>();
		}

		private void RecycleSelf(RecyclablePrefab unused) {
			ObjectPoolManager.Recycle(this);
		}
	}
}