namespace DynamicLight2D
{
		#region usings
		using UnityEngine;
		using UnityEngine.Events;
		using System;
		using System.Collections;
		using System.Collections.Generic;		// This allows for the use of lists, like <GameObject>
		using System.Linq;

		#endregion


		[ExecuteInEditMode]
		[RequireComponent (typeof (MeshFilter))]
		[RequireComponent (typeof (MeshRenderer))]

		[Serializable]

		public class DynamicLight : MonoBehaviour {

				/// <summary>
				/// Store the SIN and COS for all angles between 0-360 degrees
				/// </summary>
				private struct PseudoSinCos
				{
						internal static float[] SinArray;
						internal static float[] CosArray;

						static PseudoSinCos()
						{
								SinArray = new float[360]; 
								CosArray = new float[360];
								for (int i = 0; i < 360; i++)
								{
										SinArray[i] = Mathf.Sin((float)i * 0.0174532924f);
										CosArray[i] = Mathf.Cos((float)i * 0.0174532924f);
								}
						}
				}

				/// <summary>
				/// Single Vert
				/// </summary>
				internal class Vert
				{
						public float angle; //comparable 
						public int location; /// 1= left end point    0= middle     -1=right endpoint
						public Vector3 pos;
						public bool endpoint;
				}

				/// <summary>
				/// Vert sorting comparer
				/// </summary>
				internal class Comparer : IComparer<Vert>
				{
						public int Compare(Vert x, Vert y)
						{
								if (x.angle < y.angle)
										return 1;

								else if (x.angle != y.angle)
										return -1;

								return 0;
						}
				}

				/// <summary>
				/// Holds the Verts for this collider
				/// </summary>
				internal class VertItem
				{
						internal bool lows = false; // check si hay menores a -0.5
						internal bool his = false; // check si hay mayores a 2.0
						private List<Vert> _verts = new List<Vert>();



						internal VertItem()
						{
								lows = his = false;
								_verts = new List<Vert>();
						}

						internal List<Vert> Verts
						{
								get { return _verts; }
						}

						internal int Count
						{
								get { return _verts.Count; }
						}


						internal void Clear()
						{
								lows = his = false;
								_verts.Clear();
						}

						internal void Add(Vector2 pos, bool endpoint, float angle)
						{
								_verts.Add(new Vert() { pos = pos, endpoint = endpoint, angle = angle });
						}

						internal void Sort()
						{
								_verts.Sort(Collider.VertComparer);
						}

						internal Vert this[int index]
						{
								get { return _verts[index]; }
						}
				}

				/// <summary>
				/// Collider and all associated data
				/// </summary>
				private class Collider
				{
						internal static Comparer VertComparer = new Comparer();

						private VertItem obj = new VertItem();
						private Vector3 lastPos;
						private Quaternion lastRot;
						private RaycastHit2D _ray;

						internal CasterCollider collider;
						internal Vector3[] worldPoints;
						internal Vector3 tempWorldPoint;

						/// <summary>
						/// Constructor
						/// </summary>
						/// <param name="co"></param>
						internal Collider(CasterCollider co)
						{
								collider = co;
								//Debug.Log(collider);
								worldPoints = new Vector3[collider.TotalPointsCount];
								for (int i = 0; i < collider.points.Length; i++)
										worldPoints[i] = collider.transform.TransformPoint(collider.points[i]);

								lastPos = Vector3.forward;
								lastRot = Quaternion.identity;
						}

						/// <summary>
						/// Returns true if position has been changed since last WorldPoint update
						/// </summary>
						internal bool DidTransformChange
						{
								get { 
										if(collider.transform){
												return (collider.transform.position != lastPos) || (collider.transform.rotation != lastRot); 
										}else
										{
												return true;
										}
								}
						}

						internal bool DidTransformDeleted
						{
								get { return collider == null; }
						}


						/// <summary>
						/// Updates the Worldpoints if required
						/// </summary>
						internal void UpdateWorldPoints(Vector3 lightPos, float lightRadius, int Layer, ref bool forceToUpdate)
						{
								if(!collider.collider)
										return;

								bool comeBack = true;

								if (collider.transform.position != lastPos)
										comeBack = false;

								if (collider.transform.rotation != lastRot)
										comeBack = false;

								if(forceToUpdate == true)
										comeBack = false;


								if(comeBack == true)
										return;

								for (int i = 0; i < collider.points.Length; i++){
										tempWorldPoint = collider.transform.TransformPoint(collider.points[i]);



										if(((1 << collider.transform.gameObject.layer) & Layer) != 0){
												worldPoints[i] = tempWorldPoint;
										}

								}

								// CHECK IF RADIUS COLLIDE WITH ANY OTHER COLLIDER
								// IF IT IS, THEN CALCULATE THAT POINT AND STORE IT

									
								


								lastPos = collider.transform.position;
								lastRot = collider.transform.rotation;
						}

						/// <summary>
						/// Gets the worldpoint at the requested index
						/// </summary>
						/// <param name="index"></param>
						/// <returns></returns>
						internal Vector3 this[int index]
						{
								get { return worldPoints[index]; }
						}

						/// <summary>
						/// 
						/// </summary>
						/// <param name="lightObjectTM"></param>
						/// <param name="LightRadius"></param>
						/// <param name="sortAngles"></param>
						/// <returns></returns>
						internal VertItem GetVerts(Transform lightObjectTM, float LightRadius, float RangeLightAngle,  ref bool sortAngles, int _layerMask, float _magRange, ref bool notifyReachedEventUsed, ref List<GameObject> ListObjectsReached, ref bool Debugging ,bool forceToUpdatePoints ) // still to expensive.. improve
						{

								// refresh all worldPoints if something has changed
								bool forceCalc = notifyReachedEventUsed || forceToUpdatePoints;
								UpdateWorldPoints(lightObjectTM.position,LightRadius, _layerMask, ref forceCalc);

								// empty out the old object
								obj.Clear();
								Vector2 tempWorld2d;
								Vector2 tempLight2d = (Vector2) lightObjectTM.position;

								GameObject tmpGOWithinFOV = null;


								for (int i = 0; i < worldPoints.Length; i++)
								{                  // ...and for ever vertex we have of each mesh filter...

										if(worldPoints[i].z != lightObjectTM.position.z)
												continue;

										tempWorld2d = (Vector2) worldPoints[i];
										if((tempWorld2d - tempLight2d).sqrMagnitude > (LightRadius*LightRadius))
												continue;

										// instead of a Vert let's use basic types on the stack and construct the vert in the VertItem's Add method

										Vector3 pos;
										bool endpoint = false;
										float angle;

										// Reforma fecha 24/09/2014 (ultimo argumento lighradius X worldPoint.magnitude (expensivo pero preciso))
										//Vector2 dir = (Vector2)this[i] - (Vector2)lightObjectTM.position;
										var _to = (Vector2)worldPoints[i] - (Vector2)lightObjectTM.position;
										float _toMag = _to.magnitude;
										_ray = Physics2D.Raycast(lightObjectTM.position, _to,_toMag , _layerMask);

										if (_ray)
										{
												pos = _ray.point;
												Vector2 worldLocal = lightObjectTM.InverseTransformPoint(worldPoints[i]);
												Vector2 rayLocal = lightObjectTM.InverseTransformPoint(_ray.point);

												// USING RELATIVE RANGE COMPARATIVE MINIMIZE FLICKER ERROR WHE POS > 400 //
												//bug18747 - flickerPosX450//
												float rangeRange = _magRange + _toMag*.1f;


												// This lines are distint between point that have same direction, but different magnitude //
												//if (((Vector2)worldPoints[i]).sqrMagnitude >= (_ray.point.sqrMagnitude - _magRange) && ((Vector2)worldPoints[i]).sqrMagnitude <= (_ray.point.sqrMagnitude + _magRange))
												if (worldLocal.sqrMagnitude >= (rayLocal.sqrMagnitude - rangeRange) && worldLocal.sqrMagnitude <= (rayLocal.sqrMagnitude + rangeRange))
														endpoint = true;


												if(notifyReachedEventUsed)
												{
														if(360 != Mathf.RoundToInt(RangeLightAngle)){ 
																if (Vector3.Angle(lightObjectTM.InverseTransformPoint(pos), Vector3.up) < RangeLightAngle*.5f) {	// Light angle restriction
																		if(_ray.collider.gameObject.transform.parent){
																			if(tmpGOWithinFOV == null) tmpGOWithinFOV = _ray.collider.gameObject.transform.parent.gameObject;
																		}else{
																			if(tmpGOWithinFOV == null) tmpGOWithinFOV = _ray.collider.gameObject;
																		}
																}
														}else{
																if(_ray.collider.gameObject.transform.parent){
																	if(tmpGOWithinFOV == null) tmpGOWithinFOV = _ray.collider.gameObject.transform.parent.gameObject;
																}else{
																	if(tmpGOWithinFOV == null) tmpGOWithinFOV = _ray.collider.gameObject;
																}
														}
												}				
										}
										else
										{
											pos = (Vector2)worldPoints[i]; // hmsdmm
											endpoint = true;


											//--------IF HIT doesn't exist but the vertice is so close (this is because raycast unity is low accurate) -------
											// Events only works properly over BoxCollider2D and PolygonCollider2D --//
											if(notifyReachedEventUsed && this.collider.type == CasterCollider.CasterType.PolygonCollider2d || this.collider.type == CasterCollider.CasterType.BoxCollider2d)
											{

													//foreach(Vector2 objPoint in this.collider.points)
													for(int k = 0; k < this.collider.points.Length; k++)
													{
														if(360 != Mathf.RoundToInt(RangeLightAngle)){ 
															if (Vector3.Angle(lightObjectTM.InverseTransformPoint(pos), Vector3.up) < RangeLightAngle*.5f) {	// Light angle restriction
																if(((Vector2)pos - this.collider.points[k]).sqrMagnitude < 500f)
																{	tmpGOWithinFOV = this.collider.transform.gameObject;
																	//Debug.Log("FOUND !!  " + this.collider.transform.gameObject.name);
																	break;
																}
															}
														}else
														{
															if(((Vector2)pos - this.collider.points[k]).sqrMagnitude < 500f)
															{	tmpGOWithinFOV = this.collider.transform.gameObject;
																//Debug.Log("FOUND !!  " + this.collider.transform.gameObject.name);
																break;
															}
														}


													}

												}

												//--------------
													
										}



										pos = new Vector3(pos.x,pos.y, lightObjectTM.position.z);
										pos = lightObjectTM.InverseTransformPoint(pos); 
										angle = PseudoAngle(pos.x, pos.y);



										// -- bookmark if an angle is lower than 0 or higher than 2f --//
										//-- helper method for fix bug on shape located in 2 or more quadrants
										if (angle < 0f)
												obj.lows = true;

										if (angle > 2f)
												obj.his = true;

										//--Add verts to the main array
										//-- AVOID EXTRA CALCULOUS OF Vector3.angle --//

										if(360 != Mathf.RoundToInt(RangeLightAngle)){ 
												if (Vector3.Angle(pos, Vector3.up) < RangeLightAngle*.5f) {	// Light angle restriction
														if((pos).sqrMagnitude <= LightRadius*LightRadius){
																obj.Add(pos, endpoint, angle); // tempVerts.Add(v); // expensive
																if(Debugging == true)
																		Debug.DrawLine(lightObjectTM.position, lightObjectTM.TransformPoint(pos), Color.white);
														}
												}
										}else{
												if((pos).sqrMagnitude <= LightRadius*LightRadius){
														obj.Add(pos, endpoint, angle); //tempVerts.Add(v);
													// DEBUG Lines  ---- 
													if(Debugging == true){
														Debug.DrawLine(lightObjectTM.position, lightObjectTM.TransformPoint(pos), Color.white);	
													}
												}
										}




								}// end verts loop

								// Add GO inside in LightMesh to array of reached objects
								if(tmpGOWithinFOV != null){
									bool found = false;
									//First check if exist
									for (int i=0; i < ListObjectsReached.Count; i++){
										if(tmpGOWithinFOV.GetHashCode() == ListObjectsReached[i].GetHashCode())
										{
											found = true;
											break;
										}
									}
									// Adding
									if(!found)
										ListObjectsReached.Add(tmpGOWithinFOV);

								}

										

								if (obj.Verts.Count > 0) {
										// sort vertices
										sortAngles = true;
										obj.Sort();
								}

								return obj;
						}

				}

