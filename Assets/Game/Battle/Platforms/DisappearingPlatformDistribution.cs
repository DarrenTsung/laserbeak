using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class DisappearingPlatformDistribution : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			GameModeIntroView.OnIntroFinished += HandleIntroFinished;
		}


		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private float delayMin_ = 1.0f;

		[SerializeField]
		private float delayMax_ = 20.0f;

		private void HandleIntroFinished() {
			GameModeIntroView.OnIntroFinished -= HandleIntroFinished;
			var platforms = this.GetComponentsInChildren<DisappearingPlatform>();
			foreach (DisappearingPlatform platform in platforms) {
				this.DoAfterDelay(UnityEngine.Random.Range(delayMin_, delayMax_), () => {
					platform.Disappear();
				});
			}
		}
	}
}