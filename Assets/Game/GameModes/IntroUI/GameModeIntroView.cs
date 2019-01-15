using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Audio;
using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.GameModes {
	[RequireComponent(typeof(Animator))]
	public class GameModeIntroView : MonoBehaviour, IRecycleCleanupSubscriber {
		public enum Icon {
			Player,
			Swords,
			Skull,
		}

		// PRAGMA MARK - Static Public Interface
		public static event Action OnIntroFinished {
			add { OnIntroFinished_ += value; }
			remove { OnIntroFinished_ -= value; }
		}

		public static event Action OnIntroFinishedPossibleMock {
			add {
				OnIntroFinished_ += value;
				// Invoke if view is not showing for mock purposes
				if (view_ == null) {
					Debug.LogWarning("Mocking OnIntroFinishedPossibleMock for GameModeIntroView - should not happen unless testing!");
					value.Invoke();
				}
			}
			remove { OnIntroFinished_ -= value; }
		}

		public static void Show(string text, IList<Icon> icons, IList<int> playerOrdering = null, Action onFinishedCallback = null) {
			Cleanup();

			view_ = ObjectPoolManager.CreateView<GameModeIntroView>(GamePrefabs.Instance.GameModeIntroViewPrefab);
			view_.Init(text, icons, playerOrdering, onFinishedCallback);

			oldAllowChargingLasers_ = InGameConstants.AllowChargingLasers;

			InGameConstants.AllowBattlePlayerMovement = false;
			InGameConstants.AllowChargingLasers = false;

			AudioManager.Instance.SetBGMState(AudioManager.BGMState.Muted);
		}

		public static void Cleanup() {
			if (view_ != null) {
				ObjectPoolManager.Recycle(view_);
				view_ = null;
			}
		}


		// PRAGMA MARK - Static Internal
		private static event Action OnIntroFinished_ = delegate {};

		private static GameModeIntroView view_;
		private static bool oldAllowChargingLasers_;

		private static IEnumerable<BattlePlayer> AllBattlePlayers {
			get { return PlayerSpawner.AllSpawnedBattlePlayers.Concat(AISpawner.AllSpawnedBattlePlayers); }
		}


		// PRAGMA MARK - Public Interface
		public void Init(string text, IList<Icon> icons, IList<int> playerOrdering, Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			gameModeText_.Text = text;

			int index = 0;
			foreach (Icon icon in icons) {
				GameObject prefab = null;
				switch (icon) {
					case Icon.Player:
						prefab = playerIconPrefab_;
						break;
					case Icon.Swords:
						prefab = swordsIconPrefab_;
						break;
					case Icon.Skull:
						prefab = skullIconPrefab_;
						break;
				}

				GameObject createdObject = ObjectPoolManager.Create(prefab, parent: iconLayoutGroup_);
				if (icon == Icon.Player) {
					int playerIndex = index;
					if (playerOrdering != null) {
						playerIndex = playerOrdering[index];
					}
					createdObject.GetComponentInChildren<Image>().color = RegisteredPlayers.AllPlayers[playerIndex].Skin.UIColor;
					index++;
				}
			}

			animator_.SetTrigger("Play");
			AudioConstants.Instance.GameModeIntro.PlaySFX(randomPitchRange: 0.0f);
		}

		public void Finish() {
			if (onFinishedCallback_ != null) {
				onFinishedCallback_.Invoke();
				onFinishedCallback_ = null;
			}
			ObjectPoolManager.Recycle(this);
			view_ = null;

			InGameConstants.AllowBattlePlayerMovement = true;
			InGameConstants.AllowChargingLasers = oldAllowChargingLasers_;

			AudioManager.Instance.SetBGMState(AudioManager.BGMState.Normal);
			OnIntroFinished_.Invoke();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			iconLayoutGroup_.transform.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		[Header("Prefabs")]
		[SerializeField]
		private GameObject playerIconPrefab_;

		[SerializeField]
		private GameObject swordsIconPrefab_;

		[SerializeField]
		private GameObject skullIconPrefab_;


		[Header("Outlets")]
		[SerializeField]
		private TextOutlet gameModeText_;

		[SerializeField]
		private GameObject iconLayoutGroup_;

		private Animator animator_;
		private Action onFinishedCallback_ = null;

		private void Awake() {
			animator_ = this.GetRequiredComponent<Animator>();
		}
	}
}