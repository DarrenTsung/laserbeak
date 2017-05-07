using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class RecycleWhenOffBattleScreen : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			enabled_ = true;
		}


		// PRAGMA MARK - Internal
		private const float kMargin = 0.1f;

		private bool enabled_ = false;

		private void Update() {
			if (!enabled_) {
				return;
			}

			if (BattleCamera.Instance == null) {
				return;
			}

			Vector2 viewportPoint = BattleCamera.Instance.Camera.WorldToViewportPoint(this.transform.position);
			if (viewportPoint.x < -kMargin || viewportPoint.x > 1.0f + kMargin || viewportPoint.y < -kMargin || viewportPoint.y > 1.0f + kMargin) {
				ObjectPoolManager.Recycle(this);
				enabled_ = false;
			}
		}
	}
}