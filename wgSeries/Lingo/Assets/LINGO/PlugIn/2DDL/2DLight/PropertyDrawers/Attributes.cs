namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	
	public class AngleAttribute : PropertyAttribute
	{
		public readonly bool radians;
		
		public AngleAttribute(bool radians)
		{
			this.radians = radians;
		}
	}
	
	public class PopUpAttribute : PropertyAttribute
	{
		public readonly string []texts;
		
		public PopUpAttribute(string []text)
		{
			this.texts = text;
		}
	}

	/// <summary>
	/// Add button to inspector.</summary>
	/// <param name="caption"> the button text</param>
	/// <param name="className">the class name with following format:Namespace.ClassName</param>
	/// <param name="methodName">the name of the method(must be static)</param>
	public class ButtonAttribute : PropertyAttribute
	{
		public readonly string caption;
		public readonly string className;
		public readonly string methodName;

		public ButtonAttribute(string caption, string className, string methodName)
		{
			this.caption = caption;
			this.className = className;
			this.methodName = methodName;

		}
	}

	public class SeparatorAttribute : PropertyAttribute
	{
		public readonly float height;
		
		public SeparatorAttribute()
		{
			this.height = 2f;
		}
	}
	
	public class TitleAttribute : PropertyAttribute
	{
		public readonly string text;
		public readonly float height;
		
		public TitleAttribute(string text)
		{
			this.text = text;
			this.height = 10f;
		}

		public TitleAttribute(string text, float height)
		{
			this.text = text;
			this.height = height;
		}
	}
	
	public class FieldDescriptionAttribute : PropertyAttribute
	{
		public readonly string text;
		public readonly string textColor;
		public readonly string GUIColor;

		// Text to show when serializedProperty will be != null
		public readonly string text2; //

		
		/// <summary>
		/// Field description attribute.
		/// <param name="text"> text to show on inspector over current field
		/// </summary>
		public FieldDescriptionAttribute(string text)
		{
			this.text = text;
			this.text2 = null;
			this.textColor = "gray";
		}
		
		/// <summary>
		/// Field description attribute.
		/// <param name="textColor"> the text color in constant values
		/// <example: "red", "yellow", "white">
		/// </summary>
		public FieldDescriptionAttribute(string text, string GUIColor)
		{
			this.text = text;
			this.text2 = null;
			this.textColor = "gray";
			this.GUIColor = GUIColor;
		}

		/// <summary>
		/// Field description attribute.
		/// <param name="text2"> text to show when serializedProperty will be != null
		/// <example: "serializedProperty has been assigned">
		/// </summary>
		public FieldDescriptionAttribute(string text, string textColor, string text2)
		{
			this.text = text;
			this.text2 = text2;
			this.textColor = textColor;
		}

		/// <summary>
		/// Field description attribute.
		/// <param name="text2"> text to show when serializedProperty will be != null
		/// <example: "serializedProperty has been assigned">
		/// </summary>
		public FieldDescriptionAttribute(string text, string textColor, string text2, string GUIColor)
		{
			this.text = text;
			this.text2 = text2;
			this.textColor = textColor;
			this.GUIColor = GUIColor;
		}


	}
}