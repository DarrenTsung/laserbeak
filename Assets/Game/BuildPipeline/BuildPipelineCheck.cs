using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace DT.Game.BuildPipelineFlow {
	public static class BuildPipelineCheck {
		// PRAGMA MARK - Internal
		// NOTE (darren): Fail to compile if missing BUILT_WITH_BUILD_PIPELINE
		// please use or expand to BuildPipeline.cs for you building needs
		#if !UNITY_EDITOR && !BUILT_WITH_BUILD_PIPELINE
		private static void CompileError() {
			PleaseBuildWithTheBuildPipelineMenuItems();
		}
		#endif
	}
}