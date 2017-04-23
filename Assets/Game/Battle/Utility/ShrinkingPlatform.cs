using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class ShrinkingPlatform : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			this.transform.localScale = scale_;
			GameModeIntroView.OnIntroFinished += HandleIntroFinished;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.transform.localScale = scale_;
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private float duration_ = 30.0f;

		[SerializeField, Range(0.0f, 1.0f)]
		private float finalScale_ = 0.2f;

		private Vector3 scale_;

		private void Awake() {
			scale_ = this.transform.localScale;
		}

		private void HandleIntroFinished() {
			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;
			Vector3 finalScale = (scale_ * finalScale_).SetY(scale_.y);
			this.DoLerpFor(duration_, (float p) => {
				this.transform.localScale = Vector3.Lerp(scale_, finalScale, p);
			});
		}
	}
}