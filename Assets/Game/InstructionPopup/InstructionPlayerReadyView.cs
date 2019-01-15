using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Players;

namespace DT.Game.InstructionPopups {
	public class InstructionPlayerReadyView : MonoBehaviour, IRecycleSetupSubscriber {
		// PRAGMA MARK - Public Interface
		public event Action OnReady = delegate {};

		public void Init(Player player) {
			player_ = player;
			readyImage_.sprite = player.Skin.ThumbnailSprite;
		}

		public void StartChecking() {
			canCheck_ = true;
		}

		public bool Ready {
			get { return ready_; }
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			ready_ = false;
			readyImage_.color = Color.white.WithAlpha(0.3f);
			canCheck_ = false;
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private Image readyImage_;

		private Player player_;
		private bool ready_;
		private bool canCheck_;

		private void Update() {
			if (!canCheck_) {
				return;
			}

			if (player_ == null) {
				return;
			}

			if (player_.Input.PositiveWasPressed) {
				ready_ = true;
				readyImage_.color = Color.white.WithAlpha(1.0f);
				OnReady.Invoke();
			}
		}
	}
}