using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace DT.Game {
	public static class GameNotifications {
		// PRAGMA MARK - Static Public Interface
		public static UnityEvent OnGameWon = new UnityEvent();
	}
}