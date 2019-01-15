using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTObjectPoolManager;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.Players;
using DT.Game.Transitions;

namespace DT.Game {
	public class CornerDelayedActionView : DelayedActionView {
		// PRAGMA MARK - Static Public Interface
		public static CornerDelayedActionView Show(string actionText, CornerPoint cornerPoint, ActionType actionType, Action finishedCallback) {
			var view = ObjectPoolManager.CreateView<CornerDelayedActionView>(GamePrefabs.Instance.DelayedActionViewPrefab);
			view.InitCorner(actionText, cornerPoint, actionType, finishedCallback);
			return view;
		}


		// PRAGMA MARK - Internal
		[Header("Corner Outlets")]
		[SerializeField]
		private TransitionScreenDirection screenDirectionTransition_;
		[SerializeField]
		protected Image backdropImage_;

		[Space]
		[SerializeField]
		private LayoutGroup mainLayoutGroup_;

		private void InitCorner(string actionText, CornerPoint cornerPoint, ActionType actionType, Action finishedCallback) {
			// corner point adjustments
			var rectTransform = this.transform as RectTransform;
			rectTransform.anchorMin = cornerPoint.ToVector2();
			rectTransform.anchorMax = cornerPoint.ToVector2();
			rectTransform.pivot = cornerPoint.ToVector2();
			rectTransform.anchoredPosition = Vector2.zero;

			bool isLeft = cornerPoint.IsLeft();
			backdropImage_.transform.localScale = backdropImage_.transform.localScale.SetX(isLeft ? 1.0f : -1.0f);
			fillImage_.transform.localScale = fillImage_.transform.localScale.SetX(isLeft ? 1.0f : -1.0f);

			if (screenDirectionTransition_ != null) {
				screenDirectionTransition_.SetInDirection(isLeft ? Direction.LEFT : Direction.RIGHT);
				screenDirectionTransition_.SetOutDirection(isLeft ? Direction.LEFT : Direction.RIGHT);
			}

			bool isBottom = cornerPoint.IsBottom();
			backdropImage_.transform.localScale = backdropImage_.transform.localScale.SetY(isBottom ? 1.0f : -1.0f);
			fillImage_.transform.localScale = fillImage_.transform.localScale.SetY(isBottom ? 1.0f : -1.0f);

			mainLayoutGroup_.padding.left = isLeft ? 0 : 30;
			mainLayoutGroup_.padding.right = isLeft ? 30 : 0;

			Init(actionText, actionType, finishedCallback);
		}
	}
}