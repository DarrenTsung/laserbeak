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
	public static class AttributeContextMenuFactory {
		// PRAGMA MARK - Public Interface
		public static void CreateMenu(Type attributeType, AttributeData attribute, Action callback) {
			if (!menuItemFactoryMap_.ContainsKey(attributeType)) {
				Debug.LogWarning("Cannot create menu for attributeType: " + attributeType + " skipping!");
				callback.Invoke();
			}

			var menuItemFactory = menuItemFactoryMap_[attributeType];
			var menuItems = menuItemFactory.Invoke(attribute, callback);
			ScrollableMenuPopup.Show(LevelEditor.InputDevice, menuItems);
		}


		// PRAGMA MARK - Internal
		private delegate IList<ScrollableMenuItem> AttributeMenuFactory(AttributeData attribute, Action callback);
		private static Dictionary<Type, AttributeMenuFactory> menuItemFactoryMap_ = new Dictionary<Type, AttributeMenuFactory>() {
			{ typeof(WaveAttributeData), (attribute, callback) => {
				var menuItems = new List<ScrollableMenuItem>();
				for (int i = 1; i <= GameConstants.Instance.MaxNumberOfWaves; i++) {
					// NOTE (darren): this is necessary to capture the current state of i
					// in a local variable otherwise all callbacks will reference i which changed
					int waveId = i;
					menuItems.Add(new ScrollableMenuItem(thumbnail: null, name: string.Format("Wave {0}", waveId), callback: () => {
						((WaveAttributeData)attribute).WaveId = waveId;
						callback.Invoke();
					}));
				}
				return menuItems;
			}}
		};
	}
}