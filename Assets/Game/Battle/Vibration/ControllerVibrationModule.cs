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
		private const float kKnockbackVibrationAmount = 0.6f;
		private const float kKnockbackVibrationDuration = 0.2f;

		private const float kDeathVibrationAmount = 1.0f;
		private const float kDeathVibrationDuration = 0.5f;

		private const float kFullyChargedVibrationAmount = 0.6f;
		private const float kFullyChargedVibrationDuration = 0.2f;

		private const float kPlayerJoinedVibrationAmount = 0.45f;
		private const float kPlayerJoinedVibrationDuration = 0.2f;

		private static readonly Dictionary<InputDevice, CoroutineWrapper> vibrationCoroutineMap_ = new Dictionary<InputDevice, CoroutineWrapper>();

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			BattlePlayerHealth.OnBattlePlayerKnockbacked += HandleBattlePlayerKnockbacked;
			BattlePlayerHealth.OnBattlePlayerDied += HandleBattlePlayerDied;
			BattlePlayerInputChargedLaser.OnFullyCharged += HandleBattlePlayerFullyCharged;
			GameNotifications.OnPlayerJoinedGame.AddListener(HandlePlayerJoinedGame);
		}

		private static void HandleBattlePlayerKnockbacked(BattlePlayer battlePlayer) {
			Player player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.Input == null) {
				return;
			}

			VibrateInputDevice(player.Input, kKnockbackVibrationAmount, kKnockbackVibrationDuration);
		}

		private static void HandleBattlePlayerDied(BattlePlayer battlePlayer) {
			Player player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.Input == null) {
				return;
			}

			VibrateInputDevice(player.Input, kDeathVibrationAmount, kDeathVibrationDuration);
		}

		private static void HandleBattlePlayerFullyCharged(BattlePlayer battlePlayer) {
			Player player = PlayerSpawner.GetPlayerFor(battlePlayer);
			if (player == null || player.Input == null) {
				return;
			}

			VibrateInputDevice(player.Input, kFullyChargedVibrationAmount, kFullyChargedVibrationDuration);
		}

		private static void HandlePlayerJoinedGame(Player player) {
			if (player == null || player.Input == null) {
				return;
			}

			VibrateInputDevice(player.Input, kPlayerJoinedVibrationAmount, kPlayerJoinedVibrationDuration);
		}

		private static void VibrateInputDevice(IInputWrapper input, float vibrationAmount, float duration) {
			var inputWrapperDevice = input as InputWrapperDevice;
			if (inputWrapperDevice == null) {
				return;
			}

			InputDevice inputDevice = inputWrapperDevice.InputDevice;
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