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
		private const float kFlapDuration = 0.28f;

		[Header("Outlets")]
		[SerializeField]
		private Transform leftWingTransform_;
		[SerializeField]
		private Transform rightWingTransform_;

		private int soundOffset_ = 0;
		private int count_ = 0;
		private bool keepBeakOpen_ = false;

		private Coroutine wingFlapCoroutine_;
		private bool chainedWingFlap_ = false;

		protected override void Initialize() {
			leftWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			rightWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			wingFlapCoroutine_ = null;
		}

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
					WingFlap();
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

					GameNotifications.OnBattlePlayerCelebrated.Invoke(Player_);
				}
			}
		}

		private void KeepBeakOpenFor(float duration) {
			keepBeakOpen_ = true;
			CoroutineWrapper.DoAfterDelay(duration, () => {
				keepBeakOpen_ = false;
			});
		}

		private void WingFlap() {
			if (wingFlapCoroutine_ != null) {
				chainedWingFlap_ = true;
				return;
			}

			wingFlapCoroutine_ = this.DoEaseFor(kFlapDuration / 2.0f, EaseType.QuadraticEaseOut, (float p) => {
				leftWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(0.0f, -30.0f, p));
				rightWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(0.0f, 30.0f, p));
			}, () => {
				this.DoEaseFor(kFlapDuration / 2.0f, EaseType.QuadraticEaseIn, (float p) => {
					leftWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(-30.0f, 0.0f, p));
					rightWingTransform_.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(30.0f, 0.0f, p));
				}, () => {
					wingFlapCoroutine_ = null;
					if (chainedWingFlap_) {
						chainedWingFlap_ = false;
						WingFlap();
					}
				});
			});
		}
	}
}