				#region public Fields

				/// <summary>
				/// Occurs when on reached game objects.
				/// </summary>
				[Obsolete("OnReachedDelegate is deprecated, please use InsideFieldOfViewDelegate instead.")]
				public delegate void OnReachedDelegate(GameObject[] go);
				public delegate void InsideFieldOfViewDelegate(GameObject[] go, DynamicLight Light);
				
				public delegate void OnEnterFieldOfViewDelegate(GameObject go, DynamicLight Light);
				public delegate void OnExitFieldOfViewDelegate(GameObject go, DynamicLight Light);



				/// <summary>
				/// Suscribe to this event for receive array of game
				/// </summary>
				//	[Obsolete("OnReachedGameObjects is deprecated, please use InsideFieldOfViewEvent instead.")]
				//	public event OnReachedDelegate OnReachedGameObjects;
				public event InsideFieldOfViewDelegate InsideFieldOfViewEvent;

				public event OnEnterFieldOfViewDelegate OnEnterFieldOfView;
				public event OnExitFieldOfViewDelegate OnExitFieldOfView;


				public DynamicLightEvent DDLEvent_OnEnterFOV;
				public DynamicLightEvent DDLEvent_OnExitFOV;
				public DynamicLightEvent DDLEvent_InsideFOV;
 

				/// <summary>
				/// The version in string format.
				/// </summary>
				public string Version;

				/// <summary>
				/// If light uses solid color.
				/// </summary>
				public bool SolidColor = false;

				/// <summary>
				/// The color of the light.
				/// </summary>
				public Color LightColor;

				/// <summary>
				/// The intensity.
				/// </summary>
				public float Intensity = 1f;
			
				/// <summary>
				/// The light material.
				/// </summary>
				public Material LightMaterial;

				public enum Light2DType
				{
					Static,
					Dynamic,
					OneFrame,
					OnlySight
				}
				/// <summary>
				/// The type of light.
				/// Static = need all casters in scene before press play. You can add casters after play but you need notify to the light.
				/// Dynamic = let add casters an any time.
				/// OnlySight = compute visibility alghrythm only. (no rendering. Free CPU and GPU cycles).
				/// OneFrame = only render the light the firs time and then deactive itself (use for static backgrounds)
				/// </summary>
				public Light2DType light2DType;

				/// <summary>
				/// The sort order id.
				/// </summary>
				public int SortOrderID;

				/// <summary>
				/// Stroke revealed when render as stencil shader.
				/// Is a offset of collider towards to direction 2DLight -> caster.
				/// </summary>
				public float strokeRender = 0;

				/// <summary>
				/// The light radius.
				/// </summary>
				public float LightRadius = 20f;

				/// <summary>
				/// The segments of light mesh.
				/// </summary>
				public int Segments = 9;

				/// <summary>
				/// The layer that light uses for interact with casters.
				/// </summary>
				public LayerMask Layer; // Mesh for our light mesh

				/// <summary>
				/// Enabled suscription to events OnEnter, OnExit and OnInside.
				/// </summary>
				public bool useEvents = false;

				/// <summary>
				/// Persist Callback all caster within sight of current 2DLight. "Require useEvents = true"
				/// </summary>
				public bool objectsWithinSight = false;

				/// <summary>
				/// Use intellider convex (mesh optimizer).
				/// </summary>
				public bool intelliderConvex = true;


				/// <summary>
				/// forces to reload light mesh in the next frame update
				/// </summary>
				public static bool reloadMeshes = false;


				/// <summary>
				/// Forces to recalculate normals in the mesh build.
				/// </summary>
				public bool recalculateNormals = false;


				public bool flipXYtoXZ = false;

				/// <summary>
				/// Show debug lines
				/// </summary>
				public bool debugLines = false;

				/// <summary>
				/// The sort order.
				/// </summary>
				//public int SortOrder = 2;


				public Vector2 uv_Offset = new Vector2(.5f,.5f);

				public Vector2 uv_Scale = new Vector2(.5f,.5f);



				[HideInInspector] public int vertexWorking;
				public float RangeAngle = 360f;

				#endregion


				#region private Fields


				/// <summary>
				/// Which Layers to check for ray cast intersections
				///   move this into singleton lightManager to be used by all lights
				/// </summary>
				//private static int _layerMask = LayerMask.GetMask("ShadowCasters", "foo");

				/// <summary>
				/// Array for all of the vertices in our meshes
				/// </summary>
				private List<Vert> _allVerts;

				/// <summary>
				/// Array for all of the meshes in our scene
				/// </summary>
				private CasterCollider[] _allMeshes;

				/// <summary>
				/// The max n verts a single collider can have
				/// </summary>
				private int _maxVertsPerObj;

				/// <summary>
				/// the total ammount of verts for all colliders
				/// </summary>
				private int _maxTotalVerts;

				/// <summary>
				/// Holds all the colliders and stuff so we don't have to get them over and over again
				/// </summary>
				private Collider[] _colliders;

				/// <summary>
				/// This GOs last updated position
				/// </summary>
				private Vector3 _lastPos = Vector3.forward;

				/// <summary>
				/// This GOs last updated rotation
				/// </summary>
				private Quaternion _lastRot = Quaternion.identity;

				/// <summary>
				/// This GOs last updated radius
				/// </summary>
				private float _lastRadius = 0f;

				/// <summary>
				/// 
				/// </summary>
				private RaycastHit2D _ray;

				/// <summary>
				/// Allow sorting ?
				/// </summary>
				private bool _sortAngles;

				/// <summary>
				/// Force to manually regenerate light mesh 
				/// </summary>
				private bool _forceToRefresh;

