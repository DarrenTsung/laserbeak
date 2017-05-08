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
		public static event Action OnIntroFinished = delegate {};

		public static void Show(string text, IList<Icon> icons, IList<int> playerOrdering = null, Action onFinishedCallback = null) {
			var view = ObjectPoolManager.CreateView<GameModeIntroView>(GamePrefabs.Instance.GameModeIntroViewPrefab);
			view.Init(text, icons, playerOrdering, onFinishedCallback);

			oldAllowBattlePlayerMovement_ = InGameConstants.AllowBattlePlayerMovement;
			oldAllowChargingLasers_ = InGameConstants.AllowChargingLasers;

			InGameConstants.AllowBattlePlayerMovement = false;
			InGameConstants.AllowChargingLasers = false;
			InGameConstants.EnableFlapping = true;

			AudioManager.Instance.SetBGMState(AudioManager.BGMState.Muted);
		}

		private static bool oldAllowBattlePlayerMovement_;
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
					createdObject.GetComponentInChildren<Image>().color = RegisteredPlayers.AllPlayers[playerIndex].Skin.BodyColor;
					index++;
				}
			}

			animator_.SetTrigger("Play");
			AudioConstants.Instance.GameModeIntro.PlaySFX(randomPitchRange: 0.0f);
		}

		public void Finish() {
			if (onFinishedCallback_ != null) {
				onFinishedCallback_.Invoke();
			}
			ObjectPoolManager.Recycle(this);

			InGameConstants.AllowBattlePlayerMovement = oldAllowBattlePlayerMovement_;
			InGameConstants.AllowChargingLasers = oldAllowChargingLasers_;
			InGameConstants.EnableFlapping = false;

			AudioManager.Instance.SetBGMState(AudioManager.BGMState.Normal);
			OnIntroFinished.Invoke();
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