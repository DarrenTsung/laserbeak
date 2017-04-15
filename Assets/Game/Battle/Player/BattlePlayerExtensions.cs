using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public static class BattlePlayerExtensions {
		public static float WeightedRatio(this BattlePlayer player) {
			return player.BaseWeight / player.Weight;
		}
	}
}