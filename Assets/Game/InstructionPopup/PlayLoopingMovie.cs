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

using DT.Game.Players;

namespace DT.Game.InstructionPopups {
	[RequireComponent(typeof(RawImage))]
	public class PlayLoopingMovie : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			rawImage_.texture = movieTexture_;

			movieTexture_.loop = true;
			movieTexture_.Play();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			movieTexture_.Stop();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private MovieTexture movieTexture_;

		private RawImage rawImage_;

		private void Awake() {
			rawImage_ = this.GetRequiredComponent<RawImage>();
		}
	}
}