using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	[RequireComponent(typeof(Camera))]
	public class SetAsBattleCamera : MonoBehaviour {
		// PRAGMA MARK - Internal
		private void Awake() {
			Battle.Camera = this.GetRequiredComponent<Camera>();
		}
	}
}