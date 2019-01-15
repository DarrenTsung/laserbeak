using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;

namespace DT.Game {
	public enum IconBorderType {
		Square,
		Circle
	}

	public class ActionIconLetterView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void Init(string text, IconBorderType borderType, Color textColor) {
			textOutlet_.Text = text;

			squareBorder_.SetActive(borderType == IconBorderType.Square);
			circleBorder_.SetActive(borderType == IconBorderType.Circle);

			textOutlet_.Color = textColor;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet textOutlet_;

		[Space]
		[SerializeField]
		private GameObject squareBorder_;
		[SerializeField]
		private GameObject circleBorder_;
	}
}