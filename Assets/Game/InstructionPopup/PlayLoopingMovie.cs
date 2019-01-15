using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

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
			StartCoroutine(PlayVideoCoroutine());
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			videoPlayer_.Stop();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private VideoClip videoClip_;

		private VideoPlayer videoPlayer_;
		private RawImage rawImage_;

		private void Awake() {
			rawImage_ = this.GetRequiredComponent<RawImage>();

			videoPlayer_ = this.gameObject.AddComponent<VideoPlayer>();
			videoPlayer_.isLooping = true;
			videoPlayer_.clip = videoClip_;
			videoPlayer_.playOnAwake = false;
			videoPlayer_.audioOutputMode = VideoAudioOutputMode.None;
		}

		private IEnumerator PlayVideoCoroutine() {
			videoPlayer_.Prepare();
			while (!videoPlayer_.isPrepared) {
				yield return null;
			}

			rawImage_.texture = videoPlayer_.texture;
			videoPlayer_.Play();
		}
	}
}