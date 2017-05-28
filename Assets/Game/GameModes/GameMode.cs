using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.InstructionPopups;

namespace DT.Game.GameModes {
	public abstract class GameMode : ScriptableObject {
		// PRAGMA MARK - Static
		public static event Action<GameMode> OnActivate = delegate {};
		public static event Action<GameMode> OnFinish = delegate {};

		private static Dictionary<Type, int> idMap_ = new Dictionary<Type, int>() {
			{ typeof(LastBirdStandingGameMode), 1 },
			{ typeof(TagGameMode), 2 },
			{ typeof(TeamBattleGameMode), 3 },
			{ typeof(SumoBattleGameMode), 4 },
			{ typeof(GhostGameMode), 5 },
			{ typeof(KingOfTheHillGameMode), 6 },
		};

		public static int GetIdFor<T>() where T : GameMode {
			return idMap_.GetRequiredValueOrDefault(typeof(T));
		}


		// PRAGMA MARK - Public Interface
		public abstract string DisplayTitle {
			get;
		}

		public abstract int Id {
			get;
		}

		public void Activate(Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			Activate();
			OnActivate.Invoke(this);
		}

		public void Cleanup() {
			if (popup_ != null) {
				ObjectPoolManager.Recycle(popup_);
				popup_ = null;
			}

			CleanupInternal();
		}

		public void LoadArena() {
			if (arenaWhitelist_ != null && arenaWhitelist_.Length > 0) {
				ArenaManager.Instance.LoadArena(arenaWhitelist_.Random());
			} else {
				ArenaManager.Instance.LoadRandomArena();
			}
		}

		public void ShowInstructionsIfNecessary(Action callback) {
			// TODO (darren): in demo mode reset instructions on title screen
			if (GameModeShowedInstructionsCache.HasShowedInstructionsFor(this)) {
				callback.Invoke();
				return;
			}

			if (instructionDetailPrefab_ == null) {
				Debug.LogWarning("No instructionDetailPrefab_, skipping introduction!");
				callback.Invoke();
				return;
			}

			popup_ = ObjectPoolManager.CreateView<InstructionPopup>(GamePrefabs.Instance.InstructionPopupPrefab);
			string title = string.Format("<b>{0}</b>\n<size=23>INSTRUCTIONS", DisplayTitle);
			popup_.Init(title, instructionDetailPrefab_, () => {
				GameModeShowedInstructionsCache.MarkInstructionsAsShownFor(this);
				callback.Invoke();
				popup_ = null;
			});
		}


		// PRAGMA MARK - Internal
		[Header("Instruction Detail Prefab")]
		[SerializeField, DTValidator.Optional]
		private GameObject instructionDetailPrefab_;

		[Header("Outlets")]
		[SerializeField]
		private ArenaConfig[] arenaWhitelist_;

		private Action onFinishedCallback_ = null;
		private InstructionPopup popup_;

		protected abstract void Activate();
		protected abstract void CleanupInternal();

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