using System;
using System.Collections;
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


		// PRAGMA MARK - Public Interface
		public void Activate(Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			Activate();
			OnActivate.Invoke(this);
		}

		public abstract void Cleanup();

		public void ShowIntroductionIfNecessary(Action callback) {
			// always show introduction for now - need to cache shown or not in player prefs
			// also in demo mode reset instructions on title screen

			if (instructionDetailPrefab_ == null) {
				Debug.LogWarning("No instructionDetailPrefab_, skipping introduction!");
				callback.Invoke();
				return;
			}

			var popup = ObjectPoolManager.CreateView<InstructionPopup>(GamePrefabs.Instance.InstructionPopupPrefab);
			string title = string.Format("<b>{0}</b>\n<size=23>INSTRUCTIONS", DisplayTitle);
			popup.Init(title, instructionDetailPrefab_, callback);
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