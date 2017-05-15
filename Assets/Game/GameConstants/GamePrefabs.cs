using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.AI;
using DT.Game.Battle.Players;
using DT.Game.GameModes;

namespace DT.Game {
	public class GamePrefabs : Singleton<GamePrefabs> {
		// PRAGMA MARK - Public Interface
		public GameObject PlayerPrefab {
			get { return playerPrefab_; }
		}

		public GameObject AIPrefab {
			get { return aiPrefab_; }
		}

		public GameObject InGamePlayerScoringView {
			get { return inGamePlayerScoringView_; }
		}

		public GameObject InGamePlayerViewPrefab {
			get { return inGamePlayerViewPrefab_; }
		}

		public GameObject IndividualPlayerScoringViewPrefab {
			get { return individualPlayerScoringViewPrefab_; }
		}

		public GameObject ScoreBubbleViewPrefab {
			get { return scoreBubbleViewPrefab_; }
		}

		public GameObject GameModeIntroViewPrefab;
		public GameObject InGamePlayerHUDEffect;
		public GameObject InGameTimerPrefab;
		public GameObject InGamePauseViewPrefab;

		[Header("Element Selection")]
		public GameObject ElementSelectionViewPrefab;
		public GameObject SelectorPrefab;

		[Header("Player Customization")]
		public GameObject PlayerCustomizationViewPrefab;
		public GameObject IndividualPlayerCustomizationViewPrefab;
		public GameObject PlayerReadyView;
		public GameObject NicknameCustomizationViewPrefab;
		public GameObject SkinSelectablePrefab;
		public GameObject CanJoinViewPrefab;

		[Header("Walls")]
		public GameObject WallSegmentReflectPrefab;
		public GameObject WallSegmentBarrierPrefab;

		[Header("Tag")]
		public GameObject TagExplosivePrefab;

		[Header("Arena")]
		public GameObject InGameDynamicArenaPrefab;
		public GameObject PlayerSpawnPointPrefab;
		public GameObject DisappearingPlatformPrefab;

		[Header("Instruction Popups")]
		public GameObject InstructionPopupPrefab;
		public GameObject InstructionPlayerReadyViewPrefab;

		[Header("Scoring")]
		public GameObject StatView;

		[Header("Level Editors")]
		public GameObject LevelEditorPrefab;
		public GameObject LevelEditorCursorPrefab;

		public GameObject[] LevelEditorObjects;
		public GameObject LevelEditorPlayerSpawnPointPrefab;
		public GameObject DynamicArenaObjectContainer;

		[Space]
		public GameObject WallPlacerPrefab;
		public GameObject PlatformPlacerPrefab;
		public GameObject PlayerSpawnPointPlacerPrefab;

		[Header("Generic Menu")]
		public GameObject MenuViewPrefab;
		public GameObject MenuItemPrefab;

		[Header("Scrollable Menu Popup")]
		public GameObject ScrollableMenuPopupPrefab;
		public GameObject ScrollableMenuItemViewPrefab;


		// PRAGMA MARK - Internal
		[Header("Prefabs")]
		[SerializeField]
		private GameObject playerPrefab_;

		[SerializeField]
		private GameObject aiPrefab_;

		[SerializeField]
		private GameObject inGamePlayerScoringView_;

		[SerializeField]
		private GameObject inGamePlayerViewPrefab_;

		[SerializeField]
		private GameObject individualPlayerScoringViewPrefab_;

		[SerializeField]
		private GameObject scoreBubbleViewPrefab_;
	}
}