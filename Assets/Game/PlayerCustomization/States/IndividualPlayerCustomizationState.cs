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

namespace DT.Game.PlayerCustomization.States {
	public class IndividualPlayerCustomizationState {
		// PRAGMA MARK - Public Interface
		public IndividualPlayerCustomizationState(Player player, GameObject container, Action moveToNextState, Action moveToPreviousState) {
			player_ = player;
			container_ = container;
			moveToNextState_ = moveToNextState;
			moveToPreviousState_ = moveToPreviousState;

			Init();
		}

		public virtual void Update() {
			// stub
		}

		public virtual void Cleanup() {
			// stub
		}


		// PRAGMA MARK - Internal
		protected Player Player_ {
			get { return player_; }
		}

		protected GameObject Container_ {
			get { return container_; }
		}

		protected virtual void Init() {
			// stub
		}

		protected void MoveToNextState() {
			moveToNextState_.Invoke();
		}

		protected void MoveToPreviousState() {
			moveToPreviousState_.Invoke();
		}

		private Player player_;
		private GameObject container_;
		private Action moveToNextState_;
		private Action moveToPreviousState_;
	}
}