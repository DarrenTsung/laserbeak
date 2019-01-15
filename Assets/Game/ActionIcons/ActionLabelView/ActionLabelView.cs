using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;

namespace DT.Game {
	public class ActionLabelView : MonoBehaviour {
		// PRAGMA MARK - Static
		public static void Create(string localizationKey, ActionType actionType, GameObject parent) {
			var view = ObjectPoolManager.Create<ActionLabelView>(GamePrefabs.Instance.ActionLabelViewPrefab, parent);
			view.Init(localizationKey, actionType);
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private ActionIconOutlet actionIconOutlet_;
		[SerializeField]
		private TextOutlet actionTextOutlet_;

		private void Init(string localizationKey, ActionType actionType) {
			actionTextOutlet_.SetLocalizedKey(localizationKey);
			actionIconOutlet_.Init(actionType);
		}
	}
}