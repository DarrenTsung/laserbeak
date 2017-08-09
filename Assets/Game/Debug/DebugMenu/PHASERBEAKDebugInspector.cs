using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DTDebugMenu;

namespace DT.Game.DebugMenu {
	public static class PHASERBEAKDebugInspector {
		// PRAGMA MARK - Internal
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			GenericInspector inspector = GenericInspectorRegistry.Get("PHASERBEAK");
			inspector.RegisterHeader("Properties");
			inspector.RegisterField<Color>("Background Color", setter: (c) => GameConstants.Instance.BackgroundColor = c, getter: () => GameConstants.Instance.BackgroundColor);
			inspector.RegisterField<bool>("Show FPS", (b) => FPSView.Enabled = b, () => FPSView.Enabled);
		}
	}
}