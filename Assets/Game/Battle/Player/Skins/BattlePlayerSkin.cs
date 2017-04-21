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
		public Material BodyMaterial {
			get { return bodyMaterial_; }
		}

		public Material LaserMaterial {
			get { return laserMaterial_; }
		}

		public Color Color {
			get { return laserMaterial_.GetColor("_EmissionColor"); }
		}

		public Sprite ThumbnailSprite;


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Material bodyMaterial_;

		[SerializeField]
		private Material laserMaterial_;
	}
}