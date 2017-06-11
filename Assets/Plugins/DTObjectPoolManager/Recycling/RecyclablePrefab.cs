using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public class RecyclablePrefab : MonoBehaviour {
		public Action<RecyclablePrefab> OnCleanup = delegate { };

		public void Setup() {
			foreach (Renderer renderer in renderers_) {
				renderer.enabled = true;
			}

			foreach (Collider collider in colliders_) {
				collider.enabled = true;
			}

			foreach (Behaviour behaviour in behavioursToModify_) {
				behaviour.enabled = true;
			}

			foreach (IRecycleSetupSubscriber subscriber in setupSubscribers_) {
				subscriber.OnRecycleSetup();
			}
		}

		public void Cleanup() {
			this.OnCleanup.Invoke(this);

			foreach (Renderer renderer in renderers_) {
				renderer.enabled = false;
			}

			foreach (Collider collider in colliders_) {
				collider.enabled = false;
			}

			foreach (Behaviour behaviour in behavioursToModify_) {
				behaviour.enabled = false;
			}

			foreach (IRecycleCleanupSubscriber subscriber in cleanupSubscribers_) {
				subscriber.OnRecycleCleanup();
			}

			foreach (RecyclablePrefab r in attachedChildRecycables_) {
				r.OnCleanup -= DetachChildRecyclableObject;
				ObjectPoolManager.Recycle(r.gameObject);
			}
			attachedChildRecycables_.Clear();
		}

		public void AttachChildRecyclableObject(GameObject child) {
			RecyclablePrefab r = child.GetRequiredComponent<RecyclablePrefab>();
			bool addedSuccessfully = attachedChildRecycables_.Add(r);
			if (!addedSuccessfully) {
				Debug.LogWarning("AttachChildRecyclableObject - child recyclablePrefab already in attachedCleanupSubscribers!");
				return;
			}

			r.OnCleanup += DetachChildRecyclableObject;
		}

		public string PrefabName;

		// PRAGMA MARK - Internal
		private IRecycleSetupSubscriber[] setupSubscribers_;
		private IRecycleCleanupSubscriber[] cleanupSubscribers_;

		private HashSet<RecyclablePrefab> attachedChildRecycables_ = new HashSet<RecyclablePrefab>();

		// NOTE (darren): renderer is not a behaviour
		private Renderer[] renderers_;
		private Collider[] colliders_;
		private Behaviour[] behavioursToModify_;

		private void Awake() {
			setupSubscribers_ = this.GetDepthSortedComponentsInChildren<IRecycleSetupSubscriber>(greatestDepthFirst: true);
			cleanupSubscribers_ = this.GetDepthSortedComponentsInChildren<IRecycleCleanupSubscriber>(greatestDepthFirst: true);

			renderers_ = GetComponentsInChildren<Renderer>();
			colliders_ = GetComponentsInChildren<Collider>();

			List<Behaviour> behaviours = new List<Behaviour>();
			behaviours.AddRange(GetComponentsInChildren<Canvas>());
			behaviours.AddRange(GetComponentsInChildren<Collider2D>());
			behaviours.AddRange(GetComponentsInChildren<Light>());
			behaviours.AddRange(GetComponentsInChildren<Animator>());
			behavioursToModify_ = behaviours.ToArray();
		}

		private void DetachChildRecyclableObject(RecyclablePrefab r) {
			r.OnCleanup -= DetachChildRecyclableObject;
			bool successful = attachedChildRecycables_.Remove(r);
			if (!successful) {
				Debug.LogWarning("DetachChildRecyclableObject - failed to find child recyclablePrefab in attachedCleanupSubscribers!");
			}
		}
	}
}