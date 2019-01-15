using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.ScrollableMenuPopups;

namespace DT.Game.LevelEditor {
	public static class AttributeLevelEditorObjectSetter {
		// PRAGMA MARK - Public Interface
		public static void CheckAttributesAndSetObject(LevelEditor levelEditor, GameObject levelObjectPrefab) {
			if (levelObjectPrefab_ != null) {
				Debug.LogWarning("Cannot check attributes and set object when already setting attributes for some other prefab: " + levelObjectPrefab_);
				return;
			}

			levelEditor_ = levelEditor;
			levelObjectPrefab_ = levelObjectPrefab;

			var attributeMarkers = levelObjectPrefab_.GetComponentsInChildren<IAttributeMarker>();
			if (attributeMarkers == null || attributeMarkers.Length <= 0) {
				FinishSettingObject();
				return;
			}

			attributeDatas_ = attributeMarkers.Select(m => (AttributeData)Activator.CreateInstance(m.AttributeType)).ToArray();
			index_ = 0;

			FillAttributeMarkers();
		}


		// PRAGMA MARK - Internal
		private static LevelEditor levelEditor_;
		private static GameObject levelObjectPrefab_;

		private static AttributeData[] attributeDatas_;
		private static int index_ = 0;

		private static void FillAttributeMarkers() {
			// finished populating attribute markers
			if (index_ >= attributeDatas_.Length) {
				FinishSettingObject();
				return;
			}

			var attribute = attributeDatas_[index_];
			Type attributeType = attribute.GetType();

			index_++;
			AttributeContextMenuFactory.CreateMenu(attributeType, attribute, callback: FillAttributeMarkers);
		}

		private static void FinishSettingObject() {
			if (levelObjectPrefab_ == null) {
				Debug.LogWarning("Cannot finish LevelEditorObjectPlacer process without prefab!");
				return;
			}

			levelEditor_.SetObjectToPlace(levelObjectPrefab_, attributeDatas: attributeDatas_);
			levelObjectPrefab_ = null;
			attributeDatas_ = null;
			index_ = 0;
		}
	}
}