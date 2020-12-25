namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using DynamicLight2D;
	
	public class DynamicLightMenu : Editor {
		
		static internal DynamicLight2D.DynamicLight light;
		const string menuPath = "GameObject/2DDL Pro";
		
		static SerializedObject tmpAsset;
		
		private static string _path;
		
		internal void OnEnable(){
			light = target as DynamicLight2D.DynamicLight;
			
			reloadAsset ();
		}
		
		static void reloadAsset(){
			if(_path == null)
			{_path = EditorUtils.getMainRelativepath();}
			tmpAsset = new SerializedObject(AssetUtility.LoadAsset<DynamicLightSetting>(_path + "2DLight/Core/Editor/Settings", "Settings.asset"));
			//Debug.Log ("reloadAsset");
		}
		
		/*

	[MenuItem ( menuPath + "/Lights/ ☼ Radial NO Material",false,20)]
	static void Create(){
		reloadAsset ();
		//Debug.Log (tmpAsset);
		GameObject gameObject = new GameObject("2DLight");
		DynamicLight2D.DynamicLight dl = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
		dl.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		dl.Version = AssetUtility.LoadProperty("version", tmpAsset);

	}
	*/
		
		//[MenuItem ( menuPath + "/Lights/ ☀ Radial Procedural Gradient ",false,19)]
		static void addGradient(){
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardLightMaterialGradient.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DDLight");
			gameObject.transform.position = getCameraPos (); 
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.SortOrderID = -1;
			s.setMainMaterial(m);
			s.Rebuild();
			
		}
		
		[MenuItem ( menuPath + "/Lights/ ☀ Radial Texturized Additive",false,31)]
		static void addTexturized(){
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardLightTexturizedMaterialAdditive.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DDLight");
			gameObject.transform.position = getCameraPos (); 
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.SolidColor = true;
			s.LightColor = Color.red;
			s.uv_Scale = new Vector3 (.38f, .38f);
			s.SortOrderID = -1;
			s.setMainMaterial(m);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.Rebuild();
			
		}
		
		[MenuItem ( menuPath + "/Lights/ ☀ Radial Texturized Alpha Blend",false,32)]
		static void addTexturized2(){
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardLightTexturizedMaterialBlend.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DDLight");
			gameObject.transform.position = getCameraPos (); 
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.SolidColor = true;
			s.LightColor = Color.red;
			s.uv_Scale = new Vector3 (.38f, .38f);
			s.SortOrderID = -1;
			s.setMainMaterial(m);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.Rebuild();
			
		}
		
		//[MenuItem ( menuPath + "/Lights/ ☀ Radial with Ilumination ",false,33)]
		static void addRadialIlum(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/2DDL-Point2DLightWithIlumination.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			hex.transform.position = getCameraPos (); 
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "2DDL-Point2DLightWithIlumination";
			DynamicLight2D.DynamicLight s = hex.GetComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.SortOrderID = -1;
			//hex.layer = LayerMask.LayerToName(CustomAssetUtility.LoadPropertyAsInt("layer"));
		}

		[MenuItem ( menuPath + "/Lights/ ☀ Radial Occluder ",false,34)]
		static void addOccluder(){
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardOccluderMaterial.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DDLight");
			gameObject.transform.position = getCameraPos (); 
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.SolidColor = true;
			s.LightColor = new Color(59/255f,122/255f,59/255f);
			s.uv_Scale = new Vector3 (.38f, .38f);
			s.SortOrderID = -1;
			s.setMainMaterial(m);
			//s.strokeRender = .35f;
			s.Segments = 30;
			//s.intelliderConvex = false;
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.Rebuild();
		}
		
		
		[MenuItem ( menuPath + "/Lights/ ☀ Spot ",false,45)]
		static void addSpot(){
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardLightMaterialGradient.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DLight");
			gameObject.transform.position = getCameraPos (); 
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.setMainMaterial(m);
			s.Rebuild();
			s.Segments = 9;
			s.RangeAngle = 90;
			s.SortOrderID = -1;
			
		}
		
		//[MenuItem ( menuPath + "/Lights/ ☀ Spot with Ilumination ",false,46)]
		static void addSpotIlum(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/2DDL-Spot2DWithIlumination.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			hex.transform.position = getCameraPos ();
			DynamicLight2D.DynamicLight s = hex.GetComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			hex.name = "2DDL-Point2DLightWithIlumination";
			s.SortOrderID = -1;
			
		}
		
		[MenuItem ( menuPath + "/Lights/ ☀ Spot car type ",false,47)]
		static void addSpotCar(){
			reloadAsset ();
			
			//main spot
			
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardSpotCarType1.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DSpotCar");
			gameObject.transform.position = getCameraPos ();
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.setMainMaterial(m);
			s.Rebuild();
			s.Segments = 5;
			s.RangeAngle = 107;
			s.SolidColor = true;
			s.LightColor = new Color(229/255f,1f,171/255f);
			s.uv_Offset = new Vector2(.5f,0f);
			s.uv_Scale = new Vector2(1.25f,1f);
			s.debugLines = false;
			s.recalculateNormals = false;
			s.SortOrderID = -1;
			
			//Slave spot
			//m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardSpotCarType1.mat", typeof(Material)) as Material;
			GameObject gameObject2 = new GameObject("2DSpotCarBright");
			gameObject2.transform.parent = gameObject.transform; 
			s = gameObject2.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.setMainMaterial(m);
			s.Rebuild();
			s.Segments = 5;
			s.RangeAngle = 180;
			s.LightRadius = 20;
			s.SolidColor = true;
			s.LightColor = new Color(229/255f,1f,171/255f);
			s.uv_Offset = new Vector2(.5f,0f);
			s.uv_Scale = new Vector2(.54f,2.9f);
			s.debugLines = false;
			s.recalculateNormals = false;
			s.SortOrderID = -1;
			
			
		}
		
		
		/*
	[MenuItem ( menuPath + "/Casters/Empty",false,20)]
	static void addEmpty(){
		reloadAsset ();
		GameObject empty = new GameObject("empty");
		int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		empty.layer = layerDest;
		empty.AddComponent<SpriteRenderer>();
		GameObject emptyChild = new GameObject("collider");
		emptyChild.AddComponent<PolygonCollider2D>();
		emptyChild.transform.parent = empty.transform;
		emptyChild.layer = empty.layer;
		empty.transform.position = new Vector3(5,0,0);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}

	
	[MenuItem ( menuPath + "/Casters/ ☐ Square - PolygonCollider2D",false,31)]
	static void addSquare(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/square.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);//light.getLayerNumberFromLayerMask(light.Layer.value);
			hex.layer = layerDest;
			hex.transform.FindChild("collider").gameObject.layer = layerDest;
		//}
		
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Square";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}
	*/
		
		[MenuItem ( menuPath + "/Casters/ ☐ Box",false,31)]
		static void addSquare2(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/box.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			
			hex.transform.localEulerAngles = new Vector3 (0, 0, 0.001f);
			hex.transform.position = getCameraPos (true);
			hex.name = "Box__2ddl";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		[MenuItem ( menuPath + "/Casters/ ☐ Box as Illuminated",false,32)]
		static void addSquareIlum(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/box_ilum.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			hex.transform.position = getCameraPos (true);
			hex.transform.localEulerAngles = new Vector3 (0, 0, 0.001f);
			hex.name = "Box__2ddl__Illuminated";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}

		[MenuItem ( menuPath + "/Casters/ ☐ Box as Occluded",false,33)]
		static void addSquare3(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/box.prefab", typeof(GameObject));
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Materials/SpriteOccluded.mat", typeof(Material)) as Material;

			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			hex.GetComponent<BoxCollider2D>().enabled = false;

			SpriteRenderer sp = hex.GetComponent<SpriteRenderer>();
			sp.material = m;

			hex.transform.localEulerAngles = new Vector3 (0, 0, 0.001f);
			hex.transform.position = getCameraPos (true);
			hex.name = "Box__2ddl__Occluded";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		[MenuItem ( menuPath + "/Casters/ ☐ Shape",false,54)]
		static void addShape(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/shape.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			
			
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Shape__2ddl";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		[MenuItem ( menuPath + "/Casters/ ☐ Shape (concave)",false,55)]
		static void addShapeConc(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/shape_conc.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			
			
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Shape__c__2ddl";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		
		[MenuItem ( menuPath +"/Casters/ ◯ Circle - polygonCollider2D",false,65)]
		static void addCircle(){
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circle.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);//light.getLayerNumberFromLayerMask(light.Layer.value);
			hex.layer = layerDest;
			hex.transform.Find("collider").gameObject.layer = layerDest;
			//}
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Circle";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		
		[MenuItem ( menuPath + "/Casters/ ◯ Circle - CircleCollider2D",false,66)]
		static void addCircle2(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circle - CircleCollider2D.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			//hex.transform.FindChild("collider").gameObject.layer = layerDest;
			//}
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Circle - CircleCollider2D";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		
		//[MenuItem ( menuPath + "/Casters/ ◯ Circle - Intellider",false,45)]
		static void addIntelliderCircle(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circleWithintellider.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			if(light){
				int layerDest =  AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
				hex.layer = layerDest;
				hex.transform.Find("collider").gameObject.layer = layerDest;
			}
			hex.transform.position = new Vector3(5,0,0);
			hex.name = "Circle - Intellider";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		[MenuItem ( menuPath + "/Casters/ ◯ Circle Iluminated",false,67)]
		static void addCircleIlum(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Iluminated - Circle.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(), Quaternion.identity) as GameObject;
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Circle - Iluminated";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		[MenuItem ( menuPath + "/Casters/ --- Line Edge - EdgeCollider2D",false,80)]
		static void addEdge(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/edge - EdgeCollider2D.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			//hex.transform.FindChild("collider").gameObject.layer = layerDest;
			//	}
			//hex.transform.position = new Vector3(5,3,0);
			hex.name = "Edge - EdgeCollider2D";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		
		[MenuItem ( menuPath + "/Casters/Hexagon",false,91)]
		static void addHexagon(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/hexagon.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			hex.transform.Find("collider").gameObject.layer = layerDest;
			//}
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Hexagon";
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		[MenuItem ( menuPath + "/Casters/Pacman",false,92)]
		static void addPacman(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/pacman.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Pacman";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
			
		}
		[MenuItem ( menuPath + "/Casters/Star",false,93)]
		static void addStar(){
			
			reloadAsset ();
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/star.prefab", typeof(GameObject));
			GameObject hex = Instantiate(prefab, getCameraPos(true), Quaternion.identity) as GameObject;
			//hex.transform.position = new Vector3(5,0,0);
			hex.name = "Star";
			hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			DynamicLight2D.DynamicLight.reloadMeshes = true;
		}
		
		[MenuItem ( menuPath + "/Sight/Sight + Listener",false,71)]
		static void addSight(){
			
			
			
			
			//EditorUtility.DisplayDialog("NOTES","1. Events require 'Game' window and at least one 'Camera' to get working (runtime or/and editor). ", "OK");
			
			
			
			// adding sight object
			
			reloadAsset ();
			Material m = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/Materials/StandardLightMaterialGradient.mat", typeof(Material)) as Material;
			GameObject gameObject = new GameObject("2DSight");
			DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
			s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
			s.Version = AssetUtility.LoadProperty("version", tmpAsset);
			s.setMainMaterial(m);
			s.Rebuild();
			s.Segments = 5;
			s.RangeAngle = 80;
			s.SortOrderID = -1;
			
			//Enable events
			s.useEvents = true;
			s.objectsWithinSight = true;
			
			
			
			//adding listener
			Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/box.prefab", typeof(GameObject));
			GameObject go = Instantiate(prefab, new Vector3(0,7f,0), Quaternion.Euler(new Vector3(0,0,0.0001f))) as GameObject;
			go.name = "2DSight_Listener";
			go.AddComponent<SightListenerTemplate> ();
			go.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			//SightListenerTemplate listener = go.GetComponent<SightListenerTemplate> ();
			
			s.addingPersistentUnityEvents(go);
			
			//EditorUtility.SetDirty (s);
			
			
		}
		
		static Vector3 getCameraPos(bool isCaster = false){
			float offset = 0f;
			if (isCaster)
				offset = 5f;
			
			
			if (Camera.current)
				return new Vector3(Camera.current.transform.position.x, Camera.current.transform.position.y + offset,0);
			else
				return new Vector3(0, offset,0);
		}
		
		
	}

}