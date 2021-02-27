namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	using DynamicLight2D;

	[ExecuteInEditMode]
	public class Illumination : AddOnBase {
		
		// Tags array is used for search results in inspector
		public static string []tags = {"light", "diffuse", "sprite"};
		
		// Brief description of behavior in this Add-on
		public static string description = "Add Unity Point Light to 2DDL Object for illuminate sprites with diffuse materials";


		[TitleAttribute("This script add a illumination by adding a Unity Point Light. \n Remember using casters with 'Diffuse' shader like Box(iluminated)",21f)]
		[SerializeField]GameObject pointLightGO = null;

		[Range(1f, 20f)] public float IllumRadiusOffset = 1f;


		Light _pointLight_component;
		//GameObject _new_GO_PointL;

		public override void Start () {
			base.Start();

			Transform _t = gameObject.transform.Find("UnityPointLight");


			if(_t == null)
			{

				// CREATING AND SETTING GO
				pointLightGO = new GameObject("UnityPointLight");
				pointLightGO.transform.parent = gameObject.transform;
				Vector3 _p = pointLightGO.transform.position;
				_p.x = 0;
				_p.y = 0;
				_p.z = -1f;
				pointLightGO.transform.localPosition = _p;
				pointLightGO.transform.localEulerAngles = new Vector3(-90,0,0);
				pointLightGO.transform.localScale = Vector3.one;
				_t = pointLightGO.transform;


				// ADDING POINT LIGHT
				_pointLight_component = pointLightGO.transform.GetComponent<Light>();
				if(_pointLight_component == null) _pointLight_component = pointLightGO.AddComponent<Light>();

				_pointLight_component.intensity = dynamicLightInstance.Intensity;



			}

			pointLightGO = _t.gameObject;

		}


		public override void Update()
		{	

			if(_pointLight_component == null && pointLightGO != null) _pointLight_component = pointLightGO.GetComponent<Light>();
			if (_pointLight_component == null)
				return;

			// Angle
			if (this.dynamicLightInstance.RangeAngle < 180) {
				_pointLight_component.type = LightType.Spot;
				_pointLight_component.spotAngle = Mathf.Clamp(dynamicLightInstance.RangeAngle, 1f, 179f);
			} else {
				_pointLight_component.type = LightType.Point;
			}


			//Radius
			_pointLight_component.range = dynamicLightInstance.LightRadius * IllumRadiusOffset;

			//Color
			if (dynamicLightInstance.SolidColor) {
				_pointLight_component.color = dynamicLightInstance.LightColor;
				//_pointLight_component.intensity = dynamicLightInstance.Intensity + 1;
			}


		}
	
		void OnDestroy () {
			if(pointLightGO != null)
				DestroyImmediate(pointLightGO);
		}
	}
}


