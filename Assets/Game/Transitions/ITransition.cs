using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public interface ITransition {
		void Animate(TransitionType transitionType, float delay, Action<ITransition> callback);
	}
}