using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public class AIEventHandler {
		// PRAGMA MARK - Public Interface
		public void Init(AIStateMachine stateMachine) {
			StateMachine_ = stateMachine;
		}

		public virtual void Setup() {
			// stub
		}

		public virtual void Cleanup() {
			// stub
		}


		// PRAGMA MARK - Internal
		protected AIStateMachine StateMachine_ { get; private set; }
	}
}