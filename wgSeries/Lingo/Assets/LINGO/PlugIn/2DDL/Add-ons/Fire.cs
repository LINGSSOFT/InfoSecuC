namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	
	public class Fire : AddOnBase {
		// Need inherit from AddOnBase if you need use 
		// 2DDL instance of current Light2D
		
		// Tags array is used for search results in inspector
		public static string []tags = {"candle", "torch", "flick", "blink", "fire", "emergency", "move", "flicker"};
		
		// Brief description of behavior adding with this Add-on
		public static string description = "Add fire type candle or torch light behavior";
		
		// Use this for initialization
		//[Header ("Random variation on each frame")]
		[TitleAttribute ("Magnitude of variation")]
		[Range(0f,2f)]
		public float mag = 0.5f;
		
		[TitleAttribute ("Delay flicker")]
		[Range(0f,0.5f)]
		public float delay = 0.081f;
		
		//[Header ("Intensity Vriation")]
		[TitleAttribute("Bright Variation")]
		[Range(0,1)]
		public float BrightAmount;


		[TitleAttribute("Color Blending: Blend the current Light2D color over a time \n with this new color. you need a shader with texture -> 2D Dynamic Lights/Texturized/Additive", 30f)]

		//public Color toColor = Color.white;

		
		private float lastOffset;
		private Vector2 _initUVScale;
		private Vector3 _initPos;
		private Color _fromColor;
		//private float _initAlpha;
		private Color _lColor;
		private Color _deltaColor;
		//private DynamicLight2D.DynamicLight dl;
		
		public override void Start () {
			base.Start ();
			_initUVScale = dynamicLightInstance.uv_Scale;
			_initPos = transform.position;
			_deltaPos = _initPos;
			if (dynamicLightInstance.SolidColor) {
				_fromColor = dynamicLightInstance.LightColor;
			} else {
				dynamicLightInstance.LightColor = _fromColor;
			}

			_lColor = _fromColor;//new Color(dynamicLightInstance.LightColor.r, dynamicLightInstance.LightColor.g, dynamicLightInstance.LightColor.b, dynamicLightInstance.LightColor.a);
			//_initAlpha = _lColor.a;
			//_deltaColor = toColor - _fromColor;

			StartCoroutine(updateLoop());
			
		}
		
		int cycles = 0;
		Vector2 _deltaPos;

		IEnumerator updateLoop(){


			while(true){
				yield return new WaitForSeconds(delay);
				float chance = Random.Range(0f,1f);
				float rnd = Random.Range(-0.1f,0.1f) * mag;
				
				


				
				if(cycles == 10){
					cycles *=0;
					_deltaPos = _initPos;
				}


				
				if(chance > .5f) //Only happend in 50% chance
				{

					float descentralizationMag = Random.Range(-0.1f,0.1f) * .8f;

					//Offset pos
					_deltaPos = new Vector2(_deltaPos.x + descentralizationMag, _deltaPos.y + descentralizationMag);
					transform.position = new Vector3(_deltaPos.x, _deltaPos.y, _initPos.z);

					//Color Blending
					//_lColor = _lColor + (_deltaColor * 0.08f * Random.Range(-1,2));
					_lColor.a = _fromColor.a + Random.Range(-1f,1f) * BrightAmount;
				}
				
				yield return new WaitForEndOfFrame();
				dynamicLightInstance.LightColor = _lColor;
				dynamicLightInstance.uv_Scale = new Vector2(_initUVScale.x + rnd ,_initUVScale.y + rnd );
				dynamicLightInstance.Refresh();
				cycles++;
			}
		}
		
	}

}