using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DTObjectPoolManager;
using DTViewManager.Internal;

namespace DTViewManager {
	public class ViewManager : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void AttachView(GameObject view) {
			view.transform.SetParent(this.transform, worldPositionStays: false);

			// if the application is not playing, we don't need to manage the view order
			if (!Application.isPlaying) {
				return;
			}

			RecyclablePrefab r = view.GetRequiredComponent<RecyclablePrefab>();
			int priority = priorityMap_.PriorityForPrefabName(r.PrefabName);
			cachedPriorities_[view.transform] = priority;

			for (int i = 0; i < this.transform.childCount; i++) {
				Transform child = this.transform.GetChild(i);

				if (!cachedPriorities_.ContainsKey(child)) {
					Debug.LogWarning(string.Format("Child ({0}) is not in cached priorties, didn't go through ViewManager flow?", child.gameObject.name));
					cachedPriorities_[child] = priorityMap_.DefaultPriority;
					continue;
				}

				int childPriority = cachedPriorities_[child];
				if (childPriority > priority) {
					view.transform.SetSiblingIndex(i);
					break;
				}
			}
		}

		public Canvas Canvas {
			get { return canvas_; }
		}

		public CanvasScaler CanvasScaler {
			get { return canvasScaler_; }
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private List<ViewPriorityPair> serializedPriorities_;
		[SerializeField]
		private int defaultPriority_ = 100;

		private ViewPriorityMap priorityMap_;
		private Dictionary<Transform, int> cachedPriorities_ = new Dictionary<Transform, int>();

		private Canvas canvas_;
		private CanvasScaler canvasScaler_;

		private void Awake() {
			canvas_ = GetComponent<Canvas>();
			canvasScaler_ = GetComponent<CanvasScaler>();

			priorityMap_ = new ViewPriorityMap(defaultPriority_);
			foreach (var viewPriorityPair in serializedPriorities_) {
				priorityMap_.SetPriorityForPrefabName(viewPriorityPair.PrefabName, viewPriorityPair.Priority);
			}

			foreach (Transform child in this.transform) {
				cachedPriorities_[child] = defaultPriority_;
			}
		}
	}

	[Serializable]
	public class ViewPriorityPair {
		public string PrefabName;
		public int Priority;
	}
}