				/// <summary>
				/// Don't check for new colliders every frame.
				/// </summary>
				public bool staticScene = true;		// this is for load meshes only in start function

				/// <summary>
				/// The do not render the Light mesh. Use only in combination with useEvents = true for perform sight without light
				/// </summary>
				public bool doNotRenderMesh = false;

				/// <summary>
				/// delta angle range comparison between 2 closer points when sweep vertices (used for contrast magnitude)
				/// </summary>
				private float _magRange = 0.01f;//25.15f; //last value 0.15f;

				/// <summary>
				/// Mesh for our light mesh
				/// </summary>
				private Mesh _lightMesh;

				/// <summary>
				/// MeshFilter for our light mesh
				/// </summary>
				private MeshFilter _meshFilter;

				// <summary>
				/// MeshRenderer for our light mesh
				/// </summary>
				private MeshRenderer _meshRenderer;


				///<summary>
				/// take control when coroutine has finished.
				/// </summary>
				private bool _EnumeratorOnEnterOnExitIsRunning = false;

				/// <summary>
				/// The last type of the light2D.
				/// </summary>
				public Light2DType lastLight2DType;

				#endregion


				// List <Vert> tempVert = new List<Vert>(); // -- temporal vertices of whole mesh --//

				/// <summary>
				/// Temporal store of casters inside on field of view.
				/// </summary>
				List<GameObject> objReached = new List<GameObject>(); // -- save all GO reache by current light --//

				/// <summary>
				/// Watched list of casters for
				/// dispatch events like OnEnter OnExit
				/// </summary>
				List<GameObject> listObjectsInFOV = new List<GameObject>();

				/// <summary>
				/// List use for take count any caster IN or OUT (is a kind of temp Database)
				/// </summary>
				List<GameObject> ToChange = new List<GameObject>();


				/// <summary>
				/// delta angle range comparison between 2 points with same angle
				/// </summary>
				//private const float _magRange = 0.15f;


				/// <summary>
				/// Enumerator as Main update loop
				/// </summary>
				internal IEnumerator e;


//				#if UNITY_EDITOR
//				void OnDrawGizmos() {
//					Gizmos.DrawIcon(transform.position, "logo2DDL_gizmos.png", false);
//				}
//				#endif


				public Vector2 getMaxFromAllVerts(){
						return ((_allVerts != null && _allVerts.Count > 0) ? _allVerts[_allVerts.Count-1].pos : Vector3.zero);
				}

				public Vector2 getMinFromAllVerts(){
						return (_allVerts != null && _allVerts.Count > 0) ? _allVerts[0].pos : Vector3.zero;
				}

				/// <summary>
				/// Sets the main material.
				/// </summary>
				/// <param name="m">M.</param>
				public void setMainMaterial(Material m){
					LightMaterial = m;
					GetComponent<MeshRenderer>().sharedMaterial = LightMaterial;
				}

				/// <summary>
				/// Sets the layer mask.
				/// </summary>
				public void setLayerMask(){
						#if UNITY_EDITOR
						if(!Application.isPlaying && Layer.value <= 0){
								//Layer = 1<< LayerMask.NameToLayer("ShadowLayer");
								Layer = LayerMask.GetMask("ShadowLayer-2DDL");
						}
						#endif
				}


				/// <summary>
				/// Gets the layer number from layer mask.
				/// </summary>
				/// <returns>The layer number from layer mask.</returns>
				public int getLayerNumberFromLayerMask(){
						int layerNumber = 0;
						int layer = Layer;
						while(layer > 0)
						{
								layer = layer >> 1;
								layerNumber++;
						}
						layerNumber -=1;
						return (layerNumber);
				}
				/// <summary>
				/// Gets the layer number from layer mask.
				/// </summary>
				/// <returns>The layer number from layer mask.</returns>
				/// <param name="layerValue from layerMask.value">Layer value.</param>
				public int getLayerNumberFromLayerMask(int layerValue){
						int layerNumber = 0;
						int layer = layerValue;
						while(layer > 0)
						{
								layer = layer >> 1;
								layerNumber++;
						}
						layerNumber -=1;
						return (layerNumber);
				}

				/// <summary>
				/// Sets the internal _MagRange.
				/// </summary>
				/// <param name="mag">Mag.</param>
				public void setMagRange(float mag){
						_magRange = mag;
				}


		private void initMeshFilterAndRenderer(){

			_meshRenderer = GetComponent<MeshRenderer>();
			if (_meshRenderer==null){
				_meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;    // Add a Mesh Renderer component to the light game object so the form can become visible
			}
			_meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			#if UNITY_5_3_OR_NEWER
			_meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
			#else
			_meshRenderer.useLightProbes = false;
			#endif

			//--mesh filter--//
			_meshFilter = GetComponent<MeshFilter>();
			if (_meshFilter==null){
				_meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
				return;
			}

			if (_meshFilter.sharedMesh != null)
				DestroyImmediate(_meshFilter.sharedMesh);
		}

				/// <summary>
				/// Rebuild all mesh.
				/// </summary>
				public void Rebuild () {

					if (_meshFilter.sharedMesh == null)
					{
						_meshFilter.sharedMesh = new Mesh();
						_meshFilter.sharedMesh.name = "2DDL Mesh";
					}
					
					_meshFilter.sharedMesh.Clear();

					reloadMeshes = true;

					// Allow regenerate the light structure //
					_forceToRefresh = true;
					GetCollidersOnScene();

				}
				
				/// <summary>
				/// Update 2DLight object position in sync with render process
				/// </summary>
				public void setPositionSync(Vector3 pos){
					StartCoroutine (_setPosSync(pos));
				}
				private IEnumerator _setPosSync(Vector3 p){
					yield return new WaitForEndOfFrame ();
					transform.position = p;
				}


				/// <summary>
				/// Update 2DLight object Rotation in sync with render process
				/// </summary>
				public void setRotationSync(Vector3 euler){
					StartCoroutine (_setRotSync(euler));
				}
				public void setRotationSync(Quaternion rotation){
					StartCoroutine (_setRotSync(rotation.eulerAngles));
				}
				private IEnumerator _setRotSync(Vector3 e){
					e.x = 0;
					e.y = 0;
					yield return new WaitForEndOfFrame ();
					transform.localEulerAngles = e;
				}
				

				/// <summary>
				/// Force to build lightmesh again with know collider verts (less expensive than Rebuild method that also scan for new casters).
				/// </summary>
				public void Refresh () {
					_forceToRefresh = true;
				}

				private void Awake()
				{
					initMeshFilterAndRenderer();
					Rebuild();
				}

				private void Start(){

						_lastPos = transform.position;
						_lastRot = transform.rotation;
						_lastRadius = LightRadius;


						// -- Set Layer mask --//
						setLayerMask();

						//initMeshFilterAndRenderer();

						// -- Set mesh filter , mesh renderer and transform stuff --//
						Rebuild();

						//-- Get all Collider2D --//
						GetCollidersOnScene(); 

				}



				/// <summary>
				/// Get all vertices, and init arrays.
				/// </summary>
				public void GetCollidersOnScene()
				{

						// Get all box2D colliders
						BoxCollider2D []_allBoxes = FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];

						PolygonCollider2D [] _allPolygons = FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];

