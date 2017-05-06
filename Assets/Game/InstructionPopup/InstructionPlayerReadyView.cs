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

		public bool Ready {
			get { return ready_; }
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			ready_ = false;
			readyImage_.color = Color.white.WithAlpha(0.3f);
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private Image readyImage_;

		private Player player_;
		private bool ready_;

		private void Update() {
			if (player_ == null) {
				return;
			}

			if (InputUtil.WasPositivePressedFor(player_.InputDevice)) {
				ready_ = true;
				readyImage_.color = Color.white.WithAlpha(1.0f);
				OnReady.Invoke();
			}
		}
	}
}