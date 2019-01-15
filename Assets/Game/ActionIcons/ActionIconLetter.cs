using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;

namespace DT.Game {
	public static class ActionIconLetter {
		// PRAGMA MARK - Public Interface
		public static void RegisterMapping(InputType inputType, ActionType actionType, string text, IconBorderType borderType, Color textColor) {
			ActionIcons.RegisterHandler(inputType, new ActionIconLetterHandler(actionType, text, borderType, textColor));
		}


		private class ActionIconLetterHandler : BaseActionIconHandler {
			// PRAGMA MARK - Public Interface
			public override void Populate(GameObject container) {
				var view = ObjectPoolManager.Create<ActionIconLetterView>(GamePrefabs.Instance.ActionIconLetterView, parent: container);
				view.Init(text_, borderType_, textColor_);
			}

			public ActionIconLetterHandler(ActionType actionType, string text, IconBorderType borderType, Color textColor) : base(actionType) {
				text_ = text;
				borderType_ = borderType;
				textColor_ = textColor;
			}


			// PRAGMA MARK - Internal
			private string text_;
			private IconBorderType borderType_;
			private Color textColor_;
		}
	}
}