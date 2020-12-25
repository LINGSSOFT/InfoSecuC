using UnityEngine;
using System.Collections;

public class rotationUsingSync : MonoBehaviour {

	//Speed
	public float speed = 40f;

	// Store light
	DynamicLight2D.DynamicLight light2d;

	//Store current light rotation
	private Vector3 _lRotation;

	//Store light position
	private Vector3 lPosition;


	// Use this for initialization
	void Start () {
		light2d = GetComponent<DynamicLight2D.DynamicLight>();
	}
	
	// Update is called once per frame
	void Update () {
		if(light2d)
		{
			_lRotation = light2d.transform.localEulerAngles;
			_lRotation.z += speed;
			light2d.setRotationSync(_lRotation);
		}
	}
}
