using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game {
	public interface IEventRouter {
		void Subscribe(UnityAction unityAction);
		void Unsubscribe(UnityAction unityAction);
	}

	public interface IEventRouter<R1> {
		void Subscribe(UnityAction<R1> unityAction);
		void Unsubscribe(UnityAction<R1> unityAction);
	}
}