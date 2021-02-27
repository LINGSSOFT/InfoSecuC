namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	using System;

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[ExecuteInEditMode]
	public class Rotation : AddOnBase {
		// Need inherit from AddOnBase if you need use 
		// 2DDL instance of current Light2D

		// Tags array is used for search results in inspector
		public static string []tags = {"motion", "rotate", "angular"};

		// Brief description of behavior in this Add-on
		public static string description = "Allow rotate current 2DDL Object in Z axis with specific speed";


		[TitleAttribute("Velocity of rotation in degrees by frame")]
		[SerializeField] [Range(0,100f)]float speed = 1f;


		[Space(20)]

		[TitleAttribute("Clockwise or counter-clockwise")]
		[SerializeField] [Range(0,1)][Tooltip("clockwise or counter-clockwise")]int direction = 1;

		[Space(20)]
	
		[ButtonAttribute("Run/Stop in Editor","DynamicLight2D.Rotation", "switchRunning" )] [SerializeField] bool btn1; 

		static bool running = false;

		private Vector3 euler;

		

		public override void Start () {
			base.Start();
		}


		static void switchRunning(){
			running = !running;
		}
		
		public override void Update () {

			base.Update();

			if(!Application.isPlaying && !running)
				return;


				euler = dynamicLightInstance.gameObject.transform.localEulerAngles;

				if(direction == 0) direction = -1;

				euler.z += speed * direction;

				dynamicLightInstance.gameObject.transform.localEulerAngles = euler;

		}


	}
}