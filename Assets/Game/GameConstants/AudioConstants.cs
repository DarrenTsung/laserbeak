using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;

namespace DT.Game {
	public class AudioConstants : Singleton<AudioConstants> {
		// PRAGMA MARK - Public Interface
		[Header("Music")]
		public AudioClip BackgroundMusic;

		[Header("UI SFX")]
		public AudioClip UIBeep;
		public AudioClip GameModeIntro;
		public AudioClip ScoreAdded;
		public AudioClip Win;
		public AudioClip Negative;
		public AudioClip ScrollClick;

		[Header("Game SFX")]
		public AudioClip LaserShoot;
		public AudioClip Dash;
		public AudioClip ExplosiveTimerBeep;
		public AudioClip Cluck;
		public AudioClip CluckAlarm;

		[Header("Player")]
		public AudioClip PlayerHurt;
		public AudioClip PlayerDeath;
	}

	public static class GameAudioClipExtensions {
		public static void PlaySFX(this AudioClip clip, float volumeScale = 1.0f, float randomPitchRange = 0.05f, float pitchOffset = 0.0f) {
			if (clip == null) {
				Debug.LogError("Failed to play SFX because null!");
				return;
			}

			AudioManager.Instance.PlaySFX(clip, volumeScale, randomPitchRange, pitchOffset);
		}
	}
}