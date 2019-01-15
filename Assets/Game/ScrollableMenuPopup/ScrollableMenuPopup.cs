using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;

namespace DT.Game.ScrollableMenuPopups {
	public static class ScrollableMenuPopup {
		// PRAGMA MARK - Static
		public static event Action OnShown = delegate {};
		public static event Action OnHidden = delegate {};

		public static void Show(InputDevice inputDevice, IEnumerable<ScrollableMenuItem> items) {
			if (popup_ != null) {
				CleanupPopup();
			}

			popup_ = ObjectPoolManager.CreateView<ScrollableMenu>(GamePrefabs.Instance.ScrollableMenuPopupPrefab);
			popup_.Init(new IInputWrapper[] { new InputWrapperDevice(inputDevice) }, items);
			popup_.OnRecycled += HandlePopupRecycled;
			OnShown.Invoke();
		}

		public static void Hide() {
			CleanupPopup();
		}


		// PRAGMA MARK - Internal
		private static ScrollableMenu popup_;

		private static void HandlePopupRecycled() {
			if (popup_ == null) {
				return;
			}

			popup_.OnRecycled -= HandlePopupRecycled;
			popup_ = null;
			OnHidden.Invoke();
		}

		private static void CleanupPopup() {
			popup_.Hide();
		}
	}
}