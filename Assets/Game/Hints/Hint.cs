using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Transitions;

namespace DT.Game.Hints {
	public class Hint : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Static
		public static void Show(string hintString) {
			HintInstance_.ShowNewString(hintString);
		}

		public static void Hide() {
			HintInstance_.HideImmediate();
		}

		private static Hint hintInstance_ = null;
		private static Hint HintInstance_ {
			get { return hintInstance_ ?? (hintInstance_ = ObjectPoolManager.CreateView<Hint>(GamePrefabs.Instance.HintPrefab)); }
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			showing_ = false;
			transition_.AnimateOut(instant: true);
		}


		// PRAGMA MARK - Internal
		private const float kNoPlayersCheckDuration = 0.8f;

		[Header("Outlets")]
		[SerializeField]
		private TextOutlet hintText_;

		private Transition transition_;
		private bool showing_ = false;

		private void Awake() {
			transition_ = new Transition(this.gameObject);
		}

		private void ShowNewString(string hintString) {
			if (showing_) {
				HideImmediate(callback: () => ShowImmediate(hintString));
			} else {
				ShowImmediate(hintString);
			}
		}

		private void ShowImmediate(string hintString) {
			if (showing_) {
				Debug.LogWarning("ShowImmediate - already showing!");
			}

			hintText_.Text = hintString;
			showing_ = true;
			transition_.AnimateIn();
		}

		private void HideImmediate(Action callback = null) {
			if (!showing_) {
				return;
			}

			transition_.AnimateOut(() => {
				showing_ = false;
				if (callback != null) {
					callback.Invoke();
				}
				});
			}
		}
	}