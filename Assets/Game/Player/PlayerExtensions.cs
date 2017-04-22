using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public static class PlayerExtensions {
		// PRAGMA MARK - Static Public Interface
		public static int Index(this Player player) {
			return RegisteredPlayers.AllPlayers.IndexOf(player);
		}
	}
}