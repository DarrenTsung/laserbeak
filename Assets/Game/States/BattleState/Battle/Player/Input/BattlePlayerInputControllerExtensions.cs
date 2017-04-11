using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Player {
	 public static class BattlePlayerInputControllerExtensions {
		// PRAGMA MARK - Public Interface
		public static void MoveTo(this BattlePlayerInputController controller, BattlePlayer player, Vector3 endPosition, float duration, EaseType easeType) {
			controller.DisableInput();
			Vector3 startPosition = player.Rigidbody.position;
			var coroutine = CoroutineWrapper.DoEaseFor(duration, easeType, (float p) => {
				player.Rigidbody.MovePosition(Vector3.Lerp(startPosition, endPosition, p));
			}, () => {
				controller.EnableInput();
			});
			controller.RegisterAnimatedMovement(coroutine);
		}
	}
}