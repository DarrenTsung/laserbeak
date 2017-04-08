using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTViewManager.Internal;

namespace DTViewManager {
	public static class ViewManagerLocator {
		// PRAGMA MARK - Static Public Interface
		public static ViewManager Main {
			get {
				if (main_ == null) {
					GameObject main = GameObjectUtil.FindRequired("MainViewManager");
					main_ = main.GetRequiredComponent<ViewManager>();
				}
				return main_;
			}
		}

		private static ViewManager main_;
	}
}