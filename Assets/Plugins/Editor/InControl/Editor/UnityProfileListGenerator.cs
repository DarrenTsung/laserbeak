#if UNITY_EDITOR
namespace InControl
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEngine;


	[InitializeOnLoad]
	internal class UnityProfileListGenerator
	{
		static UnityProfileListGenerator()
		{
			DiscoverProfiles();
		}


		static void DiscoverProfiles()
		{
			var unityInputDeviceProfileType = typeof( UnityInputDeviceProfile );
			var autoDiscoverAttributeType = typeof( InControl.AutoDiscover );

			var code2 = "";
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (type.IsSubclassOf( unityInputDeviceProfileType ))
					{
						var typeAttrs = type.GetCustomAttributes( autoDiscoverAttributeType, false );
						if (typeAttrs != null && typeAttrs.Length > 0)
						{
							code2 += "\t\t\t\"" + type.FullName + "\"," + Environment.NewLine;
						}
					}
				}
			}

			var instance = ScriptableObject.CreateInstance<UnityInputDeviceProfileList>();
			var filePath = AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( instance ) );
			ScriptableObject.DestroyImmediate( instance );

			string code1 = @"namespace InControl
{
	using UnityEngine;


	public class UnityInputDeviceProfileList : ScriptableObject
	{
		public static string[] Profiles = new string[] 
		{
";

			string code3 = @"		};
	}
}";

			var code = FixNewLines( code1 + code2 + code3 );
			if (PutFileContents( filePath, code ))
			{
				Debug.Log( "InControl has updated the Unity profiles list." );
			}
		}


		static string GetFileContents( string fileName )
		{
			StreamReader streamReader = new StreamReader( fileName );
			var fileContents = streamReader.ReadToEnd();
			streamReader.Close();

			return fileContents;
		}


		static bool PutFileContents( string filePath, string content )
		{
			var oldContent = GetFileContents( filePath );
			if (CompareIgnoringWhitespace( content, oldContent ))
			{
				return false;
			}

			StreamWriter streamWriter = new StreamWriter( filePath );
			streamWriter.Write( content );
			streamWriter.Flush();
			streamWriter.Close();

			return true;
		}


		static string FixNewLines( string text )
		{
			return Regex.Replace( text, @"\r\n|\n", Environment.NewLine );
		}


		static bool CompareIgnoringWhitespace( string s1, string s2 )
		{
			return Regex.Replace( s1, @"\s", "" ) == Regex.Replace( s2, @"\s", "" );
		}
	}
}
#endif

