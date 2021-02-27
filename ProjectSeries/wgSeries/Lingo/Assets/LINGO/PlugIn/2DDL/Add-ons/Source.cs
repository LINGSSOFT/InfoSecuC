namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;

#if UNITY_EDITOR
	using UnityEditor;
	[ExecuteInEditMode][SelectionBase]
#endif


	public class Source : AddOnBase {

		// Tags array is used for search results in inspector
		public static string []tags = {"emitter", "radial", "attenuation"};
		
		// Brief description of behavior in this Add-on
		public static string description = "Add additional light texture with custom color & offset";

		[FieldDescriptionAttribute("Emitter Source Texture", "cyan")][SerializeField] Texture2D sourceTex;

		[Space(20f)]
		[SerializeField] [FieldDescriptionAttribute("X Offset from 2DDL center")] float Xoffset;

		//[Space(20f)]
		[FieldDescriptionAttribute("Y Offset from 2DDL center")][SerializeField] float Yoffset;

		[Space (25f)]
		[SerializeField] [Range(.1f,6f)] float scale = 2f;

		[Space (15f)]
		[SerializeField] Color color = Color.black;

		private Color _lastColor;

		[SerializeField] [Range(1f,8f)]float intensity = 1f;
		private float _lastIntensity;

		[SerializeField][TitleAttribute("Render Sorting Order")] int order;

		GameObject sourceGO;
		Material mat;
		MeshRenderer mesh;
		MeshFilter _meshFilter;
		Color[] _colors;

		//Used for created material instance
		static Material inst = null;


		// Use this for initialization
		public override void Start () {

			Transform _t = gameObject.transform.Find("emmiter");
	
			if(!Application.isPlaying && color == Color.black)
				color = dynamicLightInstance.SolidColor ? dynamicLightInstance.LightColor : Color.white;

			if (_t == null) {
				sourceGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
				sourceGO.name = "emmiter";
				sourceGO.transform.parent = gameObject.transform;
	
				_t = sourceGO.transform;
			} 

			sourceGO = _t.gameObject ;
			mesh = sourceGO.GetComponent<MeshRenderer> ();
			mesh.sortingOrder = order;
			_meshFilter = sourceGO.GetComponent<MeshFilter> ();

			#if UNITY_EDITOR
			if(inst == null){
				inst = AssetDatabase.LoadAssetAtPath(CoreUtils.MainPath() + "Prefabs/Lights/Materials/StandardLightTexturizedMaterialAdditive.mat", typeof(Material)) as Material;
			}else{
				inst = gameObject.transform.Find("emmiter").gameObject.GetComponent<MeshRenderer>().sharedMaterial as Material;
			}
					

			mat = Instantiate(inst) as Material;
				#else
					//mat = new Material (Shader.Find(" Transparent"));
					Debug.Log("Material cannot be created in runtime mode");
					return;
				#endif
		



			sourceTex = (Texture2D)mat.mainTexture;
			mesh.sharedMaterial = mat;
		}

		
		// Update is called once per frame
		public override void Update () {

			base.Update();

			if (sourceGO) {

				sourceGO.transform.localPosition = new Vector3 (Xoffset, Yoffset, dynamicLightInstance.transform.position.z + 1);
				sourceGO.transform.localScale = new Vector3(scale * dynamicLightInstance.LightRadius, scale * dynamicLightInstance.LightRadius, 1);
			}


			if (sourceTex != null && mat != null && sourceTex != mat.mainTexture) {
					mat.mainTexture = sourceTex;
			}



			if(_colors == null)
				_colors = new Color[_meshFilter.sharedMesh.vertices.Length];

			if((_lastColor != color || _lastIntensity != intensity ) && _meshFilter.sharedMesh != null)
			{
			

				for (int i = 0; i < _colors.Length; i++)
				{
					_colors[i] = color * intensity;
				}

				_lastColor = color;
				_lastIntensity = intensity;

				if(Application.isPlaying){
					_meshFilter.mesh.colors = _colors;
				}else{ // IS EDITOR
					_meshFilter.sharedMesh.colors = _colors;
				}
			}

			if(mesh != null)
				mesh.sortingOrder = order;


		}
#if UNITY_EDITOR
		void OnGUI(){
		#if UNITY_5_6_OR_NEWER
			EditorUtility.SetSelectedRenderState(mesh, EditorSelectedRenderState.Hidden);
		#else
		EditorUtility.SetSelectedWireframeHidden(mesh, true);
		#endif
			
		}
		void OnDrawGizmos() {
			return;
		}
#endif

		void OnDestroy () {
			if (mat)
				DestroyImmediate (mat);

			if(sourceGO != null)
				DestroyImmediate(sourceGO);

			inst = null;
		}
	}
}