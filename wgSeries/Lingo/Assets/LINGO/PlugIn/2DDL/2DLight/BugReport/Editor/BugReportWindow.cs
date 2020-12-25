namespace DynamicLight2D
{
	#if UNITY_EDITOR
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	
	public class BugReportWindow : EditorWindow {
		
		
		SerializedObject settingProfileAsset;
		int selectableLayerField;
		int selectableLayerMask;
		
		GUIStyle headerStyle;
		GUIStyle style0;
		GUIStyle btnStyle;
		GUIStyle btnStyle2;
		
		
		Texture2D headerTexture;
		Font headerFont;



		// REPORT STRUCT //
		struct Report{
			public string title;
			public string description;
			public int enviromentID;
			public string attachPath;
			public string email;
			public string assetVersion;
			public string unityVersion;
			public bool isUnityPro;
			public string buildTarget;
			public string currentOS;
		}

		Report _report;
		string[] _enviromentValues = new string[]{"Editor only", "Runtime only", "Editor and Runtime"};
		string msg;

		bool shouldDraw = true;
		
		static EditorWindow w;

		[MenuItem("2DDL/Submit Bug Report", false, 11)]
		public static void Init()
		{
			w = EditorWindow.GetWindow( typeof(BugReportWindow), true, "Submit Bug Report - 2DDL Pro" );
			w.minSize = new Vector2(500,550);
		}
		
		void OnEnable(){
			
			headerTexture = EditorUtils.HeaderTexture(); //(Texture2D) AssetDatabase.LoadAssetAtPath(System.String.Concat(EditorUtils.getMainRelativepath(), "2DLight/Misc/Textures/2DDL_logo.png"), typeof(Texture2D));
			headerFont = EditorUtils.HeaderFont();// (Font) AssetDatabase.LoadAssetAtPath(System.String.Concat(EditorUtils.getMainRelativepath(), "2DLight/Misc/Fonts/EXTRAVAGANZZA.ttf"), typeof(Font));
				
			EditorApplication.update += update;
		}
		void OnDisable(){
			EditorApplication.update -= update;
		}
		void update(){
			if (mouseOverWindow != null) {
				if (mouseOverWindow.GetType () == typeof(BugReportWindow))
					Repaint ();
			}

		}

		
		void OnGUI(){

			if(!shouldDraw){
				w.Close();
				return;
			}

			headerStyle = new GUIStyle(GUI.skin.label);
			headerStyle.fontSize = 18;
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = TextAnchor.MiddleLeft;
			headerStyle.fixedHeight = headerTexture.height;
			headerStyle.normal.textColor = Color.white;
			headerStyle.font = headerFont;
			
			style0 = new GUIStyle(GUI.skin.label);
			style0.fontSize = 10;
			style0.fontStyle = FontStyle.Italic;
			style0.alignment = TextAnchor.UpperLeft;
			style0.fixedHeight = 40f;
			style0.normal.textColor = Color.cyan;

			btnStyle = new GUIStyle (GUI.skin.button);
			btnStyle.hover.background = btnStyle.normal.background;
			btnStyle.hover.textColor = new Color( 1f, 112/255f, 4/255f, 1f );
			btnStyle.fixedWidth = 160;
			btnStyle.stretchWidth = true;

			btnStyle2 = new GUIStyle (GUI.skin.button);
			btnStyle2.hover.background = btnStyle.normal.background;
			btnStyle2.hover.textColor = new Color( 1f, 112/255f, 4/255f, 1f );
			btnStyle2.fixedWidth = 80;
			btnStyle2.stretchWidth = true;
		
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
				EditorGUILayout.LabelField ("Submit Bug Report", headerStyle);
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginVertical("box");
				EditorGUILayout.BeginVertical();
					
					// Title //
					EditorGUILayout.LabelField("Title");
					_report.title = EditorGUILayout.TextField(_report.title);
					GUILayout.Space(15f);
					
					// Description //
					EditorGUILayout.LabelField("Description");
					if(System.String.IsNullOrEmpty(_report.title))
						_report.description = "1) What happened?:" + "\n\n\n\n" + "2) How reproduce it?:"; 

					_report.description = EditorGUILayout.TextArea(_report.description, GUILayout.Height(150));
					GUILayout.Space(15f);

					// Enviroment //
					_report.enviromentID = EditorGUILayout.Popup("Where did it happen? ",_report.enviromentID,_enviromentValues);
					GUILayout.Space(15f);

					// Attachment //
					EditorGUILayout.BeginVertical();
						GUILayout.Label("Attach file");
						EditorGUILayout.BeginHorizontal("box");
							if(GUILayout.Button("Select file", EditorStyles.miniButton))
							{
				_report.attachPath = EditorUtility.OpenFilePanel("Select file", "", "*.*");
							}
						EditorGUILayout.LabelField("file://" + _report.attachPath);
						EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
					GUILayout.Space(15f);

					// Your Email //
					EditorGUILayout.LabelField("Your E-mail");
					_report.email = EditorGUILayout.TextField(_report.email);
					GUILayout.Space(15f);

					EditorGUILayout.LabelField("Your local info:");
					// Scans //
					_report.assetVersion = SettingsManager.getVersion();
					_report.unityVersion = Application.unityVersion;
					_report.isUnityPro = Application.HasProLicense();
					_report.buildTarget = EditorUserBuildSettings.activeBuildTarget.ToString();
					_report.currentOS = SystemInfo.operatingSystem;
					
					msg = "2DDL Version: "+ _report.assetVersion +"\n";
					msg += "Unity Version: "+ _report.unityVersion +"\n";
					msg += "Unity Pro?: "+ _report.isUnityPro +"\n";
					msg += "Build Target: "+ _report.buildTarget + "\n";
					msg += "Current OS: " + _report.currentOS;

					EditorGUILayout.HelpBox(msg, MessageType.None);

									
					EditorGUILayout.BeginHorizontal ();
					
						if(GUILayout.Button("Cancel", btnStyle2)){
							w.Close();
						}
						
			GUILayout.FlexibleSpace();
						if(GUILayout.Button("Submit",btnStyle)){
							if(BugReportUtils.SendReport("2DDL Bug Report tool", consolidateData(), "New Bug: " + _report.title, _report.attachPath))
								w.Close();
						}

					EditorGUILayout.EndHorizontal ();


				EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();

						
		}

		string consolidateData(){
			string data = "Title: " +_report.title;
			data += "\n";
			data += "---------------------------------------------------------";
			data += "\n";
			data += _report.description;
			data += "\n";
			data += "\n";
			data += "\n";
			data += "---------------------------------------------------------";
			data += "\n";
			data += "Enviroment: " + _enviromentValues[_report.enviromentID];
			data += "\n";
			data += "---------------------------------------------------------";
			data += "\n";
			data += msg;
			data += "\n";
			data += "---------------------------------------------------------";
			data += "\n";
			data += "Customer E-mail: " + _report.email;
			data += "\n";
			data += "---------------------------------------------------------";

			return data;

		}
		

		
	}


	#endif

}