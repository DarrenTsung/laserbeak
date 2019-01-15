using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTDebugMenu;
using DTFPSView;

using DT.Game.Battle;
using DT.Game.Debugs;
using DT.Game.GameModes;
using DT.Game.Hints;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.DebugMenu {
	public static class PHASERBEAKDebugInspector {
		// PRAGMA MARK - Internal
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			GenericInspector inspector = GenericInspectorRegistry.Get("PHASERBEAK");
			inspector.RegisterHeader("Properties");
			inspector.RegisterColorField("Background Color", () => GameConstants.Instance.BackgroundColor, (c) => GameConstants.Instance.BackgroundColor = c);
			inspector.RegisterToggle("Show FPS", () => FPSView.Enabled, (b) => FPSView.Enabled = b);
			inspector.RegisterToggle("Zoom In On Survivors", () => InGameConstants.ZoomInOnSurvivors, (b) => InGameConstants.ZoomInOnSurvivors = b);
			inspector.RegisterButton("Reset All The Things", () => {
				PHASERBEAKDebug.ResetAllThings();
			});

			inspector.RegisterHeader("Recording Mode Properties");
			inspector.RegisterToggle("Recording Mode", () => RecordingMode.Active, (b) => RecordingMode.Active = b);
			inspector.RegisterToggle("Register Human Players (off for AI only)", () => InGameConstants.RegisterHumanPlayers, (b) => InGameConstants.RegisterHumanPlayers = b);
			inspector.RegisterToggle("Show Scoring View", () => InGameConstants.ShowScoringView, (b) => InGameConstants.ShowScoringView = b);
			inspector.RegisterToggle("Show Hints View", () => InGameConstants.ShowHintsView, (b) => InGameConstants.ShowHintsView = b);
			inspector.RegisterToggle("Show Next Unlock", () => InGameConstants.ShowNextGameModeUnlockView, (b) => InGameConstants.ShowNextGameModeUnlockView = b);

			inspector.RegisterHeader("Battle");
			GameModeOverride.RegisterPopup(inspector);
			inspector.RegisterButton("Clear Focus (0)", () => BattleCamera.Instance.ClearTransformsOfInterest());
			inspector.RegisterButton("Focus On Player 1 (1)", () => BattleCameraDebug.FocusOnPlayer(1));
			inspector.RegisterButton("Focus On Player 2 (2)", () => BattleCameraDebug.FocusOnPlayer(2));
			inspector.RegisterButton("Focus On Player 3 (3)", () => BattleCameraDebug.FocusOnPlayer(3));
			inspector.RegisterButton("Focus On Player 4 (4)", () => BattleCameraDebug.FocusOnPlayer(4));

			// TODO (darren): add space
			inspector.RegisterButton("Fill Pending Scores", () => {
				Player player = RegisteredPlayers.AllPlayers.FirstOrDefault();
				if (player == null) {
					return;
				}

				for (int i = 0; i < GameConstants.Instance.ScoreToWin; i++) {
					PlayerScores.IncrementPendingScoreFor(player);
				}
			});
		}
	}
}