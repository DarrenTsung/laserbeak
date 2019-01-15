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
	public class LightenedBackgroundAccentColorOutlet : ColorOutlet {
		// PRAGMA MARK - Internal
		protected override Color GetColor() {
			Color color = GameConstants.Instance.BackgroundAccentColor;

			float H, S, V;
			Color.RGBToHSV(color, out H, out S, out V);

			S = Mathf.Clamp01(S - 0.19f);
			V = Mathf.Clamp01(V + 0.21f);

			color = Color.HSVToRGB(H, S, V);
			return color;
		}

		protected override void AttachListener(UnityAction listener) {
			GameConstants.Instance.OnBackgroundAccentColorChanged.AddListener(listener);
		}

		protected override void DettachListener(UnityAction listener) {
			GameConstants.Instance.OnBackgroundAccentColorChanged.RemoveListener(listener);
		}
	}
}