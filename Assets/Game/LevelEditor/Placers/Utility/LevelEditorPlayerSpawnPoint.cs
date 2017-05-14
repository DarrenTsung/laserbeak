using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using TMPro;

namespace DT.Game.LevelEditor {
	public class LevelEditorPlayerSpawnPoint : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Public Interface
		public int PlayerIndex = 0;


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			text_.text = PlayerIndex.ToString();
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private TMP_Text text_;
	}
}