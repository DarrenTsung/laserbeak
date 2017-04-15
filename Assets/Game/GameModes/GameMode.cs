using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.GameModes {
	public abstract class GameMode : ScriptableObject {
		// PRAGMA MARK - Public Interface
		public void Activate(Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			Activate();
		}

		public abstract void Cleanup();


		// PRAGMA MARK - Internal
		private Action onFinishedCallback_ = null;

		protected abstract void Activate();

		protected void Finish() {
			if (onFinishedCallback_ == null) {
				Debug.LogWarning("Cannot finish GameMode without onFinishedCallback_!");
				return;
			}

			onFinishedCallback_.Invoke();
		}
	}
}