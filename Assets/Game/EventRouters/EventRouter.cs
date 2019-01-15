using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;
using TMPro;

namespace DT.Game {
	// Pass-through class to conform to the interface
	public class EventRouter : IEventRouter {
		// PRAGMA MARK - IEventRouter Implementation
		void IEventRouter.Subscribe(UnityAction unityAction) {
			event_.AddListener(unityAction);
		}

		void IEventRouter.Unsubscribe(UnityAction unityAction) {
			event_.RemoveListener(unityAction);
		}


		// PRAGMA MARK - Public Interface
		public EventRouter(UnityEvent unityEvent) {
			event_ = unityEvent;
		}


		// PRAGMA MARK - Internal
		private UnityEvent event_;
	}

	public class EventRouter<U> : IEventRouter {
		// PRAGMA MARK - IEventRouter Implementation
		void IEventRouter.Subscribe(UnityAction unityAction) {
			eventU_.AddListener(RouterListener);
			event_.AddListener(unityAction);
		}

		void IEventRouter.Unsubscribe(UnityAction unityAction) {
			eventU_.RemoveListener(RouterListener);
			event_.RemoveListener(unityAction);
		}


		// PRAGMA MARK - Public Interface
		public EventRouter<U> WithPredicate(Predicate<U> predicate) {
			predicate_ = predicate;
			return this;
		}

		public EventRouter(UnityEvent<U> eventU) {
			eventU_ = eventU;
		}


		// PRAGMA MARK - Internal
		private UnityEvent<U> eventU_;

		private UnityEvent event_ = new UnityEvent();
		private Predicate<U> predicate_;

		private void RouterListener(U arg1) {
			if (!predicate_.Invoke(arg1)) {
				return;
			}

			event_.Invoke();
		}
	}

	public class EventRouter<U, V> : IEventRouter {
		// PRAGMA MARK - IEventRouter Implementation
		void IEventRouter.Subscribe(UnityAction unityAction) {
			eventUV_.AddListener(RouterListener);
			event_.AddListener(unityAction);
		}

		void IEventRouter.Unsubscribe(UnityAction unityAction) {
			eventUV_.RemoveListener(RouterListener);
			event_.RemoveListener(unityAction);
		}


		// PRAGMA MARK - Public Interface
		public EventRouter<U, V> WithPredicate(Func<U, V, bool> predicate) {
			predicate_ = predicate;
			return this;
		}

		public EventRouter(UnityEvent<U, V> eventUV) {
			eventUV_ = eventUV;
		}


		// PRAGMA MARK - Internal
		private UnityEvent<U, V> eventUV_;

		private UnityEvent event_ = new UnityEvent();
		private Func<U, V, bool> predicate_;

		private void RouterListener(U arg1, V arg2) {
			if (!predicate_.Invoke(arg1, arg2)) {
				return;
			}

			event_.Invoke();
		}
	}
}