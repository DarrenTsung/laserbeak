using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using TMPro;

namespace DT.Game.LevelEditor {
	public class LevelEditorPlayerSpawnPoint : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public int PlayerIndex = 0;

		public void SetPlayerIndex(int playerIndex) {
			PlayerIndex = playerIndex;
			text_.text = PlayerIndex.ToString();
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private TMP_Text text_;
	}
}