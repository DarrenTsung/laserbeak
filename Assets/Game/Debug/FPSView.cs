using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.Game {
	public class FPSView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public static event Action OnFPSViewEnabledChanged = delegate {};
		public static bool Enabled {
			get { return enabled_; }
			set {
				if (enabled_ == value) return;
				enabled_ = value;
				OnFPSViewEnabledChanged.Invoke();
			}
		}

		private static bool enabled_ = false;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			if (Application.isEditor) {
				FPSView.Enabled = true;
			}
		}


		// PRAGMA MARK - Internal
		private const float kUpdateInterval = 0.5f;

		[Header("Outlets")]
		[SerializeField]
		private TextOutlet fpsText_;

		private float accumulatedFPS_ = 0; // FPS accumulated over the interval
		private int frames_ = 0; // Frames drawn over the interval
		private float timeleft_; // Left time for current interval

		private void Awake() {
			OnFPSViewEnabledChanged += HandleFPSViewEnabledChanged;
		}

		private void OnDestroy() {
			OnFPSViewEnabledChanged -= HandleFPSViewEnabledChanged;
		}

		private void HandleFPSViewEnabledChanged() {
			this.enabled = Enabled;
			fpsText_.SetEnabled(Enabled);
		}

		private void Update() {
			timeleft_ -= Time.deltaTime;
			accumulatedFPS_ += Time.timeScale / Time.deltaTime;
			frames_++;

			// Interval ended - update GUI text and start new interval
			if (timeleft_ <= 0.0) {
				// display two fractional digits (f2 format)
				float fps = accumulatedFPS_ / frames_;
				fpsText_.Text = string.Format("{0:F2} FPS", fps);

				if (fps < 30) {
					fpsText_.Color = Color.yellow;
				} else if (fps < 10) {
					fpsText_.Color = Color.red;
				} else {
					fpsText_.Color = Color.green;
				}

				timeleft_ = kUpdateInterval;
				accumulatedFPS_ = 0.0f;
				frames_ = 0;
			}
		}
	}
}