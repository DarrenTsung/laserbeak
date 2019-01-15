using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;

namespace DT.Game {
	public class ActionIconOutlet : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(ActionType actionType) {
			actionType_ = actionType;
			RefreshActionIcon();
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			ActionIcons.OnInputTypeChanged += RefreshActionIcon;

			if (useSerializedActionType_) {
				actionType_ = serializedActionType_;
				RefreshActionIcon();
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			ActionIcons.OnInputTypeChanged -= RefreshActionIcon;

			this.gameObject.RecycleAllChildren();
		}

		// PRAGMA MARK - Internal
		[Header("Properties (Optional)")]
		[SerializeField]
		private bool useSerializedActionType_ = false;
		[SerializeField]
		private ActionType serializedActionType_;

		private ActionType actionType_;

		private void RefreshActionIcon() {
			this.gameObject.RecycleAllChildren();
			ActionIcons.Populate(actionType_, container: this.gameObject);
		}
	}
}