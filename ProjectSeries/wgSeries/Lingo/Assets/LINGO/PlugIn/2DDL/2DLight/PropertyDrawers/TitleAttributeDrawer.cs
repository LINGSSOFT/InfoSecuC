#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	[CustomPropertyDrawer(typeof(TitleAttribute))]
	public class DescriptionAttributeDrawer : DecoratorDrawer {
		private TitleAttribute _attributeValue = null;
		private TitleAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (TitleAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		

		public override void OnGUI(Rect position)
		{
			
			GUIStyle sty = (GUIStyle) GUI.skin.box;
			sty.normal.textColor = new Color32(180,180,180,255);
			sty.fontSize = 9;
			float space = 3f;
			GUI.Box(new Rect(position.x, position.y + (space*2), position.width, position.height - space*3 ), attributeValue.text, sty);
			
			
		}
		
		public override float GetHeight()
		{
			
			return base.GetHeight() + attributeValue.height;
		}
	}
	
}
#endif

