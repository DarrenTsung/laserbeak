using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;

namespace DT.Game.Audio {
	public class AudioManager : Singleton<AudioManager> {
		public void PlaySFX(AudioClip clip, float volumeScale = 1.0f) {
			audioSource_.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
			audioSource_.PlayOneShot(clip, volumeScale);
		}


		// PRAGMA MARK - Internal
		private AudioSource audioSource_;

		private void Awake() {
			audioSource_ = this.GetRequiredComponent<AudioSource>();
		}
	}
}