using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.Game.Battle {
	public interface IBackdropBlocker {
		float Width { get; }
		float Height { get; }
	}
}