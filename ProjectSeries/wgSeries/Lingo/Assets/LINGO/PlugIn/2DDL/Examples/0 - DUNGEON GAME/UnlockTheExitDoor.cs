using UnityEngine;
using System.Collections;

public class UnlockTheExitDoor : MonoBehaviour {
	
	// Blue torch
	public  DynamicLight2D.DynamicLight light2d;

	public GameObject traesure1;
	public GameObject traesure2;
	public GameObject traesure3;

	// Internals
	internal GameObject[] GOsReached;
	
	// privates
	private bool isGateOpen = false;
	private bool openning = false; 
	private AudioSource openGateSound;
		
		
		
		
	void Start () {
		
		// Add listener
		if(light2d)
			light2d.InsideFieldOfViewEvent += checkWaveReach;

		openGateSound = GetComponent<AudioSource>();
		
	}
		
		
		
	//- this function iterate on each object passed by 2DLigh script and compare if this object is the desired object Lit.
	//-- THIS SCRIPT MUST BE ATTACHED TO PLAYER GO --//
	
	bool found1 = false;
	bool found2 = false;
	bool found3 = false;

	void checkWaveReach(GameObject[] allObstacles, DynamicLight2D.DynamicLight light){//zxc
				
		found1 = false;
		found2 = false;
		found3 = false;

		foreach(GameObject gs in allObstacles){
			if(traesure1.GetHashCode() == gs.GetHashCode()){
				found1 = true;
			}
			if(traesure2.GetHashCode() == gs.GetHashCode()){
				found2 = true;
			}
			if(traesure3.GetHashCode() == gs.GetHashCode()){
				found3 = true;
			}
		}
		if(found1 && found2 && found3 && isGateOpen == false){
			OpenThisGate();
			Debug.Log("yeahh");
		}
	}

	internal void OpenThisGate(){
		// First unsubscribe event
		light2d.InsideFieldOfViewEvent -= checkWaveReach;

		isGateOpen = true;

		openning = true;

		openGateSound.Play();

	}

	void Update(){
		if(openning == true){
			Vector3 p = transform.position;
			p.y += 0.2f * Time.deltaTime;
			transform.position = p;

			Debug.Log("opening");

			if(transform.localPosition.y >= -0.05f)
				openning = false;
		}
	}

}
