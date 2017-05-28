using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.GameModes.KingOfTheHill {
	public class KingOfTheHillArea : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public ICollection<BattlePlayer> ActivePlayers {
			get { return activePlayers_; }
		}



		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			RecalculateAreaEdges();
			RefreshEdgeColor();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		private const float kEdgeStrokeSize = 0.1f;
		private const float kHalfEdgeStrokeSize = kEdgeStrokeSize / 2.0f;

		[Header("Outlets")]
		[SerializeField]
		private Material edgeMaterial_;

		private readonly RaycastHit[] raycastHits_ = new RaycastHit[8];
		private readonly HashSet<BattlePlayer> activePlayers_ = new HashSet<BattlePlayer>();
		private Vector3 cachedParentLocalScale_;

		private MeshRenderer[] planeRenderers_;

		private void Update() {
			UpdateScaling();
			UpdateActivePlayers();
		}

		private void UpdateActivePlayers() {
			int hits = Physics.BoxCastNonAlloc(this.transform.position - Vector3.up, cachedParentLocalScale_ / 2.0f, Vector3.up, raycastHits_, Quaternion.identity, maxDistance: Mathf.Infinity, layermask: InGameConstants.PlayersLayerMask);
			// HACK (darren): hits is going to twice is large as count because 2 colliders per object
			if (hits * 2 != activePlayers_.Count) {
				activePlayers_.Clear();
				for (int i = 0; i < hits; i++) {
					RaycastHit hit = raycastHits_[i];
					var battlePlayer = hit.collider.GetComponentInParent<BattlePlayer>();
					if (battlePlayer == null) {
						Debug.LogError("KingOfTheHillArea - Hit non battle player?");
						continue;
					}

					activePlayers_.Add(battlePlayer);
				}

				RefreshEdgeColor();
			}
		}

		private void UpdateScaling() {
			if (this.transform.parent == null) {
				Debug.LogWarning("Parent is null?");
				return;
			}

			if (this.transform.parent.localScale == cachedParentLocalScale_) {
				return;
			}

			RecalculateAreaEdges();
			cachedParentLocalScale_ = this.transform.parent.localScale;
		}

		private void RecalculateAreaEdges() {
			if (this.transform.parent == null) {
				Debug.LogError("Cannot RecalculateAreaEdges when parent is null!");
				return;
			}

			this.RecycleAllChildren();

			Vector3 scale = this.transform.parent.localScale;

			// assumption that edge prefab is a 1x1 cube
			// note that the cube is already scaled because of the parent
			var topEdge = ObjectPoolManager.Create(GamePrefabs.Instance.KotHEdgePrefab, parent: this.gameObject);
			topEdge.transform.localScale = new Vector3(1.0f, 1.0f, (1.0f / scale.z) * kEdgeStrokeSize);
			topEdge.transform.localPosition = new Vector3(0.0f, 0.01f, 0.5f - (kHalfEdgeStrokeSize / scale.z));

			var bottomEdge = ObjectPoolManager.Create(GamePrefabs.Instance.KotHEdgePrefab, parent: this.gameObject);
			bottomEdge.transform.localScale = new Vector3(1.0f, 1.0f, (1.0f / scale.z) * kEdgeStrokeSize);
			bottomEdge.transform.localPosition = new Vector3(0.0f, 0.01f, -0.5f + (kHalfEdgeStrokeSize / scale.z));

			var leftEdge = ObjectPoolManager.Create(GamePrefabs.Instance.KotHEdgePrefab, parent: this.gameObject);
			leftEdge.transform.localScale = new Vector3((1.0f / scale.x) * kEdgeStrokeSize, 1.0f, 1.0f);
			leftEdge.transform.localPosition = new Vector3(-0.5f + (kHalfEdgeStrokeSize / scale.x), 0.01f, 0.0f);

			var rightEdge = ObjectPoolManager.Create(GamePrefabs.Instance.KotHEdgePrefab, parent: this.gameObject);
			rightEdge.transform.localScale = new Vector3((1.0f / scale.x) * kEdgeStrokeSize, 1.0f, 1.0f);
			rightEdge.transform.localPosition = new Vector3(0.5f - (kHalfEdgeStrokeSize / scale.x), 0.01f, 0.0f);
		}

		private void RefreshEdgeColor() {
			if (activePlayers_.Count != 1) {
				edgeMaterial_.SetColor("_EmissionColor", Color.white);
				return;
			}

			BattlePlayer player = activePlayers_.First();
			edgeMaterial_.SetColor("_EmissionColor", player.Skin.BodyColor);
		}
	}
}