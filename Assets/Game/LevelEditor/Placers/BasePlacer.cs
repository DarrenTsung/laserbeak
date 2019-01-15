using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public abstract class BasePlacer : MonoBehaviour, IPlacer, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IPlacer Implementation
		void IPlacer.Init(GameObject prefab, DynamicArenaData dynamicArenaData, UndoHistory undoHistory, InputDevice inputDevice, LevelEditor levelEditor, Action<GameObject> instanceInitialization, IList<AttributeData> attributeDatas) {
			if (prefab == null) {
				Debug.LogWarning("Cannot set preview object of null object!");
				return;
			}

			dynamicArenaData_ = dynamicArenaData;
			undoHistory_ = undoHistory;
			inputDevice_ = inputDevice;
			attributeDatas_ = attributeDatas;

			levelEditor_ = levelEditor;
			levelEditor_.Cursor.OnMoved += HandleCusorMoved;

			CleanupCurrentPlacable();

			placablePrefab_ = prefab;
			previewObject_ = ObjectPoolManager.Create(prefab, parent: this.gameObject);
			previewObject_.transform.localPosition = Vector3.zero;
			foreach (var collider in previewObject_.GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}

			if (instanceInitialization != null) {
				instanceInitialization.Invoke(previewObject_);
			}

			HandlePreviewObjectCreated();
			Initialize();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			CleanupCurrentPlacable();

			if (levelEditor_ != null) {
				if (levelEditor_.Cursor != null) {
					levelEditor_.Cursor.OnMoved -= HandleCusorMoved;
				}
				levelEditor_ = null;
			}

			Cleanup();
		}


		// PRAGMA MARK - Internal
		private DynamicArenaData dynamicArenaData_;
		private UndoHistory undoHistory_;
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;

		private GameObject placablePrefab_;
		private GameObject previewObject_;

		private IList<AttributeData> attributeDatas_;

		protected DynamicArenaData DynamicArenaData_ { get { return dynamicArenaData_; } }
		protected UndoHistory UndoHistory_ { get { return undoHistory_; } }
		protected InputDevice InputDevice_ { get { return inputDevice_; } }
		protected LevelEditor LevelEditor_ { get { return levelEditor_; } }

		protected GameObject PlacablePrefab_ { get { return placablePrefab_; } }
		protected GameObject PreviewObject_ { get { return previewObject_; } }

		protected abstract int SerializeToDynamicData();

		protected void Place() {
			int uniqueId = SerializeToDynamicData();
			bool hasAttributes = attributeDatas_ != null && attributeDatas_.Count > 0;
			if (hasAttributes) {
				bool validUniqueId = uniqueId > 0;
				if (validUniqueId) {
					foreach (var attributeData in attributeDatas_) {
						AttributeDynamicArenaDataWriter.Serialize(attributeData, uniqueId, DynamicArenaData_);
					}
				} else {
					Debug.LogWarning("Cannot add attributes to invalid uniqueId - will not add!");
				}
			}

			UndoHistory_.RecordState();
		}

		protected virtual void HandleCusorMoved() {
			// stub
		}

		protected virtual void HandlePreviewObjectCreated() {
			// stub
		}

		protected virtual void Initialize() {
			// stub
		}

		protected virtual void Cleanup() {
			// stub
		}

		private void CleanupCurrentPlacable() {
			placablePrefab_ = null;
			if (previewObject_ != null) {
				GameObject.Destroy(previewObject_);
				previewObject_ = null;
			}
		}
	}
}