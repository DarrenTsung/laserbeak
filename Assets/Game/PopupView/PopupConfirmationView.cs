using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Players;

namespace DT.Game.Popups {
	public class PopupConfirmationView {
		// PRAGMA MARK - Static
		public static PopupView Create(string message, Player player, Action confirmCallback, Action cancelCallback) {
			return PopupView.Create(message, player, new PopupButtonConfig[] {
				new PopupButtonConfig("YES", confirmCallback, defaultOption: true),
				new PopupButtonConfig("NO", cancelCallback),
			});
		}
	}
}