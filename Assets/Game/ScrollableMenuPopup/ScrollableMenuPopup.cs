using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;

namespace DT.Game.ScrollableMenuPopups {
	public class ScrollableMenuItem {
		public Sprite Thumbnail;
		public string Name;
		public Action Callback;

		public ScrollableMenuItem(Sprite thumbnail, string name, Action callback) {
			Thumbnail = thumbnail;
			Name = name;
			Callback = callback;
		}
	}

	public class ScrollableMenuPopup : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Static
		public static ScrollableMenuPopup Create(InputDevice inputDevice, IList<ScrollableMenuItem> items) {
			var popup = ObjectPoolManager.CreateView<ScrollableMenuPopup>(GamePrefabs.Instance.ScrollableMenuPopupPrefab);
			popup.Init(inputDevice, items);
			return popup;
		}



		// PRAGMA MARK - Public Interface
		public event Action OnHidden = delegate {};

		public void Init(InputDevice inputDevice, IList<ScrollableMenuItem> items) {
			foreach (var item in items) {
				var view = ObjectPoolManager.Create<ScrollableMenuItemView>(GamePrefabs.Instance.ScrollableMenuItemViewPrefab, parent: layoutRectTransform_.gameObject);
				view.Init(item, onCallbackInvoked: HideMenu);
			}

			selectionView_ = ObjectPoolManager.CreateView<ElementSelectionView>(GamePrefabs.Instance.ElementSelectionViewPrefab);
			selectionView_.OnSelectableHover += HandleNewSelection;
			selectionView_.Init(new InputDevice[] { inputDevice }, layoutRectTransform_.gameObject);
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (selectionView_ != null) {
				selectionView_.OnSelectableHover -= HandleNewSelection;
				ObjectPoolManager.Recycle(selectionView_);
				selectionView_ = null;
			}

			layoutRectTransform_.RecycleAllChildren();
		}


		// PRAGMA MARK - Internal
		private const float kLerpDuration = 0.5f;

		[Header("Outlets")]
		[SerializeField]
		private RectTransform layoutRectTransform_;

		[SerializeField]
		private RectTransform viewportRectTransform_;

		private ElementSelectionView selectionView_;

		// cache memory since not threading
		private static Vector3[] selectableCorners_ = new Vector3[4];
		private static Vector3[] viewportCorners_ = new Vector3[4];
		private void HandleNewSelection(ISelectable selectable) {
			RectTransform selectionRectTransform = (selectable as MonoBehaviour).transform as RectTransform;
			selectionRectTransform.GetWorldCorners(selectableCorners_);
			viewportRectTransform_.GetWorldCorners(viewportCorners_);

			Rect selectionRect = ConvertToRect(selectableCorners_);
			Rect viewportRect = ConvertToRect(viewportCorners_);

			if (selectionRect.yMax > viewportRect.yMax) {
				LerpLayoutYTranslation(viewportRect.yMax - selectionRect.yMax);
			} else if (selectionRect.yMin < viewportRect.yMin) {
				LerpLayoutYTranslation(viewportRect.yMin - selectionRect.yMin);
			}
		}

		private void LerpLayoutYTranslation(float yDistance) {
			this.StopAllCoroutines();

			Vector2 startPosition = layoutRectTransform_.anchoredPosition;
			Vector2 endPosition = startPosition.AddY(yDistance);

			this.DoEaseFor(kLerpDuration, EaseType.QuadraticEaseOut, (float p) => {
				layoutRectTransform_.anchoredPosition = Vector2.Lerp(startPosition, endPosition, p);
			});
		}

		private Rect ConvertToRect(Vector3[] corners) {
			return new Rect((Vector2)corners[0], (Vector2)corners[2] - (Vector2)corners[0]);
		}

		private void HideMenu() {
			ObjectPoolManager.Recycle(this);
			OnHidden.Invoke();
		}
	}
}