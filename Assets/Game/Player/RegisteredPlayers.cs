using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public static bool IsInputDeviceAlreadyRegistered(InputDevice inputDevice) {
			// null inputDevice represents AI player
			if (inputDevice == null) {
				return false;
			}

			return players_.Any(p => p.InputDevice == inputDevice);
		}

		public static void Add(Player player) {
			if (IsInputDeviceAlreadyRegistered(player.InputDevice)) {
				Debug.LogWarning("Cannot add player: " + player + " because input device: " + player.InputDevice + " is already registered!");
				return;
			}

			players_.Add(player);
			OnPlayerAdded.Invoke();
		}

		public static void Remove(Player player) {
			players_.Remove(player);
			OnPlayerRemoved.Invoke();
		}

		public static IList<Player> AllPlayers {
			get { return players_; }
		}


		// PRAGMA MARK - Static Internal
		private static readonly List<Player> players_ = new List<Player>();
	}
}