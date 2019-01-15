using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTLocalization;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;
using DT.Game.ElementSelection;
using DT.Game.GameModes;
using DT.Game.Popups;
using DT.Game.Transitions;

namespace DT.Game.MainMenu {
	public class MainMenu : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Action battleHandler, Action levelEditorHandler, Action coopHandler) {
			battleHandler_ = battleHandler;
			levelEditorHandler_ = levelEditorHandler;
			coopHandler_ = coopHandler;

			levelEditorContainer_.SetActive(Application.isEditor);
			#if UNITY_EDITOR || DEBUG
			coopContainer_.SetActive(true);
			#else
			coopContainer_.SetActive(false);
			#endif

			transition_.AnimateIn(() => {
				if (InGameConstants.IsAlphaBuild && !InGameConstants.ShowedAlphaDisclaimer) {
					InGameConstants.ShowedAlphaDisclaimer = true;
					popupView_ = PopupView.Create("<b>DISCLAIMER (please read):</b><size=25>\n\nThis is an PRE-ALPHA build of PHASERBEAK. It is not representive of how the final game and has undergone only basic testing and optimization. Please DO NOT distribute this build.\n\nIf you have any feedback or suggestions, join our Discord (through www.phaserbeak.com). Thanks for playing!\n\n-Darren", InputUtil.AllInputs, new PopupButtonConfig[] {
						new PopupButtonConfig("ACCEPT", CreateSelectionView, defaultOption: true)
					});
				} else {
					CreateSelectionView();
				}
			});
		}

		public void AnimateOut(Action callback) {
			CleanupSelectionView();
			transition_.AnimateOut(callback);
			ActionLabelBar.Hide();
		}

		public void HandleBattlePressed() {
			if (battleHandler_ == null) {
				return;
			}

			InGameConstants.FillWithAI = true;
			AudioConstants.Instance.UIBeep.PlaySFX();
			battleHandler_.Invoke();
			// avoid invoke handler twice
			battleHandler_ = null;
		}

		public void HandleCoopPressed() {
			if (coopHandler_ == null) {
				return;
			}

			InGameConstants.FillWithAI = false;
			AudioConstants.Instance.UIBeep.PlaySFX();
			coopHandler_.Invoke();
			coopHandler_ = null;
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
			ActionLabelBar.Show(new ActionLabelViewConfig[] {
				new ActionLabelViewConfig("GENERIC_ACTION_SELECT", ActionType.Positive),
			});
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			CleanupSelectionView();

			if (popupView_ != null) {
				ObjectPoolManager.Recycle(popupView_);
				popupView_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject menuContainer_;
		[SerializeField]
		private GameObject levelEditorContainer_;
		[SerializeField]
		private GameObject coopContainer_;

		private ElementSelectionView selectionView_;
		private Action levelEditorHandler_;
		private Action battleHandler_;
		private Action coopHandler_;
		private PopupView popupView_;

		private Transition transition_;

		private void Awake() {
			transition_ = new Transition(this.gameObject);
		}

		private void CleanupSelectionView() {
			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}

		private void CreateSelectionView() {
			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(InputUtil.AllInputs, menuContainer_);
		}
	}
}