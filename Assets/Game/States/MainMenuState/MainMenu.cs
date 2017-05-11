using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;
using DT.Game.ElementSelection;
using DT.Game.GameModes;

namespace DT.Game.MainMenu {
	public class MainMenu : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Action battleHandler, Action levelEditorHandler) {
			battleHandler_ = battleHandler;
			levelEditorHandler_ = levelEditorHandler;
		}

		public void HandleBattlePressed() {
			if (battleHandler_ == null) {
				return;
			}

			AudioConstants.Instance.UIBeep.PlaySFX();
			battleHandler_.Invoke();
			// avoid invoke handler twice
			battleHandler_ = null;
		}

		public void HandleLevelEditorPressed() {
			if (levelEditorHandler_ == null) {
				return;
			}

			AudioConstants.Instance.UIBeep.PlaySFX();
			levelEditorHandler_.Invoke();
			// avoid invoke handler twice
			levelEditorHandler_ = null;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			RefreshDemoMode();

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(InputManager.Devices, menuContainer_);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject demoModeContainer_;

		[SerializeField]
		private GameObject menuContainer_;

		private ElementSelectionView selectionView_;
		private Action levelEditorHandler_;
		private Action battleHandler_;

		private void Update() {
			if (InputUtil.WasAnyCommandButtonPressed()) {
				// TODO (darren): better place for this?
				GameConstants.Instance.DemoMode = !GameConstants.Instance.DemoMode;
				RefreshDemoMode();
				GameModeShowedInstructionsCache.ResetShowedInstructionsCache();
			}
		}

		private void RefreshDemoMode() {
			demoModeContainer_.SetActive(GameConstants.Instance.DemoMode);
		}
	}
}