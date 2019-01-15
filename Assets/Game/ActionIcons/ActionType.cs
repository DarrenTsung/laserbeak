using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;

using DT.Game.Players;

namespace DT.Game {
	public enum ActionType {
		Positive,
		Command,
		Negative,
	}

	public static class ActionTypeExtensions {
		public static bool IsHeld(this ActionType actionType, IEnumerable<IInputWrapper> inputs = null) {
			if (inputs == null) {
				inputs = RegisteredPlayers.AllInputs;
			}

			switch (actionType) {
				case ActionType.Positive:
					return inputs.Any(i => i.PositiveIsPressed);
				case ActionType.Command:
					return inputs.Any(i => i.CommandIsPressed);
				case ActionType.Negative:
				default:
					return inputs.Any(i => i.NegativeIsPressed);
			}
		}
	}
}