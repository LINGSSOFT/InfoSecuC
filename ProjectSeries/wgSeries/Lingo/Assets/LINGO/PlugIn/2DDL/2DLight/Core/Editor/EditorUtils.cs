namespace DynamicLight2D
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.IO;
	using UnityEngine;
	using UnityEditor;
	
	public class EditorUtils : Editor {
		

		
		private static Texture2D headerTexture;
		private static Font headerFont;

		
		
		public static string getMainRelativepath () {
			return CoreUtils.MainPath ();
		}

		public static void ListOfFilesUnderPath (string path, ref FileInfo[] filesInfo, string filter = "*.*")
		{
			//string myPath = string.Concat(EditorUtils.getMainRelativepath(), "AddOns");
			
			DirectoryInfo dir = new DirectoryInfo(path);
			filesInfo = dir.GetFiles("*.cs");
			/*foreach (FileInfo f in filesInfo) 
			{
				Debug.Log(f.FullName);
			}
			*/
		}

		public static Texture2D MakeTex(int width, int height, Color c)
		{

			Color[] pix = new Color[width * height];
			for( int i = 0; i < pix.Length; ++i )
			{
				pix[ i ] = c;
			}
			Texture2D result = new Texture2D( width, height );
			result.SetPixels( pix );
			result.Apply();
			return result;

		}

		public static Texture2D HeaderTexture()
		{
			if(headerTexture == null)
				headerTexture = (Texture2D) AssetDatabase.LoadAssetAtPath(System.String.Concat(EditorUtils.getMainRelativepath(), "2DLight/Misc/Textures/2DDL_logo.png"), typeof(Texture2D));

			return headerTexture;
		}

		public static Font HeaderFont()
		{
			if(headerFont == null)
				headerFont = (Font) AssetDatabase.LoadAssetAtPath(System.String.Concat(EditorUtils.getMainRelativepath(), "2DLight/Misc/Fonts/EXTRAVAGANZZA.ttf"), typeof(Font));
			
			return headerFont;
		}

	}
}