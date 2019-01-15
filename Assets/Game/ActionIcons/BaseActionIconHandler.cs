using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;

namespace DT.Game {
	public abstract class BaseActionIconHandler : IActionIconHandler {
		// PRAGMA MARK - IActionIconHandler Implementation
		ActionType IActionIconHandler.ActionType {
			get { return actionType_; }
		}

		public abstract void Populate(GameObject container);


		// PRAGMA MARK - Public Interface
		public BaseActionIconHandler(ActionType actionType) {
			actionType_ = actionType;
		}


		// PRAGMA MARK - Internal
		private ActionType actionType_;
	}
}