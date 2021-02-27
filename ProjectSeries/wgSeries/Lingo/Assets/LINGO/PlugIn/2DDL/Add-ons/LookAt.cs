namespace DynamicLight2D
{
	using UnityEngine;

	using System.Collections;

	#region Editor Section
		#if UNITY_EDITOR
		using UnityEditor;
		[CustomEditor( typeof( LookAt ) )]
		public class LookAtEditor : Editor
		{
			DynamicLight2D.LookAt lookAT;
			SerializedProperty destiny;
			GameObject GO_destiny;
			
			void OnEnable(){
				lookAT = target as DynamicLight2D.LookAt;
				destiny = serializedObject.FindProperty("target");
				GO_destiny = (GameObject)destiny.objectReferenceValue;
				
			}
			
			public override void OnInspectorGUI ()
			{
				base.OnInspectorGUI ();
			}
			
			void OnSceneGUI()
			{
				
				if(lookAT && GO_destiny){
					Handles.color = Color.gray;
					Handles.DrawDottedLine(lookAT.transform.position,GO_destiny.transform.position, 5f);
					
					Handles.color = Color.white;
					Handles.DrawWireDisc(GO_destiny.transform.position, -Vector3.forward,2f);
					Handles.DrawWireDisc(GO_destiny.transform.position, -Vector3.forward,1f);
				}
				
			}
		}
		#endif


	#endregion


	[ExecuteInEditMode] // Attribute that execute the above code while the Editor is not in playmode
	public class LookAt : AddOnBase {

		// Tags array is used for search results in inspector
		public static string []tags = {"direction", "pointer", "focus", "move"};
		
		// Brief description of behavior in this Add-on
		public static string description = "Point 2DDL Object to look another object in persistent mode";

		// The target to point
 		[FieldDescriptionAttribute("Select the target to point", "gray", "Light is looking towards 'target'")]
		public GameObject target;

	

		

		public override void Update()
		{
			if(target)
			{
				// dynamicLightInstance is the current 2D Light Object instance inherit from AddOnBase

				dynamicLightInstance.transform.up = -(transform.position - target.transform.position).normalized;
			}
		}
	}
}
