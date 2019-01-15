using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTEasings;
using DTLocalization;
using DTObjectPoolManager;
using InControl;
using TMPro;

using DT.Game.Players;

namespace DT.Game.ElementSelection {
	public class ElementSelectionView : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public event Action OnSelectorMoved = delegate {};
		public event Action<ISelectable> OnSelectableHover = delegate {};
		public event Action<ISelectable> OnSelectableSelected = delegate {};

		public void Init(Player player, GameObject elementsContainer, ISelectable startSelectable = null) {
			player_ = player;
			inputs_ = new List<IInputWrapper>() { player_.Input };

			Init(elementsContainer, startSelectable);
		}

		public void Init(IEnumerable<IInputWrapper> inputs, GameObject elementsContainer, ISelectable startSelectable = null) {
			inputs_ = inputs;

			Init(elementsContainer, startSelectable);
		}

		public void SetPaused(bool paused) {
			paused_ = paused;
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			CleanupSelectorAnimationCoroutine();

			if (selectorTransform_ != null) {
				ObjectPoolManager.Recycle(selectorTransform_.gameObject);
				selectorTransform_ = null;
			}

			player_ = null;
			inputs_ = null;
			paused_ = false;

			Localization.OnCultureChanged -= HandleLocalizationChanged;
		}


		// PRAGMA MARK - Internal
		private static readonly Vector2 kPadding = new Vector2(8, 8);
		private const float kIntentThreshold = 0.3f;

		private const float kMoveDelay = 0.16f;
		private const float kSelectorAnimationDuration = 0.1f;

		private RectTransform selectorTransform_;
		private Player player_;

		private IEnumerable<IInputWrapper> inputs_ = null;
		private ISelectable[] selectables_;
		private CoroutineWrapper selectorAnimationCoroutine_;

		private float delay_ = 0.0f;
		private bool paused_ = false;

		private ISelectable currentSelectable_;
		private ISelectable CurrentSelectable_ {
			get { return currentSelectable_; }
		}

