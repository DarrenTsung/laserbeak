using System;
using System.Collections;
using UnityEngine;

using DT.Game.ElementSelection;
using DT.Game.GameModes;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Pausing {
	public class InGamePauseView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Player player, Action exitPauseCallback, Action restartCallback) {
			exitPauseCallback_ = exitPauseCallback;
			restartCallback_ = restartCallback;

			text_.Text = string.Format("PAUSED\n<size=25>{0}", player.Nickname);

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.Init(player, this.gameObject);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectionView_ != null) {
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}
		}


		// PRAGMA MARK - Serialized Callbacks
		public void ExitPause() {
			if (exitPauseCallback_ != null) {
				exitPauseCallback_.Invoke();
				exitPauseCallback_ = null;
			}
		}

		public void Restart() {
			if (restartCallback_ != null) {
				restartCallback_.Invoke();
				restartCallback_ = null;
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet text_;

		private ElementSelectionView selectionView_;

		private Action exitPauseCallback_;
		private Action restartCallback_;
	}
}