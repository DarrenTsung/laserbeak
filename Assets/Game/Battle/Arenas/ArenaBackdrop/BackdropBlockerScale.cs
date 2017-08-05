using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.Game.Battle {
	public class BackdropBlockerScale : MonoBehaviour, IBackdropBlocker {
		// PRAGMA MARK - IBackdropBlocker Implementation
		float IBackdropBlocker.Width { get { return this.transform.parent.localScale.x; } }
		float IBackdropBlocker.Height { get { return this.transform.parent.localScale.z; } }
	}
}