#if UNITY_EDITOR
namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	[CustomPropertyDrawer(typeof(SeparatorAttribute))]
	public class SeparatorAttributeDrawer : DecoratorDrawer {
		private SeparatorAttribute _attributeValue = null;
		private SeparatorAttribute attributeValue
		{
			get
			{
				if (_attributeValue == null)
				{
					_attributeValue = (SeparatorAttribute)attribute;
				}
				return _attributeValue;
			}
		}
		

		public override void OnGUI(Rect position)
		{
			
			float defaultHeight = position.height;
			position.height = attributeValue.height + defaultHeight;
			GUI.color = Color.black;
			GUI.Box(new Rect(position.x, position.y + (position.height*.5f), position.width, attributeValue.height), "");
			GUI.color = Color.white;
			GUI.Box(new Rect(position.x, position.y + 1.3f + (position.height*.5f), position.width, attributeValue.height), "");
			
		}
		
		public override float GetHeight()
		{
			float diff = 0;
			if(attributeValue.height > base.GetHeight()){
				diff = attributeValue.height - base.GetHeight();
				diff += 5f;
			}
			return base.GetHeight() + diff;
		}
	}
	
	
}
#endif
