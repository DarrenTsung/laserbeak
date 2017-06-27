using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using DTObjectPoolManager;

namespace DT.Game.Transitions {
	public interface ITransition {
		TransitionType Type {
			get;
		}

		void Animate(Action<ITransition> callback);
	}
}