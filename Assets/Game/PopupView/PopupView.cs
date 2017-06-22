using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;
using DT.Game.Players;

namespace DT.Game.Popups {
	public class PopupView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		public static PopupView Create(string message, Player player, IList<PopupButtonConfig> buttonConfigs) {
			var view = ObjectPoolManager.CreateView<PopupView>(GamePrefabs.Instance.PopupViewPrefab);
			view.Init(message, player, buttonConfigs);
			return view;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			buttonContainer_.RecycleAllChildren();

			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet messageOutlet_;
		[SerializeField]
		private GameObject buttonContainer_;

		private ElementSelectionView selectionView_;

		private void Init(string message, Player player, IList<PopupButtonConfig> buttonConfigs) {
			messageOutlet_.Text = message;

			ISelectable startSelectable = null;
			foreach (var config in buttonConfigs) {
				var buttonView = ObjectPoolManager.Create<PopupButtonView>(GamePrefabs.Instance.PopupButtonViewPrefab, parent: buttonContainer_);
				buttonView.Init(config, HandleButtonSelected);

				if (config.DefaultOption) {
					startSelectable = buttonView;
				}
			}

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(player, buttonContainer_, startSelectable);
		}

		private void HandleButtonSelected() {
			ObjectPoolManager.Recycle(this);
		}
	}
}