using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public interface IInputDelegate {
		Vector2 MovementVector {
			get;
		}

		bool DashPressed {
			get;
		}

		bool LaserPressed {
			get;
		}
	}
}