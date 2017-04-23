using System;
using System.Collections;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle.Players {
	 public static class BattlePlayerInputControllerExtensions {
		// PRAGMA MARK - Public Interface
		public static void MoveTo(this BattlePlayerInputController controller, BattlePlayer player, Vector3 endPosition, float duration, EaseType easeType) {
			controller.DisableInput(BattlePlayerInputController.PriorityKey.Movement);
			Vector3 startPosition = player.Rigidbody.position;
			Vector3 lastPosition = startPosition;
			player.Rigidbody.AddForce((endPosition - startPosition) * 10, ForceMode.Impulse);
			var coroutine = CoroutineWrapper.DoEaseFor(duration, easeType, (float p) => {
				// Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, p);
				// player.Rigidbody.velocity = newPosition - player.Rigidbody.position;
			}, () => {
				// player.Rigidbody.velocity = Vector3.zero;
				controller.EnableInput(BattlePlayerInputController.PriorityKey.Movement);
				controller.CancelAnyAnimatedMovements();
			});
			controller.RegisterAnimatedMovement(coroutine);
		}
	}
}