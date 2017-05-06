using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.InstructionPopups;

namespace DT.Game.GameModes {
	public abstract class GameMode : ScriptableObject {
		// PRAGMA MARK - Static
		public static event Action<GameMode> OnActivate = delegate {};
		public static event Action<GameMode> OnFinish = delegate {};

		public static void ResetShowedInstructionsCache() {
			cache_ = null;
			PlayerPrefs.DeleteKey("GameModeShowedInstructionsCache");
		}

		private class GameModeShowedInstructionsCache {
			public HashSet<string> ShowedDisplayNames = new HashSet<string>();
		}

		private static GameModeShowedInstructionsCache cache_;
		private static GameModeShowedInstructionsCache Cache_ {
			get {
				if (cache_ == null) {
					string cacheString = PlayerPrefs.GetString("GameModeShowedInstructionsCache", defaultValue: "");
					cache_ = JsonUtility.FromJson<GameModeShowedInstructionsCache>(cacheString);

					if (cache_ == null) {
						cache_ = new GameModeShowedInstructionsCache();
					}
				}

				return cache_;
			}
		}

		private static bool HasShowedInstructionsFor(GameMode mode) {
			return Cache_.ShowedDisplayNames.Contains(mode.DisplayTitle);
		}

		private static void MarkInstructionsAsShownFor(GameMode mode) {
			Cache_.ShowedDisplayNames.Add(mode.DisplayTitle);
			PlayerPrefs.SetString("GameModeShowedInstructionsCache", JsonUtility.ToJson(Cache_));
		}



		// PRAGMA MARK - Public Interface
		public void Activate(Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			Activate();
			OnActivate.Invoke(this);
		}

		public abstract void Cleanup();

		public void ShowIntroductionIfNecessary(Action callback) {
			// TODO (darren): in demo mode reset instructions on title screen
			if (HasShowedInstructionsFor(this)) {
				callback.Invoke();
				return;
			}

			if (instructionDetailPrefab_ == null) {
				Debug.LogWarning("No instructionDetailPrefab_, skipping introduction!");
				callback.Invoke();
				return;
			}

			var popup = ObjectPoolManager.CreateView<InstructionPopup>(GamePrefabs.Instance.InstructionPopupPrefab);
			string title = string.Format("<b>{0}</b>\n<size=23>INSTRUCTIONS", DisplayTitle);
			popup.Init(title, instructionDetailPrefab_, () => {
				MarkInstructionsAsShownFor(this);
				callback.Invoke();
			});
		}


		// PRAGMA MARK - Internal
		[Header("Instruction Detail Prefab")]
		[SerializeField]
		private GameObject instructionDetailPrefab_;

		private Action onFinishedCallback_ = null;

		protected abstract string DisplayTitle {
			get;
		}

		protected abstract void Activate();

		protected void Finish() {
			if (onFinishedCallback_ == null) {
				Debug.LogWarning("Cannot finish GameMode without onFinishedCallback_!");
				return;
			}

			onFinishedCallback_.Invoke();
			onFinishedCallback_ = null;
			OnFinish.Invoke(this);
		}
	}
}