using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayerInputController : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SetInput(InputDevice inputDevice) {
			foreach (var component in playerInputComponents_) {
				component.Init(this, inputDevice);
			}
		}


		// PRAGMA MARK - Internal
		private IBattlePlayerInputComponent[] playerInputComponents_;

		private void Awake() {
			playerInputComponents_ = this.GetComponentsInChildren<IBattlePlayerInputComponent>();
		}
	}
}