using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public static class LevelEditorUtil {
		public static Vector3 SnapToGridCenter(Vector3 position) {
			Vector3 newPosition = position;
			newPosition = newPosition.SetY(0.0f);
			int clampedX = MathUtil.Clamp((int)newPosition.x, (int)-LevelEditorConstants.kArenaHalfWidth + 1, (int)LevelEditorConstants.kArenaHalfWidth - 1);
			int clampedZ = MathUtil.Clamp((int)newPosition.z, (int)-LevelEditorConstants.kArenaHalfHeight + 1, (int)LevelEditorConstants.kArenaHalfHeight - 1);

			float newX = clampedX;
			float newZ = clampedZ;
			if (clampedX != 0) {
				newX += (clampedX > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newX = position.x >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}
			if (clampedZ != 0) {
				newZ += (clampedZ > 0.0f) ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			} else {
				newZ = position.z >= 0.0f ? LevelEditorConstants.kHalfGridSize : -LevelEditorConstants.kHalfGridSize;
			}

			newPosition = newPosition.SetX(newX);
			newPosition = newPosition.SetZ(newZ);

			return newPosition;
		}

		public static Vector3 SnapToVertex(Vector3 position) {
			Vector3 newPosition = position;

			newPosition = newPosition.SetY(0.0f);
			newPosition = newPosition.SetX(Mathf.Round(newPosition.x));
			newPosition = newPosition.SetZ(Mathf.Round(newPosition.z));

			return newPosition;
		}
	}
}