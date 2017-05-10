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
	public abstract class Stat {
		// PRAGMA MARK - Public Interface
		public abstract string DisplayName {
			get;
		}

		public abstract string DisplayValue {
			get;
		}

		public Stat(Player player) {
			Player_ = player;
		}

		public abstract IList<StatAward> GetQualifiedAwards();

		public abstract void Clear();


		// PRAGMA MARK - Internal
		protected Player Player_ {
			get; private set;
		}
	}
}