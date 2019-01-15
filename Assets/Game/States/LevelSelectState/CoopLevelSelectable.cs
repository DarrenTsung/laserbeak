using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.LevelSelect {
	public class CoopLevelSelectable : MonoBehaviour, ISelectable, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public ArenaConfig ArenaConfig {
			get { return config_.ArenaConfig; }
		}

		public void Init(CoopLevelConfig config, Action<CoopLevelConfig> levelSelectedCallback) {
			levelSelectedCallback_ = levelSelectedCallback;
			config_ = config;
			displayNameTextOutlet_.Text = config.DisplayName;
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			if (levelSelectedCallback_ == null) {
				return;
			}

			levelSelectedCallback_.Invoke(config_);
			levelSelectedCallback_ = null;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet displayNameTextOutlet_;

		private CoopLevelConfig config_;
		private Action<CoopLevelConfig> levelSelectedCallback_;
	}
}