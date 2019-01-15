using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

#if UNITY_EDITOR
using UnityEditor;
#endif

using DT.Game.Battle;

namespace DT.Game.LevelEditor {
	public class LevelEditorMenu {
		// PRAGMA MARK - Public Interface
		public LevelEditorMenu(InputDevice inputDevice, Action exitCallback, Action saveCallback, Action<ArenaConfig> editArenaCallback) {
			inputDevice_ = inputDevice;
			exitCallback_ = exitCallback;
			saveCallback_ = saveCallback;
			editArenaCallback_ = editArenaCallback;

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		public void Dispose() {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
			HideMenu();
		}


		// PRAGMA MARK - Internal
		private InputDevice inputDevice_;
		private Action exitCallback_;
		private Action saveCallback_;
		#pragma warning disable 0414 // this is used in the UNITY_EDITOR block
		private Action<ArenaConfig> editArenaCallback_;
		#pragma warning restore 0414

		private void HandleUpdate() {
			if (inputDevice_.Command.WasPressed && !MenuView.Showing) {
				MenuView.Show(new InputWrapperDevice(inputDevice_), "LEVEL EDITOR", new Dictionary<string, Action>() {
					{ "BACK TO EDITING", HideMenu },
					{ "LOAD", ShowLoadMenu },
					{ "SAVE", () => {
						saveCallback_.Invoke();
						HideMenu();
					} },
					{ "EXIT EDITING", () => exitCallback_.Invoke() },
				});
			}
		}

		private void HideMenu() {
			MenuView.Hide();
		}

		private void ShowLoadMenu() {
			MenuView.Hide();

			var levelOpenActions = new Dictionary<string, Action>();
			#if UNITY_EDITOR
			var arenaConfigs = AssetDatabaseUtil.AllAssetsOfType<ArenaConfig>();
			foreach (var arenaConfig in arenaConfigs) {
				levelOpenActions[arenaConfig.name] = () => {
					editArenaCallback_.Invoke(arenaConfig);
					HideMenu();
				};
			}
			#endif
			// TODO (darren): support loading from custom levels saved outside of editor
			MenuView.Show(new InputWrapperDevice(inputDevice_), "LOAD LEVEL TO EDIT", levelOpenActions);
		}
	}
}