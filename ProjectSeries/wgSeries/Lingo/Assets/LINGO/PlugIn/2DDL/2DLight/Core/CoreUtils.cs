
#if UNITY_EDITOR
namespace DynamicLight2D
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.IO;
	using UnityEngine;
	using UnityEditor;
	
	public class CoreUtils: ScriptableObject{
		
		public static ScriptableObject coreUtils;
		
		internal static string relativepath = null;
		
		public static string MainPath () {
			
			if(coreUtils == null)
			{
				coreUtils = (ScriptableObject) ScriptableObject.CreateInstance("CoreUtils");
			}
			
			if (relativepath != null)
			{
				return relativepath;
			}else
			{
				
				
				
				MonoScript ms = MonoScript.FromScriptableObject(coreUtils);
				string m_ScriptFilePath = AssetDatabase.GetAssetPath( ms );
				
				
				//If 2DDL FREE
				//string _name = "Scripts/2DLight/Editor/" + Path.GetFileName(m_ScriptFilePath);
				
				//If 2DDL PRO
				string _name = "2DLight/Core/" + Path.GetFileName(m_ScriptFilePath);
				
				Regex rex = new Regex(_name);
				string result = rex.Replace(m_ScriptFilePath, "", 1);
				
				relativepath = result;
				return relativepath;
			}
		}
		
	}
		

	/*
	class PostProcessThings : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			Debug.Log("JUST FINISHED!!!&&");
			//WRITE FILE
			string line = "";

			//#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5  || UNITY_5_6|| UNITY_2017_1
			line = "-define:PAPAPAPA"; //
			//#endif
			// Write the string to a file.
	

			System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/gmcs.rsp");
			file.WriteLine(line);
			file.Close();
			File.SetAttributes(Application.dataPath + "/gmcs.rsp", FileAttributes.Hidden);

			file = new System.IO.StreamWriter(Application.dataPath + "/mcs.rsp");
			file.WriteLine(line);
			file.Close();
			File.SetAttributes(Application.dataPath + "/mcs.rsp", FileAttributes.Hidden);

			file = new System.IO.StreamWriter(Application.dataPath + "/smcs.rsp");
			file.WriteLine(line);
			file.Close();
			File.SetAttributes(Application.dataPath + "/smcs.rsp", FileAttributes.Hidden);



			WorkWithFile(Application.dataPath + "/gmcs.rsp", line);
			WorkWithFile(Application.dataPath + "/mcs.rsp", line);
			WorkWithFile(Application.dataPath + "/smcs.rsp", line);

			//PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "NONONO");//


		}

		static void WorkWithFile(string filePath, string line)
		{
			//if (! File.Exists(filePath))
			//	File.Create(filePath);

			//File.SetAttributes(filePath, FileAttributes.Normal);

			System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);
			file.WriteLine(line);
			file.Close();
			//File.Delete(filePath);
		}


	}
	*/
}
#endif



