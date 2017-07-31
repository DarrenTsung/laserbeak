using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTViewManager;

namespace DT.Game {
	public static class GameViewManagerLocator {
		// PRAGMA MARK - Static Public Interface
		public static ViewManager Battle {
			get {
				if (battle_ == null) {
					GameObject main = GameObjectUtil.FindRequired("BattleViewManager");
					battle_ = main.GetRequiredComponent<ViewManager>();
				}
				return battle_;
			}
		}

		public static ViewManager Selector {
			get {
				if (selector_ == null) {
					GameObject main = GameObjectUtil.FindRequired("SelectorViewManager");
					selector_ = main.GetRequiredComponent<ViewManager>();
				}
				return selector_;
			}
		}

		private static ViewManager battle_;
		private static ViewManager selector_;
	}
}