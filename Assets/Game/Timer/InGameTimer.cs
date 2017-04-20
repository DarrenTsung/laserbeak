using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game {
	public class InGameTimer : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void Start(int seconds, Action finishedCallback) {
			if (timer_ != null) {
				timer_.Cleanup();
			} else {
				timer_ = ObjectPoolManager.CreateView<InGameTimer>(GamePrefabs.Instance.InGameTimerPrefab);
			}

			timer_.Init(seconds, finishedCallback);
		}

		public static void CleanupAndHide() {
			if (timer_ != null) {
				ObjectPoolManager.Recycle(timer_);
				timer_ = null;
			}
		}

		private static InGameTimer timer_;


		// PRAGMA MARK - Public Interface
		public void Init(int seconds, Action finishedCallback) {
			secondsLeft_ = seconds;
			finishedCallback_ = finishedCallback;
		}

		public void Cleanup() {
			finishedCallback_ = null;
			secondsLeft_ = 0.0f;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			Cleanup();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet text_;

		private float secondsLeft_ = 0.0f;
		private Action finishedCallback_;

		private void Update() {
			bool previouslyPositive = secondsLeft_ > 0;

			secondsLeft_ -= Time.deltaTime;
			if (previouslyPositive && secondsLeft_ <= 0) {
				if (finishedCallback_ != null) {
					finishedCallback_.Invoke();
				}
				finishedCallback_ = null;
			}

			text_.Text = string.Format("{0}s", (int)Mathf.Max(secondsLeft_, 0.0f));
		}
	}
}