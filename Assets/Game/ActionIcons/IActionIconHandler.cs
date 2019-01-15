using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;

namespace DT.Game {
	public interface IActionIconHandler {
		ActionType ActionType { get; }

		void Populate(GameObject container);
	}
}