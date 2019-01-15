using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;
using DT.Game.Transitions;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.LevelSelect {
	public class LevelSelectView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Action<CoopLevelConfig> levelSelectedCallback) {
			levelSelectedCallback_ = levelSelectedCallback;

			foreach (CoopLevelConfig config in GameConstants.Instance.CoopLevels) {
				var coopLevel = ObjectPoolManager.Create<CoopLevelSelectable>(GamePrefabs.Instance.CoopLevelSelectablePrefab, parent: selectableContainer_);
				coopLevel.Init(config, HandleLevelSelected);
			}

			ActionLabelBar.Show(new ActionLabelViewConfig[] {
				new ActionLabelViewConfig("GENERIC_ACTION_SELECT", ActionType.Positive)
			});

			transition_.AnimateIn(() => {
				selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
				selectionView_.Init(InputUtil.AllInputs, selectableContainer_);
				selectionView_.OnSelectableHover += HandleSelectableHovered;
			});
		}

		public void AnimateOutAndRecycle() {
			CleanupSelectionView();
			transition_.AnimateOut(() => {
				ObjectPoolManager.Recycle(this);
			});
			ActionLabelBar.Hide();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			selectableContainer_.RecycleAllChildren();

			CleanupSelectionView();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject selectableContainer_;

		private ElementSelectionView selectionView_;
		private Action<CoopLevelConfig> levelSelectedCallback_;

		private Transition transition_;

		private void Awake() {
			transition_ = new Transition(this.gameObject);
		}

		private void HandleSelectableHovered(ISelectable selectable) {
			var coopLevelSelectable = selectable as CoopLevelSelectable;
			if (coopLevelSelectable == null) {
				Debug.LogWarning("Not expecting selectable to not be a coopLevelSelectable... " + selectable);
				return;
			}

			ArenaManager.Instance.AnimateLoadArena(coopLevelSelectable.ArenaConfig, callback: null);
		}

		private void HandleLevelSelected(CoopLevelConfig config) {
			if (levelSelectedCallback_ == null) {
				return;
			}

			levelSelectedCallback_.Invoke(config);
			levelSelectedCallback_ = null;
		}

		private void CleanupSelectionView() {
			if (selectionView_ != null) {
				selectionView_.OnSelectableHover -= HandleSelectableHovered;
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}
	}
}