		private void SetCurrentSelectable(ISelectable selectable, bool animate = true) {
			if (selectable == null) {
				return;
			}

			currentSelectable_ = selectable;
			OnSelectableHover.Invoke(currentSelectable_);

			RefreshSelectorPosition(animate);
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
				selectorTransform_ = ObjectPoolManager.CreateView(GamePrefabs.Instance.SelectorPrefab, viewManager: GameViewManagerLocator.Selector).GetComponent<RectTransform>();

				if (player_ != null) {
					// hacky way to get a color
					selectorTransform_.GetComponentInChildren<Image>().color = GameConstants.Instance.PlayerSkins[player_.Index()].UIColor;
				}

				if (startSelectable != null) {
					SetCurrentSelectable(startSelectable, animate: false);
				} else {
					DefaultSelectableMarker defaultSelectableMarker = elementsContainer.GetComponentInChildren<DefaultSelectableMarker>();
					if (defaultSelectableMarker != null) {
						SetCurrentSelectable(defaultSelectableMarker.GetComponent<ISelectable>(), animate: false);
					} else {
						SetCurrentSelectable(selectables_[0], animate: false);
					}
				}

				Localization.OnCultureChanged += HandleLocalizationChanged;
			});
		}

		private void Update() {
			if (paused_) {
				return;
			}

			if (inputs_ == null) {
				return;
			}

			delay_ -= Time.deltaTime;

			foreach (IInputWrapper input in inputs_) {
				UpdateMovement(input);
			}

			float horizontal = (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.LeftArrow) ? -1.0f : 0.0f);
			float vertical = (Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.DownArrow) ? -1.0f : 0.0f);
			UpdateMovement(horizontal, vertical);

			if (inputs_.Any(input => input.PositiveWasPressed)) {
				OnSelectableSelected.Invoke(currentSelectable_);
				currentSelectable_.HandleSelected();
			}
		}

		private void UpdateMovement(IInputWrapper input) {
			UpdateMovement(input.MovementVector.x, input.MovementVector.y);
		}

		private void UpdateMovement(float xMovement, float yMovement) {
			bool resetDelay = false;
			if (delay_ <= 0.0f && Mathf.Abs(xMovement) > kIntentThreshold) {
				// placeholder
				if (xMovement > 0) {
					SetCurrentSelectable(GetBestSelectableFor((r, other) => r.xMax <= other.xMin));
				} else {
					SetCurrentSelectable(GetBestSelectableFor((r, other) => r.xMin >= other.xMax));
				}

				resetDelay = true;
			}

			if (delay_ <= 0.0f && Mathf.Abs(yMovement) > kIntentThreshold) {
				// placeholder
				if (yMovement > 0) {
					SetCurrentSelectable(GetBestSelectableFor((r, other) => r.yMax <= other.yMin));
				} else {
					SetCurrentSelectable(GetBestSelectableFor((r, other) => r.yMin >= other.yMax));
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
							   .MinBy(s => Vector2.Distance(currentRect.position, GetRectFor(s).position));
		}

		private static readonly Vector3[] corners_ = new Vector3[4];
		private Rect GetRectFor(ISelectable selectable) {
			((RectTransform)((MonoBehaviour)selectable).transform).GetWorldCorners(corners_);

			return new Rect((Vector2)corners_[0], (Vector2)corners_[2] - (Vector2)corners_[0]);
		}

		private void HandleLocalizationChanged() {
			this.DoAfterFrame(() => {
				this.DoAfterFrame(() => {
					RefreshSelectorPosition(animate: false);
				});
			});
		}

		private void CleanupSelectorAnimationCoroutine() {
			if (selectorAnimationCoroutine_ != null) {
				selectorAnimationCoroutine_.Cancel();
				selectorAnimationCoroutine_ = null;
			}
		}

		private void RefreshSelectorPosition(bool animate) {
			CleanupSelectorAnimationCoroutine();

			MonoBehaviour selectableMonoBehaviour = (currentSelectable_ as MonoBehaviour);

			Vector3[] fourCorners = new Vector3[4];
			RectTransform selectableTransform = selectableMonoBehaviour.GetComponent<RectTransform>();
			selectableTransform.GetWorldCorners(fourCorners);

			Vector2 targetStartPosition = (Vector2)selectableTransform.position;

			// relies on XY coordinate space
			Vector2 targetPosition = new Vector2(fourCorners.Average(v => v.x), fourCorners.Average(v => v.y));

			selectorTransform_.GetWorldCorners(fourCorners);
			Vector2 currentPosition = new Vector2(fourCorners.Average(v => v.x), fourCorners.Average(v => v.y));

			float scaleFactor = GameViewManagerLocator.Selector.transform.localScale.x;
			Vector2 startPosition = selectorTransform_.anchoredPosition;
			Vector2 endPosition = startPosition + ((targetPosition - currentPosition) / scaleFactor);

			Vector2 startSize = selectorTransform_.sizeDelta;
			Vector2 endSize = selectableTransform.sizeDelta + kPadding;

			AudioConstants.Instance.ScrollClick.PlaySFX(volumeScale: 0.7f);
			OnSelectorMoved.Invoke();
			if (animate) {
				selectorAnimationCoroutine_ = CoroutineWrapper.DoEaseFor(kSelectorAnimationDuration, EaseType.QuadraticEaseOut, (p) => {
					Vector2 currentTargetPosition = (Vector2)selectableTransform.position;
					Vector2 targetOffsetPosition = currentTargetPosition - targetStartPosition;
					selectorTransform_.anchoredPosition = Vector2.Lerp(startPosition, endPosition + targetOffsetPosition, p);
					selectorTransform_.sizeDelta = Vector2.Lerp(startSize, endSize, p);
				});
			} else {
				selectorTransform_.anchoredPosition = endPosition;
				selectorTransform_.sizeDelta = endSize;
			}
		}
	}
}