using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTLocalization;
using DTObjectPoolManager;

using DT.Game.Transitions;

namespace DT.Game {
	public class ActionLabelViewConfig {
		public string LocalizationKey;
		public ActionType ActionType;

		public ActionLabelViewConfig(string localizationKey, ActionType actionType) {
			LocalizationKey = localizationKey;
			ActionType = actionType;
		}
	}

	public static class ActionLabelBar {
		// PRAGMA MARK - Static
		public static void Show(ActionLabelViewConfig[] viewConfigs) {
			ActionLabelBar_.RecycleAllChildren();

			foreach (var viewConfig in viewConfigs) {
				ActionLabelView.Create(viewConfig.LocalizationKey, viewConfig.ActionType, ActionLabelBar_);
			}

			RefreshBarContainer();
			Localization.OnCultureChanged += RefreshBarContainer;

			Transition_.AnimateIn();
		}

		public static void Hide() {
			Transition_.AnimateOut(() => {
				Localization.OnCultureChanged -= RefreshBarContainer;
				ActionLabelBar_.RecycleAllChildren();
			});
		}


		// PRAGMA MARK - Internal
		private static GameObject actionLabelBar_;
		private static GameObject ActionLabelBar_ {
			get { return actionLabelBar_ ?? (actionLabelBar_ = ObjectPoolManager.CreateView(GamePrefabs.Instance.ActionLabelBarPrefab)); }
		}

		private static Transition Transition_ {
			get { return new Transition(ActionLabelBar_); }
		}

		private static void RefreshBarContainer() {
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ActionLabelBar_.transform);
		}
	}
}