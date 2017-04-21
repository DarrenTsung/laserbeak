using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DT.Game.Battle.Players;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.PlayerCustomization {
	public class PlayerCustomizationView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		public static void Show(Action allPlayersReadyCallback) {
			view_ = ObjectPoolManager.CreateView<PlayerCustomizationView>(GamePrefabs.Instance.PlayerCustomizationViewPrefab);
			view_.Init(allPlayersReadyCallback);
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
			// NOTE (darren): because transform position for UI is calculated based on
			// pivot we need to negate the vector2s
			PlayerUtil.Sort(playerAnchors, t => t.position * -1);

			views_.Clear();
			int i = 0;
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var view = ObjectPoolManager.Create<IndividualPlayerCustomizationView>(GamePrefabs.Instance.IndividualPlayerCustomizationViewPrefab, parent: playerAnchors[i].gameObject);
				view.Init(player);
				view.OnReady += HandlePlayerReady;
				views_.Add(view);

				i++;
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			foreach (var view in views_) {
				view.OnReady -= HandlePlayerReady;
				ObjectPoolManager.Recycle(view);
			}
			views_.Clear();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerAnchorsContainer_;

		private Action allPlayersReadyCallback_;

		private readonly List<IndividualPlayerCustomizationView> views_ = new List<IndividualPlayerCustomizationView>();

		private void Init(Action allPlayersReadyCallback) {
			if (allPlayersReadyCallback == null) {
				Debug.LogError("allPlayersReadyCallback is null!");
			}

			allPlayersReadyCallback_ = allPlayersReadyCallback;
		}

		private void HandlePlayerReady() {
			if (views_.All(v => v.Ready)) {
				allPlayersReadyCallback_.Invoke();
			}
		}
	}
}