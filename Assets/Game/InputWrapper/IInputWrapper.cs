using System;
using System.Collections;
using UnityEngine;

namespace DT.Game {
	public interface IInputWrapper {
		bool CommandWasPressed { get; }
		bool CommandIsPressed { get; }
		Vector2 MovementVector { get; }
		bool LaserIsPressed { get; }
		bool PositiveWasPressed { get; }
		bool PositiveIsPressed { get; }
		bool NegativeWasPressed { get; }
		bool NegativeIsPressed { get; }
	}
}