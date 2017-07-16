using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	[RequireComponent(typeof(Animator))]
	public class AIStateMachine : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public enum State {
			Attack,
			Idle,
			Dash,
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

		public GizmoOutlet GizmoOutlet {
			get; private set;
		}

		public Vector2 DashDirection {
			get; private set;
		}

		public void Dash(Vector2 direction) {
			DashDirection = direction.normalized;
			SwitchState(State.Dash);
		}

		public void Init(BattlePlayer player, AIConfiguration configuration) {
			player_ = player;
			configuration_ = configuration;

			player_.SetInputDelegate(InputState);
			playerRecyclable_ = player.GetComponentInChildren<RecyclablePrefab>();
			playerRecyclable_.OnCleanup += RecycleSelf;

			foreach (var eventHandler in eventHandlers_) {
				eventHandler.Setup();
			}

			SwitchState(State.Idle);
		}

		public void SwitchState(State state) {
			animator_.CrossFade(Enum.GetName(typeof(State), state), transitionDuration: 0.0f);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			this.EnableAllStateBehaviours(animator_);
			this.ConfigureAllStateBehaviours(animator_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.DisableAllStateBehaviours(animator_);

			player_ = null;
			configuration_ = null;

			if (playerRecyclable_ != null) {
				playerRecyclable_.OnCleanup -= RecycleSelf;
				playerRecyclable_ = null;
			}

			foreach (var eventHandler in eventHandlers_) {
				eventHandler.Cleanup();
			}
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField, ReadOnly, DTValidator.Optional]
		private AIConfiguration configuration_;

		private BattlePlayer player_;
		private Animator animator_;
		private AIInputState inputState_;

		private RecyclablePrefab playerRecyclable_;

		private AIEventHandler[] eventHandlers_ = new AIEventHandler[] {
			new AIDodgeHandler()
		};

		private void Awake() {
			inputState_ = this.GetRequiredComponent<AIInputState>();
			animator_ = this.GetRequiredComponent<Animator>();

			GizmoOutlet = this.GetRequiredComponent<GizmoOutlet>();

			foreach (var eventHandler in eventHandlers_) {
				eventHandler.Init(this);
			}
		}

		private void RecycleSelf(RecyclablePrefab unused) {
			ObjectPoolManager.Recycle(this);
		}
	}
}