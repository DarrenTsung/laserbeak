using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public interface IBattlePlayerInputComponent {
		void Init(BattlePlayerInputController controller, InputDevice inputDevice);
	}
}