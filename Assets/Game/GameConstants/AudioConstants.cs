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

		[Header("SFX")]
		public AudioClip UIBeep;
		public AudioClip GameModeIntro;
		public AudioClip LaserShoot;
		public AudioClip Dash;

		[Header("Player")]
		public AudioClip PlayerHurt;
		public AudioClip PlayerDeath;
	}

	public static class GameAudioClipExtensions {
		public static void PlaySFX(this AudioClip clip, float volumeScale = 1.0f, float randomPitchRange = 0.05f) {
			AudioManager.Instance.PlaySFX(clip, volumeScale, randomPitchRange);
		}
	}
}