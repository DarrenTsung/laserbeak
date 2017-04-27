using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	public class BattlePlayer : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static Public Interface
		public static IList<BattlePlayer> ActivePlayers {
			get { return activePlayers_; }
		}


		// PRAGMA MARK - Static Internal
		private static readonly List<BattlePlayer> activePlayers_ = new List<BattlePlayer>();


		// PRAGMA MARK - Public Interface
		public const float kBaseDrag = 6.0f;

		public event Action OnSkinChanged = delegate {};

		public void Init(IInputDelegate inputDelegate, BattlePlayerSkin skin) {
			SetInputDelegate(inputDelegate);
			SetSkin(skin);
		}

		public void SetInputDelegate(IInputDelegate inputDelegate) {
			inputController_.InitInput(this, inputDelegate);
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

		public Renderer[] BodyRenderers {
			get { return bodyRenderers_; }
		}

		public GameObject AccessoriesContainer {
			get { return accessoriesContainer_; }
		}

		public BattlePlayerHealth Health {
			get { return health_; }
		}

		public void SetWeightModification(object key, float weightModification) {
			weightModifications_[key] = weightModification;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			activePlayers_.Add(this);
			Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.drag = kBaseDrag;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			activePlayers_.Remove(this);
			weightModifications_.Clear();
			AccessoriesContainer.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		private const float kBaseWeight = 1.0f;

		[Header("Outlets")]
		[SerializeField]
		private BattlePlayerInputController inputController_;

		[SerializeField]
		private GameObject body_;

		[SerializeField]
		private GameObject accessoriesContainer_;

		private Renderer[] bodyRenderers_;
		private BattlePlayerHealth health_;

		private Rigidbody rigidbody_;
		private BattlePlayerSkin skin_;

		private readonly Dictionary<object, float> weightModifications_ = new Dictionary<object, float>();

		private void Awake() {
			bodyRenderers_ = body_.GetComponentsInChildren<Renderer>();
			health_ = this.GetRequiredComponentInChildren<BattlePlayerHealth>();
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();

			foreach (BattlePlayerComponent component in this.GetComponentsInChildren<BattlePlayerComponent>()) {
				component.Init(this);
			}
		}
	}
}