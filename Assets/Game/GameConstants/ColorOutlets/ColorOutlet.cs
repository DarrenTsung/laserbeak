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
	public abstract class ColorOutlet : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField, DTValidator.Optional]
		private TextOutlet textOutlet_;
		[SerializeField, DTValidator.Optional]
		private Image image_;

		[Header("Properties")]
		[SerializeField, Range(0.0f, 1.0f)]
		private float alpha_ = 1.0f;

		protected abstract Color GetColor();
		protected abstract void AttachListener(UnityAction listener);
		protected abstract void DettachListener(UnityAction listener);

		private void OnEnable() {
			RefreshColor();
			AttachListener(RefreshColor);
		}

		private void OnDisable() {
			DettachListener(RefreshColor);
		}

		private void RefreshColor() {
			Color color = GetColor().WithAlpha(alpha_);
			if (image_ != null) {
				image_.color = color;
			}

			if (textOutlet_ != null) {
				textOutlet_.Color = color;
			}
		}
	}
}