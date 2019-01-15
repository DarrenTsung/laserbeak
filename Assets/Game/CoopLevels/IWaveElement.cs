using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;

namespace DT.Game.LevelSelect {
	public interface IWaveElement {
		void Spawn(Action removalCallback);
	}
}