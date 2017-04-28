using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class BeatPlatform : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		// Tron End of Line - 196 BPM - * 2
		private const float kBeatDuration = 0.6122f;

		private static float CurrentBeatCount() {
			return Time.time / kBeatDuration;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			startBeat_ = (int)CurrentBeatCount();
			SetVisible(true, ignoreChecks: true);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject visibleContainer_;

		[SerializeField]
		private GameObject invisibleContainer_;

		[SerializeField]
		private Collider collider_;


		[Header("Properties")]
		[SerializeField]
		private int beatCountSwitch_ = 0;

		[SerializeField]
		private int beatOffset_ = 0;

		private bool visible_ = true;
		private int startBeat_ = 0;

		private void Update() {
			float beat = CurrentBeatCount() - startBeat_ - beatOffset_;
			bool visible = (int)(beat / beatCountSwitch_) % 2 == 0;

			Vector3 scale = Vector3.one * Mathf.Lerp(1.0f, 1.03f, Easings.QuarticEaseIn(Mathf.Repeat(beat, length: 1.0f)));
			visibleContainer_.transform.localScale = scale;
			invisibleContainer_.transform.localScale = scale;

			SetVisible(visible);
		}

		private void SetVisible(bool visible, bool ignoreChecks = false) {
			if (!ignoreChecks && visible_ == visible) {
				return;
			}

			visible_ = visible;

			visibleContainer_.SetActive(visible_);
			invisibleContainer_.SetActive(!visible_);

			if (visible_) {
				collider_.enabled = true;
			} else {
				this.DoAfterDelay(GameConstants.Instance.ColliderDisappearDelay, () => {
					collider_.enabled = false;
				});
			}
		}
	}
}