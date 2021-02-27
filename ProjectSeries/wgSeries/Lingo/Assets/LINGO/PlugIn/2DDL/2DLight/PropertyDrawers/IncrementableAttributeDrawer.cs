#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	[CustomPropertyDrawer(typeof(IncrementableAttribute))]
	public class IncrementableAttributeDrawer : PropertyDrawer
	{
		
		private IncrementableAttribute _attributeValue = null;
		private IncrementableAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (IncrementableAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		
		private float propertyExtraHeight = 70f;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			float myHeight = position.height * .5f;
			float mySpace = position.height * .05f;
			
			
			int incrementDirection = 0;
			
			int buttonWidth = 40;
			
			if (GUI.Button(new Rect(position.x, position.y, buttonWidth, myHeight*.5f), ("-" + attributeValue.incrementBy)))
			{
				incrementDirection = -1;
			}
			
			if (GUI.Button(new Rect(position.width - buttonWidth, position.y, buttonWidth, myHeight*.5f), ("+" + attributeValue.incrementBy)))
			{
				incrementDirection = 1;
			}
			
			string valueString = "";
			
			if (property.propertyType == SerializedPropertyType.Float)
			{
				property.floatValue += attributeValue.incrementBy * incrementDirection;
				valueString = property.floatValue.ToString();
			}
			else if (property.propertyType == SerializedPropertyType.Integer)
			{
				property.intValue += (int)attributeValue.incrementBy * incrementDirection;
				valueString = property.intValue.ToString();
			}
			
			//EditorGUI.BeginProperty(position, label,property);
			
			
			EditorGUI.LabelField(new Rect(position.x + buttonWidth + 40, position.y, position.width - (buttonWidth * 2 + 80), myHeight*.5f), new GUIContent(property.name + ": " + valueString));
			
			
			EditorGUI.HelpBox(new Rect(position.x, position.y + myHeight*.5f + mySpace, position.width, myHeight), "msdmdf", MessageType.Info);
			
			GUI.Box(new Rect(position.x, position.y, position.width, ((position.height - myHeight*.5f) + mySpace*1.5f)),"");
			
			//EditorGUI.EndProperty();
			
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + propertyExtraHeight;
		}
	}
}
#endif

