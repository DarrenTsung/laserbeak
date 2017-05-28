using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.GameModes.KingOfTheHill {
	public interface IKotHScoreSource {
		event Action OnAnyScoreChanged;

		float GetPercentageScoreFor(Player player);
	}
}