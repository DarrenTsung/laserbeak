using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;

namespace DT.Game.Popups {
	public class PopupButtonView : MonoBehaviour, ISelectable {
		// PRAGMA MARK - Public Interface
		public void Init(PopupButtonConfig config, Action onSelectedCallback) {
			textOutlet_.Text = config.ButtonText;
			callback_ = config.Callback;
			callback_ += onSelectedCallback;

			if (config.DefaultOption) {
				// generic way to do this?
				backgroundImage_.color = kDefaultOptionColor;
			} else {
				backgroundImage_.color = kNonDefaultOptionColor;
			}
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			if (callback_ != null) {
				callback_.Invoke();
				callback_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private static readonly Color kDefaultOptionColor = ColorUtil.HexStringToColor("FFFFFF00");
		private static readonly Color kNonDefaultOptionColor = ColorUtil.HexStringToColor("FFFFFF00");

		[Header("Outlets")]
		[SerializeField]
		private TextOutlet textOutlet_;
		[SerializeField]
		private Image backgroundImage_;

		private Action callback_;
	}
}