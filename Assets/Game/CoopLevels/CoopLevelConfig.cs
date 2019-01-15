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
	[Serializable]
	[CreateAssetMenu(fileName = "Level1-1", menuName = "Game/CoopLevelConfig")]
	public class CoopLevelConfig : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public ArenaConfig ArenaConfig {
			get { return arenaConfig_; }
		}

		public string DisplayName {
			get { return displayName_; }
		}



		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private ArenaConfig arenaConfig_;

		[Header("Properties")]
		[SerializeField]
		private string displayName_;
	}
}