using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerComponent : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void Init(BattlePlayer player) {
			player_ = player;
		}


		// PRAGMA MARK - Internal
		protected BattlePlayer Player_ {
			get { return player_; }
		}

		private BattlePlayer player_;
	}
}