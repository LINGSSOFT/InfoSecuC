namespace DynamicLight2D
{
	#if UNITY_EDITOR
	
	using UnityEngine;
	using UnityEditor;
	using Object = UnityEngine.Object;
	using System.Collections;
	
	
	
	public class DynamicLightAboutWindow : EditorWindow {
		
		DynamicLightAboutWindow  window;
		//public Rect _handleArea;
		private bool _nodeOption, _options, _handleActive, _action;
		private Texture2D _rawIcon;
		private GUIContent _icon;
		private float _winMinX, _winMinY;
		private int _mainwindowID;

		Texture2D headerTexture;
		Font headerFont;

		GUIStyle style1;
		GUIStyle style0;
		GUIStyle headerStyle;
		
		// Updates variables
		static string assetFileName = "CheckUpdateAssetFile.asset";
		static string updateUrlAsset = "http://martinysa.com/MyFiles/2DDL/Updates/"; //"http://martinysa.com/MyFiles/2DDL/Updates/DynamicLightServerAsset.asset";
		bool isWWWDone = false;
		WWW remoteLogUpdates;
		static bool _init;
		
		bool doAction = false;
		
		bool enableBtn = false;
		string destinyFolder;
		
		SerializedObject profile;
		int framesCount = 0;
		
		UnityEngine.Object asset;
		
		static EditorWindow _window;
		
		private struct checkVersionStruct{
			public string version;
			public string dataReleased;
			public string changeLog;
			public string link;
		}
		
		private checkVersionStruct _chVersion;

		[MenuItem("2DDL/About",false, 100)]
		public static void Init()
		{

			
			//window = (DynamicLightAboutWindow )EditorWindow.GetWindow(typeof(DynamicLightAboutWindow));
			_window = EditorWindow.GetWindow( typeof(DynamicLightAboutWindow), true, "2DDL about" );
			_window.minSize = new Vector2 (500,380);
			
			
		}
		
		void OnEnable(){
			_init = true;
			headerTexture = EditorUtils.HeaderTexture(); 
			headerFont = EditorUtils.HeaderFont();
			StartDownload();
		}
		
		void OnFocus(){
			//EditorApplication.update += editorCallbackUpdate;
		}
		
		void OnLostFocus(){//
			//EditorApplication.update -= editorCallbackUpdate;
		}
		
		void OnDisable(){
			EditorApplication.update -= editorCallbackUpdate;
			bool b = FileUtil.DeleteFileOrDirectory("Assets/" + assetFileName);
			//if (System.IO.File.Exists (destinyFolder)) {
			//	System.IO.File.Delete(destinyFolder);
			//}
			AssetDatabase.Refresh ();
			Debug.Log ("Ondestroy" + b);
		}
		
		
		
		void OnGUI()
		{
			
			if (_init) {
				_init = false;
				//StartDownload();
			}

			style1 = new GUIStyle(GUI.skin.label);
			style1.fontSize = 14;
			style1.fontStyle = FontStyle.Bold;
			style1.alignment = TextAnchor.MiddleLeft;
			
			style0 = new GUIStyle(GUI.skin.label);
			style0.fontSize = 10;
			style0.fontStyle = FontStyle.Italic;
			style0.alignment = TextAnchor.UpperLeft;
			style0.fixedHeight = 40f;
			style0.normal.textColor = Color.cyan;
			
			headerStyle = new GUIStyle(GUI.skin.label);
			headerStyle.fontSize = 18;
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = TextAnchor.MiddleLeft;
			headerStyle.fixedHeight = headerTexture.height;
			headerStyle.normal.textColor = Color.white;
			headerStyle.font = headerFont;
			
			
			GUIStyle currentStyle = new GUIStyle( GUI.skin.box );
			currentStyle.normal.background = EditorUtils.MakeTex( 2, 2, new Color( 1f, 112/255f, 4/255f, 1f ) );
			currentStyle.fixedHeight = headerStyle.fixedHeight * 1.2f;
			
			
			// Header //
			EditorGUILayout.BeginHorizontal(currentStyle);
			Rect rect = GUILayoutUtility.GetRect(0f, 0f);
			rect.width = headerTexture.width;
			rect.height = headerTexture.height;
			GUILayout.Space(rect.height);
			GUI.DrawTexture(rect, headerTexture);
			EditorGUILayout.LabelField ("About", headerStyle);
			EditorGUILayout.EndHorizontal();
			
			style1 = new GUIStyle(GUI.skin.label);
			style1.fontStyle = FontStyle.Bold;
			style1.alignment = TextAnchor.MiddleCenter;
			//style1.
			
			//GUILayout.Label(m_Logo);

			GUILayout.Space(25);

			GUILayout.BeginVertical("box");
				
				GUILayout.Label("2DDL PRO: Dynamic Lights and Shadows Effects");
				GUILayout.Label ("Version: " + SettingsManager.getVersion());

			GUILayout.EndVertical();


			GUILayout.BeginHorizontal("box");
				if (GUILayout.Button ("Documentation")) {
					Application.OpenURL("http://martinysa.com/2d-dynamic-lights-doc/");
				}
				if (GUILayout.Button ("AssetStore")) {
					UnityEditorInternal.AssetStore.Open("/content/25933");
				}
				
				if (GUILayout.Button ("Contact")) {
					Application.OpenURL("mailto:info@martinysa.com");
				}
			GUILayout.EndHorizontal();
			
			
			
			// si se ha leido desde server
			if(enableBtn){
				
				LoadCheckUpdateAsset();
				
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				
				//GUILayout.Label("Remote Version");
				if (GUILayout.Button ("Check")) {
					LoadCheckUpdateAsset();
				}
				
				EditorGUILayout.Separator();
				EditorGUILayout.SelectableLabel("Lastest Version:    " + _chVersion.version);
				EditorGUILayout.SelectableLabel("Date Released:    " + _chVersion.dataReleased);
				EditorGUILayout.SelectableLabel("Link:    " + _chVersion.link);

				GUILayout.Space(20);
				EditorGUILayout.BeginHorizontal("box");
					if(GUILayout.Button("See ChangeLog")){
						Application.OpenURL(_chVersion.changeLog);
					}
					
					
					if(GUILayout.Button("Open in AssetStore tab")){
						UnityEditorInternal.AssetStore.Open("/content/25933");
						
					}
				EditorGUILayout.EndHorizontal();

				
			}else{
				string lbl1 = "Checking new version";
				string lbl2 = " .";
				if(framesCount == 0){
					lbl2 = " .";
				}else if(framesCount == 1){
					lbl2 = " ..";
				}else if(framesCount == 2){
					lbl2 = " ...";
				}else if(framesCount == 3){
					lbl2 = " ....";
				}else if(framesCount == 4){
					lbl2 = " .....";
				}else if(framesCount == 5){
					lbl2 = " ......";
					framesCount *=0;
				}
				
				framesCount++;
				
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				
				GUILayout.Label(lbl1 + lbl2);
				Repaint();
			}
			
		}
		
		void StartDownload(){
			EditorApplication.update += editorCallbackUpdate;
			

			
			//Start downloading updates log Asset
			Debug.Log ("donwloadin started");
			// Full path
			updateUrlAsset += assetFileName;
			
			remoteLogUpdates = new WWW (updateUrlAsset);
			
			destinyFolder = Application.dataPath;
			destinyFolder = destinyFolder.Substring(0, destinyFolder.Length - 5);
			destinyFolder = destinyFolder.Substring(0, destinyFolder.LastIndexOf("/"));
			destinyFolder += "/Assets/" + assetFileName;//"/Assets/2DDL/2DLight/CheckUpdate/" + assetFileName;
		}
		
		void editorCallbackUpdate(){
			if (remoteLogUpdates != null && remoteLogUpdates.isDone)
				isWWWDone = true;
			
			if (isWWWDone && !doAction)
				syncUpdateInfo();
			
			
		}
		
		
		void syncUpdateInfo(){
			
			#if !UNITY_WEBPLAYER
			
			doAction = true;
			
			Debug.Log ("downloaded");
			
			Debug.Log ("parsing...");
			
			byte[] data = remoteLogUpdates.bytes;
			
			
			
			//if (System.IO.File.Exists (destinyFolder)) {
			//	System.IO.File.Delete(destinyFolder);
			//}
			System.IO.File.WriteAllBytes(destinyFolder, data);
			
			
			//		System.IO.FileStream fs;
			//		fs = System.IO.File.Create(destinyFolder);
			//		fs.Write(data,1,5);
			//		fs.Close ();
			//		
			
			AssetDatabase.Refresh ();
			
			//AssetDatabase.RemoveUnusedAssetBundleNames ();
			
			enableBtn = true;
			#else
			
			Debug.LogWarning("Can't check updates under Web Player platform. Change your project to another target in File -> Build Settings");
			#endif
			
			
			
		}
		
		void LoadCheckUpdateAsset(){
			asset = AssetDatabase.LoadAssetAtPath<CheckUpdateAsset> ("Assets/" + assetFileName);
			Debug.Log (destinyFolder);
			
			if (asset == null) {
				Debug.LogError("Can't load Updates Log File");
				return;
			}
			
			Debug.Log(asset);			
						
			profile = new SerializedObject(asset);
			
			if (profile != null) {
				Debug.Log("Profile loaded");
				
				_chVersion.version = AssetUtility.LoadProperty("version", profile);
				_chVersion.dataReleased = AssetUtility.LoadProperty("dateReleased", profile);
				_chVersion.changeLog = AssetUtility.LoadProperty("changeLog", profile);
				_chVersion.link = AssetUtility.LoadProperty("link", profile);
			}
			
			
		}
		
	}
	
	#endif
}
