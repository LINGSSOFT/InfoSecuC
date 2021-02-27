#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Collections;
	using System.Reflection;
	
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonAttributeDrawer : PropertyDrawer {
		private ButtonAttribute _attributeValue = null;
		private ButtonAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (ButtonAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			
			float myHeight = position.height * .5f;
			float _Yoffset = position.y;
			
			
			_Yoffset += myHeight*.5f;
			
			if (GUI.Button (new Rect (position.x, position.y, position.width, myHeight * 2f), attributeValue.caption)) {
				Type t =  Type.GetType(attributeValue.className);
				MethodInfo method = t.GetMethod(attributeValue.methodName, BindingFlags.Static | BindingFlags.NonPublic);
				method.Invoke(null, null);
			}
			
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + .5f;
		}
		
		
	}
	
}
#endif
