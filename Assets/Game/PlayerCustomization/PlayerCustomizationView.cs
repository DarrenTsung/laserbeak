using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Popups;
using DT.Game.Transitions;

namespace DT.Game.PlayerCustomization {
	public class PlayerCustomizationView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		public static void Show(Action goBackCallback, Action continueCallback) {
			view_ = ObjectPoolManager.CreateView<PlayerCustomizationView>(GamePrefabs.Instance.PlayerCustomizationViewPrefab);
			view_.Init(goBackCallback, continueCallback);
		}

		public static void Hide() {
			if (view_ != null) {
				ObjectPoolManager.Recycle(view_);
				view_ = null;
			}
		}

		private static PlayerCustomizationView view_;


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			List<Transform> playerAnchors = playerAnchorsContainer_.transform.Cast<Transform>().ToList();

			views_.Clear();
			for (int i = 0; i < playerAnchors.Count; i++) {
				Player player = RegisteredPlayers.AllPlayers.GetValueOrDefault(i);
				if (player == null) {
					playerAnchors[i].gameObject.SetActive(false);
				} else {
					playerAnchors[i].gameObject.SetActive(true);
					var view = ObjectPoolManager.Create<IndividualPlayerCustomizationView>(GamePrefabs.Instance.IndividualPlayerCustomizationViewPrefab, parent: playerAnchors[i].gameObject);
					view.Init(player);
					view.OnStateChanged += RefreshReadyToFight;
					view.OnStateChanged += RefreshControlsTab;
					views_.Add(view);
				}
			}

			readyToFightContainer_.SetActive(false);
			readyToFightShowing_ = false;
			controlsContainer_.SetActive(false);
			controlsTabShowing_ = false;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			foreach (var view in views_) {
				view.OnStateChanged -= RefreshReadyToFight;
				view.OnStateChanged -= RefreshControlsTab;
				ObjectPoolManager.Recycle(view);
			}
			views_.Clear();

			if (popupView_ != null) {
				ObjectPoolManager.Recycle(popupView_);
				popupView_ = null;
			}

			paused_ = false;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerAnchorsContainer_;

		[SerializeField]
		private GameObject readyToFightContainer_;

		[SerializeField]
		private GameObject controlsContainer_;

		[SerializeField]
		private GameObject viewTransitionsContainer_;

		private Action goBackCallback_;
		private Action continueCallback_;

		private PopupView popupView_;
		private bool paused_ = false;

		private readonly List<IndividualPlayerCustomizationView> views_ = new List<IndividualPlayerCustomizationView>();

		private TransitionWrapper viewTransitionWrapper_;
		private TransitionWrapper readyToFightTransitionWrapper_;
		private TransitionWrapper controlsTransitionWrapper_;
		private bool readyToFightShowing_ = false;
		private bool controlsTabShowing_ = false;

		private bool AtLeastOnePlayerReady {
			get { return views_.Any(v => v.IsReady); }
		}

		private bool AllPlayersDoneCustomizing {
			get { return views_.All(v => !v.IsCustomizing); }
		}

		private bool Paused_ {
			get { return paused_; }
			set {
				if (paused_ == value) {
					return;
				}

				paused_ = value;
				foreach (var view in views_) {
					view.SetPaused(paused_);
				}
			}
		}

		private bool ReadyToFight {
			get { return AtLeastOnePlayerReady && AllPlayersDoneCustomizing; }
		}

		private void Awake() {
			// NOTE (darren): dynamic because the child views (which have transitions)
			// are created dynamically and object pooled
			viewTransitionWrapper_ = new TransitionWrapper(viewTransitionsContainer_).SetDynamic(true);
			readyToFightTransitionWrapper_ = new TransitionWrapper(readyToFightContainer_);
			controlsTransitionWrapper_ = new TransitionWrapper(controlsContainer_);
		}

		private void Init(Action goBackCallback, Action continueCallback) {
			if (goBackCallback == null || continueCallback == null) {
				Debug.LogError("callback is null!");
			}

			goBackCallback_ = goBackCallback;
			continueCallback_ = continueCallback;

			viewTransitionWrapper_.AnimateIn();

			RefreshReadyToFight();
			RefreshControlsTab();
		}

		private void Update() {
			if (Paused_) {
				return;
			}

			UpdateCheckContinue();
			UpdateCheckGoBack();
		}

		private void UpdateCheckContinue() {
			if (!ReadyToFight) {
				return;
			}

			if (continueCallback_ == null) {
				return;
			}

			foreach (InputDevice inputDevice in InputManager.Devices) {
				if (InputUtil.WasCommandPressedFor(inputDevice)) {
					LeavePlayerCustomization();
				}
			}
		}

		private void UpdateCheckGoBack() {
			foreach (var view in views_) {
				if (view.IsJoined) {
					continue;
				}

				Player player = view.Player;
				if (InputUtil.WasNegativePressedFor(player.InputDevice)) {
					OpenGoBackView(player);
				}
			}
		}

		private void OpenGoBackView(Player player) {
			Paused_ = true;
			popupView_ = PopupConfirmationView.Create("GO BACK TO THE MAIN MENU?", player, confirmCallback: () => {
				if (goBackCallback_ != null) {
					goBackCallback_.Invoke();
					goBackCallback_ = null;
				}
				popupView_ = null;
			}, cancelCallback: () => {
				Paused_ = false;
				popupView_ = null;
			});
		}

		private void RefreshControlsTab() {
			bool show = AtLeastOnePlayerReady;
			if (show == controlsTabShowing_) {
				return;
			}

			if (!controlsContainer_.activeSelf) {
				controlsContainer_.SetActive(true);
			}

			controlsTabShowing_ = show;
			if (controlsTabShowing_) {
				controlsTransitionWrapper_.AnimateIn();
			} else {
				controlsTransitionWrapper_.AnimateOut();
			}
		}

		private void RefreshReadyToFight() {
			if (ReadyToFight == readyToFightShowing_) {
				return;
			}

			if (!readyToFightContainer_.activeSelf) {
				readyToFightContainer_.SetActive(true);
			}

			readyToFightShowing_ = ReadyToFight;
			if (ReadyToFight) {
				readyToFightTransitionWrapper_.AnimateIn();
			} else {
				AnimateOutReadyToFight();
			}
		}

		private void AnimateOutReadyToFight() {
			readyToFightTransitionWrapper_.AnimateOut();
		}

		private void LeavePlayerCustomization() {
			// unregister all players who aren't ready
			foreach (IndividualPlayerCustomizationView view in views_.Where(v => !v.IsReady)) {
				RegisteredPlayers.Remove(view.Player);
			}

			// prevent players from modifying state while animating out
			Paused_ = true;

			viewTransitionWrapper_.AnimateOut(() => {
				if (continueCallback_ != null) {
					continueCallback_.Invoke();
					continueCallback_ = null;
				}
			});
			controlsTransitionWrapper_.AnimateOut();
			AnimateOutReadyToFight();
		}
	}
}