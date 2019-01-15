using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Hints {
	public class ActionHintAccumulator {
		// PRAGMA MARK - Public Interface
		public void BeginAccumulating() {
			actionPerformed_ = false;
			eventRouter_.Subscribe(HandleActionPerformed);
		}

		public void EndAccumulating() {
			ActionHintTrackerRegistry.Get(hintKey_).HandleActionPeriod(actionPerformed_);

			actionPerformed_ = false;
			eventRouter_.Unsubscribe(HandleActionPerformed);
		}

		public ActionHintAccumulator(HintKey hintKey, IEventRouter eventRouter) {
			hintKey_ = hintKey;

			eventRouter_ = eventRouter;
		}


		// PRAGMA MARK - Internal
		private IEventRouter eventRouter_;

		private HintKey hintKey_;
		private bool actionPerformed_ = false;

		private void HandleActionPerformed() {
			actionPerformed_ = true;
		}
	}
}