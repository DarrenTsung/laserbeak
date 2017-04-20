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
		[Header("Audio")]
		public AudioClip UIBeep;
		public AudioClip GameModeIntro;
	}

	public static class GameAudioClipExtensions {
		public static void PlaySFX(this AudioClip clip, float volumeScale = 1.0f) {
			AudioManager.Instance.PlaySFX(clip, volumeScale);
		}
	}
}