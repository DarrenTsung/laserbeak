using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DT.Game.Battle.Players;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Players {
	public static class PlayerUtil {
		// PRAGMA MARK - Public Interface
		public static void Sort<T>(this List<T> list, Func<T, Vector2> pointTransformation) {
			list.Sort((T a, T b) => {
				Vector2 aPoint = pointTransformation.Invoke(a);
				Vector2 bPoint = pointTransformation.Invoke(b);

				if (aPoint.y != bPoint.y) {
					// higher y -> first
					return bPoint.y.CompareTo(aPoint.y);
				}

				// lower x -> first
				return aPoint.x.CompareTo(bPoint.x);
			});
		}
	}
}