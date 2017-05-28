using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

using NUnit.Framework;
using UnityEngine.TestTools;

using DTValidator;

namespace DT.Game.Tests {
	public static class ValidateSavedScenes {
		[Test]
		public static void Validate() {
			IList<IValidationError> errors = ValidationUtil.ValidateAllGameObjectsInSavedScenes(earlyExitOnError: true);
			if (errors.Count > 0) {
				foreach (IValidationError error in errors) {
					Debug.Log("Found scene validation error: " + error + "!");
				}
			}

			Assert.That(errors, Is.Empty);
		}
	}
}
