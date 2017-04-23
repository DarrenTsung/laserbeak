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
			player.Rigidbody.AddForce((endPosition - startPosition) * 10, ForceMode.Impulse);
			var coroutine = CoroutineWrapper.DoAfterDelay(duration, () => {
				controller.EnableInput(BattlePlayerInputController.PriorityKey.Movement);
				controller.CancelAnyAnimatedMovements();
			});
			controller.RegisterAnimatedMovement(coroutine);
		}
	}
}