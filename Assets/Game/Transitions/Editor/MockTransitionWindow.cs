using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using DTEasings;
using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public class MockTransitionWindow : EditorWindow {
		// PRAGMA MARK - Static
		[UnityEditor.MenuItem("Window/Mock Transition Window")]
		public static void Open() {
			EditorWindow.GetWindow<MockTransitionWindow>(utility: false, title: "Mock Transition", focus: true);
		}



		// PRAGMA MARK - Internal
		private GameObject transitionTarget_ = null;

		private ITransition[] transitions_;
		private float maxDuration_;

		private TransitionType transitionType_;
		private float value_ = 0.0f;

		private float? playTime_ = null;
		private double previousTimeSinceStartup_;

		private bool IsPlaying_ {
			get { return playTime_ != null; }
		}

		private void OnGUI() {
			EditorGUI.BeginChangeCheck();

			transitionTarget_ = (GameObject)EditorGUILayout.ObjectField("Transition Target: ", transitionTarget_, typeof(GameObject), allowSceneObjects: true);

			bool changed = EditorGUI.EndChangeCheck();
			if (changed && transitionTarget_ != null) {
				RefreshTransitions();
				transitionType_ = TransitionType.In;
				value_ = 0.0f;
				Refresh();
			}

			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();

			transitionType_ = (TransitionType)EditorGUILayout.EnumPopup("Transition Type: ", (Enum)transitionType_);
			value_ = EditorGUILayout.Slider("Transition Value: ", value_, 0.0f, 1.0f);

			bool valueOrTypeChanged = EditorGUI.EndChangeCheck();
			if (valueOrTypeChanged) {
				Refresh();
			}

			if (GUILayout.Button("Play")) {
				playTime_ = 0.0f;
				previousTimeSinceStartup_ = EditorApplication.timeSinceStartup;
				EditorApplication.update += HandleUpdate;
			}
		}

		private void HandleUpdate() {
			if (!IsPlaying_) {
				EditorApplication.update -= HandleUpdate;
				return;
			}

			float deltaTime = (float)(EditorApplication.timeSinceStartup - previousTimeSinceStartup_);
			previousTimeSinceStartup_ = EditorApplication.timeSinceStartup;

			float playTime = playTime_.Value;
			playTime += deltaTime;
			value_ = Mathf.Clamp01(playTime / maxDuration_);

			if (playTime >= maxDuration_) {
				playTime_ = null;
			} else {
				playTime_ = playTime;
			}
			Refresh();
			Repaint();
		}

		private void RefreshTransitions() {
			if (transitionTarget_ == null) {
				return;
			}

			transitions_ = transitionTarget_.GetComponentsInChildren<ITransition>();
		}

		private void Refresh() {
			if (transitions_ == null && transitionTarget_ != null) {
				RefreshTransitions();
			}

			if (transitions_ == null) {
				return;
			}

			maxDuration_ = transitions_.Max(t => t.Duration + t.BaseDelay);

			float time = value_ * maxDuration_;
			foreach (ITransition transition in transitions_) {
				float transitionValue = Mathf.Clamp01((time - transition.BaseDelay) / transition.Duration);
				transition.Refresh(transitionType_, transitionValue);
			}
		}
	}
}
