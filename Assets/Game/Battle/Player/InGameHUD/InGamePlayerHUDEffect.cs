using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class InGamePlayerHUDEffect : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static void CreateForAllPlayers() {
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				var effect = ObjectPoolManager.CreateView<InGamePlayerHUDEffect>(GamePrefabs.Instance.InGamePlayerHUDEffect);
				effect.Init(player);
			}
		}


		// PRAGMA MARK - Public Interface
		public void Init(Player player) {
			battlePlayer_ = PlayerSpawner.GetBattlePlayerFor(player);
			nicknameText_.Text = player.Nickname;
			playerColor_ = player.Skin.BodyColor;

			UpdateAnchoredPosition();
			Animate();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			battlePlayer_ = null;
		}


		// PRAGMA MARK - Internal
		private const float kAnimateDuration = 4.5f;
		private const float kAnimateColorDuration = 0.6f;

		[Header("Outlets")]
		[SerializeField]
		private TextOutlet nicknameText_;
		[SerializeField]
		private Image pointer_;

		[SerializeField, ReadOnly]
		private Color playerColor_;

		private BattlePlayer battlePlayer_;
		private RectTransform rectTransform_;

		private Canvas canvas_;
		private Canvas Canvas_ {
			get {
				if (canvas_ == null) {
					canvas_ = this.GetComponentInParent<Canvas>();
				}
				return canvas_;
			}
		}

		private void Awake() {
			rectTransform_ = this.GetRequiredComponent<RectTransform>();
		}

		private void Update() {
			UpdateAnchoredPosition();
		}

		private void UpdateAnchoredPosition() {
			if (battlePlayer_ == null) {
				return;
			}

			rectTransform_.anchoredPosition = Camera.main.WorldToScreenPoint(battlePlayer_.transform.position) / Canvas_.scaleFactor;
		}

		private void Animate() {
			// animate color in
			CoroutineWrapper.DoEaseFor(kAnimateColorDuration, EaseType.CubicEaseInOut, (float p) => {
				nicknameText_.Color = Color.Lerp(playerColor_.WithAlpha(0.0f), playerColor_, p);
				pointer_.color = Color.Lerp(playerColor_.WithAlpha(0.0f), playerColor_, p);
			});

			// animate color out
			CoroutineWrapper.DoAfterDelay(kAnimateDuration - kAnimateColorDuration, () => {
				CoroutineWrapper.DoEaseFor(kAnimateColorDuration, EaseType.CubicEaseInOut, (float p) => {
					nicknameText_.Color = Color.Lerp(playerColor_.WithAlpha(0.0f), playerColor_, 1.0f - p);
					pointer_.color = Color.Lerp(playerColor_.WithAlpha(0.0f), playerColor_, 1.0f - p);
				}, () => {
					ObjectPoolManager.Recycle(this);
				});
			});
		}
	}
}