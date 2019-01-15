using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.ScrollableMenuPopups;

namespace DT.Game.LevelEditor {
	public static class AttributeDynamicArenaDataWriter {
		// PRAGMA MARK - Public Interface
		public static void Serialize(AttributeData attribute, int uniqueId, DynamicArenaData dynamicArenaData) {
			Type attributeType = attribute.GetType();
			if (!writerMap_.ContainsKey(attributeType)) {
				Debug.LogWarning("Cannot serialize attribute of type: " + attributeType);
				return;
			}

			var writer = writerMap_[attributeType];
			writer.Invoke(attribute, uniqueId, dynamicArenaData);
		}


		// PRAGMA MARK - Internal
		private delegate void DataWriter(AttributeData attribute, int uniqueId, DynamicArenaData dynamicArenaData);
		private static Dictionary<Type, DataWriter> writerMap_ = new Dictionary<Type, DataWriter>() {
			{ typeof(WaveAttributeData), (attribute, uniqueId, dynamicArenaData) => {
				var waveAttribute = (WaveAttributeData)attribute;
				dynamicArenaData.SerializeWaveAttribute(uniqueId, waveAttribute.WaveId);
			}}
		};
	}
}