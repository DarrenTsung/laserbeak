using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

using DTAnimatorStateMachine;
using DTCommandPalette.ScriptingDefines;
using DTEasings;
using DTLocalization;

namespace DT.Game {
	public class BuildFlow {
		// PRAGMA MARK - Public Interface
		public static void PreBuildQA() {
			PreBuild();
			ScriptingDefinesManager.AddDefineIfNotFound("DEBUG");
		}

		public static void PreBuildRelease() {
			PreBuild();
			ScriptingDefinesManager.RemoveDefine("DEBUG");
		}

		public static void PreBuildReleaseDemo() {
			PreBuildRelease();
			ScriptingDefinesManager.AddDefineIfNotFound("DEMO");
		}

		[UnityEditor.MenuItem("BuildPipeline/BuildRun QA - Mac")]
		public static void BuildAndRunQAMac() {
			PreBuildQA();
			BuildAndRun(gameName: "PHASERBEAK - Mac QA", buildTarget: BuildTarget.StandaloneOSXUniversal, buildOptions: BuildOptions.None);
			PostBuild();
		}

		[UnityEditor.MenuItem("BuildPipeline/BuildRun Release - Mac")]
		public static void BuildAndRunReleaseMac() {
			PreBuildRelease();
			BuildAndRun(gameName: "PHASERBEAK - Mac Release", buildTarget: BuildTarget.StandaloneOSXUniversal, buildOptions: BuildOptions.None);
			PostBuild();
		}

		[UnityEditor.MenuItem("BuildPipeline/BuildRun Release Demo - Mac")]
		public static void BuildAndRunReleaseDemoMac() {
			PreBuildReleaseDemo();
			BuildAndRun(gameName: "PHASERBEAK - Mac Release (Demo)", buildTarget: BuildTarget.StandaloneOSXUniversal, buildOptions: BuildOptions.None);
		}

		[UnityEditor.MenuItem("BuildPipeline/BuildRun Release - Windows")]
		public static void BuildAndRunReleaseWindows() {
			PreBuildRelease();
			BuildAndRun(gameName: "PHASERBEAK - Windows Release", buildTarget: BuildTarget.StandaloneWindows, buildOptions: BuildOptions.None);
			PostBuild();
		}


		// PRAGMA MARK - Internal
		private static string[] previousScriptingDefines_ = null;

		private static void BuildAndRun(string gameName, BuildTarget buildTarget, BuildOptions buildOptions) {
			string buildPath = Path.Combine(Application.dataPath.Replace("Assets", ""), "Builds");
			string gamePath = Path.Combine(buildPath, gameName);
			string[] scenes = new string[] {"Assets/Main.unity" };
			string error = BuildPipeline.BuildPlayer(scenes, gamePath, buildTarget, buildOptions);

			if (buildTarget == BuildTarget.StandaloneOSXUniversal) {
				var process = new System.Diagnostics.Process();
				process.StartInfo.FileName = gamePath + ".app";
				process.Start();
			}

			if (!string.IsNullOrEmpty(error)) {
				Debug.LogWarning("Error building: " + gameName + " | error: " + error + "!");
			} else {
				Debug.Log("Finished building: " + gameName + " successfully!");
			}
		}

		private static void PreBuild() {
			previousScriptingDefines_ = ScriptingDefinesManager.GetCurrentDefines();
			LocalizationOfflineCache.CacheBundledLocalizationTables();
			TMPLocalization.DownloadAndBakeAllUsedLocalizationCharactersIntoFonts();

			ScriptingDefinesManager.AddDefineIfNotFound("BUILT_WITH_BUILD_PIPELINE");
		}

		private static void PostBuild() {
			EditorApplication.delayCall += () => {
				if (previousScriptingDefines_ != null) {
					ScriptingDefinesManager.ResetDefinesTo(previousScriptingDefines_.Where(d => d != "BUILT_WITH_BUILD_PIPELINE").ToArray());
					previousScriptingDefines_ = null;
				}
			};
		}
	}
}
