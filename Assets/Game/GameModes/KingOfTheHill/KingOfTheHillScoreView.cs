using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.GameModes.KingOfTheHill {
	public class KingOfTheHillScoreView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(IKotHScoreSource scoreSource) {
			foreach (var player in RegisteredPlayers.AllPlayers) {
				var view = ObjectPoolManager.Create<IndividualKotHScoreView>(GamePrefabs.Instance.IndividualKotHScorePrefab, parent: layoutContainer_);
				view.Init(player, scoreSource);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			layoutContainer_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject layoutContainer_;
	}
}