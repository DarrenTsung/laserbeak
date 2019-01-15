using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Demos {
	public class DemoElement : MonoBehaviour {
		// PRAGMA MARK - Internal
		private void Awake() {
			#if DEMO
			this.gameObject.SetActive(true);
			#else
			this.gameObject.SetActive(false);
			#endif
		}
	}
}