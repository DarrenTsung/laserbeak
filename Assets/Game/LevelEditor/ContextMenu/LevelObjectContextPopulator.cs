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
	public class LevelObjectContextPopulator : IScrollableMenuPopulator {
		// PRAGMA MARK - Public Interface
		public LevelObjectContextPopulator(LevelEditor levelEditor) {
			levelEditor_ = levelEditor;
		}


		// PRAGMA MARK - IScrollableMenuPopulator Implementation
		List<ScrollableMenuItem> IScrollableMenuPopulator.GetItems() {
			var items = new List<ScrollableMenuItem>();
			foreach (GameObject levelObjectPrefab in GamePrefabs.Instance.LevelEditorObjects) {
				items.Add(new ScrollableMenuItem(thumbnail: null, name: levelObjectPrefab.name, callback: () => {
					AttributeLevelEditorObjectSetter.CheckAttributesAndSetObject(levelEditor_, levelObjectPrefab);
				}));
			}
			return items;
		}

		void IScrollableMenuPopulator.Dispose() {}


		// PRAGMA MARK - Internal
		private LevelEditor levelEditor_;
	}
}