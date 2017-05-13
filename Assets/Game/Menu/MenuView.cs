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

namespace DT.Game {
	public class MenuView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		private static MenuView menuView_;

		public static void Show(InputDevice inputDevice, string title, Dictionary<string, Action> menuItemMap) {
			menuView_ = ObjectPoolManager.CreateView<MenuView>(GamePrefabs.Instance.MenuViewPrefab);
			menuView_.Init(inputDevice, title, menuItemMap);
		}

		public static void Hide() {
			if (menuView_ != null) {
				ObjectPoolManager.Recycle(menuView_);
				menuView_ = null;
			}
		}



		// PRAGMA MARK - Public Interface
		public void Init(InputDevice inputDevice, string title, Dictionary<string, Action> menuItemMap) {
			titleText_.Text = title;

			foreach (var kvp in menuItemMap) {
				MenuItem menuItem = ObjectPoolManager.Create<MenuItem>(GamePrefabs.Instance.MenuItemPrefab, parent: layoutContainer_);
				menuItem.Init(kvp.Key, kvp.Value);
			}

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(new InputDevice[] { inputDevice }, layoutContainer_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}

			layoutContainer_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet titleText_;

		[SerializeField]
		private GameObject layoutContainer_;

		private ElementSelectionView selectionView_;
	}
}