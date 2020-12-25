namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	
	public class RegisterCaster : MonoBehaviour {

		[ButtonAttribute("register", "DynamicLight2D.RegisterCaster", "click")]
		public bool log; // just declare for show the button
		// Called when button is pressed
		static void click () {
			
			//BugReportUtils.SendReport();
		}
	}
}
