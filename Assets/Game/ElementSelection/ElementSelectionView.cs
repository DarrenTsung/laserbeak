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

			foreach (var del in elementsContainer.GetComponentsInChildren<ISelectionViewDelegate>()) {
				del.HandleSelectionView(this);
			}

			selectables_ = elementsContainer.GetComponentsInChildren<ISelectable>();
			if (selectables_.Length <= 0) {
				Debug.LogError("ElementSelectionView - needs selectables??");
			}

			CoroutineWrapper.DoAtEndOfFrame(() => {
				selectorTransform_ = ObjectPoolManager.CreateView(GamePrefabs.Instance.SelectorPrefab).GetComponent<RectTransform>();

				// hacky way to get a color
				selectorTransform_.GetComponentInChildren<Image>().color = GameConstants.Instance.PlayerSkins[player_.Index()].BodyColor;

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


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectorTransform_ != null) {
				ObjectPoolManager.Recycle(selectorTransform_.gameObject);
				selectorTransform_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private static readonly Vector2 kPadding = new Vector2(8, 8);
		private const float kIntentThreshold = 0.3f;

		private const float kMoveDelay = 0.16f;

		private RectTransform selectorTransform_;
		private Player player_;

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

		private void Update() {
			if (InputUtil.WasPositivePressedFor(player_.InputDevice)) {
				OnSelectableSelected.Invoke(currentSelectable_);
				currentSelectable_.HandleSelected();
			}

			UpdateMovement();
		}

		private void UpdateMovement() {
			delay_ -= Time.deltaTime;

			if (delay_ <= 0.0f && Mathf.Abs(player_.InputDevice.LeftStick.X) > kIntentThreshold) {
				// placeholder
				if (player_.InputDevice.LeftStick.X > 0) {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.xMax <= other.xMin);
				} else {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.xMin >= other.xMax);
				}

				delay_ = kMoveDelay;
			}

			if (delay_ <= 0.0f && Mathf.Abs(player_.InputDevice.LeftStick.Y) > kIntentThreshold) {
				// placeholder
				if (player_.InputDevice.LeftStick.Y > 0) {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.yMax <= other.yMin);
				} else {
					CurrentSelectable_ = GetBestSelectableFor((r, other) => r.yMin >= other.yMax);
				}

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

			return new Rect(corners_[0].Vector2XZValue(), corners_[2].Vector2XZValue() - corners_[0].Vector2XZValue());
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