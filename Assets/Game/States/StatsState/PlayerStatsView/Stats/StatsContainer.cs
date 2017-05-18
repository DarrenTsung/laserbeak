using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Stats;
using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Stats {
	public class StatsContainer : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Player player) {
			List<StatAward> allAwards = StatsManager.GetStatsFor(player).SelectMany(s => s.GetQualifiedAwards()).ToList();
			StatAward chosenAward = null;
			if (allAwards.Count > 0) {
				chosenAward = allAwards.Random();
			} else {
				chosenAward = new StatAward(sourceStat: null, awardText: "BEST AT: <b>PARTICIPATING</b>");
			}

			awardText_.Text = chosenAward.AwardText;

			Color color = player.Skin.BodyColor;
			awardText_.Color = color;
			separatorImage_.color = color;

			foreach (Stat stat in StatsManager.GetStatsFor(player)) {
				var view = ObjectPoolManager.Create<StatView>(GamePrefabs.Instance.StatView, parent: statViewContainer_);
				view.Init(player, stat.DisplayName, stat.DisplayValue, showMarker: chosenAward.SourceStat == stat);

				views_.Add(view);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			foreach (var view in views_) {
				ObjectPoolManager.Recycle(view);
			}
			views_.Clear();
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet awardText_;

		[SerializeField]
		private GameObject statViewContainer_;

		[SerializeField]
		private Image separatorImage_;

		private readonly List<StatView> views_ = new List<StatView>();
	}
}