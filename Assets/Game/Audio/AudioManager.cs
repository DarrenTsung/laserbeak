using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;

namespace DT.Game.Audio {
	public class AudioManager : Singleton<AudioManager> {
		public enum BGMState {
			Normal,
			Muted
		}

		public void PlaySFX(AudioClip clip, float volumeScale = 1.0f, float randomPitchRange = 0.05f) {
			sfxAudioSource_.pitch = UnityEngine.Random.Range(1.0f - randomPitchRange, 1.0f + randomPitchRange);
			sfxAudioSource_.PlayOneShot(clip, volumeScale);
		}

		public void PlayBGM(AudioClip clip) {
			backgroundMusicAudioSource_.loop = true;
			backgroundMusicAudioSource_.clip = clip;
			backgroundMusicAudioSource_.Play();
		}

		public void SetBGMState(BGMState state) {
			if (state_ == state) {
				return;
			}

			AnimateToState(state);
			state_ = state;
		}


		// PRAGMA MARK - Internal
		private const float kBGMStateTransitionDuration = 0.3f;

		private const float kBGMMutedVolume = 0.7f;

		[Header("Outlets")]
		[SerializeField]
		private AudioSource sfxAudioSource_;
		[SerializeField]
		private AudioSource backgroundMusicAudioSource_;

		private CoroutineWrapper coroutine_;
		private BGMState state_ = (BGMState)(-1);

		private void AnimateToState(BGMState state) {
			if (coroutine_ != null) {
				coroutine_.Cancel();
				coroutine_ = null;
			}

			float oldVolume = backgroundMusicAudioSource_.volume;
			float newVolume = 0.0f;
			switch (state) {
				case BGMState.Normal:
					newVolume = 1.0f;
					break;
				case BGMState.Muted:
					newVolume = kBGMMutedVolume;
					break;
			}

			coroutine_ = CoroutineWrapper.DoEaseFor(kBGMStateTransitionDuration, EaseType.SineEaseInOut, (float p) => {
				backgroundMusicAudioSource_.volume = Mathf.Lerp(oldVolume, newVolume, p);
			});
		}
	}
}