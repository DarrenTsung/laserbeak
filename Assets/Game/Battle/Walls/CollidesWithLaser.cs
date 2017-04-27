using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Walls {
	public class CollidesWithLaser : MonoBehaviour {
		// PRAGMA MARK - Internal
		private void OnTriggerEnter(Collider collider) {
			Laser laser = collider.gameObject.GetComponentInParent<Laser>();
			if (laser == null) {
				return;
			}

			laser.Ricochet(-this.transform.right);
		}
	}
}