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
	public class RotatesContinuously : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private float rotationSpeed_ = 10.0f;

		private Vector3 rotationDirection_;

		private void Awake() {
			rotationDirection_ = UnityEngine.Random.onUnitSphere;
		}

		private void Update() {
			this.transform.localRotation = Quaternion.Euler(rotationSpeed_ * rotationDirection_ * Time.deltaTime) * this.transform.localRotation;
		}
	}
}