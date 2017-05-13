using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;

namespace DT.Game.ScrollableMenuPopups {
	public class ScrollableMenuItemView : MonoBehaviour, ISelectable {
		// PRAGMA MARK - Public Interface
		public void Init(ScrollableMenuItem item, Action onCallbackInvoked) {
			thumbnailImage_.sprite = item.Thumbnail;
			nameOutlet_.Text = item.Name;

			callback_ = () => {
				item.Callback.Invoke();
				onCallbackInvoked.Invoke();
			};
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			if (callback_ != null) {
				callback_.Invoke();
				callback_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet nameOutlet_;

		[SerializeField]
		private Image thumbnailImage_;

		private Action callback_;
	}
}