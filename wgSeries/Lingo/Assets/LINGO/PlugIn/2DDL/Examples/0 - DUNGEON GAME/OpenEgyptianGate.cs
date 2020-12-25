using UnityEngine;
using System.Collections;

public class OpenEgyptianGate : MonoBehaviour {
	
	// Blue torch
	public  DynamicLight2D.DynamicLight light2d;

	// Internals
	internal GameObject[] GOsReached;
	
	// privates
	private bool isGateOpen = false;
	private bool openning = false; 
		
		
		
		
	void Start () {
		
		// Add listener
		if(light2d){
			light2d.OnEnterFieldOfView += waveReach;
			light2d.OnExitFieldOfView += waveReach;
			//light2d.InsideFieldOfViewEvent += waveReach2;
		}


	}
		
		
		
	//- this function iterate on each object passed by 2DLigh script and compare if this object is the desired object Lit.
	//-- THIS SCRIPT MUST BE ATTACHED TO PLAYER GO --//
	
	void waveReach(GameObject g, DynamicLight2D.DynamicLight light){



		bool found = false;


		if(gameObject.GetHashCode() == g.GetHashCode()){
			found = true;
		}
		if(found == true && isGateOpen == false){
			OpenThisGate();
		}
	}

	void waveReach2(GameObject[] g, DynamicLight2D.DynamicLight light){



			bool found = false;


			foreach(GameObject gs in g){
			if(gameObject.GetHashCode() == gs.GetHashCode()){
					found = true;
			}
			}
			if(found == true && isGateOpen == false){
					OpenThisGate();
					Debug.Log("yeahh");
			}
	}


	internal void OpenThisGate(){
		// First unsubscribe event
		light2d.OnEnterFieldOfView -= waveReach;
		light2d.OnExitFieldOfView -= waveReach;
		light2d.InsideFieldOfViewEvent -= waveReach2;

		isGateOpen = true;

		openning = true;

	}

	void Update(){
		if(openning == true){
			Vector3 p = transform.position;
			p.y += 0.2f * Time.deltaTime;
			transform.position = p;

			Debug.Log("opening");

			if(transform.localPosition.y >= -1.099f)
				openning = false;
		}
	}

}
