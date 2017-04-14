using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public static class RegisteredPlayers {
		// PRAGMA MARK - Static Public Interface
		public static event Action OnPlayerAdded = delegate {};
		public static event Action OnPlayerRemoved = delegate {};

		public static void Add(Player player) {
			players_.Add(player);
			OnPlayerAdded.Invoke();
		}

		public static void Remove(Player player) {
			players_.Remove(player);
			OnPlayerRemoved.Invoke();
		}

		public static IList<Player> All {
			get { return players_; }
		}


		// PRAGMA MARK - Static Internal
		private static readonly List<Player> players_ = new List<Player>();
	}
}