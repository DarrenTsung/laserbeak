using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.ElementSelection {
	public class CallFunctionWhenSelected : MonoBehaviour, ISelectable {
		// PRAGMA MARK - ISelectable Implementation
		void ISelectable.HandleSelected() {
			event_.Invoke();
		}


		// PRAGMA MARK - Internal
		[SerializeField]
		private UnityEvent event_;
	}
}