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
		public Color UIColor {
			get { return bodyColor_; }
		}

		public Color BodyColor {
			get { return bodyColor_; }
		}

		public Color LaserColor {
			get { return laserColor_; }
		}

		public Material BodyPartMaterial {
			get { return cachedBodyPartMaterial_ ?? (cachedBodyPartMaterial_ = CreateCachedMaterial(GameConstants.Instance.PlayerOpaqueMaterial, BodyColor)); }
		}

		public Material OpaqueBodyMaterial {
			get { return cachedOpaqueBodyMaterial_ ?? (cachedOpaqueBodyMaterial_ = CreateCachedMaterial(GameConstants.Instance.PlayerOpaqueMaterial, BodyColor)); }
		}

		public Material TransparentBodyMaterial {
			get { return cachedTransparentBodyMaterial_ ?? (cachedTransparentBodyMaterial_ = CreateCachedMaterial(GameConstants.Instance.PlayerOpaqueMaterial, BodyColor)); }
		}

		public Material BeakMaterial {
			get { return cachedBeakMaterial_ ?? (cachedBeakMaterial_ = CreateCachedMaterial(GameConstants.Instance.BeakMaterial)); }
		}

		public Material EyeMaterial {
			get { return cachedEyeMaterial_ ?? (cachedEyeMaterial_ = CreateCachedMaterial(GameConstants.Instance.EyeMaterial)); }
		}

		public Material SmearedLaserMaterial {
			get { return cachedSmearedLaserMaterial_ ?? (cachedSmearedLaserMaterial_ = CreateCachedMaterial(GameConstants.Instance.LaserMaterial, LaserColor, LaserColor)); }
		}

		public Material LaserMaterial {
			get { return cachedLaserMaterial_ ?? (cachedLaserMaterial_ = CreateCachedMaterial(GameConstants.Instance.LaserMaterial, LaserColor, LaserColor)); }
		}

		public Sprite ThumbnailSprite;


		// PRAGMA MARK - Internal
		[SerializeField]
		private Color bodyColor_;
		[SerializeField]
		private Color laserColor_;

		[NonSerialized]
		private Material cachedBodyPartMaterial_ = null;
		[NonSerialized]
		private Material cachedOpaqueBodyMaterial_ = null;
		[NonSerialized]
		private Material cachedTransparentBodyMaterial_ = null;
		[NonSerialized]
		private Material cachedBeakMaterial_ = null;
		[NonSerialized]
		private Material cachedEyeMaterial_ = null;
		[NonSerialized]
		private Material cachedSmearedLaserMaterial_ = null;
		[NonSerialized]
		private Material cachedLaserMaterial_ = null;

		private Material CreateCachedMaterial(Material material, Color? diffuseColor = null, Color? emissionColor = null) {
			var cachedMaterial = new Material(material);
			cachedMaterial.name = material.name + "(cached)";
			cachedMaterial.enableInstancing = true;
			if (diffuseColor != null) {
				cachedMaterial.SetColor("_DiffuseColor", diffuseColor.Value);
			}
			if (emissionColor != null) {
				cachedMaterial.SetColor("_EmissionColor", emissionColor.Value);
			}
			return cachedMaterial;
		}
	}
}