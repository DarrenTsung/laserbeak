#if UNITY_EDITOR
namespace InControl
{
	using System.IO;
	using iOS.Xcode;
	using UnityEditor;
	using UnityEditor.Callbacks;
	using UnityEngine;


	public class ICadePostProcessor
	{
		static readonly string[] sourceFiles = new[] {
			"ICadeManager.h",
			"ICadeManager.m",
			"ICadeReaderView.h",
			"ICadeReaderView.m"
		};


		[PostProcessBuild]
		public static void OnPostProcessBuild( BuildTarget buildTarget, string buildPath )
		{
#if UNITY_5
			if (buildTarget == BuildTarget.iOS)
#else
			if (buildTarget == BuildTarget.iPhone)
#endif
			{
				var projPath = PBXProject.GetPBXProjectPath( buildPath );
				PBXProject proj = new PBXProject();
				proj.ReadFromString( File.ReadAllText( projPath ) );
				var targetGuid = proj.TargetGuidByName( "Unity-iPhone" );

				var instance = ScriptableObject.CreateInstance<ICadePluginPath>();
				var pluginPath = Path.GetDirectoryName( AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( instance ) ) );
				ScriptableObject.DestroyImmediate( instance );

				var targetPath = Path.Combine( "Libraries", pluginPath.Substring( 7 ) );
				Directory.CreateDirectory( Path.Combine( buildPath, targetPath ) );

				foreach (var fileName in sourceFiles)
				{
					var sourcePath = Path.Combine( pluginPath, fileName );
					var targetFile = Path.Combine( targetPath, fileName );
					File.Copy( sourcePath, Path.Combine( buildPath, targetFile ), true );
					proj.AddFileToBuild( targetGuid, proj.AddFile( targetFile, targetFile, PBXSourceTree.Source ) );
				}

				File.WriteAllText( projPath, proj.WriteToString() );
			}
		}
	}
}
#endif

