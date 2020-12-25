using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PostProcessMethods{
	
	static bool chk = false;
	public static void checkIconTexture() {
			
		if (!chk && !File.Exists("Assets/Gizmos/logo2DDL_gizmos.png")) {
			
			chk = true;
			
			string folderPath = "Assets/Gizmos";
			if(! Directory.Exists(folderPath)){
				Directory.CreateDirectory (folderPath);
				//Debug.Log("folder does not exist");
			}
			
			string oldPath = DynamicLight2D.CoreUtils.MainPath() + "2DLight/Misc/logo2DDL_gizmos.png";
			
			string newPath = folderPath +  "/logo2DDL_gizmos.png";
			
			FileUtil.CopyFileOrDirectory (oldPath, newPath);
		}
	}
}


