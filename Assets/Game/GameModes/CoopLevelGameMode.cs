using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.Battle;
using DT.Game.Battle.Players;
using DT.Game.LevelSelect;
using DT.Game.Players;
using DT.Game.Scoring;

namespace DT.Game.GameModes {
	public class CoopLevelGameMode : GameMode {
		// PRAGMA MARK - Public Interface
		public override string DisplayTitle {
			get { return config_.DisplayName; }
		}

		public override int Id {
			get { return GameMode.GetIdFor<CoopLevelGameMode>(); }
		}

		public void Init(CoopLevelConfig config) {
			config_ = config;
			configArenas_ = new ArenaConfig[] { config.ArenaConfig };
			waves_ = new HashSet<WaveAttributeMarker>[GameConstants.Instance.MaxNumberOfWaves + 1];
		}


		// PRAGMA MARK - Internal
		private CoopLevelConfig config_;
		private ArenaConfig[] configArenas_;

		private int waveIndex_ = 0;
		private HashSet<WaveAttributeMarker>[] waves_;

		protected override void Activate() {
			PlayerSpawner.SpawnAllPlayers();

			List<GameModeIntroView.Icon> icons = new List<GameModeIntroView.Icon>();
			foreach (Player player in RegisteredPlayers.AllPlayers) {
				icons.Add(GameModeIntroView.Icon.Player);
			}
			GameModeIntroView.Show(DisplayTitle, icons);

			PlayerSpawner.OnSpawnedPlayerRemoved += HandleSpawnedPlayerRemoved;
		}

		protected override void CleanupInternal() {
			PlayerSpawner.OnSpawnedPlayerRemoved -= HandleSpawnedPlayerRemoved;

			waveIndex_ = 0;
			foreach (var wave in waves_) {
				if (wave == null) {
					continue;
				}

				wave.Clear();
			}
		}

		protected override IList<ArenaConfig> GetArenasWhitelisted() {
			return configArenas_;
		}

		protected override void HandleArenaLoaded() {
			var waveAttributes = ArenaManager.Instance.LoadedArena.GameObject.GetComponentsInChildren<WaveAttributeMarker>(includeInactive: true);
			foreach (var waveAttribute in waveAttributes) {
				int waveId = waveAttribute.WaveId;
				if (waveId <= 0 || waveId >= waves_.Length) {
					continue;
				}

				waves_.GetValueOrCreateNew(waveId).Add(waveAttribute);
			}

			LoadNextWave();
		}

		private void HandleSpawnedPlayerRemoved() {
			if (PlayerSpawner.AllSpawnedPlayers.Count() > 0) {
				return;
			}

			// Finish with no winners
			Finish();
		}

		private void LoadNextWave() {
			if (waveIndex_ >= waves_.Length) {
				// Finish because all waves are defeated
				Finish();
				return;
			}

			HashSet<WaveAttributeMarker> wave = GetCurrentWave();
			if (wave == null || wave.Count <= 0) {
				waveIndex_++;
				LoadNextWave();
				return;
			}

			// spawn in everything in the wave
			foreach (var waveAttribute in wave) {
				var waveElement = waveAttribute.GetComponentInParent<IWaveElement>(includeInactive: true);
				if (waveElement == null) {
					Debug.LogWarning("Could not find IWaveElement for WaveAttribute: " + waveAttribute + " || inactive components are not found!");
					continue;
				}

				waveElement.Spawn(() => {
					HandleWaveAttributeRemoved(waveAttribute);
				});
			}
		}

		private HashSet<WaveAttributeMarker> GetCurrentWave() {
			return waves_.GetValueOrDefault(waveIndex_);
		}

		private void HandleWaveAttributeRemoved(WaveAttributeMarker waveAttribute) {
			HashSet<WaveAttributeMarker> wave = GetCurrentWave();
			if (wave == null) {
				Debug.LogWarning("No current wave - cannot HandleWaveAttributeRemoved!");
				return;
			}

			bool successful = wave.Remove(waveAttribute);
			if (!successful) {
				Debug.LogWarning("HandleWaveAttributeRemoved - wave attribute not removed??");
			}

			if (wave.Count <= 0) {
				LoadNextWave();
			}
		}
	}
}