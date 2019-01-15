using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.AI;
using DT.Game.Battle.Players;
using DT.Game.GameModes;
using DT.Game.LevelSelect;

namespace DT.Game {
	public class GameConstants : PreExistingSingleton<GameConstants> {
		// PRAGMA MARK - Public Interface
		public UnityEvent OnBackgroundAccentColorChanged = new UnityEvent();

		public GameMode[] GameModes {
			get { return gameModes_; }
		}

		public void ConfigureWithSubstitutePlayerAI(BattlePlayer player) {
			AIStateMachine ai = ObjectPoolManager.Create<AIStateMachine>(GamePrefabs.Instance.AIPrefab, parent: player.gameObject);
			ai.Init(player, substitutePlayerAIConfiguration_);
		}

		[Header("Basic")]
		public int ScoreToWin = 5;
		public int PlayersToFill = 4;

		[Space]
		public float UIOffsetDelay = 0.1f;

		[Space]
		public float ColliderDisappearDelay = 0.2f;

		[Header("Player")]
		public BattlePlayerSkin[] PlayerSkins;
		public BattlePlayerSkin EnemySkin;

		[Space]
		public float PlayerShieldAlphaMin = 0.2f;

		[Header("Materials")]
		public Material PlayerOpaqueMaterial;
		public Material PlayerTransparentMaterial;

		public Material EyeMaterial;
		public Material BeakMaterial;

		public Material LaserMaterial;

		[Header("Battle")]
		public Vector3 PlayerFocusOffset = new Vector3(0.0f, 4.0f, 0.0f);
		public ArenaConfig PlayerCustomizationLobbyArena;

		[Space]
		public float BattlePlayerPartFadeDuration = 5.0f;

		[Header("Coop")]
		public CoopLevelConfig[] CoopLevels;
		public int MaxNumberOfWaves = 5;

		public Color BackgroundColor {
			get { return backgroundColor_; }
			set {
				backgroundColor_ = value;
				RefreshBackgroundColor();
			}
		}

		public Color BackgroundAccentColor {
			get { return backgroundAccentColor_; }
		}

		[Space]
		[SerializeField, FormerlySerializedAs("BackgroundColor")]
		private Color backgroundColor_;
		[SerializeField]
		private Color backgroundAccentColor_;

		[Header("AI")]
		public int AIPositionRetries = 30;

		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private GameMode[] gameModes_;

		[SerializeField]
		private AIConfiguration substitutePlayerAIConfiguration_;

		private void Awake() {
			RefreshBackgroundColor();
		}

		private void OnValidate() {
			RefreshBackgroundColor();
		}

		private void RefreshBackgroundColor() {
			if (Application.isPlaying) {
				BattleCamera.Instance.Camera.backgroundColor = BackgroundColor;
			} else {
				foreach (var camera in UnityEngine.Object.FindObjectsOfType<Camera>().Where(c => c.gameObject.name == "BattleCamera")) {
					camera.backgroundColor = BackgroundColor;
				}
			}
			RenderSettings.fogColor = BackgroundColor;

			RefreshBackgroundAccentColor();
		}

		private void RefreshBackgroundAccentColor() {
			float H, S, V;
			Color.RGBToHSV(backgroundColor_, out H, out S, out V);

			H = Mathf.Repeat(H + 0.03f, 1.0f);
			S = Mathf.Clamp01(S - 0.05f);
			V = Mathf.Clamp01(V - 0.68f);

			backgroundAccentColor_ = Color.HSVToRGB(H, S, V);
		}
	}
}