#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	[CustomPropertyDrawer(typeof(FieldDescriptionAttribute))]
	public class FieldDescriptionAttributeDrawer : PropertyDrawer {
		private FieldDescriptionAttribute _attributeValue = null;
		private FieldDescriptionAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (FieldDescriptionAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		
		
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float space = 5f;
			GUIStyle sty = (GUIStyle) GUI.skin.box;
			sty.fontSize = 9;

			// TEXT COLOR
			switch (attributeValue.textColor) 
			{
			case "gray":
				sty.normal.textColor = Color.gray;
				break;
			case "white":
				sty.normal.textColor = Color.white;
				break;
			case "yellow":
				sty.normal.textColor = Color.yellow;
				break;
			case "green":
				sty.normal.textColor = Color.green;
				break;
			case "black":
				sty.normal.textColor = Color.black;
				break;
			case "blue":
				sty.normal.textColor = Color.blue;
				break;
			case "cyan":
				sty.normal.textColor = Color.cyan;
				break;
			case "magenta":
				sty.normal.textColor = Color.magenta;
				break;
			case "red":
				sty.normal.textColor = Color.red;
				break;
				
			default:
				sty.normal.textColor = Color.white;
				break;
			}

			Color lastGUIColor = GUI.color;

			// GUI COLOR
			switch (attributeValue.GUIColor) 
			{
			case "gray":
				GUI.color = Color.gray;
				break;
			case "white":
				GUI.color = Color.white;
				break;
			case "yellow":
				GUI.color = Color.yellow;
				break;
			case "green":
				GUI.color = Color.green;
				break;
			case "black":
				GUI.color = Color.black;
				break;
			case "blue":
				GUI.color = Color.blue;
				break;
			case "cyan":
				GUI.color = Color.cyan;
				break;
			case "magenta":
				GUI.color = Color.magenta;
				break;
			case "red":
				GUI.color = Color.red;
				break;
				
			default:
				GUI.color = Color.white;
				break;
			}


			string txt = attributeValue.text;
			if(attributeValue.text2 != null && property.objectReferenceValue != null)
			{
				txt = attributeValue.text2;
			}
		
			GUI.Box(new Rect(position.x, position.y + space, position.width,  position.height - space) ,txt, sty);
			EditorGUI.PropertyField(new Rect(position.x, position.y + space*5.5f, position.width,  position.height - space*7), property, label);
			GUI.color = lastGUIColor;
			
			
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + 35f;
		}
	}
	
}
#endif
