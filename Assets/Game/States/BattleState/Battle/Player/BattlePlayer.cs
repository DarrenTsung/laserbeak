using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	public class BattlePlayer : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static IList<BattlePlayer> ActivePlayers {
			get { return activePlayers_; }
		}


		// PRAGMA MARK - Static Internal
		private static readonly List<BattlePlayer> activePlayers_ = new List<BattlePlayer>();


		// PRAGMA MARK - Public Interface
		public event Action OnSkinChanged = delegate {};

		public void Init(IInputDelegate inputDelegate, BattlePlayerSkin skin) {
			inputController_.InitInput(this, inputDelegate);
			SetSkin(skin);
		}

		public void SetSkin(BattlePlayerSkin skin) {
			skin_ = skin;
			OnSkinChanged.Invoke();
		}

		public float BaseWeight {
			get { return kBaseWeight; }
		}

		public float Weight {
			get { return kBaseWeight + weightModifications_.Values.Sum(); }
		}

		public BattlePlayerSkin Skin {
			get { return skin_; }
		}

		public Rigidbody Rigidbody {
			get { return rigidbody_; }
		}

		public BattlePlayerInputController InputController {
			get { return inputController_; }
		}

		public void SetWeightModification(object key, float weightModification) {
			weightModifications_[key] = weightModification;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			activePlayers_.Add(this);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			activePlayers_.Remove(this);
			weightModifications_.Clear();
		}


		// PRAGMA MARK - Internal
		private const float kBaseWeight = 1.0f;

		[Header("Outlets")]
		[SerializeField]
		private BattlePlayerInputController inputController_;

		private Rigidbody rigidbody_;
		private BattlePlayerSkin skin_;

		private readonly Dictionary<object, float> weightModifications_ = new Dictionary<object, float>();

		private void Awake() {
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();

			foreach (BattlePlayerComponent component in this.GetComponentsInChildren<BattlePlayerComponent>()) {
				component.Init(this);
			}
		}
	}
}