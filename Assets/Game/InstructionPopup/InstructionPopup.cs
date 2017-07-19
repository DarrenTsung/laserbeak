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

using DT.Game.Players;
using DT.Game.Transitions;

namespace DT.Game.InstructionPopups {
	 public class InstructionPopup : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(string title, GameObject detailPrefab, Action allPlayersReadyCallback) {
			titleText_.Text = title;

			ObjectPoolManager.Create(detailPrefab, parent: detailContainer_);

			views_.Clear();
			allPlayersReadyCallback_ = allPlayersReadyCallback;

			foreach (Player player in RegisteredPlayers.AllPlayers.Where(p => !p.IsAI)) {
				var view = ObjectPoolManager.Create<InstructionPlayerReadyView>(GamePrefabs.Instance.InstructionPlayerReadyViewPrefab, parent: playersReadyContainer_);
				view.Init(player);
				view.OnReady += CheckIfAllPlayersReady;

				views_.Add(view);
			}

			transition_.AnimateIn(() => {
				// NOTE (darren): don't allow players to ready up until transition is finished
				foreach (var view in views_) {
					view.StartChecking();
				}
				CheckIfAllPlayersReady();
			});
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			foreach (InstructionPlayerReadyView view in views_) {
				view.OnReady -= CheckIfAllPlayersReady;
			}
			views_.Clear();

			playersReadyContainer_.RecycleAllChildren();
			detailContainer_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet titleText_;

		[SerializeField]
		private GameObject detailContainer_;

		[SerializeField]
		private GameObject playersReadyContainer_;

		private Action allPlayersReadyCallback_;

		private readonly List<InstructionPlayerReadyView> views_ = new List<InstructionPlayerReadyView>();
		private Transition transition_;

		private void Awake() {
			transition_ = new Transition(this.gameObject).SetOffsetDelay(0.0f);
		}

		private void CheckIfAllPlayersReady() {
			// if not all ready
			if (views_.Any(v => !v.Ready)) {
				return;
			}

			// animation would go here I think
			transition_.AnimateOut(() => {
				if (allPlayersReadyCallback_ != null) {
					allPlayersReadyCallback_.Invoke();
					allPlayersReadyCallback_ = null;
				}

				ObjectPoolManager.Recycle(this);
			});
		}
	}
}