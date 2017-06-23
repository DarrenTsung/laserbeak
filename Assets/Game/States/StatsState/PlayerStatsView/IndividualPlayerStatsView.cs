using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.Stats {
	public class IndividualPlayerStatsView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Player player) {
			player_ = player;

			var inGamePlayerView = ObjectPoolManager.Create<InGamePlayerView>(GamePrefabs.Instance.InGamePlayerViewPrefab, parent: playerViewContainer_);
			inGamePlayerView.InitWith(player, enableNudge: true);

			statsContainer_.Init(player_);
			statsContainer_.gameObject.SetActive(true);

			int rank = PlayerScores.GetRankFor(player_);
			Color rankColor = Color.clear;
			if (rank == 1) {
				rankColor = ColorUtil.HexStringToColor("EAD94FFF");
			} else if (rank == 2) {
				rankColor = ColorUtil.HexStringToColor("B6B6B6FF");
			} else if (rank == 3) {
				rankColor = ColorUtil.HexStringToColor("A79376FF");
			} else {
				rankColor = ColorUtil.HexStringToColor("B08A76FF");
			}
			rankBannerImage_.color = rankColor;
			rankText_.Text = string.Format("{0}", rank);

			crownObject_.SetActive(PlayerScores.Winner == player_);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			playerViewContainer_.transform.RecycleAllChildren();
			player_ = null;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private GameObject playerViewContainer_;

		[SerializeField]
		private StatsContainer statsContainer_;

		[Header("Rank")]
		[SerializeField]
		private GameObject rankContainer_;

		[SerializeField]
		private GameObject crownObject_;

		[SerializeField]
		private Image rankBannerImage_;

		[SerializeField]
		private TextOutlet rankText_;

		private Player player_;
	}
}