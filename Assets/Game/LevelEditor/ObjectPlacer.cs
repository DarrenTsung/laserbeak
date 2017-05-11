using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class ObjectPlacer : MonoBehaviour, IRecycleCleanupSubscriber {
		public void Init(DynamicArenaData dynamicArenaData, InputDevice inputDevice, LevelEditorCursor cursor) {
			dynamicArenaData_ = dynamicArenaData;
			inputDevice_ = inputDevice;

			cursor_ = cursor;
			cursor_.OnMoved += RefreshPosition;

			RefreshPosition();
		}

		public void SetObjectToPlace(GameObject prefab) {
			if (prefab == null) {
				Debug.LogWarning("Cannot set preview object of null object!");
				return;
			}

			placablePrefab_ = prefab;
			previewObject_ = ObjectPoolManager.Create(prefab, parent: this.gameObject);
			foreach (var collider in previewObject_.GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (previewObject_ != null) {
				GameObject.Destroy(previewObject_);
				previewObject_ = null;
			}

			if (cursor_ != null) {
				cursor_.OnMoved -= RefreshPosition;
				cursor_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private DynamicArenaData dynamicArenaData_;
		private InputDevice inputDevice_;
		private LevelEditorCursor cursor_;
		private GameObject placablePrefab_;
		private GameObject previewObject_;

		private void Update() {
			if (placablePrefab_ == null) {
				return;
			}

			if (inputDevice_.Action3.WasReleased) {
				dynamicArenaData_.SerializeObject(placablePrefab_, this.transform.position, Quaternion.identity, this.transform.localScale);
			}
		}

		private void RefreshPosition() {
			// snap onto grid - assume preview object is 1x1 for now
			Vector3 newPosition = cursor_.transform.position;
			newPosition = newPosition.SetY(0.0f);
			int clampedX = MathUtil.Clamp((int)newPosition.x, (int)-LevelEditorConstants.kArenaHalfWidth + 1, (int)LevelEditorConstants.kArenaHalfWidth - 1);
			int clampedZ = MathUtil.Clamp((int)newPosition.z, (int)-LevelEditorConstants.kArenaHalfHeight + 1, (int)LevelEditorConstants.kArenaHalfHeight - 1);

			float newX = clampedX;
			float newZ = clampedZ;
			if (clampedX != 0) {
				newX += (clampedX > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newX = cursor_.transform.position.x >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}
			if (clampedZ != 0) {
				newZ += (clampedZ > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newZ = cursor_.transform.position.z >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}

			newPosition = newPosition.SetX(newX);
			newPosition = newPosition.SetZ(newZ);

			if (this.transform.position == newPosition) {
				return;
			}

			this.transform.position = newPosition;
			RefreshPreviewObjectValidity();
		}

		private void RefreshPreviewObjectValidity() {
			if (previewObject_ == null) {
				return;
			}

			Vector3 halfExtents = this.transform.localScale / 2.0f;
			halfExtents = halfExtents.SetX(halfExtents.x - 0.1f);
			halfExtents = halfExtents.SetZ(halfExtents.z - 0.1f);

			bool hit = Physics.BoxCast(this.transform.position + Vector3.up, halfExtents: halfExtents, direction: -Vector3.up, orientation: Quaternion.identity, maxDistance: Mathf.Infinity, layerMask: InGameConstants.PlatformsLayerMask);
			foreach (var renderer in previewObject_.GetComponentsInChildren<Renderer>()) {
				renderer.material.color = hit ? Color.red : Color.green;
			}
		}
	}
}