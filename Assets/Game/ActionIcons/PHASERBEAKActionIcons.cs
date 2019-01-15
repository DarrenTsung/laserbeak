using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;

namespace DT.Game {
	public static class PHASERBEAKActionIcons {
		// PRAGMA MARK - Internal
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			// Keyboard
			ActionIconLetter.RegisterMapping(InputType.Keyboard, ActionType.Positive, "N", IconBorderType.Square, textColor: Color.white);
			ActionIconLetter.RegisterMapping(InputType.Keyboard, ActionType.Negative, "M", IconBorderType.Square, textColor: Color.white);
			ActionIconLetter.RegisterMapping(InputType.Keyboard, ActionType.Command, "Esc", IconBorderType.Square, textColor: Color.white);

			// Xbox Controller
			ActionIconLetter.RegisterMapping(InputType.XBoxController, ActionType.Positive, "A", IconBorderType.Circle, textColor: Color.white);
			ActionIconLetter.RegisterMapping(InputType.XBoxController, ActionType.Negative, "B", IconBorderType.Circle, textColor: Color.white);
			ActionIconLetter.RegisterMapping(InputType.XBoxController, ActionType.Command, "START", IconBorderType.Circle, textColor: Color.white);
		}
	}
}