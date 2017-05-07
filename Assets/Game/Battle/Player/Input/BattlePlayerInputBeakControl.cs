using System;
using System.Collections;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayerInputBeakControl : BattlePlayerInputComponent {
		// PRAGMA MARK - Internal
		private const int kMaxSoundOffset = 5;
		private int soundOffset_ = 0;
		private bool canPlayAlarm_ = true;
		private bool hasPlayedAlarm_ = false;

		private bool keepBeakOpen_ = false;

		protected override void Cleanup() {
			soundOffset_ = 0;
			canPlayAlarm_ = false;
			hasPlayedAlarm_ = false;
			keepBeakOpen_ = false;
		}

		private void Update() {
			if (keepBeakOpen_) {
				Player_.Animator.SetBool("BeakOpen", true);
				return;
			}

			bool previouslyOpen = Player_.Animator.GetBool("BeakOpen");
			if (!Enabled) {
				Player_.Animator.SetBool("BeakOpen", false);
				return;
			}

			Player_.Animator.SetBool("BeakOpen", InputDelegate_.LaserPressed);
			if (InGameConstants.EnableQuacking && InputDelegate_.LaserPressed && previouslyOpen == false) {
				if (canPlayAlarm_ && !hasPlayedAlarm_ && UnityEngine.Random.Range(0.0f, 1.0f) > 0.4f) {
					AudioConstants.Instance.CluckAlarm.PlaySFX(volumeScale: 1.9f);
					KeepBeakOpenFor(AudioConstants.Instance.CluckAlarm.length - 0.1f);
					hasPlayedAlarm_ = true;
				} else {
					AudioConstants.Instance.Cluck.PlaySFX(volumeScale: 2.4f, randomPitchRange: 0.1f, pitchOffset: 0.03f - (soundOffset_ * 0.03f));
					KeepBeakOpenFor(AudioConstants.Instance.Cluck.length - 0.1f);
				}
				soundOffset_ = Mathf.Min(soundOffset_ + 1, kMaxSoundOffset);

				// allow playing alarm after non-alarm at least once
				canPlayAlarm_ = true;
			}
		}

		private void KeepBeakOpenFor(float duration) {
			keepBeakOpen_ = true;
			CoroutineWrapper.DoAfterDelay(duration, () => {
				keepBeakOpen_ = false;
			});
		}
	}
}