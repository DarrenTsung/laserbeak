using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.ElementSelection {
	public class ElementSelectionView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public event Action OnSelectorMoved = delegate {};
		public event Action<ISelectable> OnSelectableHover = delegate {};
		public event Action<ISelectable> OnSelectableSelected = delegate {};

		public void Init(Player player, GameObject elementsContainer, ISelectable startSelectable = null) {
			player_ = player;
			inputDevices_ = new List<InputDevice>() { player_.InputDevice };

			Init(elementsContainer, startSelectable);
		}

		public void Init(IEnumerable<InputDevice> inputDevices, GameObject elementsContainer, ISelectable startSelectable = null) {
			inputDevices_ = inputDevices;

			Init(elementsContainer, startSelectable);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectorTransform_ != null) {
				ObjectPoolManager.Recycle(selectorTransform_.gameObject);
				selectorTransform_ = null;
			}

			player_ = null;
			inputDevices_ = null;
		}


		// PRAGMA MARK - Internal
		private static readonly Vector2 kPadding = new Vector2(8, 8);
		private const float kIntentThreshold = 0.3f;

		private const float kMoveDelay = 0.16f;

		private RectTransform selectorTransform_;
		private Player player_;

		private IEnumerable<InputDevice> inputDevices_ = null;

		private float delay_ = 0.0f;

		private ISelectable[] selectables_;

		private ISelectable currentSelectable_;
		private ISelectable CurrentSelectable_ {
			get { return currentSelectable_; }
			set {
				if (value == null) {
					return;
				}

				currentSelectable_ = value;
				OnSelectableHover.Invoke(currentSelectable_);

				RefreshSelectorPosition();
			}
		}

		private void Init(GameObject elementsContainer, ISelectable startSelectable) {
			foreach (var del in elementsContainer.GetComponentsInChildren<ISelectionViewDelegate>()) {
				del.HandleSelectionView(this);
			}

			selectables_ = elementsContainer.GetComponentsInChildren<ISelectable>();
			if (selectables_.Length <= 0) {
				Debug.LogError("ElementSelectionView - needs selectables??");
			}

			CoroutineWrapper.DoAtEndOfFrame(() => {
				selectorTransform_ = ObjectPoolManager.CreateView(GamePrefabs.Instance.SelectorPrefab).GetComponent<RectTransform>();

				if (player_ != null) {
					// hacky way to get a color
					selectorTransform_.GetComponentInChildren<Image>().color = GameConstants.Instance.PlayerSkins[player_.Index()].BodyColor;
				}

				if (startSelectable != null) {
					CurrentSelectable_ = startSelectable;
				} else {
					DefaultSelectableMarker defaultSelectableMarker = elementsContainer.GetComponentInChildren<DefaultSelectableMarker>();
					if (defaultSelectableMarker != null) {
						CurrentSelectable_ = defaultSelectableMarker.GetComponent<ISelectable>();
					} else {
						CurrentSelectable_ = selectables_[0];
					}
				}
			});
		}

		private void Update() {
			if (inputDevices_ == null) {
				return;
			}

			foreach (InputDevice inputDevice in inputDevices_) {
				UpdateMovement(inputDevice);
			}

			if (inputDevices_.Any(i => InputUtil.WasPositivePressedFor(i))) {
				OnSelectableSelected.Invoke(currentSelectable_);
				currentSelectable_.HandleSelected();
			}
		}

		private void UpdateMovement(InputDevice inputDevice) {
			delay_ -= Time.deltaTime;

			bool resetDelay = false;
			if (delay_ <= 0.0f && Mathf.Abs(inputDevice.LeftStick.X) > kIntentThreshold) {
				// placeholder
				if (inputDevice.LeftStick.X > 0) {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.xMax <= other.xMin);
				} else {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.xMin >= other.xMax);
				}

				resetDelay = true;
			}

			if (delay_ <= 0.0f && Mathf.Abs(inputDevice.LeftStick.Y) > kIntentThreshold) {
				// placeholder
				if (inputDevice.LeftStick.Y > 0) {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.yMax <= other.yMin);
				} else {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.yMin >= other.yMax);
				}

				resetDelay = true;
			}

			if (resetDelay) {
				delay_ = kMoveDelay;
			}
		}

		private ISelectable GetBestSelectableFor(Func<Rect, Rect, bool> predicate) {
			Rect currentRect = GetRectFor(currentSelectable_);
			return selectables_.Where(s => s != currentSelectable_)
							   .Where(s => predicate.Invoke(currentRect, GetRectFor(s)))
							   .Min(s => Vector2.Distance(currentRect.position, GetRectFor(s).position));
		}

		private static readonly Vector3[] corners_ = new Vector3[4];
		private Rect GetRectFor(ISelectable selectable) {
			((RectTransform)((MonoBehaviour)selectable).transform).GetWorldCorners(corners_);

			return new Rect((Vector2)corners_[0], (Vector2)corners_[2] - (Vector2)corners_[0]);
		}

		private void RefreshSelectorPosition() {
			MonoBehaviour selectableMonoBehaviour = (currentSelectable_ as MonoBehaviour);

			Vector3[] fourCorners = new Vector3[4];
			RectTransform selectableTransform = selectableMonoBehaviour.GetComponent<RectTransform>();
			selectableTransform.GetLocalCorners(fourCorners);

			// NOTE (darren): god this sucks... (setting the parent)
			// maybe we just won't animate the selector :(
			selectorTransform_.SetParent(selectableMonoBehaviour.transform.parent, worldPositionStays: false);
			selectorTransform_.anchorMin = selectableTransform.anchorMin;
			selectorTransform_.anchorMax = selectableTransform.anchorMax;
			selectorTransform_.sizeDelta = selectableTransform.sizeDelta + kPadding;
			selectorTransform_.pivot = selectableTransform.pivot;
			selectorTransform_.anchoredPosition = selectableTransform.anchoredPosition + Vector2.Scale(kPadding, (selectableTransform.pivot - new Vector2(0.5f, 0.5f)));

			AudioConstants.Instance.ScrollClick.PlaySFX(volumeScale: 0.7f);
			OnSelectorMoved.Invoke();
		}
	}
}