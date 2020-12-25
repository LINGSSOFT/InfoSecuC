namespace DynamicLight2D
{
	#if UNITY_EDITOR
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	
	public class SettingsWindow : EditorWindow {
		
		
		SerializedObject settingProfileAsset;
		int selectableLayerField;
		int selectableLayerMask;
		
		GUIStyle style1;
		GUIStyle style0;
		GUIStyle headerStyle;


		Texture2D headerTexture;
		Font headerFont;

		static EditorWindow _window;
		
		
		[MenuItem("2DDL/Settings", false, 10)]
		public static void Init()
		{
			
			//window = (DynamicLightAboutWindow )EditorWindow.GetWindow(typeof(DynamicLightAboutWindow));
			_window = EditorWindow.GetWindow( typeof(SettingsWindow), true, "Settings - 2DDL Pro" );
			_window.minSize = new Vector2 (500,300);
			
		}
		
		void OnEnable(){
			settingProfileAsset = new SerializedObject(SettingsManager.LoadMainSettings());
			selectableLayerField = AssetUtility.LoadPropertyAsInt ("layerCaster", settingProfileAsset);
			selectableLayerMask = AssetUtility.LoadPropertyAsInt("layerMask", settingProfileAsset);

			headerTexture = EditorUtils.HeaderTexture(); 
			headerFont = EditorUtils.HeaderFont();

			
		}
		
		void OnGUI(){
			
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
			EditorGUILayout.LabelField ("Settings", headerStyle);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20);

			EditorGUILayout.BeginHorizontal("box");			
				EditorGUILayout.LabelField ("Casters Default Layer");
				selectableLayerField = EditorGUILayout.LayerField ("", selectableLayerField);
			EditorGUILayout.EndHorizontal();
			GUI.color = Color.green;
			EditorGUILayout.HelpBox ("Layer by default for any caster creation.", MessageType.None);
			GUI.color = Color.white;

			GUILayout.Space(20);

			EditorGUILayout.BeginHorizontal("box");			
				EditorGUILayout.LabelField ("2DDLight Default LayerMask");
				selectableLayerMask = LayerMaskField("", selectableLayerMask);
			EditorGUILayout.EndHorizontal();
			GUI.color = Color.green;
			EditorGUILayout.HelpBox ("LayerMask by default for any 2DDLight creation.", MessageType.None);
			GUI.color = Color.white;

			//EditorGUILayout.LabelField ("2D LIGHT: LayerMask by Default");
			//selectableLayerMask = LayerMaskField("", selectableLayerMask);
			
			AssetUtility.SaveProperty ("layerCaster",selectableLayerField, settingProfileAsset);
			
			EditorGUILayout.Separator ();
			
			AssetUtility.SaveProperty ("layerMask",selectableLayerMask, settingProfileAsset);
		}
		
		private LayerMask LayerMaskField( string label, LayerMask layerMask) {
			List<string> layers = new List<string>();
			List<int> layerNumbers = new List<int>();
			
			for (int i = 0; i < 32; i++) {
				string layerName = LayerMask.LayerToName(i);
				if (layerName != "") {
					layers.Add(layerName);
					layerNumbers.Add(i);
				}
			}
			int maskWithoutEmpty = 0;
			for (int i = 0; i < layerNumbers.Count; i++) {
				if (((1 << layerNumbers[i]) & layerMask.value) > 0)
					maskWithoutEmpty |= (1 << i);
			}
			maskWithoutEmpty = EditorGUILayout.MaskField( label, maskWithoutEmpty, layers.ToArray());
			int mask = 0;
			for (int i = 0; i < layerNumbers.Count; i++) {
				if ((maskWithoutEmpty & (1 << i)) > 0)
					mask |= (1 << layerNumbers[i]);
			}
			layerMask.value = mask;
			return layerMask;
		}
		
		
	}
	
	#endif

}