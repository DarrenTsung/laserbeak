using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTDebugMenu;
using DTFPSView;

using DT.Game.Battle;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game {
	public static class RecordingMode {
		// PRAGMA MARK - Public Interface
		public static bool Active {
			get { return active_; }
			set {
				if (active_ == value) {
					return;
				}

				active_ = value;
				if (active_) {
					FPSView.Enabled = false;

					InGameConstants.RegisterHumanPlayers = false;
					InGameConstants.ShowScoringView = false;
					InGameConstants.ShowHintsView = false;
					InGameConstants.ShowNextGameModeUnlockView = false;
					GenericInspectorRegistry.Get("PHASERBEAK").SetDirty();
				}
			}
		}


		// PRAGMA MARK - Internal
		private static bool active_ = false;
	}
}