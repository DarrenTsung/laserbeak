using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.GameModes;
using DT.Game.Players;

namespace DT.Game.Stats {
	public class StatView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void Init(Player player, string statName, string statValue, bool showMarker) {
			if (player.Skin == null) {
				Debug.LogWarning("StatView - player: " + player + " has no skin!");
			} else {
				Color color = player.Skin.BodyColor;
				markerObject_.color = color;
				bottomBarImage_.color = color;
				statNameText_.Color = color;
				statValueText_.Color = color;
			}

			markerObject_.gameObject.SetActive(showMarker);

			statNameText_.Text = statName;
			statValueText_.Text = statValue;
		}


		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Image markerObject_;
		[SerializeField]
		private TextOutlet statNameText_;
		[SerializeField]
		private TextOutlet statValueText_;
		[SerializeField]
		private Image bottomBarImage_;
	}
}