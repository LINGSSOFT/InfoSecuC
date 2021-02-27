namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Reflection;
	using System.Collections;
	using System.IO;
	using System;
	using UnityEngine.UI;
	using DynamicLight2D;
	
	
	public class CustomDragData{
		public int originalIndex;
		public IList originalList;
	}
	
	public class AddOnFileInfo
	{
		public FileInfo file;
		public string className { get{return _csName;}}
		public string titleName { get{return _tlName;}}
		public Type classType{get{return _classType;}}
		public bool enabled;
		public string []tags { get{return _tags;}}
		public string description { get{return _description;}}

		
		private string _csName;
		private string _tlName;
		private string[] _tags;
		private string _description;
		private Type _classType;

				
		public AddOnFileInfo(FileInfo _fileInfo)
		{
			file = _fileInfo;
			
			int startindex = 0;
			bool counting = true;
			string output = file.Name;
			string output2 = "";
			
			// Remove file extension
			foreach (char letter in output)
			{
				if(letter.Equals('.'))
				{
					counting = false;
				}
				else if(!letter.Equals('.') && counting)
				{
					startindex++;
				}
			}
			
			output = output.Remove(startindex);
			
			
			// Split with Capital
			foreach (char letter in output)
			{
				if (char.IsUpper(letter) && output.Length > 0)
					output2 += " " + letter;
				else
					output2 += letter;
			}
			
			_csName = output;
			_tlName = output2;
			enabled = false;

			// Saving _classType and Getting "tags" and "description" fields //
			string addOnspath = string.Concat(EditorUtils.getMainRelativepath(), "Add-ons/");
			MonoScript compScript = (MonoScript)AssetDatabase.LoadAssetAtPath ((addOnspath + _csName + ".cs"), typeof(MonoScript)) as MonoScript; 
			_classType = compScript.GetClass ();

			FieldInfo _addon = _classType.GetField("tags");
			//_tags 
			if (_addon != null) {
				_tags = (string [])_addon.GetValue (null) as string[];
			}else{
				_tags = new string[0];
			}

			// _description
			_addon = _classType.GetField("description");
			if (_addon != null) {
				_description = (string)_addon.GetValue (null) as String;
			} else {
				_description = "";
			}
			
		}
		public AddOnFileInfo()
		{
			file = new FileInfo("");
			_csName = "";
			_tlName = "";
			enabled = false;
		}
		
	}
	
	
	
	[CustomEditor (typeof (DynamicLight2D.DynamicLight))] 
	[CanEditMultipleObjects]
	
	
	public class DynamicLightEditor : Editor {
		
		static internal DynamicLight2D.DynamicLight light;
		
		SerializedProperty version, lmaterial, lUseSolidColor, lColor, intensity, strokeRender, radius, segments, range, light2DType;
		SerializedProperty layerm, useEvents,  usePersistFOV, intelliderConvex, recalcNorms, debugLines, sortOrder;
		SerializedProperty DDLEvent_OnEnterFOV, DDLEvent_OnExitFOV, DDLEvent_InsideFOV;
		SerializedProperty uv_offset, uv_scale;

		
		
		private GUIStyle titleStyle, subTitleStyle, bgStyle, btnStyle;
		
		private Vector2 lightRectOrigin, lightRectSize;
		
		private string _path;
	
		bool _timeToReview;
		
		AddOnFileInfo [] AddOnsfiles;
		
		static bool foldoutEvents = true;
		static bool foldoutMoreOptions = true;
		static bool showInfo = false;

		private Texture2D headerTexture;
		private Font headerFont;

		static bool _lastEventsState;

		private Tool _cTool;
		internal void OnEnable(){

			_cTool = Tools.current;
			Tools.current = Tool.None;
			//serialtarget = new SerializedObject(target);
			light = target as DynamicLight2D.DynamicLight;
			
			version = serializedObject.FindProperty("Version");
			lmaterial = serializedObject.FindProperty("LightMaterial");
			lUseSolidColor = serializedObject.FindProperty("SolidColor");
			lColor = serializedObject.FindProperty("LightColor");
			intensity = serializedObject.FindProperty("Intensity");
			radius = serializedObject.FindProperty ("LightRadius");
			segments = serializedObject.FindProperty ("Segments");
			range = serializedObject.FindProperty ("RangeAngle");
			layerm = serializedObject.FindProperty ("Layer");
			light2DType = serializedObject.FindProperty ("light2DType");
			useEvents = serializedObject.FindProperty("useEvents");
			usePersistFOV = serializedObject.FindProperty("objectsWithinSight");
			intelliderConvex = serializedObject.FindProperty("intelliderConvex");
			recalcNorms = serializedObject.FindProperty("recalculateNormals");
			strokeRender = serializedObject.FindProperty("strokeRender");
			debugLines = serializedObject.FindProperty("debugLines");
			DDLEvent_OnEnterFOV = serializedObject.FindProperty("DDLEvent_OnEnterFOV");
			DDLEvent_OnExitFOV = serializedObject.FindProperty("DDLEvent_OnExitFOV");
			DDLEvent_InsideFOV = serializedObject.FindProperty("DDLEvent_InsideFOV");
			
			// Teselation
			uv_offset = serializedObject.FindProperty("uv_Offset");
			uv_scale = serializedObject.FindProperty ("uv_Scale");

			//Order
			sortOrder = serializedObject.FindProperty("SortOrderID");

			//Events State
			_lastEventsState = useEvents.boolValue;
			
			// Bounds
			getLightBounds();

			//Header stuffs
			headerTexture = EditorUtils.HeaderTexture(); 
			headerFont = EditorUtils.HeaderFont();
			
			Undo.undoRedoPerformed += refreshLightObject;
			
			_path = EditorUtils.getMainRelativepath();
			
			
			// REVIEW System
			//EditorPrefs.SetInt("2DDLReview",0);
			int _r = EditorPrefs.GetInt("2DDLReview", 0);
			if (_r >= 0)
				EditorPrefs.SetInt("2DDLReview", _r + 1);
			
			if (_r >= 100)
				_timeToReview = true;
			
			reloadAddons();
			
		}
		
		
		
		internal void getLightBounds(){
			//Rect lightRect = new Rect(light.renderer.bounds.min.x,light.renderer.bounds.max.y, light.renderer.bounds.max.x, light.renderer.bounds.min.y);    
			//lightRect = new Rect(Camera.main.WorldToScreenPoint(lightRect.x),Camera.main.WorldToScreenPoint(lightRect.y),Camera.main.WorldToScreenPoint(lightRect.width),Camera.main.WorldToScreenPoint(lightRect.height));
			lightRectOrigin = new Vector2(light.GetComponent<Renderer>().bounds.min.x,light.GetComponent<Renderer>().bounds.min.y);
			lightRectSize = new Vector2(light.GetComponent<Renderer>().bounds.max.x, light.GetComponent<Renderer>().bounds.max.y);
		}
		
		internal void OnDisable(){
			Undo.undoRedoPerformed -= refreshLightObject;
			Tools.current = _cTool;
		}
		
		
		
		public override void OnInspectorGUI () {
			if (light == null){return;}
			
			serializedObject.Update();
			
			initStyles();
			
			GUILayoutOption miniButtonWidth = GUILayout.Width(60f);
			
			float fRange = range.floatValue;
			if(fRange > 360.001f)
				fRange = 360f;
			if(fRange < .999f)
				fRange = 1f;
			
			
			//Debug.Log(fRange - Mathf.FloorToInt(fRange));
			if(Mathf.Abs(fRange - Mathf.FloorToInt(fRange)) > 0.5f){
				fRange = Mathf.Round(fRange);
				range.floatValue = fRange;
			}
			
			if(range.floatValue != fRange){
				range.floatValue = fRange;
			}
			
			float fradius = radius.floatValue;
			if(fradius < 0)
				fradius *= -1;
			
			if(radius.floatValue != fradius){
				radius.floatValue = fradius;
			}
			
			string v = version.stringValue;
			

			GUIStyle headerStyle = new GUIStyle( GUI.skin.box );
			headerStyle.normal.background = EditorUtils.MakeTex( 2, 2, new Color( 1f, 112/255f, 4/255f, 1f ) );
			headerStyle.fixedHeight = headerStyle.fixedHeight * 1.2f;

			GUIStyle textHeaderStyle = new GUIStyle(GUI.skin.label);
			textHeaderStyle.fontSize = 18;
			textHeaderStyle.fontStyle = FontStyle.Bold;
			textHeaderStyle.alignment = TextAnchor.MiddleCenter;
			textHeaderStyle.fixedHeight = headerTexture.height;
			textHeaderStyle.normal.textColor = Color.white;
			textHeaderStyle.font = headerFont;


			// Header //
			EditorGUILayout.BeginHorizontal(headerStyle);
			GUILayoutOption[] reBtnStyle ={GUILayout.Width(35f), GUILayout.Height(35f)};

			//Focus btn
			if(GUILayout.Button(AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/focus.png", typeof(Texture2D)) as Texture2D , headerStyle, reBtnStyle))
			{
				focusGameObjectInSceneView();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField ("2DDLight", textHeaderStyle);
			GUILayout.FlexibleSpace();
			//Reload btn
			if(GUILayout.Button(AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/reload-w.png", typeof(Texture2D)) as Texture2D , headerStyle, reBtnStyle))
			{
				refreshLightObject();
				ShowUpNotification("reloaded", 50f);
			}
			EditorGUILayout.EndHorizontal();
			  

			/*
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Window");
			EditorGUILayout.BeginHorizontal("Box");
			GUILayoutOption[] reBtnStyle ={GUILayout.Width(35f), GUILayout.Height(35f)};
			//Focus btn
			if(GUILayout.Button(AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/focus.png", typeof(Texture2D)) as Texture2D , reBtnStyle))
			{
				focusGameObjectInSceneView();
			}
			GUILayout.Label("2DDL Object", titleStyle);
			
			//Reload btn
			if(GUILayout.Button(AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/reload-w.png", typeof(Texture2D)) as Texture2D , reBtnStyle))
			{
				refreshLightObject();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

*/
			
			/* http://answers.unity3d.com/questions/465983/where-is-the-box-part-of-editorguilayoutbeginverti.html

							 //Auto fit styles:
			 EditorGUILayout.BeginVertical("Box");
			 EditorGUILayout.BeginVertical("Button");
			 EditorGUILayout.BeginVertical("TextArea");
			 EditorGUILayout.BeginVertical("Window");
			 EditorGUILayout.BeginVertical("Textfield");
			 EditorGUILayout.BeginVertical("HorizontalScrollbar"); //Fixed height
			 EditorGUILayout.BeginVertical("Label"); //No style
			 EditorGUILayout.BeginVertical("Toggle"); //Just puts a non usable CB to the left 
			 EditorGUILayout.BeginVertical("Toolbar"); //Fixed height
			  */

			#region Events Warning
			if(useEvents.boolValue == true)
			{
				
				if(light.DDLEvent_InsideFOV.GetPersistentEventCount() < 1 &&
				   light.DDLEvent_OnEnterFOV.GetPersistentEventCount() < 1 &&
				   light.DDLEvent_OnExitFOV.GetPersistentEventCount() < 1 )
				{
					Color c = GUI.color;
					GUI.color = new Color32(103,188,114,255);
					EditorGUILayout.HelpBox("WARNING: Events are enabled but you have no listeners on the list", MessageType.Warning, true);
					GUI.color = c;
				}
			}
			#endregion
			
			#region review box
			if (_timeToReview)
			{
				Color c = GUI.color;
				GUI.color = new Color32(103,188,114,255);
				EditorGUILayout.HelpBox("Sorry to disturb you. Reviews on the Asset Store are very important for me to keep enhanced 2DDL. Please take a minute to write a responsable review.", MessageType.Info, true);
				EditorGUILayout.BeginHorizontal("box");
				if (GUILayout.Button("Sure, I'll write a review!"))
				{
					_timeToReview = false;
					EditorPrefs.SetInt("2DDLReview", -1);
					//Application.OpenURL("http://u3d.as/asp");
					UnityEditorInternal.AssetStore.Open("/content/25933");
				}
				if (GUILayout.Button("No, thanks."))
				{
					_timeToReview = false;
					EditorPrefs.SetInt("2DDLReview", -1);
				}
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(10);
				GUI.color = c;
			}
			#endregion
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Main", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("Box");
			if(fRange < 360f){
				if(segments.intValue < 5) segments.intValue = 5;
				
				EditorGUILayout.IntSlider(segments, 5, 20, new GUIContent("Segments","Quantity of line segments is used for build mesh render of 2DLight. 5 at least in spot Lights"));
			}else{
				EditorGUILayout.IntSlider(segments, 3, 20, new GUIContent("Segments","Quantity of line segments is used for build mesh render of 2DLight. 3 at least"));
			}
			
			
			EditorGUILayout.Slider (radius, 1f, 100f, new GUIContent ("Radius"));
			EditorGUILayout.Slider (range, 1f, 360f, new GUIContent ("Angle"));
			
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(layerm, new GUIContent("Layer", "Current layer where 2DDL is working. Must be the same in the colliders"));
			if (showInfo)
				EditorGUILayout.HelpBox("Layer is a LayerMask filter that define what layers are using by 2DDL. \nMultiple layers selection is allowed.", MessageType.None, true);


			GUILayout.Space (10);
			EditorGUILayout.PropertyField (light2DType,new GUIContent("Type"));
			if (showInfo)
				EditorGUILayout.HelpBox("Types of Lights are defined by:\n" +
					"Static: produce dynamic shadows but only works with caster registers in the start.\n" +
					"Dynamic: allow Add and Delete caster without notification to 2DDL.\n" +
					"One Frame: use the first frame in the gameplay for light calculation and then is deactivated. Use i.e. backgrounds. \n" +
					"Only Sight: only Visibility algorithm is performed.", MessageType.None, true);



			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndVertical();
			
			
			
			GUILayout.Space (20);
			
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Rendering", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(lmaterial, new GUIContent("Light Material", "Material Object used for render into light mesh"));
			EditorGUILayout.PropertyField (sortOrder, new GUIContent ("Sorting Order", "The rendering order in foward direction"));
			GUILayout.Space(10);

			EditorGUILayout.BeginVertical ("box");
			lUseSolidColor.boolValue = GUILayout.Toggle (lUseSolidColor.boolValue, "Solid Color");//, "if TRUE, render properly texturized or vertex color lights, if is FALSE, allow work with procedural gradient shaders."));
			if (showInfo)
				EditorGUILayout.HelpBox("Solid color only reflect changes with texturized materials", MessageType.None, true);

			if (lUseSolidColor.boolValue == true) {
				EditorGUILayout.PropertyField (lColor, new GUIContent ("Light Color", "Only can be used with Is Solid Color check to TRUE"));
				EditorGUILayout.Slider (intensity, .1f, 7f, new GUIContent ("Intensity"));
			}
			EditorGUILayout.EndVertical ();

			GUILayout.Space(10);
			EditorGUILayout.PropertyField(strokeRender, new GUIContent("Shadow Offset","Is the empty space between caster and shadow start. (Mostly Applicable on Stencil shaders for stroke effect)"));
			if (strokeRender.floatValue < 0f)
				strokeRender.floatValue = 0;

			if (showInfo)
				EditorGUILayout.HelpBox("Add an offset to shadows edges position.", MessageType.None, true);

			recalcNorms.boolValue =  GUILayout.Toggle(recalcNorms.boolValue, "Recalculate normals");
			
			EditorGUILayout.EndVertical ();
			
			
			
			GUILayout.Space(20);
			EditorGUILayout.LabelField ("Tessellation", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.PropertyField(uv_offset, new GUIContent("UV Offset", ""));
			EditorGUILayout.PropertyField(uv_scale, new GUIContent("UV Scale", ""));
			EditorGUILayout.EndVertical();
			
			if (GUILayout.Button("restore",  EditorStyles.miniButton, miniButtonWidth)){
				uv_offset.vector2Value = new Vector2(.5f,.5f);
				uv_scale.vector2Value = new Vector2(.5f,.5f);
			}
			
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.EndVertical ();
			
			EditorGUILayout.EndVertical();
			//EditorGUILayout.Separator();
			//EditorGUILayout.Separator();
			//EditorGUILayout.Separator();

			// -- ADD-ONS BUTTONS -- //
			GUILayout.Space(20);
			generateAddOnsButtons();
			GUILayout.Space(20);
			
			
			
			foldoutEvents = EditorGUILayout.Foldout(foldoutEvents, "Events");
			if(foldoutEvents)
			{
				//Notification
				if(_lastEventsState != useEvents.boolValue)
				{
					_lastEventsState = useEvents.boolValue;
					ShowUpNotification("Events Enabled: " + _lastEventsState, 50f);
				}

				EditorGUILayout.BeginVertical("Box");
				useEvents.boolValue = EditorGUILayout.BeginToggleGroup("Events", useEvents.boolValue);

				GUILayout.Space(5);
				if(!useEvents.boolValue){
					EditorGUILayout.HelpBox("Force to register and notify events like OnEnter(), OnExit() and OnInside() ", MessageType.None);

				}
				else
				{

					GUILayout.Space(5);

					EditorGUI.indentLevel++;
					
					EditorGUILayout.PropertyField(DDLEvent_OnEnterFOV, new GUIContent("OnEnter", "List of callback functions called when caster stay in sight of current light"));
					EditorGUILayout.PropertyField(DDLEvent_OnExitFOV, new GUIContent("OnExit", "List of callback functions when caster just exit from sight of current light"));
					
					EditorGUILayout.Separator();
					EditorGUILayout.Separator();
					

					usePersistFOV.boolValue = EditorGUILayout.BeginToggleGroup("Persistent", usePersistFOV.boolValue);
					EditorGUILayout.PropertyField(DDLEvent_InsideFOV, new GUIContent("OnInside", "List of callback functions when caster just exit from sight of current light"));
					EditorGUILayout.EndToggleGroup();
					
					EditorGUI.indentLevel--;
					
				}
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndVertical();
			}
			
			
			
			
			GUILayout.Space(20);
			foldoutMoreOptions = EditorGUILayout.Foldout(foldoutMoreOptions, "More Options");
			if(foldoutMoreOptions)
			{
				EditorGUILayout.BeginVertical("Box");
				
					EditorGUILayout.BeginVertical ("Box");
						intelliderConvex.boolValue = GUILayout.Toggle(intelliderConvex.boolValue,"Ignore middle vertices");
						if (showInfo)
							EditorGUILayout.HelpBox("2DDL algorithm use only edge vertices. Better for performance", MessageType.None, true);

					EditorGUILayout.EndVertical ();
					
					EditorGUILayout.BeginVertical ("Box");
						debugLines.boolValue = GUILayout.Toggle(debugLines.boolValue, "Debug Lines");
						if (showInfo)
							EditorGUILayout.HelpBox("Draw debug lines in editor mode", MessageType.None, true);

					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ("Box");
						showInfo =  GUILayout.Toggle(showInfo, "Show Help");
						if (showInfo)
							EditorGUILayout.HelpBox("Show captions like this one", MessageType.None, true);

					EditorGUILayout.EndVertical ();

				EditorGUILayout.EndVertical();

				//divider ();
				
				GUILayout.Space(10);
				GUILayout.Label("Info", subTitleStyle);
				EditorGUILayout.HelpBox("2DDL PRO version: " + v, MessageType.Info, true);
				EditorGUILayout.Separator();
				
				
				// --------------  MINI TOOLBAR BUTTONS (settings-about-support) ----------------------------------
				GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				
				
				//EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Settings",  EditorStyles.miniButtonLeft, miniButtonWidth)){
					SettingsWindow.Init();
				}
				
				GUILayout.Space(2);
				if (GUILayout.Button("Doc",  EditorStyles.miniButtonMid, miniButtonWidth)){
					Application.OpenURL("http://martinysa.com/2d-dynamic-lights-doc/");
				}
				
				GUILayout.Space(2);
				if (GUILayout.Button("About",  EditorStyles.miniButtonMid,miniButtonWidth)){
					DynamicLightAboutWindow.Init();
				}
				
				GUILayout.Space(2);
				if (GUILayout.Button("Support",  EditorStyles.miniButtonRight,miniButtonWidth)){
					Application.OpenURL("mailto:info@martinysa.com");
				}
				//EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();	
				


			}
			
			GUILayout.Space(50);

			serializedObject.ApplyModifiedProperties();
			
			
			
			
		}
		
		
		

		
		void OnSceneGUI () {
			if(light){
				
				Event _event = Event.current;//
				
				Transform lTransform = light.transform;

				#if UNITY_5_6_OR_NEWER
				EditorUtility.SetSelectedRenderState(light.GetComponent<MeshRenderer>(), EditorSelectedRenderState.Hidden);
				#else
				EditorUtility.SetSelectedWireframeHidden(light.GetComponent<MeshRenderer>(), true);
				#endif

				
				
				Vector3 oldPoint = lTransform.TransformPoint(new Vector3(light.LightRadius,0,0));
				float size = HandleUtility.GetHandleSize(oldPoint);
				
				Undo.RecordObject(light, "Move Light Radius Point");
				
				
				Color color = ((Color.gray - (Color)light.LightColor)*.3f);
				color.a = 1;
				if(!light.SolidColor)
					color = Color.black;
				
				Handles.color = color;
				
				
				#region Size/Rotation Handles
				float r = light.LightRadius;
				Vector3 newHandlePoint = Vector3.zero;
				bool dirtyRotation;
				Vector3 dirLookAt = Vector3.zero;
				
				for (int i = 0; i <= 2 ; i++)
				{
					
					Vector3 direction = (Quaternion.AngleAxis((float)i *.25f * light.RangeAngle - light.RangeAngle *.25f, -lTransform.forward) * lTransform.up).normalized;

					#if UNITY_5_6_OR_NEWER
						newHandlePoint = (lTransform.position - Handles.FreeMoveHandle(
						lTransform.position + direction * r,
						Quaternion.identity,
						size * 0.035f, Vector3.zero, Handles.DotHandleCap));
					#else
						newHandlePoint = (lTransform.position - Handles.FreeMoveHandle(
						lTransform.position + direction * r,
						Quaternion.identity,
						size * 0.035f, Vector3.zero, Handles.DotCap));
					#endif


					
					r = newHandlePoint.magnitude;
					
					
					dirtyRotation = false;
					
					if (GUIUtility.hotControl == GetLastControlId()){
						//if(direction != newHandlePoint){
						if(i ==1){
							
							dirLookAt = (direction  - newHandlePoint).normalized;
							dirtyRotation = true;
							
						}else if(i == 2)
						{
							float ang = Vector3.Angle(newHandlePoint, direction);
							if (Mathf.Abs(ang-180) > 0)
							{
								Vector3 cross = Vector3.Cross(newHandlePoint, direction);
								if(cross.z < 0)
									ang *= -1; 
								
								Vector3 dir = (Quaternion.AngleAxis((float)(.25f + ang) , -lTransform.forward) * lTransform.up).normalized;
								dirLookAt = -dir;
								dirtyRotation = true;
							}
							
						}else if(i == 0)
						{
							float ang = Vector3.Angle(newHandlePoint, direction);
							Vector3 cross = Vector3.Cross(newHandlePoint, direction);
							if(cross.z < 0)
								ang *= -1; 
							
							Vector3 dir = (Quaternion.AngleAxis((float)(ang) , -lTransform.forward) * lTransform.up).normalized;
							dirLookAt = -dir;
							dirtyRotation = true;
							
						}
						
						
						//IF Ctrl is pressed , does not rotate
						if (dirtyRotation && (!_event.control)){
							lTransform.up = dirLookAt;
							Vector3 _r = lTransform.localEulerAngles;
							if(_r.z > 360 || _r.z < -360) _r.z *=0;
							_r.x = 0;
							_r.y = 0;
							
							lTransform.localEulerAngles = _r;
						}
						
					}
				}
				
				//IF Shift is pressed , does not redimensionate
				r = Mathf.Round(r * 1000) / 1000f;
				if(r != light.LightRadius && (!_event.shift)){
					light.LightRadius = r;
					light.Rebuild();
				}
				// END Size Handles ------------
				#endregion
				
				
				#region Wire Arc
				Handles.DrawWireArc(lTransform.position, lTransform.forward, Quaternion.AngleAxis(180 - light.RangeAngle *.5f, lTransform.forward) * -lTransform.up, light.RangeAngle, light.LightRadius);
				
				
				//Get max bounds
				if(light.RangeAngle < 360){
					Handles.DrawLine(lTransform.position, lTransform.TransformPoint(light.getMaxFromAllVerts()));
					Handles.DrawLine(lTransform.position, lTransform.TransformPoint(light.getMinFromAllVerts()));
				}
				
				#endregion
				
				
				
				PerformAndDrawAngleHandles(light);
				
				
				
				if(light.LightMaterial != light.GetComponent<MeshRenderer>().sharedMaterial)
				{
					light.GetComponent<MeshRenderer>().sharedMaterial = light.LightMaterial;
					light.Rebuild();
				}

				if(showInfo)
					showSceneGUIHint();
				
				//Show notification on Scene i.e. add ons added
				showNotificationSceneGUI();

			}
			
		}

		void showSceneGUIHint(){
			Handles.BeginGUI ();
			GUI.color = Color.green;
			GUILayout.BeginArea(new Rect(Screen.width - 140, Screen.height - 120, 140,120));
				EditorGUILayout.HelpBox ("Rotation only: SHIFT + click", MessageType.None);
				EditorGUILayout.HelpBox ("Resize only: CTRL + click", MessageType.None);
			GUILayout.EndArea ();
			GUI.color = Color.white;
			Handles.EndGUI ();
		}
		
		
		#region AddOns
		
		private void reloadAddons()
		{

			
			FileInfo [] _files = new FileInfo[0];
			
			if(_path == null) _path = EditorUtils.getMainRelativepath();
			string addOnspath = string.Concat(_path, "Add-ons");
			EditorUtils.ListOfFilesUnderPath(addOnspath, ref _files, "*.cs");
			
			AddOnsfiles = new AddOnFileInfo[_files.Length];
			for(int i = 0 ; i < AddOnsfiles.Length; i++)
			{
				AddOnsfiles[i] = new AddOnFileInfo(_files[i]);
			}
			
		}

		static string searchString = "";
		//static Vector2 scroolpos = Vector2.zero;
		private void generateAddOnsButtons(){
	
			GUIStyle buttonGUIStyle = new GUIStyle( GUI.skin.button );
			//buttonGUIStyle.fixedWidth = 200f;
			buttonGUIStyle.hover.background = buttonGUIStyle.normal.background;
			buttonGUIStyle.hover.textColor = Color.yellow;
			buttonGUIStyle.onHover.textColor = Color.yellow;

			
			//GUIStyle currentStyle = new GUIStyle( GUI.skin.box );
			//currentStyle.normal.background = MakeTex( 1, 1, new Color( .5f, 0f, .8f, 0.3f ) );
			
			Color c = GUI.color;
			GUI.color = Color.cyan;
			
			GUILayout.BeginVertical("box");
			
			GUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField("Add-ons", subTitleStyle);
			
			searchString = EditorGUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(50));
			if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
			{
				// Remove focus if cleared
				searchString = "";
				GUI.FocusControl(null);
			}
			
			GUILayout.EndHorizontal ();
			GUILayout.Space (10);

			//GUILayout.FlexibleSpace();
			//scroolpos = EditorGUILayout.BeginScrollView (scroolpos, false, false, GUILayout.Height(130));
			
			GUILayout.BeginVertical("box");
			
			

			
			
			if(AddOnsfiles != null && AddOnsfiles.Length > 0)
			{
				//string output = "";
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.Space(5);
				int count = 0;
				for (int i = 0; i < AddOnsfiles.Length; i++)
				{

					// FILTER BY ENABLED
					if(System.String.Equals(searchString, "E") && !checkingComponent(AddOnsfiles[i]))
					{
						continue;
					}


					bool foundByName = true;
					// Filter by classname //
					if(! AddOnsfiles[i].className.ToLower().Contains(searchString.ToLower()))
						foundByName = false;



					// Second pass: Filter by tags //
					bool foundByTag = foundByName;//false;
					for (int id = 0; id < AddOnsfiles[i].tags.Length; id++)
					{
						if(!System.String.IsNullOrEmpty(AddOnsfiles[i].tags[id])){
							if(AddOnsfiles[i].tags[id].Contains(searchString.ToLower())){
								foundByTag = true;
									break;
							}
						} 
					}





					if(!foundByTag && !System.String.IsNullOrEmpty(searchString))
					{
						continue;
					}

					//----------------//


					
					bool toggleVal = false;
					bool newToggleVal = toggleVal;
					bool lastToggleVal = toggleVal;
					if(checkingComponent(AddOnsfiles[i]))
					{
						toggleVal = true;
						newToggleVal = GUILayout.Toggle(toggleVal, new GUIContent(AddOnsfiles[i].titleName, AddOnsfiles[i].description), buttonGUIStyle);
					}
					else{
						toggleVal = false;
						Color nc = GUI.color;
						GUI.color = new Color32(255,141,141,255);
						newToggleVal = GUILayout.Toggle(toggleVal, new GUIContent(AddOnsfiles[i].titleName, AddOnsfiles[i].description), buttonGUIStyle);
						GUI.color = nc;
					}

					//Show the description if do i have
					if(!System.String.IsNullOrEmpty(AddOnsfiles[i].description) && showInfo)
						EditorGUILayout.HelpBox(AddOnsfiles[i].description, MessageType.None);
				
					if(lastToggleVal != toggleVal){
						lastToggleVal = toggleVal;

					}

					if(newToggleVal != toggleVal)
					{

						if(newToggleVal == true) // need to add comp
						{
							//Debug.Log("AddingComp");
							addingComponent(AddOnsfiles[i]);
							ShowUpNotification(AddOnsfiles[i].titleName + " Added", 50f);
						}else{
							//Debug.Log("removingComponent");
							removingComponent(AddOnsfiles[i]);
						}
					}
					
					
					count++;
					
					GUILayout.Space(10);
					
				}
				if(count == 0)
					EditorGUILayout.LabelField("no results found");


				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				//EditorGUILayout.EndScrollView ();
				//GUILayout.FlexibleSpace(); //
				
			}
			
			GUI.color = c;
			
			GUILayout.Space(20);
			
			//GUILayout.FlexibleSpace();
			
			GUILayout.EndVertical();
			
			GUILayout.EndVertical();

			Repaint ();
			
		}

		private bool checkingComponent(AddOnFileInfo file)
		{	
			
			Component[] c = light.gameObject.GetComponents<MonoBehaviour>();
			foreach(Component _c in c)
			{
				if((_c.GetType().ToString().Equals(file.className)) || (_c.GetType().ToString().Equals("DynamicLight2D." + file.className)))
				{
					return true; // component is already in gameobject
				}
			}
			return false; // component is not in gameobject
			
		}
		
		private void addingComponent(AddOnFileInfo file)
		{
			#region unused methods
			//----------------METHOD 1 --------------------------------------//
			/*

			bool exist = checkingComponent(file);


			if(!exist)
				UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(light.gameObject, file.file.FullName, file.className);

		*/
			
			//--------------------------------------------------------------//

			
			/*
		//----------------METHOD 2 --------------------------------------//
		string addOnspath = string.Concat(EditorUtils.getMainRelativepath(), "AddOns/");
		UnityEditorInternal.
			InternalEditorUtility.
				AddScriptComponentUnchecked(light.gameObject, 
				                            AssetDatabase.LoadAssetAtPath((addOnspath + file.className + ".cs"), 
				                              typeof(MonoScript)) as MonoScript);
		//--------------------------------------------------------------//
		*/
		
		/*

			//----------------METHOD 3 --------------------------------------//
			
			string addOnspath = string.Concat(EditorUtils.getMainRelativepath(), "Add-ons/");
			MonoScript compScript = (MonoScript)AssetDatabase.LoadAssetAtPath ((addOnspath + file.className + ".cs"), typeof(MonoScript)) as MonoScript; 
			Type type = compScript.GetClass ();
			light.gameObject.AddComponent (type);
			//--------------------------------------------------------------//

		*/
		#endregion

			//----------------METHOD 4 --------------------------------------//

			light.gameObject.AddComponent (file.classType);
			//---------------------------------------------------------------//
		}
		
		private bool removingComponent(AddOnFileInfo file)
		{	
			
			Component[] c = light.gameObject.GetComponents<MonoBehaviour>();
			Component compToKill = null;
			foreach(Component _c in c)
			{
				if((_c.GetType().ToString().Contains(file.className)))
				{
					compToKill = _c;
					//return true; // component is already in gameobject
				}
			}
			
			if (compToKill != null) {
				DestroyImmediate(compToKill);
				EditorGUIUtility.ExitGUI();
				return true;
			}
			
			return false; // component is not in gameobject
			
		}
		
		private Texture2D MakeTex( int width, int height, Color col )
		{
			Color[] pix = new Color[width * height];
			for( int i = 0; i < pix.Length; ++i )
			{
				pix[ i ] = col;
			}
			Texture2D result = new Texture2D( width, height );
			result.SetPixels( pix );
			result.Apply();
			return result;
		}
		#endregion
		
		void focusGameObjectInSceneView()
		{
			Selection.activeGameObject = light.gameObject;
			SceneView.FrameLastActiveSceneView();
			EditorGUIUtility.PingObject(light.gameObject);
			Selection.activeGameObject = light.gameObject;
		}
		
		void PerformAndDrawAngleHandles(DynamicLight2D.DynamicLight light)
		{
			bool dirtyAngles = false;
			
			//Handle1
			Vector3 cwPos = light.transform.position + (Quaternion.AngleAxis(-light.RangeAngle *.5f, -light.transform.forward) * (light.transform.up)) * (light.LightRadius + HandleUtility.GetHandleSize(light.transform.position) * 0.3f);
			Vector3 cwBasePos = light.transform.position + (Quaternion.AngleAxis(-light.RangeAngle * .5f, -light.transform.forward) * (light.transform.up)) * light.LightRadius;
			#if UNITY_5_6_OR_NEWER
			Vector3 cwHandle = Handles.FreeMoveHandle(cwPos, Quaternion.identity, HandleUtility.GetHandleSize(light.transform.position) * .04f, Vector3.zero, Handles.CircleHandleCap);
			#else
			Vector3 cwHandle = Handles.FreeMoveHandle(cwPos, Quaternion.identity, HandleUtility.GetHandleSize(light.transform.position) * .04f, Vector3.zero, Handles.CircleCap);
			#endif
			Vector3 toCwHandle = (light.transform.position - cwHandle).normalized;
			Handles.DrawLine(cwBasePos, cwPos - (cwPos - cwBasePos).normalized * HandleUtility.GetHandleSize(light.transform.position) * 0.05f);
			
			if (cwPos != cwHandle)
			{
				light.RangeAngle = Mathf.RoundToInt(360 - 2 * Quaternion.Angle(Quaternion.FromToRotation(light.transform.up, toCwHandle), Quaternion.identity));
				dirtyAngles = true;
			}
			
			//Handle2
			cwPos = light.transform.position + (Quaternion.AngleAxis(light.RangeAngle *.5f, -light.transform.forward) * (light.transform.up)) * (light.LightRadius + HandleUtility.GetHandleSize(light.transform.position) * 0.3f);
			cwBasePos = light.transform.position + (Quaternion.AngleAxis(light.RangeAngle * .5f, -light.transform.forward) * (light.transform.up)) * light.LightRadius;
			#if UNITY_5_6_OR_NEWER
			cwHandle = Handles.FreeMoveHandle(cwPos, Quaternion.identity, HandleUtility.GetHandleSize(light.transform.position) * .04f, Vector3.zero, Handles.CircleHandleCap);
			#else
			cwHandle = Handles.FreeMoveHandle(cwPos, Quaternion.identity, HandleUtility.GetHandleSize(light.transform.position) * .04f, Vector3.zero, Handles.CircleCap);
			#endif

			toCwHandle = (light.transform.position - cwHandle).normalized;
			Handles.DrawLine(cwBasePos, cwPos - (cwPos - cwBasePos).normalized * HandleUtility.GetHandleSize(light.transform.position) * 0.05f);
			
			if (cwPos != cwHandle)
			{
				light.RangeAngle = Mathf.RoundToInt(360 - 2 * Quaternion.Angle(Quaternion.FromToRotation(light.transform.up, toCwHandle), Quaternion.identity));
				dirtyAngles = true;
			}
			
			
			
			#region Events modifiers
			// Press Ctrl for snapping
			Event e = Event.current;
			if (e.control && dirtyAngles){
				light.RangeAngle = Mathf.Round(light.RangeAngle / EditorPrefs.GetFloat("RotationSnap")) * EditorPrefs.GetFloat("RotationSnap");
				//e.Use();
			}
			
			//Reset
			if(e.clickCount == 2 && e.control){
				light.RangeAngle = 360;
				light.transform.localEulerAngles = Vector3.zero;
				dirtyAngles = true;
				//e.Use();
			}
			
			#endregion
			
			if (dirtyAngles)
				light.Rebuild ();
			
		}
		
		private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
		{
			Vector3.Dot(vec1,vec2);
			// Divide the dot by the product of the magnitudes of the vectors
			float dot = 0f;
			dot /= (vec1.magnitude*vec2.magnitude);
			//Get the arc cosin of the angle, you now have your angle in radians 
			float acos = Mathf.Acos(dot);
			//Multiply by 180/Mathf.PI to convert to degrees
			float angle = acos*180f/Mathf.PI;
			//Congrats, you made it really hard on yourself.
			Debug.Log(angle-90);
			return angle-90;
		}
		
		
		void refreshLightObject(){
			foreach (DynamicLight2D.DynamicLight s in targets) {
				s.Rebuild();
			}
		}

		
		internal void initStyles(){
			if(_path == null)
			{_path = EditorUtils.getMainRelativepath();}
			
			titleStyle = new GUIStyle(GUI.skin.label);
			titleStyle.fontSize = 15;
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.alignment = TextAnchor.MiddleCenter;
			titleStyle.margin = new RectOffset(4, 4, 10, 0);
			
			subTitleStyle = new GUIStyle(GUI.skin.label);
			subTitleStyle.fontSize = 13;
			subTitleStyle.fontStyle = FontStyle.Bold;
			subTitleStyle.alignment = TextAnchor.MiddleLeft;
			subTitleStyle.margin = new RectOffset(4, 4, 10, 0);
			
			bgStyle = new GUIStyle(GUI.skin.box);
			bgStyle.margin = new RectOffset(4, 4, 0, 4);
			bgStyle.padding = new RectOffset(1, 1, 1, 2);
			
			btnStyle = new GUIStyle(GUI.skin.button);
			Sprite bg = (Sprite)AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Textures/box_sprite.png", typeof(Sprite));
			Sprite bgClicked = bg;//(Sprite)AssetDatabase.LoadAssetAtPath(_path + "Textures/box_sprite.png", typeof(Sprite));
			btnStyle.margin = new RectOffset(0, 0, 0, 0);
			btnStyle.padding = new RectOffset(0, 0, 4, 4);
			btnStyle.normal.background = bg.texture;
			btnStyle.active.background = bgClicked.texture;
			
		}
		
		internal Material lastMat;
		internal Material newMat;
		internal bool previewingNewMat = false;
		
		void dragAndDropEvents(){
			Event evt = Event.current;
			
			foreach (object dragged_object in DragAndDrop.objectReferences) {
				if(dragged_object.GetType() != typeof(Material))
					return;
			}
			
			Vector2 Origin = Camera.current.WorldToScreenPoint(lightRectOrigin);
			Vector2 Size = Camera.current.WorldToScreenPoint(lightRectSize);
			
			
			Rect lightRect = new Rect(Origin.x,Origin.y,Size.x,Size.y);    
			
			
			// To screen point
			Vector2 mouseWorld = Event.current.mousePosition;
			mouseWorld.y = Screen.height - (mouseWorld.y + 25);
			
			
			
			//Debug.Log(" rect " +lightRect);
			//Debug.Log(" mouse " +evt.mousePosition);
			
			if(!lastMat)
				lastMat = light.LightMaterial;
			
			
			bool isOver = lightRect.Contains(mouseWorld);
			//Debug.Log(isOver);
			
			
			switch (evt.type) {
			case EventType.MouseDown:
				DragAndDrop.PrepareStartDrag();// reset data
				break;
			case EventType.MouseUp:
				// Clean up, in case MouseDrag never occurred:
				DragAndDrop.PrepareStartDrag();
				//newMat = null;
				//if(!isOver){
				//	light.LightMaterial = lastMat;
				//}
				
				break;
				
			case EventType.DragUpdated:
				if(!isOver){
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
					evt.Use();
				}else{
					foreach (object dragged_object in DragAndDrop.objectReferences) {
						newMat = (Material) dragged_object;
						light.LightMaterial = newMat;
					}
				}
				
				break;
				
				
				
			case EventType.DragPerform:
				
				DragAndDrop.AcceptDrag ();
				if(isOver){
					foreach (object dragged_object in DragAndDrop.objectReferences) {
						newMat = (Material) dragged_object;
						light.LightMaterial = newMat;
					}
					evt.Use();
				}
				break;
				
				
			}
		}
		
		public static FieldInfo LastControlIdField = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
		public static int GetLastControlId()
		{
			if (LastControlIdField == null)
			{
				Debug.LogError("Compatibility with Unity broke: can't find lastControlId field in EditorGUI");
				return 0;
			}
			return (int)LastControlIdField.GetValue(null);
		}
		
		void divider(){
			EditorGUILayout.Space();
			GUI.color = Color.black;
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			GUI.color = Color.white;
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			//GUI.color = Color.white;
		}

		
		#region SceneGUINotifications
		
		static bool allowNotificationScene = false;
		float _t = 0f;
		Color notifyColor = Color.cyan;
		static string _tmpText;
		static float _tmpTime;
		float _tmpDelay;
		public void ShowUpNotification(string text, float time = 100f, float delay = 0f){
			if (allowNotificationScene == true)
				return;
			
			_tmpText = text;
			_tmpTime = time;
			_tmpDelay = delay;
			allowNotificationScene = true;
		}
		private void showNotificationSceneGUI(){

			if (!allowNotificationScene)
				return;

			if (_tmpDelay > 0f) {
				_tmpDelay--;
				return;
			}
		


			GUIStyle notifyStyle = new GUIStyle(GUI.skin.label);
			notifyStyle.fontSize = 22;
			notifyStyle.fontStyle = FontStyle.Bold;
			notifyStyle.alignment = TextAnchor.MiddleCenter;
			notifyStyle.margin = new RectOffset(4, 4, 10, 0);
			
			
			GUI.color = notifyColor;
			Handles.BeginGUI ();
			GUILayout.BeginArea(new Rect((Screen.width*.5f)-150f, (Screen.height*.5f)-150f, 300,300));
			GUILayout.BeginVertical("box");		
			GUILayout.Label (_tmpText, notifyStyle);
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
			Handles.EndGUI ();
			GUI.color = Color.white;
			
			_t++;
			
			if (_t >= _tmpTime - 10f) {
				
				notifyColor.a -= .1f;
				
				if (_t >= _tmpTime) {
					allowNotificationScene = false;
					_t *= 0;
					notifyColor.a = 1f;
				}
			}
			
			
		}
		
		#endregion

		
	}

}