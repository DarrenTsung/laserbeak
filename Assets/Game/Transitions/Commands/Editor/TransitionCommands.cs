using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DT.Game.Transitions {
	public static class TransitionCommands {
		// PRAGMA MARK - Static
		[DTCommandPalette.MethodCommand]
		public static void AnimateIn() {
			if (Selection.activeGameObject == null) {
				Debug.LogWarning("Can't AnimateIn when no selected GameObject!");
				return;
			}

			Transition transition = new Transition(Selection.activeGameObject);
			transition.AnimateIn();
		}

		[DTCommandPalette.MethodCommand]
		public static void AnimateOut() {
			if (Selection.activeGameObject == null) {
				Debug.LogWarning("Can't AnimateOut when no selected GameObject!");
				return;
			}

			Transition transition = new Transition(Selection.activeGameObject);
			transition.AnimateOut();
		}
	}
}