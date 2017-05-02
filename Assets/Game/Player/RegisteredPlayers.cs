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

		static RegisteredPlayers() {
			foreach (InputDevice inputDevice in InputManager.Devices) {
				RegisterPlayerFor(inputDevice);
			}

			InputManager.OnDeviceAttached += HandleDeviceAttached;
			InputManager.OnDeviceDetached += HandleDeviceDetached;
		}

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

		public static void Clear() {
			players_.Clear();
			OnPlayerRemoved.Invoke();
		}

		public static IList<Player> AllPlayers {
			get { return players_; }
		}


		// PRAGMA MARK - Static Internal
		private static readonly List<Player> players_ = new List<Player>();

		private static void RegisterPlayerFor(InputDevice inputDevice) {
			if (IsInputDeviceAlreadyRegistered(inputDevice)) {
				Debug.LogWarning("Attempting to register player for: " + inputDevice + " but already registered! Should not happen.");
				return;
			}

			Player player = new Player(inputDevice);
			player.Nickname = string.Format("P{0}", players_.Count + 1);
			player.Skin = RegisteredPlayersUtil.GetBestRandomSkin();

			Add(player);
		}

		private static void HandleDeviceAttached(InputDevice inputDevice) {
			// TODO (darren): only allow these handlers during specific times (player customization?)
			// right now players can join in middle of game.. which is bleh
			RegisterPlayerFor(inputDevice);
		}

		private static void HandleDeviceDetached(InputDevice inputDevice) {
			var player = players_.FirstOrDefault(p => p.InputDevice == inputDevice);
			if (player == null) {
				Debug.LogWarning("DeviceRemoved but not found in player list, unexpected.");
				return;
			}

			Remove(player);
		}
	}
}