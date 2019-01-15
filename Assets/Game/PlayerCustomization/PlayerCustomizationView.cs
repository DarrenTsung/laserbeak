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
		public static void Show(Action continueCallback) {
			view_ = ObjectPoolManager.CreateView<PlayerCustomizationView>(GamePrefabs.Instance.PlayerCustomizationViewPrefab);
			view_.Init(continueCallback);
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
		[SerializeField]
		private DelayedActionView readyToFightActionView_;

		private Action continueCallback_;

		private PopupView popupView_;

		private readonly List<IndividualPlayerCustomizationView> views_ = new List<IndividualPlayerCustomizationView>();

		private Transition viewTransition_;
		private Transition readyToFightTransition_;
		private Transition controlsTransition_;
		private bool readyToFightShowing_ = false;
		private bool controlsTabShowing_ = false;

		private bool AtLeastOnePlayerReady {
			get { return views_.Any(v => v.IsReady); }
		}

		private bool AllPlayersDoneCustomizing {
			get { return views_.All(v => !v.IsCustomizing); }
		}

		private bool ReadyToFight {
			get { return AtLeastOnePlayerReady && AllPlayersDoneCustomizing; }
		}

		private void Awake() {
			// NOTE (darren): dynamic because the child views (which have transitions)
			// are created dynamically and object pooled
			viewTransition_ = new Transition(viewTransitionsContainer_).SetDynamic(true);
			readyToFightTransition_ = new Transition(readyToFightContainer_);
			controlsTransition_ = new Transition(controlsContainer_);
		}

		private void Init(Action continueCallback) {
			if (continueCallback == null) {
				Debug.LogError("callback is null!");
			}

			continueCallback_ = continueCallback;

			viewTransition_.AnimateIn();

			RefreshReadyToFight();
			RefreshControlsTab();

			readyToFightActionView_.Init("READY TO FIGHT - HOLD ", ActionType.Command, finishedCallback: LeavePlayerCustomization, animate: false);
			readyToFightActionView_.SetActivePredicate(() => ReadyToFight);
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
				controlsTransition_.AnimateIn();
			} else {
				controlsTransition_.AnimateOut();
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
				readyToFightTransition_.AnimateIn();
			} else {
				AnimateOutReadyToFight();
			}
		}

		private void AnimateOutReadyToFight() {
			readyToFightTransition_.AnimateOut();
		}

		private void LeavePlayerCustomization() {
			// unregister all players who aren't ready
			foreach (IndividualPlayerCustomizationView view in views_.Where(v => !v.IsReady)) {
				RegisteredPlayers.Remove(view.Player);
			}

			viewTransition_.AnimateOut(() => {
				if (continueCallback_ != null) {
					continueCallback_.Invoke();
					continueCallback_ = null;
				}
			});
			controlsTransition_.AnimateOut();
			AnimateOutReadyToFight();
		}
	}
}