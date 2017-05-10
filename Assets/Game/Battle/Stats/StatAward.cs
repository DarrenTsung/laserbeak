using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.Battle.Stats {
	public class StatAward {
		// PRAGMA MARK - Public Interface
		public Stat SourceStat {
			get; private set;
		}

		public string AwardText {
			get; private set;
		}

		public StatAward(Stat sourceStat, string awardText) {
			SourceStat = sourceStat;
			AwardText = awardText;
		}
	}
}