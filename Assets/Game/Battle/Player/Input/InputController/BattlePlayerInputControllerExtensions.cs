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
		public static void MoveTo(this BattlePlayerInputController controller, BattlePlayer player, Vector3 endPosition, float duration, EaseType easeType, Action onFinishedCallback = null) {
			Vector3 startPosition = player.Rigidbody.position;
			float oldDrag = player.Rigidbody.drag;

			controller.DisableInput(BattlePlayerInputController.PriorityKey.Movement);
			player.Rigidbody.drag = BattlePlayer.kBaseDrag;
			player.Rigidbody.AddForce((endPosition - startPosition) * 10, ForceMode.Impulse);
			var coroutine = CoroutineWrapper.DoAfterDelay(duration, () => {
				player.Rigidbody.drag = oldDrag;
				controller.EnableInput(BattlePlayerInputController.PriorityKey.Movement);
				controller.CancelAnyAnimatedMovements();
				if (onFinishedCallback != null) {
					onFinishedCallback.Invoke();
				}
			});
			controller.RegisterAnimatedMovement(coroutine);
		}
	}
}