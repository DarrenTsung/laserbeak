using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	[CreateAssetMenu(fileName = "BattlePlayerSkin", menuName = "Game/BattlePlayerSkin")]
	public class BattlePlayerSkin : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public Color BodyColor {
			get { return bodyColor_; }
		}

		public Color LaserColor {
			get { return laserColor_; }
		}

		public Sprite ThumbnailSprite;


		// PRAGMA MARK - Internal
		[SerializeField]
		private Color bodyColor_;
		[SerializeField]
		private Color laserColor_;
	}
}