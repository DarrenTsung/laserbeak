using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.ElementSelection;

namespace DT.Game {
	public class MenuItem : MonoBehaviour, ISelectable {
		// PRAGMA MARK - Public Interface
		public void Init(string name, Action callback) {
			nameText_.Text = name;
			callback_ = callback;
		}


		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			if (callback_ != null) {
				callback_.Invoke();
			}
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private TextOutlet nameText_;

		private Action callback_;
	}
}