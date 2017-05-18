using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Stats {
	public class PlayerStatsView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void Show(Action finishedCallback) {
			var view = ObjectPoolManager.CreateView<PlayerStatsView>(GamePrefabs.Instance.PlayerStatsViewPrefab);
			view.Init(finishedCallback);
		}


		// PRAGMA MARK - Public Interface
		public void Init(Action finishedCallback) {
			finishedCallback_ = finishedCallback;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var individualPlayerStatsView = ObjectPoolManager.Create<IndividualPlayerStatsView>(GamePrefabs.Instance.IndividualPlayerStatsViewPrefab, parent: playerViewsContainer_);
				individualPlayerStatsView.Init(player);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewsContainer_.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewsContainer_;

		private Action finishedCallback_;

		private void Update() {
			if (InputUtil.WasAnyCommandButtonPressed()) {
				Finish();
			}
		}

		private void Finish() {
			finishedCallback_.Invoke();
			ObjectPoolManager.Recycle(this);
		}
	}
}