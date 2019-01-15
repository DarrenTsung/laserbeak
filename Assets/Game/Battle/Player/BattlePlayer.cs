using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DT.Game.Transitions;
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

		public void PlaySpawnTransition() {
			BattleCamera.Shake(0.6f);
			AudioConstants.Instance.PlayerSpawn.PlaySFX();
			SetShieldAlpha(0.0f);
			spawnTransition_.AnimateIn(() => {
				SetShieldAlpha(GameConstants.Instance.PlayerShieldAlphaMin);
			});
		}

		public void Init(IBattlePlayerInputDelegate inputDelegate, BattlePlayerSkin skin) {
			SetSkin(skin);
			SetInputDelegate(inputDelegate);
		}

		public void SetInputDelegate(IBattlePlayerInputDelegate inputDelegate) {
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

		public ParticleSystem DustParticleSystem {
			get { return dustParticleSystem_; }
		}

		public GameObject AccessoriesContainer {
			get { return accessoriesContainer_; }
		}

		public BattlePlayerHealth Health {
			get { return health_; }
		}

		public Animator Animator {
			get { return animator_; }
		}

		public void SetShieldAlpha(float alpha) {
			if (alpha <= 0.0f) {
				shieldRenderer_.enabled = false;
				return;
			}

			if (!InGameConstants.ShowShields) {
				shieldRenderer_.enabled = false;
				return;
			}

			shieldRenderer_.enabled = true;
			Color shieldColor = shieldRenderer_.material.GetColor("_Color");
			Color newShieldColor = shieldColor.WithAlpha(alpha);
			shieldRenderer_.material.SetColor("_Color", newShieldColor);
		}

		public void SetShieldAlphaMultiply(float alphaMultiply) {
			shieldRenderer_.material.SetFloat("_Alpha", alphaMultiply);
		}

		public void SetWeightModification(object key, float weightModification) {
			weightModifications_[key] = weightModification;
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		public void OnRecycleSetup() {
			activePlayers_.Add(this);
			Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;
			Rigidbody.drag = kBaseDrag;

			Laser.RegisterLaserTarget(this.transform);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		public void OnRecycleCleanup() {
			activePlayers_.Remove(this);
			weightModifications_.Clear();
			AccessoriesContainer.RecycleAllChildren();

			Laser.UnregisterLaserTarget(this.transform);
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

		[SerializeField]
		private ParticleSystem dustParticleSystem_;

		[SerializeField]
		private MeshRenderer shieldRenderer_;

		[SerializeField]
		private Animator animator_;

		private Renderer[] bodyRenderers_;
		private BattlePlayerHealth health_;

		private Rigidbody rigidbody_;
		private BattlePlayerSkin skin_;

		private Transition spawnTransition_;

		private readonly Dictionary<object, float> weightModifications_ = new Dictionary<object, float>();

		private void Awake() {
			bodyRenderers_ = body_.GetComponentsInChildren<Renderer>();
			health_ = this.GetRequiredComponentInChildren<BattlePlayerHealth>();
			rigidbody_ = this.GetRequiredComponent<Rigidbody>();

			foreach (BattlePlayerComponent component in this.GetComponentsInChildren<BattlePlayerComponent>()) {
				component.Init(this);
			}

			// override the offset delay present in all UI transitions
			spawnTransition_ = new Transition(this.gameObject).SetOffsetDelay(0.0f);
		}
	}
}