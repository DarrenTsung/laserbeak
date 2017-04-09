using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public abstract class BattlePlayerInputComponent : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public bool Enabled {
			get { return enabled_; }
			set { enabled_ = value; }
		}

		public void Init(BattlePlayer player, BattlePlayerInputController controller, InputDevice inputDevice) {
			player_ = player;
			controller_ = controller;
			inputDevice_ = inputDevice;
		}


		// PRAGMA MARK - Internal
		protected BattlePlayerInputController Controller_ {
			get { return controller_; }
		}

		protected InputDevice InputDevice_ {
			get { return inputDevice_; }
		}

		protected BattlePlayer Player_ {
			get { return player_; }
		}

		private BattlePlayerInputController controller_;
		private InputDevice inputDevice_;
		private BattlePlayer player_;

		private bool enabled_;
	}
}