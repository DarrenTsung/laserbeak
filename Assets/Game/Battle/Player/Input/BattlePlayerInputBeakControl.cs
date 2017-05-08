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
		private const int kMaxSoundOffset = 3;
		private int soundOffset_ = 0;

		private int count_ = 0;

		private bool keepBeakOpen_ = false;

		protected override void Cleanup() {
			soundOffset_ = 0;
			keepBeakOpen_ = false;

			count_ = 0;
		}

		private void Update() {
			if (keepBeakOpen_) {
				Player_.Animator.SetBool("BeakOpen", true);
				return;
			}

			if (!Enabled) {
				if (Player_ != null) {
					Player_.Animator.SetBool("BeakOpen", false);
				}
				return;
			}

			bool previouslyOpen = Player_.Animator.GetBool("BeakOpen");
			Player_.Animator.SetBool("BeakOpen", InputDelegate_.LaserPressed);

			if (InputDelegate_.LaserPressed && previouslyOpen == false) {
				if (InGameConstants.EnableFlapping) {
					Player_.Animator.SetTrigger("WingFlap");
				}

				if (InGameConstants.EnableQuacking) {
					if (count_ % 2 == 1) {
						AudioConstants.Instance.CluckAlarm.PlaySFX(volumeScale: 1.9f, randomPitchRange: 0.1f);
						KeepBeakOpenFor(AudioConstants.Instance.CluckAlarm.length - 0.1f);
					} else {
						AudioConstants.Instance.Cluck.PlaySFX(volumeScale: 2.4f, randomPitchRange: 0.1f, pitchOffset: 0.03f - (soundOffset_ * 0.03f));
						KeepBeakOpenFor(AudioConstants.Instance.Cluck.length - 0.1f);
					}
					soundOffset_ = Mathf.Min(soundOffset_ + 1, kMaxSoundOffset);
					count_++;
				}
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