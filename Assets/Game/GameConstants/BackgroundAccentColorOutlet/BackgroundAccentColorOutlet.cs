using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public class BackgroundAccentColorOutlet : ColorOutlet {
		// PRAGMA MARK - Internal
		protected override Color GetColor() {
			return GameConstants.Instance.BackgroundAccentColor;
		}

		protected override void AttachListener(UnityAction listener) {
			GameConstants.Instance.OnBackgroundAccentColorChanged.AddListener(listener);
		}

		protected override void DettachListener(UnityAction listener) {
			GameConstants.Instance.OnBackgroundAccentColorChanged.RemoveListener(listener);
		}
	}
}