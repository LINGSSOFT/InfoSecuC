#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	[CustomPropertyDrawer(typeof(PopUpAttribute))]
	public class PopUpAttributeDrawer : PropertyDrawer {
		private PopUpAttribute _attributeValue = null;
		private PopUpAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (PopUpAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		
		private float propertyExtraHeight = 0f;
		private int popUpSelected = 0;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			
			float myHeight = position.height * .5f;
			//float mySpace = position.height * .05f;
			float labelWidth = 100f;
			float _Yoffset = position.y;
			
			
			_Yoffset += myHeight*.5f;
			
			EditorGUI.LabelField(new Rect(position.x, position.y, position.width, myHeight*2f), label);
			
			
			float xPos = (labelWidth + position.width *.25f)-30f;
			popUpSelected = EditorGUI.Popup(new Rect(xPos, position.y, (position.width - xPos), myHeight*2f),popUpSelected, attributeValue.texts);
			
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + propertyExtraHeight;
		}
	}
	
}
#endif
