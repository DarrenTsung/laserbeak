using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public static class LevelEditorConstants {
		public const float kArenaWidth = 24.0f;
		public const float kArenaLength = 20.0f;

		public const float kArenaHalfWidth = kArenaWidth / 2.0f;
		public const float kArenaHalfLength = kArenaLength / 2.0f;

		public const float kGridSize = 1.0f;
		public const float kHalfGridSize = kGridSize / 2.0f;
	}
}