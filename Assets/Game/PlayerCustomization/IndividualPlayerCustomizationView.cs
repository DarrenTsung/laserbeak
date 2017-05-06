using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.PlayerCustomization.States;

namespace DT.Game.PlayerCustomization {
	public class IndividualPlayerCustomizationView : MonoBehaviour, IRecycleCleanupSubscriber {
		private enum State {
			CanJoin = 0,
			Skin = 1,
			Nickname = 2,
			Ready = 3,
		}


		// PRAGMA MARK - Public Interface
		public event Action OnStateChanged = delegate {};

		public void Init(Player player) {
			player_ = player;

			var view = ObjectPoolManager.Create<InGamePlayerView>(GamePrefabs.Instance.InGamePlayerViewPrefab, parent: playerViewContainer_);
			view.InitWith(player);

			if (player_.IsAI || player_.IsProperlyCustomized()) {
				state_ = State.Ready;
			} else {
				state_ = (State)0;
			}
			HandleNewState();
		}

		public Player Player {
			get { return player_; }
		}

		public bool IsReady {
			get { return state_ == State.Ready; }
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewContainer_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;

		[SerializeField]
		private GameObject currentStateContainer_;

		private Player player_;
		private State state_;
		private IndividualPlayerCustomizationState stateHandler_;

		private void Update() {
			if (player_.IsAI) {
				return;
			}

			if (stateHandler_ != null) {
				stateHandler_.Update();
			}
		}

		private void MoveToNextState() {
			ChangeState(transformation: s => EnumUtil.ClampMove(s, 1));
		}

		private void MoveToPreviousState() {
			ChangeState(transformation: s => EnumUtil.ClampMove(s, -1));
		}

		private void ChangeState(Func<State, State> transformation) {
			State previousState = state_;
			state_ = transformation.Invoke(state_);

			if (previousState == state_) {
				return;
			}

			HandleNewState();
		}

		private void HandleNewState() {
			if (stateHandler_ != null) {
				stateHandler_.Cleanup();
				stateHandler_ = null;
			}

			switch (state_) {
				case State.CanJoin:
					stateHandler_ = new StateCanJoin(player_, currentStateContainer_, MoveToNextState, MoveToPreviousState);
					break;
				case State.Skin:
					stateHandler_ = new StateSkinCustomization(player_, currentStateContainer_, MoveToNextState, MoveToPreviousState);
					break;
				case State.Nickname:
					stateHandler_ = new StateNicknameCustomization(player_, currentStateContainer_, MoveToNextState, MoveToPreviousState);
					break;
				case State.Ready:
					stateHandler_ = new StateReady(player_, currentStateContainer_, MoveToNextState, MoveToPreviousState);
					break;
			}

			OnStateChanged.Invoke();
		}
	}
}