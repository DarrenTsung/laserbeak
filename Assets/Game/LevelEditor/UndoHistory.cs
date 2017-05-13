using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.LevelEditor {
	public class UndoHistory {
		// PRAGMA MARK - Public Interface
		public UndoHistory(DynamicArenaData data, InputDevice inputDevice) {
			data_ = data;
			inputDevice_ = inputDevice;

			RecordState();

			MonoBehaviourWrapper.OnUpdate += HandleUpdate;
		}

		public void Dispose() {
			MonoBehaviourWrapper.OnUpdate -= HandleUpdate;
		}

		public void Undo() {
			if (serializedDataHistory_.IsEmpty()) {
				return;
			}

			if (index_ <= 0) {
				return;
			}

			index_--;
			ReloadFromIndex();
		}

		public void Redo() {
			if (serializedDataHistory_.IsEmpty()) {
				return;
			}

			if (index_ >= serializedDataHistory_.Count - 1) {
				return;
			}

			index_++;
			ReloadFromIndex();
		}

		public void RecordState() {
			// if index is not last element in history
			// remove all elements past the current index
			if (index_ < serializedDataHistory_.Count - 1) {
				serializedDataHistory_.RemoveRange(index_ + 1, (serializedDataHistory_.Count - 1) - index_);
			}

			serializedDataHistory_.Add(data_.Serialize());
			index_ = serializedDataHistory_.Count - 1;
		}


		// PRAGMA MARK - Internal
		private int index_ = 0;
		private readonly List<string> serializedDataHistory_ = new List<string>();

		private DynamicArenaData data_;
		private InputDevice inputDevice_;

		private void ReloadFromIndex() {
			if (serializedDataHistory_.IsEmpty()) {
				return;
			}

			if (!serializedDataHistory_.ContainsIndex(index_)) {
				Debug.LogWarning("Index out of bounds in serializedDataHistory_! Very weird!!");
			}

			string serializedData = serializedDataHistory_.GetClamped(index_);
			data_.ReloadFromSerialized(serializedData);
		}

		private void HandleUpdate() {
			if (inputDevice_.LeftBumper.WasPressed) {
				Undo();
			}

			if (inputDevice_.RightBumper.WasPressed) {
				Redo();
			}
		}
	}
}