using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game {
	// Pass-through class to conform to the interface
	public class HumanPlayerFilterEventRouter : IEventRouter {
		// PRAGMA MARK - IEventRouter Implementation
		void IEventRouter.Subscribe(UnityAction unityAction) {
			eventRouter_.Subscribe(FilterNonHumanPlayers);
			event_.AddListener(unityAction);
		}

		void IEventRouter.Unsubscribe(UnityAction unityAction) {
			eventRouter_.Unsubscribe(FilterNonHumanPlayers);
			event_.RemoveListener(unityAction);
		}


		// PRAGMA MARK - Public Interface
		public HumanPlayerFilterEventRouter(IEventRouter<BattlePlayer> eventRouter) {
			eventRouter_ = eventRouter;
		}


		// PRAGMA MARK - Internal
		private IEventRouter<BattlePlayer> eventRouter_;
		private UnityEvent event_ = new UnityEvent();

		private void FilterNonHumanPlayers(BattlePlayer battlePlayer) {
			var player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.IsAI) {
				return;
			}

			event_.Invoke();
		}
	}
}
