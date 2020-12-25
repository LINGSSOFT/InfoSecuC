using UnityEngine;
using System.Collections;

public class SetProximityFactor : MonoBehaviour {
	public float proximityCasters = 25f;
	DynamicLight2D.DynamicLight l2d ;
	void Start () {
		l2d = GetComponent<DynamicLight2D.DynamicLight>();
		l2d.setMagRange(proximityCasters);
	}
}
