using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DT.Game.Battle.Players;
using DT.Game.ElementSelection;
using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.PlayerCustomization.Skin {
	public class PopulateWithSkinSelectables : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			foreach (BattlePlayerSkin skin in GameConstants.Instance.PlayerSkins) {
				SkinSelectable skinSelectable = ObjectPoolManager.Create<SkinSelectable>(GamePrefabs.Instance.SkinSelectablePrefab, parent: this.gameObject);
				skinSelectable.Init(skin);
			}
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			this.transform.RecycleAllChildren();
		}
	}
}