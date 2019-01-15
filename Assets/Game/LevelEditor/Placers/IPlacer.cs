using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public interface IPlacer {
		void Init(GameObject prefab, DynamicArenaData dynamicArenaData, UndoHistory undoHistory, InputDevice inputDevice, LevelEditor levelEditor, Action<GameObject> instanceInitialization, IList<AttributeData> attributeDatas);
	}
}