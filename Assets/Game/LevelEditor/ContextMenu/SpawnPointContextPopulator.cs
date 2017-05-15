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
	public class SpawnPointContextPopulator : IScrollableMenuPopulator {
		// PRAGMA MARK - Public Interface
		public SpawnPointContextPopulator(InputDevice inputDevice, LevelEditor levelEditor) {
			inputDevice_ = inputDevice;
			levelEditor_ = levelEditor;
		}


		// PRAGMA MARK - IScrollableMenuPopulator Implementation
		List<ScrollableMenuItem> IScrollableMenuPopulator.GetItems() {
			var items = new List<ScrollableMenuItem>();
			items.Add(new ScrollableMenuItem(thumbnail: null, name: "SpawnPoint", callback: () => {
				var spawnPointItems = new List<ScrollableMenuItem>();
				for (int i = 0; i < 4; i++) {
					int playerIndex = i;
					spawnPointItems.Add(new ScrollableMenuItem(thumbnail: null, name: "Player" + playerIndex, callback: () => {
						levelEditor_.SetObjectToPlace(GamePrefabs.Instance.LevelEditorPlayerSpawnPointPrefab, (GameObject g) => {
							g.GetComponent<LevelEditorPlayerSpawnPoint>().SetPlayerIndex(playerIndex);
						});
					}));
				}
				contextMenu_ = ScrollableMenuPopup.Create(inputDevice_, spawnPointItems);
				contextMenu_.GetComponent<RecyclablePrefab>().OnCleanup += ClearContextMenuReferences;
			}));
			return items;
		}

		void IScrollableMenuPopulator.Dispose() {
			if (contextMenu_ != null) {
				ObjectPoolManager.Recycle(contextMenu_);
				contextMenu_ = null;
			}
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private LevelEditor levelEditor_;
		private ScrollableMenuPopup contextMenu_;

		private void ClearContextMenuReferences(RecyclablePrefab unused) {
			contextMenu_.GetComponent<RecyclablePrefab>().OnCleanup -= ClearContextMenuReferences;
			contextMenu_ = null;
		}
	}
}