						CircleCollider2D [] _allCircles = FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];

						EdgeCollider2D [] _allEdges = FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];

						//CasterCollider c = new CasterCollider(_allCircles[0], (Vector2)transform.position);


						_allMeshes = new CasterCollider[_allBoxes.Length + _allPolygons.Length + _allCircles.Length + _allEdges.Length];

						///int j2 = 0;
						//int _counter = 0;

						// -- BOXES --//
						for(int j = 0; j < _allBoxes.Length; j++){
								if(_allBoxes[j].enabled)
									_allMeshes[j] = new CasterCollider(_allBoxes[j]);
						}
						
						
						// -- Polygons --//	
						for(int j = _allBoxes.Length; j < (_allBoxes.Length + _allPolygons.Length); j++){
							if(_allPolygons[j - _allBoxes.Length].enabled)	
								_allMeshes[j] = new CasterCollider(_allPolygons[j - _allBoxes.Length]);
						}



						// -- Circles --//
						for(int j = _allBoxes.Length + _allPolygons.Length; j < (_allBoxes.Length + _allPolygons.Length + _allCircles.Length); j++){
							if(_allCircles[j - (_allBoxes.Length + _allPolygons.Length)].enabled)	
								_allMeshes[j] = new CasterCollider(_allCircles[j - (_allBoxes.Length + _allPolygons.Length)], (Vector2)transform.position);
						}

						// -- Edges --//
						for(int j = _allBoxes.Length + _allPolygons.Length+ _allCircles.Length; j < (_allBoxes.Length + _allPolygons.Length + _allCircles.Length + _allEdges.Length); j++){
							if(_allEdges[j - (_allBoxes.Length + _allPolygons.Length + _allCircles.Length)].enabled)	
								_allMeshes[j] = new CasterCollider(_allEdges[j - (_allBoxes.Length + _allPolygons.Length + _allCircles.Length)]);
						}


						//Resize array
						_allMeshes = _allMeshes.Where(c => c != null).ToArray();

						_maxTotalVerts = _allMeshes.Sum(p => p.getTotalPointsCount());
						
						_allVerts = new List<Vert>(_maxTotalVerts); // let's preAllocate thte max ammount so we don't have to wait for expansions later on

						_colliders = new Collider[_allMeshes.Length];

						// Construct the colliders
						for (int i = 0; i < _allMeshes.Length; i++)
								_colliders[i] = new Collider(_allMeshes[i]);
				}

				/// <summary>
				/// Gets the colliders on scene non static.
				/// </summary>

				public void GetCollidersOnSceneNonStatic()
				{
						Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, LightRadius, Layer);

						// redim _allmeshes array
						_allMeshes = new CasterCollider[cols.Length];

						for (int i=0; i<_allMeshes.Length; i++) {
								if (cols[i] is BoxCollider2D)
										_allMeshes[i] = new CasterCollider((BoxCollider2D)cols[i]);
								else if (cols[i] is PolygonCollider2D)
										_allMeshes[i] = new CasterCollider((PolygonCollider2D)cols[i]);
								else if (cols[i] is CircleCollider2D)
										_allMeshes[i] = new CasterCollider((CircleCollider2D)cols[i],(Vector2)transform.position);
								else if (cols[i] is EdgeCollider2D)
										_allMeshes[i] = new CasterCollider((EdgeCollider2D)cols[i]);
						}

						_maxTotalVerts = _allMeshes.Sum(p => p.getTotalPointsCount());

						_allVerts = new List<Vert>(_maxTotalVerts); // let's preAllocate thte max ammount so we don't have to wait for expansions later on

						_colliders = new Collider[_allMeshes.Length];

						// Construct the colliders
						for (int i = 0; i < _allMeshes.Length; i++)
								_colliders[i] = new Collider(_allMeshes[i]);
				}

				/// <summary>
				/// Adds specific collider to scene (work with static scenes).
				/// </summary>
				public void addColliderToScene(Collider2D newColl){
						// for each collider
						// resize_allmeshes
						// conert to caster collider
						// add to main array


						CasterCollider newCasterCollider; // = new CasterCollider();

						if(newColl.GetType() == typeof(PolygonCollider2D)){
								newCasterCollider = new CasterCollider((PolygonCollider2D) newColl);
						}else if(newColl.GetType() == typeof(BoxCollider2D)){
								newCasterCollider = new CasterCollider((BoxCollider2D) newColl);
						}else if(newColl.GetType() == typeof(CircleCollider2D)){
								newCasterCollider = new CasterCollider((CircleCollider2D) newColl, (Vector2)transform.position);
						}else{
								//EdgeCollider2D
								newCasterCollider = new CasterCollider((EdgeCollider2D) newColl);
						}


						_maxTotalVerts += newCasterCollider.getTotalPointsCount();



						//resize _colliders array
						Collider[] newColliders = new Collider[_colliders.Length + 1];



						for(int i=0; i< newColliders.Length-1; i++){
								newColliders[i] = _colliders[i];
						}
						newColliders[newColliders.Length-1] = new Collider(newCasterCollider);

						_colliders = newColliders;

						_forceToRefresh = true;

				}


				// ----------- MAIN UPDATE ----------- //
				private void LateUpdate()
				{
					if (_meshFilter.sharedMesh == null) {
						Rebuild ();
						return;
					}
							

			// ------------- TYPE OF LIGHT ---------------------- //
					if (light2DType != Light2DType.Dynamic && lastLight2DType != Light2DType.Dynamic)
						staticScene = true;
					 else
						staticScene = false;
					

					if (light2DType == Light2DType.OnlySight && lastLight2DType != Light2DType.OnlySight) {
						doNotRenderMesh = true;
						useEvents = true;
						Rebuild ();
					}else if(light2DType != Light2DType.OnlySight)
						doNotRenderMesh = false;
						
					if (light2DType == Light2DType.OneFrame && Application.isPlaying)
						StartCoroutine (deactivateItself (.2f));
						

					lastLight2DType = light2DType;
			//-----------------------------------------------------//
				
				#if UNITY_EDITOR
						//Adding listeners
						if(permissionToAddListeners == true)
						{
							permissionToAddListeners = false;
							AddListeners();
						}

				#endif
						fixedLimitations();

						//Sorting
						if(_meshRenderer.sortingOrder != SortOrderID) _meshRenderer.sortingOrder = SortOrderID;


						// Reaload _allMeshes when is in editor mode //
						if(!Application.isPlaying){
								GetCollidersOnScene();
								_forceToRefresh = true;

						}else{
								if(staticScene == true){
										// Only reload when is notificated 
										if(reloadMeshes == true){
												GetCollidersOnScene();
												reloadMeshes = false;
										}
								}else{
										//GetCollidersOnScene();
										GetCollidersOnSceneNonStatic();
								}
						}
						//--------------------------------------------------
						ColliderState anyColliderChange = DidAnyColliderTMChange();




						if (anyColliderChange == ColliderState.HasBeenDeleted) {
								reloadMeshes = true;
								_forceToRefresh = true;
								return;
						}


						if ((anyColliderChange != ColliderState.NoChanged) || (transform.position != _lastPos) || (transform.rotation != _lastRot) || (_forceToRefresh) || (LightRadius != _lastRadius))
						{

								// Restore force to reload all meshes //
								_forceToRefresh = false;
								_lastPos = transform.position;
								_lastRot = transform.rotation;
								_lastRadius = LightRadius;


								// First check if any collider has been removed //
								if (anyColliderChange == ColliderState.HasBeenDeleted)
								{	reloadMeshes = true;
										_forceToRefresh = true;
										
								}


								// since this is rather expensive let's chop it up
								_allVerts.Clear();// Since these lists are populated every frame, clear them first to prevent overpopulation

								// Reset List GameObjects touched by light
								resetReachedEvent();


								// Obtain vertices for each mesh
								GenerateColliderVerts();

								// Generate vectors for light mesh
								GenerateLightVectors();
							
								// Reorder the verts based on vector angle
								SwapVertOrders(ref _allVerts);

								// Render the final mesh wit allvert collected			
								RenderLightMesh();

								// reset mesh bounds
								ResetBounds();
						}			

				}


				private IEnumerator deactivateItself(float t){
					yield return new WaitForSeconds (t);
					GetComponent<DynamicLight> ().enabled = false;
				}



				private enum ColliderState{
						NoChanged,
						HasChanged,
						HasBeenDeleted
				}

				/// <summary>
				/// Checks if any of the colliders transformation has changed
				/// </summary>
				/// <returns>0=false</returns>
				/// <returns>1=true</returns>
				/// <returns>-1=killed</returns>
				private ColliderState DidAnyColliderTMChange()
				{

						for (int i = 0; i < _colliders.Length; i++){

								if(_colliders[i].collider.transform == null)
										return ColliderState.HasBeenDeleted;

								if(_colliders[i].DidTransformDeleted)
										return ColliderState.HasBeenDeleted; // has been deleted


								if (_colliders[i].DidTransformChange)
										return ColliderState.HasChanged; // has been changed

						}

						return ColliderState.NoChanged;
				}


				/// <summary>
				/// Reset the center of game object pivot
				/// </summary>
				private void ResetBounds()
				{
						var b = _meshFilter.sharedMesh.bounds;
						b.center = Vector3.zero;
						_meshFilter.sharedMesh.bounds = b;
				}


				private void addingObjectToObjectReachedList(List<GameObject> _listToAdd, GameObject _object)
				{
					bool found = false;
					//First check if exist
					for (int i=0; i < _listToAdd.Count; i++){
						if(_listToAdd[i].GetHashCode() == _object.GetHashCode())
						{
							found = true;
							break;
						}
					}
					if(!found)
						_listToAdd.Add(_object);
				
				}

				/// <summary>
				///  Obtain vertices for each mesh-collider
				/// </summary>

				private VertItem obj;
				private void GenerateColliderVerts()
				{
						// las siguientes variables usadas para arregla bug de ordenamiento cuando
						// los angulos calcuados se encuentran en cuadrantes mixtos (1 y 4)

						int worldCount = 0;

						for (int m = 0; m < _colliders.Length; m++)
						{
								// Avoid Error when call destroy(light2d) with collider2d attached to it.
								if (_colliders[m].collider.transform == null)
										return;


								//Layer restriction //
								if(((1 << _colliders[m].collider.transform.gameObject.layer) & Layer) == 0)
										continue;


								bool forceToUpdatePoints = false;		
								if(_colliders[m].collider.type == CasterCollider.CasterType.CircleCollider2d){
										// Recalc the tangents points
										_colliders[m].collider.recalcTan((Vector2) transform.position);
										forceToUpdatePoints = true;
								}



								obj = _colliders[m].GetVerts(transform, LightRadius, RangeAngle, ref _sortAngles, Layer, _magRange, ref useEvents, ref objReached, ref debugLines, forceToUpdatePoints);


							
								//check if collide with othre collider
								//	*--------*-*-*-*-*-*-*
									/*					
								for(int i = 0; i < _colliders[m].worldPoints.Length; i++)
								{
									Vector2 pA = _colliders[m].worldPoints[i];
									Vector2 pB = _colliders[m].worldPoints[(i+1)%_colliders[m].worldPoints.Length];
									Vector2 pC = (Vector2)transform.position;



									//CircleLine.intersectLineCirc(pA,pB,pC,LightRadius);
								}

								*/



								if(obj.Verts.Count <= 0) continue; 


								// DEBUG: how many vertices is working //
								if(debugLines)
									worldCount += _colliders[m].worldPoints.Count();


								//SwapEndPointLocation(obj);


								// Indentify the endpoints (left and right)
								if (obj.Count > 0)
								{


									bool Cuadrant0 = false;
									bool Cuadrant1 = false;
									bool Cuadrant2 = false;
									bool Cuadrant3 = false;
									for (int d = 0; d < obj.Count; d++)
									{

										if(!Cuadrant0)
											Cuadrant0 = !Cuadrant0 && (obj[d].angle > -1 && obj[d].angle < 0);

										if(!Cuadrant1)
											Cuadrant1 = (obj[d].angle > 0 && obj[d].angle < 1);

										if(!Cuadrant2)
											Cuadrant2 = (obj[d].angle > 1 && obj[d].angle < 2);

										if(!Cuadrant3)
											Cuadrant3 = (obj[d].angle > 2 && obj[d].angle < 3);

									}

										int posLowAngle = 0; // save the indice of left ray
										int posHighAngle = 0; //

										if (obj.his && obj.lows)
										{  //-- FIX BUG OF SORTING CUANDRANT 1-4 --//
												float lowestAngle = -1f;//tempVerts[0].angle; // init with first data
												float highestAngle = obj[0].angle;


												for (int d = 0; d < obj.Count; d++)
												{

														


														if (obj[d].angle < 1f && obj[d].angle > lowestAngle)
														
														{
																lowestAngle = obj[d].angle;
																posLowAngle = d;
														}

														if (obj[d].angle > 2f && obj[d].angle < highestAngle)
														
														{
																highestAngle = obj[d].angle;
																posHighAngle = d;
														}

														if (obj[d].angle > 2f && obj[d].angle < highestAngle)
															
														{
															highestAngle = obj[d].angle;
															posHighAngle = d;
														}
												}

												/// issue 2
												if(Cuadrant0 == true && Cuadrant1 == false && Cuadrant2 == true && Cuadrant3 == true)
												{
													float minAngle = 2;
													int idMin = 0;
													for (int d = 0; d < obj.Count; d++)
													{
														if (obj[d].angle > 1f &&  obj[d].angle < minAngle){
															minAngle = obj[d].angle;
															idMin = d;
														}
														
													}
													posHighAngle = idMin;
												}
												
										}
										else if (obj.his && !obj.lows)
										{
											/// issue 3
											if(Cuadrant0 == false && Cuadrant1 == true && Cuadrant2 == false && Cuadrant3 == true)
											{
												float minAngle = 3f;
												float maxAngle = 0f;
												int idMin = 0;
												int idMax = 0;
												///bool cuadrant1HighMiddle = false; // know is angle between 0 and 1 is higher than .5f
												//bool cuadrant3HighMiddle = false; // know is angle between 2 and 3 is higher than 2.5f


												for (int d = 0; d < obj.Count; d++)
												{
													if (obj[d].angle > 2f &&  obj[d].angle < minAngle){
														minAngle = obj[d].angle;
														idMin = d;
														//if(obj[d].angle >= 2.5f) cuadrant3HighMiddle = true;
													}
													
													if (obj[d].angle < 1f &&  obj[d].angle > maxAngle){
														maxAngle = obj[d].angle;
														idMax = d;
														//if(obj[d].angle >= .5f) cuadrant1HighMiddle = true;
													}
												}

													posLowAngle = 0;
													posHighAngle = obj.Count - 1;

													Vert resultante = new Vert();
													resultante.pos = obj[posLowAngle].pos;
													Vector2 dir = (obj[posHighAngle].pos - obj[posLowAngle].pos);

													float mag = dir.magnitude *.5f;
													dir.Normalize();

													//resultante.pos += (Vector3) (dir*mag);
													resultante.pos += new Vector3(dir.x*mag,dir.y*mag,0);
													resultante.angle = PseudoAngle(resultante.pos.x, resultante.pos.y);
													
													//Debug.Log(resultante.angle);

													if((resultante.angle >-1 && resultante.angle <0.7f) || (resultante.angle > 2.3f))
													{
														posLowAngle = idMax;
														posHighAngle = idMin;
													
													} else {
														posLowAngle = 0;
														posHighAngle = obj.Count - 1;
													}
											
											} else {
												posLowAngle = 0;
												posHighAngle = obj.Count - 1;
											}
											
										}else if (!obj.his && obj.lows){
											if(Cuadrant0 == true && Cuadrant1 == false && Cuadrant2 == true && Cuadrant3 == false){
												float minAngle = 2f;
												float maxAngle = -1f;
												int idMin = 0;
												int idMax = 0;
												for (int d = 0; d < obj.Count; d++)
												{
													if (obj[d].angle > 1f &&  obj[d].angle < minAngle){
														minAngle = obj[d].angle;
														idMin = d;
													}
													
													if (obj[d].angle < 0f &&  obj[d].angle > maxAngle){
														maxAngle = obj[d].angle;
														idMax = d;
													}
													
												}
												posLowAngle = 0;
												posHighAngle = obj.Count - 1;
												
												Vert resultante = new Vert();
												resultante.pos = obj[posLowAngle].pos;
												Vector2 dir = (obj[posHighAngle].pos - obj[posLowAngle].pos);
												
												float mag = dir.magnitude *.5f;
												dir.Normalize();
												
												//resultante.pos += (Vector3) (dir*mag);
												resultante.pos += new Vector3(dir.x*mag,dir.y*mag,0);
												resultante.angle = PseudoAngle(resultante.pos.x, resultante.pos.y);		
												
												if((resultante.angle <2.8f) && (resultante.angle > 1f))
												{
													posLowAngle = idMax;
													posHighAngle = idMin;
													
												} else {
													posLowAngle = 0;
													posHighAngle = obj.Count - 1;
												}
												
											
											} else {
												posLowAngle = 0;
												posHighAngle = obj.Count - 1;
											}
										
											

										}else
										{
												//-- convencional position of ray points
												// save the indice of left ray
												posLowAngle = 0;
												posHighAngle = obj.Count - 1;
												
										}


										/// CATCHING ISSUES FOR CUADRANT SORTING
										//-- fix error when sort vertex with only 1 tempvert AND rangeAngle < 360 --//
										// --------   ver 1.0.7    ---------//
										//--------------------------------------------------------------------------//
										int endPointLimit = 2;

										if(obj.Count == 1){ 
												endPointLimit = 1;
												obj[0].location = 7; // --lucky se7en
												//Debug.Log("uno solo");
												// --------------------------------------------------------------------------------------------- //
												// --------------------------------------------------------------------------------------------- //

										}else{
												// -- more than one... --//
												obj[posLowAngle].location = 1; // right
												obj[posHighAngle].location = -1; // left

										
												// *************************FIX BUG WHEN LIGHT IS I.E 340 DEGREES ANGLE AND CONTAIN A CASTER PARTIALLY *****************************************
												if((posHighAngle == posLowAngle) && obj.Count > 1){
													posHighAngle++;
													obj[posHighAngle].endpoint = true;
													obj[posLowAngle].location = 1; // right
													obj[posHighAngle].location = -1; // left
												}
												// *************************-----------------------*****************************************

										}



										//--Add vertices to the main meshes vertexes--//
										if(intelliderConvex == true && endPointLimit > 1){
												_allVerts.Add(obj[posLowAngle]);
												_allVerts.Add(obj[posHighAngle]);
										}else{
												_allVerts.AddRange(obj.Verts);
										}




										// -- r ==0 --> right ray
										// -- r ==1 --> left ray
										for (int r = 0; r < endPointLimit; r++)
										{

												//-- Cast a ray in same direction continuos mode, start a last point of last ray --//
												Vector3 fromCast = new Vector3();
												bool isEndpoint = false;


												if (r == 0)
												{
														fromCast = transform.TransformPoint(obj[posLowAngle].pos);
														isEndpoint = obj[posLowAngle].endpoint;
														if(debugLines)
																Debug.DrawLine(fromCast, transform.position, Color.yellow);	


												}
												else if (r == 1)
												{
														//posHighAngle -=1;
														fromCast = transform.TransformPoint(obj[posHighAngle].pos);
														isEndpoint = obj[posHighAngle].endpoint;
														if(debugLines)
																Debug.DrawLine(fromCast, transform.position, Color.red);

												}



												if (isEndpoint == true)
												{

														Vector2 from = (Vector2)fromCast;
														Vector2 dir = (from - (Vector2)transform.position);

														// this is last position when first ray collide, and
														// i use like a start point for continue raycasting
														// i set this offset for dont collide with itself.

														const float checkPointLastRayOffset= 0.005f; 

														from += (dir * checkPointLastRayOffset);

														var mag = (LightRadius);// - fromCast.magnitude;
														_ray = Physics2D.Raycast(from, dir, mag, Layer);

														Vector2 hitp;
														if (_ray){
																hitp = _ray.point;

																// If point is inside light radius
																if(((Vector2)gameObject.transform.position - hitp).sqrMagnitude < (mag * mag)){
																	//-----------------This Detect the gameObects touched by ray extension---
																	if(useEvents )
																	{
																		if(360 != Mathf.RoundToInt(RangeAngle)){ 
																			if (Vector3.Angle(transform.InverseTransformPoint(hitp), Vector3.up) < RangeAngle*.5f) {	// Light angle restriction
																				if(_ray.collider.gameObject.transform.parent){
																					addingObjectToObjectReachedList(objReached, _ray.collider.gameObject.transform.parent.gameObject);
																				}else{
																					addingObjectToObjectReachedList(objReached, _ray.collider.gameObject);
																				}
																			}
																		}else{
																			if(_ray.collider.gameObject.transform.parent){
																				addingObjectToObjectReachedList(objReached, _ray.collider.gameObject.transform.parent.gameObject);
																			}else{
																				addingObjectToObjectReachedList(objReached, _ray.collider.gameObject);
																			}
																		}
																	}
																	//-----------------------
																}

								
														}else{
															
															//-- FIX ERROR WEIRD MESH WHEN ENDPOINT COLLIDE OUTSIDE RADIUS VERSION 1.1.2 --//
																//-- NEW INSTANCE OF DIR VECTOR3 ADDED --//
																Vector2 newDir = transform.InverseTransformDirection(dir);	//local p
																hitp = (Vector2)transform.TransformPoint( newDir.normalized * mag); //world p

																if(debugLines == true)
																		//Debug.DrawLine(new Vector3(fromCast.x, fromCast.y, transform.position.z), new Vector3(hitp.x, hitp.y, transform.position.z), Color.blue);
																		Debug.DrawLine(new Vector3(from.x, from.y, transform.position.z), new Vector3(hitp.x, hitp.y, transform.position.z), Color.blue);

														}

														// --- VER 1.0.6 -- //
														//--- this fix magnitud of end point ray (green) ---//

														if(((Vector2)hitp - (Vector2)transform.position ).sqrMagnitude > (LightRadius * LightRadius)){
																//-- FIX ERROR WEIRD MESH WHEN ENDPOINT COLLIDE OUTSIDE RADIUS VERSION 1.1.2  --//
																dir = (Vector2)transform.InverseTransformDirection(dir);	//local p
																hitp = (Vector2)transform.TransformPoint( dir.normalized * mag);
														}

														if(debugLines == true && _ray)
																Debug.DrawLine(new Vector3(fromCast.x, fromCast.y, transform.position.z), new Vector3(hitp.x, hitp.y, transform.position.z), Color.green);


														Vector3 v3Hitp = new Vector3(hitp.x, hitp.y, transform.position.z);
														var vL = new Vert();
														vL.pos = transform.InverseTransformPoint(v3Hitp);
														vL.angle = PseudoAngle(vL.pos.x, vL.pos.y);

														_allVerts.Add(vL);

												}



										}// end for


								}

								// Here's where event's are performed //
								if(useEvents == true){
										
									//notify if not null
									if(InsideFieldOfViewEvent != null){
										InsideFieldOfViewEvent(objReached.ToArray(), this);
									}
								}

						}// end colliders loop 
				}

				/// <summary>
				/// Deletes the superflous verts.
				/// </summary>
				/// <param name="item">Item.</param>
				internal void DeleteSuperflousVerts(ref VertItem item){
						float _rangeComparative = 0.0005f;
						int[] indexesToDelete = new int[item.Verts.Count];
						int indexCounter = 0;

						// Init with no dangereous value
						for(int i = 0; i<indexesToDelete.Length; i++){
								indexesToDelete[i] = -1;
						}

						// SEARCH WITH _ALLVERTS OBJECTS AND CURRENT COLLIDER
						//----------------------------------------------------
						for(int i = 0; i<_allVerts.Count(); i++){

								if(item.Verts.Count == 0) break; // getout if no more verts to compare

								for(int j = 0; j<item.Verts.Count(); j++){

										// If persist in range of same angle
										if(_allVerts[i].angle >= (item.Verts[j].angle - _rangeComparative) && _allVerts[i].angle <= (item.Verts[j].angle + _rangeComparative))
										{

												// SAME MAGNITUDE (when have overlap colliders exactly one over each other)
												if(_allVerts[i].pos.sqrMagnitude == item.Verts[j].pos.sqrMagnitude)
												{

														if(indexCounter >= indexesToDelete.Count()){
																j = item.Verts.Count;
																break;
														}

														indexesToDelete[indexCounter] = j;
														indexCounter++;
														item.Verts.RemoveAt(j);
												}
										}
								}
						}
				}

				void SwapEndPointLocation (VertItem obj)
				{
						// loop all obj vertices and indentify the endpoints  
						for(int i = 0; i<obj.Verts.Count(); i++){
								// find the first end point
								// if is endpoint the look for other vertice endpoint=true and same angle and more magnitude
								if(obj.Verts[i].endpoint){
										float sqrMag = obj.Verts[i].pos.sqrMagnitude;
										float ang = obj.Verts[i].angle;

										for(int j = 0; j<obj.Verts.Count(); j++){
												if(i == j) continue;

												if(obj.Verts[j].endpoint == false && obj.Verts[j].pos.sqrMagnitude > sqrMag && obj.Verts[j].angle == ang)
												{
														print("same cond " + i + "   " + j);
														// if have same conditions then swap
														//	Vert aux = obj.Verts[i];
														//	obj.Verts[j].endpoint = true;
														//	obj.Verts[i] = obj.Verts[j];

														//	aux.endpoint = false;
														//	obj.Verts[j] = aux;

														GameObject g = new GameObject("gg");
														g.transform.position = obj[i].pos;

														GameObject g1 = new GameObject("gg1");
														g1.transform.position = obj[j].pos;

														//i = j = obj.Verts.Count();
												}
										}
								}
								print(obj.Verts[i].endpoint + "  isEndpoint");
								print(obj.Verts[i].pos.sqrMagnitude + "  " + obj.Verts[i].angle);
						}
				}



				/// <summary>
				/// Fixeds the limitations of system.
				/// </summary>
				float lastZRot;
				internal void fixedLimitations(){
						gameObject.transform.localScale = Vector3.one;

						
						if(lastZRot != gameObject.transform.localEulerAngles.z)
						{
							lastZRot = gameObject.transform.localEulerAngles.z;
							gameObject.transform.localEulerAngles = new Vector3(0,0,lastZRot);
						}


						// Angle
						if(RangeAngle > 360.0001f)
								RangeAngle = 360;


				}

				

				/// <summary>
				/// Resets the reached event container.
				/// </summary>
				private void resetReachedEvent(){

					if(!useEvents)
						return;

					if (!Application.isPlaying) {
						// Check events before clear
#if UNITY_EDITOR
						if(_EnumeratorOnEnterOnExitIsRunning == false)
							checkOnEnterOnExitEventsEditor();
#endif
					}else{
						// Check events before clear
							if(_EnumeratorOnEnterOnExitIsRunning == false)
								StartCoroutine(checkOnEnterOnExitEvents());
					}


						

						if(useEvents == true) // work only in neccesary cases -- optimization ver 1.1.0--
								objReached.Clear();

				}

				/// <summary>
				/// Checks the on enter & on exit events.
				/// v1.3.3 implementation 12/01/2016
				/// </summary>
				IEnumerator checkOnEnterOnExitEvents(){

						bool found = false;
						if(useEvents == true)
						{
								_EnumeratorOnEnterOnExitIsRunning = true;

								// OnExit
								ToChange.Clear();
								for (int i = 0; i < listObjectsInFOV.Count; i++)
								{
										GameObject _currentObject = listObjectsInFOV[i];
										found = false;
										
										for (int j = 0; j < objReached.Count; j++)
										{
												
											if(_currentObject.GetHashCode() == objReached[j].GetHashCode())
												{
														found = true;
														break;
												}
										}

										if(found == false){
												// If GO is out of FOV
												ToChange.Add(_currentObject); // add to the list to remove later
												
												// Editor --> unityEvents
												DDLEvent_OnExitFOV.Invoke(_currentObject);

												if(OnExitFieldOfView != null)
													OnExitFieldOfView(_currentObject, this);
										}
								}

								//wait one frame
								yield return null;

								// Time to Remove
								if(ToChange.Count > 0 && listObjectsInFOV.Count > 0){
										for (int j = 0; j < ToChange.Count; j++)
										{
											listObjectsInFOV.Remove(ToChange[j]);
										}
								}

								//wait one frame
								yield return null;

								// OnEnter
									for (int i = 0; i < objReached.Count; i++)
									{
										found = false;
											for (int j = 0; j < listObjectsInFOV.Count; j++)
											{
												if( objReached[i].GetHashCode() == listObjectsInFOV[j].GetHashCode())
												{
														found = true;
														// Enhanced Persistent check if is in the sight
														if(objectsWithinSight){
															// CHECK BEFORE BREAK
															DDLEvent_InsideFOV.Invoke( objReached[i]);
														}
														
														break;
												}
										}

										if(found == false){
												// If GO is out of FOV
												listObjectsInFOV.Add( objReached[i]);
												
												if(DDLEvent_OnEnterFOV != null)
													DDLEvent_OnEnterFOV.Invoke( objReached[i]);
												
												if(OnEnterFieldOfView != null){
													OnEnterFieldOfView( objReached[i], this);
												}

										}
								}

						}
						yield return new WaitForEndOfFrame ();
						_EnumeratorOnEnterOnExitIsRunning = false;
				}


				#if UNITY_EDITOR
						private void checkOnEnterOnExitEventsEditor(){


							
							if (!Application.isPlaying)
							{
								bool found = false;
								if(useEvents == true)
								{
									_EnumeratorOnEnterOnExitIsRunning = true;
									
									// OnExit
									ToChange.Clear();
									for (int i = 0; i < listObjectsInFOV.Count; i++)
									{
										GameObject _currentObject = listObjectsInFOV[i];
										found = false;
										for (int j = 0; j < objReached.Count; j++)
										{
											if(_currentObject.GetHashCode() == objReached[j].GetHashCode())
											{
												found = true;
												break;
											}
										}
										
										if(found == false){
											// If GO is out of FOV
											ToChange.Add(_currentObject); // add to the list to remove later
											
											// Editor --> unityEvents
											DDLEvent_OnExitFOV.Invoke(_currentObject);
											
											if(OnExitFieldOfView != null)
												OnExitFieldOfView(_currentObject, this);
										}
									}
									
									
									// Time to Remove
									if(ToChange.Count > 0 && listObjectsInFOV.Count > 0){
										for (int i = 0; i < ToChange.Count; i++)
										{
											listObjectsInFOV.Remove(ToChange[i]);
										}
									}
									
									
									// OnEnter
									for (int i = 0; i < objReached.Count; i++)
									{
										found = false;
										for (int j = 0; j < listObjectsInFOV.Count; j++)
										{
											if( objReached[i].GetHashCode() == listObjectsInFOV[j].GetHashCode())
											{
												found = true;
												// Enhanced Persistent check if is in the sight
												if(objectsWithinSight){
													// CHECK BEFORE BREAK
													DDLEvent_InsideFOV.Invoke(objReached[i]);
												}
												
												break;
											}
											
										}
										
										if(found == false){
											// If GO is out of FOV
											listObjectsInFOV.Add(objReached[i]);
											
											if(DDLEvent_OnEnterFOV != null)
											DDLEvent_OnEnterFOV.Invoke(objReached[i]);
											
											if(OnEnterFieldOfView != null){
												OnEnterFieldOfView(objReached[i], this);
											}
											
										}
									}
									
								}
								_EnumeratorOnEnterOnExitIsRunning = false;
							}
							
							
						}
				#endif
						


				/// <summary>
				/// Sorts the list.
				/// </summary>
				/// <param name="lista">Lista.</param>
				void sortList(List<Vert> lista){
						lista.Sort((item1, item2) => (item2.angle.CompareTo(item1.angle)));
				}

				/// <summary>
				/// Draws one line per vertex.
				/// </summary>
				void drawLinePerVertex(){
						for (int i = 0; i < _allVerts.Count; i++)
						{
								if (i < (_allVerts.Count -1))
								{
										Debug.DrawLine(_allVerts [i].pos , _allVerts [i+1].pos, new Color(i*0.02f, i*0.02f, i*0.02f));
								}
								else
								{
										Debug.DrawLine(_allVerts [i].pos , _allVerts [0].pos, new Color(i*0.02f, i*0.02f, i*0.02f));
								}
						}
				}

				/// <summary>
				/// Gets the vector angle.
				/// </summary>
				/// <returns>The vector angle.</returns>
				/// <param name="pseudo">If set to <c>true</c> pseudo.</param>
				/// <param name="x">The x coordinate.</param>
				/// <param name="y">The y coordinate.</param>
				float getVectorAngle(bool pseudo, float x, float y){
						float ang = 0;
						if(pseudo == true){
								ang = PseudoAngle(x, y);
						}else{
								ang = Mathf.Atan2(y, x);
						}
						return ang;
				}



				Vert []vertLightVector;
				/// <summary>
				///  Generate vectors for light cast into _allVerts
				/// </summary>
				private void GenerateLightVectors()
				{
						var theta = 0;
						float amount = RangeAngle / Segments;

						if(vertLightVector == null || vertLightVector.Length != Segments+1)
							vertLightVector = new Vert[Segments+1];

						for (int i = 0; i <= Segments; i++)
						{
								theta = Mathf.RoundToInt(amount * i);
								if (theta >= 360f) theta = 0;
								
								if(vertLightVector[i] == null)
									vertLightVector[i] = new Vert();
								
								//v.pos = new Vector3((Mathf.Sin(theta)), (Mathf.Cos(theta)), 0); // in radians, calculate on the fly, low performance
								vertLightVector[i].pos = new Vector3((PseudoSinCos.SinArray[theta]), (PseudoSinCos.CosArray[theta]), 0); // in dregrees (calculated previusly)

								Quaternion quat = Quaternion.AngleAxis(RangeAngle*.5f + transform.eulerAngles.z, Vector3.forward);
								vertLightVector[i].pos = quat * vertLightVector[i].pos;

								//v.angle = PseudoAngle(v.pos.x, v.pos.y);
								vertLightVector[i].pos *= LightRadius;
								vertLightVector[i].pos += transform.position;

								Vector3 to = vertLightVector[i].pos - transform.position;
								to.z = gameObject.transform.position.z;

								_ray = Physics2D.Raycast(transform.position, to, LightRadius, Layer);



								
								if (_ray){
									vertLightVector[i].pos = transform.InverseTransformPoint(_ray.point);
									vertLightVector[i].pos = new Vector3(vertLightVector[i].pos.x, vertLightVector[i].pos.y, 0);


								} else {
									vertLightVector[i].pos = transform.InverseTransformPoint(vertLightVector[i].pos);

								}
								vertLightVector[i].angle = PseudoAngle(vertLightVector[i].pos.x, vertLightVector[i].pos.y);
								_allVerts.Add(vertLightVector[i]);





								if(debugLines)
									Debug.DrawRay(transform.position, (transform.TransformPoint(vertLightVector[i].pos) - transform.position), Color.grey);








						}

						// Step 4: Sort each vertice by angle (along sweep ray 0 - 2PI)
						if (_sortAngles)
								_allVerts.Sort(Collider.VertComparer);
				}

				/// <summary>
				/// Auxiliar step (change order vertices close to light first in position when has same direction) 
				/// </summary>
				/// <param name="allVerts"></param>


				internal static float rangeAngleComparision = 0.00001f;

				private void SwapVertOrders(ref List<Vert> allVerts)
				{
					if (doNotRenderMesh) // return if mesh render is disabled when are using onli sight
						return;

						for (int i = 0; i < allVerts.Count; i += 1)
						{
								var uno = allVerts[i];
								var dos = allVerts[(i +1) % allVerts.Count];
								//var dos = allVerts[(i +1) % allVerts.Count];
								//var tres = allVerts[(i +2) % allVerts.Count];
					

								// -- Comparo el angulo local de cada vertex y decido si tengo que hacer un exchange-- //
								if(uno.angle >= (dos.angle-rangeAngleComparision) && uno.angle <= (dos.angle + rangeAngleComparision)){
								

										
										//--------------------------------------------------------------------------//
										// FINDING 'dos'
										//-------------
										// VERSION 1.3.1
										// Here i have find the 'dos' with a loop
										// 'dos' vert it will be the last vert with the same range that 'one'
										// and then only exchange the 2 elements First and Last
										// BUT 'dos' must be more extend than 'uno'

										int idx = 0;
										float maxMag = uno.pos.sqrMagnitude;
										//int idMaxMag = 0;
										for (int j = (i +1) % allVerts.Count; j < allVerts.Count; j += 1)
										{
												if(uno.angle >= (dos.angle-rangeAngleComparision) && uno.angle <= (dos.angle + rangeAngleComparision)){
														idx++;
														if(dos.pos.sqrMagnitude > maxMag){
																maxMag = dos.pos.sqrMagnitude;
																//idMaxMag = j;
														}
												}else{
														dos = allVerts[i + idx];
														i += idx; 
														break;
												}

										}
										

										if(dos.location == -1){ // Right of caster Ray

												if(uno.pos.sqrMagnitude > dos.pos.sqrMagnitude){
														allVerts[i] = dos;
														allVerts[(i+1) % allVerts.Count] = uno;
												}
										}


										if(uno.location == 1){ // Left of Caster Ray

												if(uno.pos.sqrMagnitude < dos.pos.sqrMagnitude){
														allVerts[i] = dos;
														allVerts[(i+1) % allVerts.Count] = uno;

												}
										}

								}



								if(uno.location == 7){ 

									// Left Ray of caster when is SPOT
									if(i > 0)
									{
										if((uno.pos.sqrMagnitude < dos.pos.sqrMagnitude) &&  
										   allVerts[i-1].location != 7 &&
										   allVerts[i-1].pos.sqrMagnitude > allVerts[(i+2)%allVerts.Count].pos.sqrMagnitude &&
										   (uno.angle >= (dos.angle-rangeAngleComparision) &&
										 uno.angle <= (dos.angle + rangeAngleComparision))){
											
											
											allVerts[i] = dos;
											allVerts[(i+1)] = uno;
											
										}
									}
									

									
									


									// Right Ray of caster when is SPOT
									if(i > 1)
									{


										Vert anterior = allVerts[i-1];
										Vert anterior2x = allVerts[(i-2)%allVerts.Count];


									

										if(isBetween(uno.angle, anterior.angle, rangeAngleComparision) &&

										 (uno.angle < anterior2x.angle) && anterior2x.pos.sqrMagnitude < dos.pos.sqrMagnitude){
											
											allVerts[i] = anterior;
											allVerts[i-1] = uno;
											
										}

										if((anterior.location == 7 &&
										    (uno.angle >= (dos.angle-rangeAngleComparision)) &&
										 uno.angle <= (dos.angle + rangeAngleComparision)) &&
						    			uno.pos.sqrMagnitude < dos.pos.sqrMagnitude){ 
											
											allVerts[i] = dos;
											allVerts[i+1] = uno;

											
										}

									}

								}
						}
				}


				/// <summary>
				/// return true if val1 is close to val2 (Between +\- threshold)
				/// </summary>
				/// <param name="dx"></param>
				/// <param name="dy"></param>
				/// <returns></returns>
				private bool isBetween(float val1, float val2, float threshold){
					if(val1 >= (val2-threshold) &&	val1 <= (val2 + threshold))
						return true;
					else
						return false;
				}

				/// <summary>
				/// Aproximates angle vectors quickly
				/// </summary>
				/// <param name="dx"></param>
				/// <param name="dy"></param>
				/// <returns></returns>
				private static float PseudoAngle(float dx, float dy)
				{
						// Hight performance for calculate angle on a vector (only for sort)
						// APROXIMATE VALUES -- NOT EXACT!! //
						float p = dy / (Mathf.Abs(dx) + Mathf.Abs(dy));
						if (dx < 0)
								p = 2 - p;

						return p;
				}


				private Vector3[] _initVerticesMeshLight; // these will also land on the heap, not the stack
				private Vector2[] _uvs;
				private Vector3[] _normals;
				private Color[] _colors;
				private int[] _triangles;

				/// <summary>
				/// Build the mesh
				/// </summary>
				private void RenderLightMesh()
				{
					if (doNotRenderMesh) // return if mesh render is disabled when are using onli sight
						return;

					// Step 5: fill the mesh with vertices

						//interface_touch.vertexCount = allVertices.Count; // notify to UI
					if (_initVerticesMeshLight == null || _initVerticesMeshLight.Length != _allVerts.Count + 1){
						_initVerticesMeshLight = new Vector3[_allVerts.Count + 1];
					}
								

						_initVerticesMeshLight[0] = Vector3.zero;

						for (int i = 0; i < _allVerts.Count; i++)
						{
								// Let shine an stroke predefinied.
								if(strokeRender > 0){
										Vector3 dir = _allVerts[i].pos;
										dir.Normalize();

										_initVerticesMeshLight[i + 1] = _allVerts[i].pos + (dir*strokeRender);
								}else{
										_initVerticesMeshLight[i + 1] = _allVerts[i].pos;
								}

						}

						if(_meshFilter.sharedMesh != null)
						{
							_meshFilter.sharedMesh.Clear();
							_meshFilter.sharedMesh.vertices = _initVerticesMeshLight;
						}
						

						if (_uvs == null || _uvs.Length != _initVerticesMeshLight.Length)
								_uvs = new Vector2[_initVerticesMeshLight.Length];

						if (_normals == null || _normals.Length != _initVerticesMeshLight.Length)
								_normals = new Vector3[_initVerticesMeshLight.Length];

						if (_colors == null || _colors.Length != _initVerticesMeshLight.Length)
								_colors = new Color[_initVerticesMeshLight.Length];

						for (int i = 0; i < _initVerticesMeshLight.Length; i++){
							_uvs[i] = (new Vector2((uv_Offset.x + (_initVerticesMeshLight[i].x * uv_Scale.x) / LightRadius), (uv_Offset.y + (_initVerticesMeshLight[i].y * uv_Scale.y) / LightRadius)));
								
								//_uvs[i] = (new Vector2((0.5f + (_initVerticesMeshLight[i].x * 0.5f) / LightRadius), (0.5f + (_initVerticesMeshLight[i].y * 0.5f) / LightRadius)));

								_normals[i] = -Vector3.forward;

								// --- Only add vertex colors for render plain shaders like texturized light --- //
							if(SolidColor){
								_colors[i] = LightColor * Intensity;
							}
				
						}



						// triangles
						int idx = 0;
						if (_triangles == null || _triangles.Length != _allVerts.Count * 3)
							_triangles = new int[(_allVerts.Count * 3)];

						for (int i = 0; i < (_allVerts.Count * 3); i += 3)
						{
								_triangles[i] = 0;
								_triangles[i + 1] = idx + 1;

								if (i == (_allVerts.Count * 3) - 3)
								if(Mathf.RoundToInt(RangeAngle) == 360) {
										_triangles[i+2] = 1;							// last triangle closes full round
								} else {
										_triangles[i+2] = 0;							// no closing when light angle < 360°
								}
								else
										_triangles[i + 2] = idx + 2; 

								idx++;
						}



						_meshFilter.sharedMesh.triangles = _triangles;
						_meshFilter.sharedMesh.normals = _normals;
						_meshFilter.sharedMesh.uv = _uvs;


						if(SolidColor)
						_meshFilter.sharedMesh.colors = _colors;

						if(recalculateNormals == true)
							_meshFilter.sharedMesh.RecalculateNormals();

						Material _shaMat = GetComponent<Renderer>().sharedMaterial;
						if(_shaMat == null || _shaMat != LightMaterial){
							GetComponent<Renderer>().sharedMaterial = LightMaterial;
						//	Rebuild();
						}

				}

	#if UNITY_EDITOR
			SightListenerTemplate listener;
			bool permissionToAddListeners = false;
			/// <summary>
			/// Adds the listeners - use for sight events.
			/// </summary>

			void AddListeners(){

			//INIT DDLEvents

			if (DDLEvent_OnEnterFOV == null)
				DDLEvent_OnEnterFOV = new DynamicLightEvent ();

			if (DDLEvent_OnExitFOV == null)
				DDLEvent_OnExitFOV = new DynamicLightEvent ();

			if (DDLEvent_InsideFOV == null)
				DDLEvent_InsideFOV = new DynamicLightEvent ();
				


				UnityAction<GameObject> callback_onEnter = new UnityAction<GameObject>(listener.myListener_onEnter);
				UnityAction<GameObject> callback_onExit = new UnityAction<GameObject>(listener.myListener_onExit);
				UnityAction<GameObject> callback_onInside = new UnityAction<GameObject>(listener.myListener_onInside);


				// Registering events

				UnityEditor.Events.UnityEventTools.RemovePersistentListener(DDLEvent_OnEnterFOV, callback_onEnter);
				UnityEditor.Events.UnityEventTools.AddPersistentListener(DDLEvent_OnEnterFOV, callback_onEnter);

				UnityEditor.Events.UnityEventTools.RemovePersistentListener(DDLEvent_OnExitFOV, callback_onExit);
				UnityEditor.Events.UnityEventTools.AddPersistentListener(DDLEvent_OnExitFOV, callback_onExit);

				UnityEditor.Events.UnityEventTools.RemovePersistentListener(DDLEvent_InsideFOV, callback_onInside);
				UnityEditor.Events.UnityEventTools.AddPersistentListener(DDLEvent_InsideFOV, callback_onInside);

				DDLEvent_OnEnterFOV.SetPersistentListenerState (0, UnityEventCallState.EditorAndRuntime);
				DDLEvent_OnExitFOV.SetPersistentListenerState (0, UnityEventCallState.EditorAndRuntime);
				DDLEvent_InsideFOV.SetPersistentListenerState (0, UnityEventCallState.EditorAndRuntime);



				
			}

			
			public void addingPersistentUnityEvents(GameObject listenObj){

				listener =  listenObj.GetComponent<SightListenerTemplate>();
				permissionToAddListeners = true;
				
			}


			/// <summary>
			/// Avoid an in-editor memory leak by destroying the mesh when this light is destroyed.
			/// </summary>
			void OnDestroy()
			{
				if (_meshFilter.sharedMesh != null)
					DestroyImmediate(_meshFilter.sharedMesh);
			}

	#endif

		}


}

