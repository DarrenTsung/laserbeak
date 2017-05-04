using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle.Players;
using DT.Game.Players;

namespace DT.Game.Battle.Vibration {
	public static class ControllerVibrationModule {
		// PRAGMA MARK - Static
		private const float kDamageVibrationAmount = 0.6f;
		private const float kDamageVibrationDuration = 0.2f;

		private const float kDeathVibrationAmount = 1.0f;
		private const float kDeathVibrationDuration = 0.5f;

		private static readonly Dictionary<InputDevice, CoroutineWrapper> vibrationCoroutineMap_ = new Dictionary<InputDevice, CoroutineWrapper>();

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			BattlePlayerHealth.OnBattlePlayerDamaged += HandleBattlePlayerDamaged;
			BattlePlayerHealth.OnBattlePlayerDied += HandleBattlePlayerDied;
		}

		private static void HandleBattlePlayerDamaged(BattlePlayer battlePlayer, int damage) {
			Player player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.InputDevice == null) {
				return;
			}

			VibrateInputDevice(player.InputDevice, kDamageVibrationAmount, kDamageVibrationDuration);
		}

		private static void HandleBattlePlayerDied(BattlePlayer battlePlayer) {
			Player player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.InputDevice == null) {
				return;
			}

			VibrateInputDevice(player.InputDevice, kDeathVibrationAmount, kDeathVibrationDuration);
		}

		private static void VibrateInputDevice(InputDevice inputDevice, float vibrationAmount, float duration) {
			if (vibrationCoroutineMap_.ContainsKey(inputDevice)) {
				vibrationCoroutineMap_[inputDevice].Cancel();
			}

			inputDevice.Vibrate(vibrationAmount);
			vibrationCoroutineMap_[inputDevice] = CoroutineWrapper.DoAfterDelay(duration, () => {
				inputDevice.StopVibration();
			});
		}
	}
}