using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.GameModes {
	[RequireComponent(typeof(Animator))]
	public class GameModeIntroView : MonoBehaviour, IRecycleCleanupSubscriber {
		public enum Icon {
			Player,
			Swords,
		}

		// PRAGMA MARK - Static Public Interface
		public static void Show(string text, IList<Icon> icons, Action onFinishedCallback) {
			var view = ObjectPoolManager.CreateView<GameModeIntroView>(GamePrefabs.Instance.GameModeIntroViewPrefab);
			view.Init(text, icons, onFinishedCallback);
		}


		// PRAGMA MARK - Public Interface
		public void Init(string text, IList<Icon> icons, Action onFinishedCallback) {
			onFinishedCallback_ = onFinishedCallback;
			gameModeText_.Text = text;

			int playerIndex = 0;
			foreach (Icon icon in icons) {
				GameObject prefab = null;
				switch (icon) {
					case Icon.Player:
						prefab = playerIconPrefab_;
						break;
					case Icon.Swords:
						prefab = swordsIconPrefab_;
						break;
				}

				GameObject createdObject = ObjectPoolManager.Create(prefab, parent: iconLayoutGroup_);
				if (icon == Icon.Player) {
					createdObject.GetComponentInChildren<Image>().color = RegisteredPlayers.AllPlayers[playerIndex].Skin.Color;
					playerIndex++;
				}
			}

			animator_.SetTrigger("Play");
		}

		public void Finish() {
			onFinishedCallback_.Invoke();
			ObjectPoolManager.Recycle(this);
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