using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Player;
using DTAnimatorStateMachine;

namespace DT.Game.Battle.AI {
	[CreateAssetMenu(fileName = "AIConfiguration", menuName = "Game/AIConfiguration")]
	public class AIConfiguration : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public float SkillLevel {
			get { return skillLevel_; }
		}


		// PRAGMA MARK - Internal
		[SerializeField, Range(0.0f, 1.0f)]
		private float skillLevel_;
	}
}