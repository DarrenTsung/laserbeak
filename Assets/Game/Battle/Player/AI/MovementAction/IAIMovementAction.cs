using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;

namespace DT.Game.Battle.AI {
	public interface IAIMovementAction : IDisposable {
		// PRAGMA MARK - Public Internal
		void Init(AIStateMachine stateMachine);
	}
}