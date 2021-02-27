namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	using System;
	using System.Diagnostics;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif


	[ExecuteInEditMode]
	public class ColorBlending : AddOnBase {

		// Tags array is used for search results in inspector
		public static string []tags = {"mix", "attenuation", "linear", "interpolation"};
		
		// Brief description of behavior in this Add-on
		public static string description = "Linear interpolation color effect";

		public Color colorFrom;
		public Color colorTo;


		[SerializeField]float time = 2f;
		[SerializeField] static bool run = false;
		[Space(30)]
		[ButtonAttribute("run/stop", "DynamicLight2D.ColorBlending", "changeVal")]public bool btn;

		Color deltaColor;
		double scriptTime; 
		double acumTime;
		bool lastRun;

		[ButtonAttribute("restore", "DynamicLight2D.ColorBlending", "restoreVal")]public bool btn2;
		static bool _restoreColors = false;


		static void changeVal(){
			run = !run;
		}

		static void restoreVal(){
			_restoreColors = true;
		}

		public override void Start()
		{
			base.Start();
			colorFrom = dynamicLightInstance.LightColor;
			colorTo = Color.blue;

			//if (!Application.isEditor)
				
		}

		void OnEnable()
		{
#if UNITY_EDITOR
			scriptTime = EditorApplication.timeSinceStartup;
#else
			run = true;
#endif
			recalcColor();
			lastRun = run;

		}

		void OnDisable()
		{
			#if UNITY_EDITOR
			//EditorApplication.update -= Update;
			#endif
			run = lastRun;
		}

		void recalcColor(){
			deltaColor = (colorTo - colorFrom)/ time;
			UnityEngine.Debug.Log("Color Recalculated.");
		}

		double deltat;
		public override void Update()
		{
			base.Update();

#if UNITY_EDITOR
			deltat = Application.isPlaying ? Time.deltaTime : EditorApplication.timeSinceStartup - scriptTime;
			scriptTime = EditorApplication.timeSinceStartup;
#else
			deltat = Time.deltaTime;
			scriptTime = Time.realtimeSinceStartup;
#endif

			if(_restoreColors == true){
				_restoreColors = false;
				dynamicLightInstance.LightColor = colorFrom;
				dynamicLightInstance.Refresh();
				recalcColor();
				acumTime *=0;
				deltat *=0;
			}


			if(lastRun != run && run == false)
			{
				lastRun = run;
				recalcColor();
			}

			if (!run) {
				acumTime *=0;
				return;
			}
				







			acumTime += Application.isPlaying ? (deltat) : (deltat * 1.6d);
			//UnityEngine.Debug.Log(acumTime);

			if (acumTime >= time) {
				run = false;
				acumTime *= 0;
				recalcColor();
				return;
			} else {
				dynamicLightInstance.LightColor += (deltaColor * (float)(deltat * 1.6d));
				dynamicLightInstance.Refresh ();

			}


		}

	}
}
