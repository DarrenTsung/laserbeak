using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.AI;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.LevelEditor;

namespace DT.Game {
	public static class InGameConstants {
		// PRAGMA MARK - Public Interface
		public static bool IsAlphaBuild {
			#if ALPHA
			get { return true; }
			#else
			get { return false; }
			#endif
		}

		public static readonly Rect ArenaRect = RectUtil.MakeRect(Vector2.zero, new Vector2(LevelEditorConstants.kArenaWidth, LevelEditorConstants.kArenaLength), pivot: new Vector2(0.5f, 0.5f));

		public static bool ShowedAlphaDisclaimer = false;

		public static bool ShowShields = true;

		public static bool AllowChargingLasers = true;
		public static bool AllowBattlePlayerMovement = true;

		public static bool EnableQuacking = false;
		public static bool EnableFlapping = true;

		public static bool ZoomInOnSurvivors = true;

		public static bool BattlePlayerPartsFade = false;

		public static bool FillWithAI = true;

		// Recording Mode Properties
		public static bool RegisterHumanPlayers = true;
		public static bool ShowScoringView = true;
		public static bool ShowHintsView = true;
		public static bool ShowNextGameModeUnlockView = true;

		public static readonly HashSet<BattlePlayer> AllowedChargingLasersWhitelist = new HashSet<BattlePlayer>();

		private static int? platformsLayerMask_;
		public static int PlatformsLayerMask {
			get {
				if (platformsLayerMask_ == null) {
					platformsLayerMask_ = LayerMask.GetMask("Platforms");
				}
				return platformsLayerMask_.Value;
			}
		}

		private static int? playersLayerMask_;
		public static int PlayersLayerMask {
			get {
				if (playersLayerMask_ == null) {
					playersLayerMask_ = LayerMask.GetMask("Player");
				}
				return playersLayerMask_.Value;
			}
		}

		public static bool IsAllowedToChargeLasers(BattlePlayer battlePlayer) {
			if (AllowChargingLasers == false) {
				return AllowedChargingLasersWhitelist.Contains(battlePlayer);
			}

			return true;
		}
	}
}