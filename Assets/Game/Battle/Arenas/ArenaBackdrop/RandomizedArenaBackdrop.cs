using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;
using DT.Game.Transitions;

namespace DT.Game.Battle {
	public class RandomizedArenaBackdrop : IArenaBackdrop {
		// PRAGMA MARK - Public Interface
		public RandomizedArenaBackdrop(GameObject container) {
			container_ = container;

			List<Rect> placedPlatforms = new List<Rect>();
			for (int i = 0; i < kFadedPlatformCount; i++) {
				bool placed = false;
				for (int j = 0; j < kMaxPlacementTries; j++) {
					float x = UnityEngine.Random.Range(-kBackdropXRange, kBackdropXRange);
					float z = UnityEngine.Random.Range(-kBackdropZRange / 2.0f, kBackdropZRange * 3.0f / 2.0f);

					float width = UnityEngine.Random.Range(kRandomPlatformWidthMin, kRandomPlatformWidthMax);
					float height = UnityEngine.Random.Range(kRandomPlatformHeightMin, kRandomPlatformHeightMax);

					Rect placementRect = RectUtil.MakeRect(new Vector2(x, z), new Vector2(width + kHalfPadding, height + kHalfPadding), pivot: new Vector2(0.5f, 0.5f));

					bool intersects = false;
					foreach (var placedRect in placedPlatforms) {
						if (placementRect.Overlaps(placedRect)) {
							intersects = true;
							break;
						}
					}

					if (intersects) {
						continue;
					}

					float y = UnityEngine.Random.Range(kBackdropYMin, kBackdropYMax);

					// because the further back the z is (towards -kBackdropZRange) the higher it appears
					// to the camera, we want to make sure that the y is not too high
					//
					// when z is -kBackdropZRange, y is subtracted by kBackdropYZInfluence
					float zPercentage = (z + (kBackdropZRange / 2.0f)) / (2.0f * kBackdropZRange);
					y -= (1.0f - zPercentage) * kBackdropYZInfluence;


					placedPlatforms.Add(placementRect);
					GameObject instance = ObjectPoolManager.Create(GamePrefabs.Instance.BackdropPlatformPrefab, position: new Vector3(x, y, z), rotation: Quaternion.identity, parent: container_);
					instance.transform.localScale = instance.transform.localScale.SetX(width).SetZ(height);

					placed = true;
					break;
				}

				// if failed to place container, don't try again
				if (!placed) {
					break;
				}
			}

			transition_ = new Transition(container_).SetShuffledOrder(true);
			transition_.SetOffsetDelay(kAnimateTime / transition_.TransitionCount);

			transition_.AnimateIn();
		}


		// PRAGMA MARK - IArenaBackdrop Implementation
		void IDisposable.Dispose() {
			container_.RecycleAllChildren();
		}

		void IArenaBackdrop.AnimateOut() {
			transition_.AnimateOut();
		}


		// PRAGMA MARK - Internal
		private const float kAnimateTime = 0.3f;

		private const int kBackdropXRange = (int)LevelEditorConstants.kArenaWidth * 3;
		private const int kBackdropZRange = (int)LevelEditorConstants.kArenaLength * 3;

		private const int kBackdropYMin = -60;
		private const int kBackdropYMax = -40;

		private const float kBackdropYZInfluence = 10;

		// padding between the platforms
		private const float kHalfPadding = 1.0f;

		private const int kRandomPlatformWidthMin = 6;
		private const int kRandomPlatformWidthMax = 25;
		private const int kRandomPlatformHeightMin = 6;
		private const int kRandomPlatformHeightMax = 25;

		private const int kMaxPlacementTries = 200;
		private const int kFadedPlatformCount = 15;

		private GameObject container_;

		private Transition transition_;
	}
}