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
					view.OnStateChanged += RefreshReadyToFight;
					view.Init(player);
					views_.Add(view);
				}
			}

			RefreshReadyToFight();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			foreach (var view in views_) {
				view.OnStateChanged -= RefreshReadyToFight;
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

		private Action goBackCallback_;
		private Action continueCallback_;

		private PopupView popupView_;

		private readonly List<IndividualPlayerCustomizationView> views_ = new List<IndividualPlayerCustomizationView>();

		private bool AtLeastOnePlayerReady {
			get { return views_.Any(v => v.IsReady); }
		}

		private bool AllPlayersDoneCustomizing {
			get { return views_.All(v => !v.IsCustomizing); }
		}

		private bool ReadyToFight {
			get { return AtLeastOnePlayerReady && AllPlayersDoneCustomizing; }
		}

		private void Init(Action goBackCallback, Action continueCallback) {
			if (goBackCallback == null || continueCallback == null) {
				Debug.LogError("callback is null!");
			}

			goBackCallback_ = goBackCallback;
			continueCallback_ = continueCallback;
		}

		private void Update() {
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
			foreach (var view in views_) {
				view.SetPaused(true);
			}
			popupView_ = PopupConfirmationView.Create("GO BACK TO THE MAIN MENU?", player, confirmCallback: () => {
				if (goBackCallback_ != null) {
					goBackCallback_.Invoke();
					goBackCallback_ = null;
				}
				popupView_ = null;
			}, cancelCallback: () => {
				foreach (var view in views_) {
					view.SetPaused(false);
				}
				popupView_ = null;
			});
		}

		private void RefreshReadyToFight() {
			readyToFightContainer_.SetActive(ReadyToFight);
		}

		private void LeavePlayerCustomization() {
			// unregister all players who aren't ready
			foreach (IndividualPlayerCustomizationView view in views_.Where(v => !v.IsReady)) {
				RegisteredPlayers.Remove(view.Player);
			}

			if (continueCallback_ != null) {
				continueCallback_.Invoke();
				continueCallback_ = null;
			}
		}
	}
}