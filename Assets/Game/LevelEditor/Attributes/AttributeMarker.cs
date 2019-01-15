using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

using DT.Game.LevelEditor;
using DT.Game.ScrollableMenuPopups;

namespace DT.Game {
	public abstract class AttributeMarker<T> : MonoBehaviour, IAttributeMarker, IRecycleSetupSubscriber where T : AttributeData {
		// PRAGMA MARK - IAttributeMarker Implementation
		Type IAttributeMarker.AttributeType {
			get { return typeof(T); }
		}

		void IAttributeMarker.SetAttribute(AttributeData attributeData) {
			T castedAttributeData = attributeData as T;
			if (castedAttributeData == null) {
				Debug.LogWarning("Cannot SetAttribute with incorrect type of attributeData: " + attributeData + ", expected: " + typeof(T).Name);
				return;
			}

			SetAttribute(castedAttributeData);
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			attributeData_ = null;
		}


		// PRAGMA MARK - Internal
		// NOTE (darren): uncomment to inspect attributeData but keep in mind that
		// serializing this field makes it non-null + the warnings for replacing the AttributeData_
		// will be hit
		// [SerializeField, ReadOnly]
		private T attributeData_ = null;

		protected T GetAttribute() {
			if (attributeData_ == null) {
				Debug.LogWarning("AttributeData_ is not set - GetAttribute will fail!");
			}
			return attributeData_;
		}

		private void SetAttribute(T attributeData) {
			if (attributeData_ != null) {
				Debug.LogWarning("AttributeData_ is already set!");
			}
			attributeData_ = attributeData;
		}
	}
}