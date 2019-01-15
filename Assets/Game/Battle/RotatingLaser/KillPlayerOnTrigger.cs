using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class KillPlayerOnTrigger : MonoBehaviour {
		// PRAGMA MARK - Internal
		private void OnTriggerEnter(Collider collider) {
			BattlePlayer player = collider.GetComponentInParent<BattlePlayer>();
			if (player == null) {
				return;
			}

			player.Health.Kill();
		}
	}
}