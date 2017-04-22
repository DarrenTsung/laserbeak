using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DT.Game.Players;
using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game.ElementSelection {
	public interface ISelectionViewDelegate {
		void HandleSelectionView(ElementSelectionView view);
	}
}