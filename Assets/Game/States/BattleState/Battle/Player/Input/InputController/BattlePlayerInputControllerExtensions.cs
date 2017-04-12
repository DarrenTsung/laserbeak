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
			controller.DisableInput(BattlePlayerInputController.PriorityKey.Movement);
			Vector3 startPosition = player.Rigidbody.position;
			Vector3 lastPosition = startPosition;
			var coroutine = CoroutineWrapper.DoEaseFor(duration, easeType, (float p) => {
				Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, p);
				player.Rigidbody.MovePosition(new Vector3(newPosition.x, player.Rigidbody.position.y, newPosition.z));

				Vector3 xzVelocity = newPosition - lastPosition;
				player.Rigidbody.velocity = new Vector3(xzVelocity.x, player.Rigidbody.velocity.y, xzVelocity.z);
			}, () => {
				controller.EnableInput(BattlePlayerInputController.PriorityKey.Movement);
			});
			controller.RegisterAnimatedMovement(coroutine);
		}
	}
}