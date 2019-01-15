using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;
using DT.Game.ScrollableMenuPopups;

namespace DT.Game {
	public class WaveAttributeMarker : AttributeMarker<WaveAttributeData> {
		public int WaveId {
			get { return GetAttribute().WaveId; }
		}
	}
}