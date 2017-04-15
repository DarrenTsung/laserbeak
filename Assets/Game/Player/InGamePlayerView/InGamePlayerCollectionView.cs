using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public class InGamePlayerCollectionView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void Show() {
			collectionView_ = ObjectPoolManager.CreateView<InGamePlayerCollectionView>();
		}

		public static void Hide() {
			if (collectionView_ != null) {
				ObjectPoolManager.Recycle(collectionView_);
				collectionView_ = null;
			}
		}


		private static InGamePlayerCollectionView collectionView_;




		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var inGamePlayerView = ObjectPoolManager.Create<InGamePlayerView>(inGamePlayerViewPrefab_, parent: layoutCollection_);
				inGamePlayerView.InitWith(player);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			layoutCollection_.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Prefabs")]
		[SerializeField]
		private GameObject inGamePlayerViewPrefab_;

		[Header("Outlets")]
		[SerializeField]
		private GameObject layoutCollection_;
	}
}