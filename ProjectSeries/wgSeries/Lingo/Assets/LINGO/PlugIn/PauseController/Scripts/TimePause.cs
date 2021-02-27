using UnityEngine;
using System.Collections;

public class TimePause : MonoBehaviour, IPausable {
	public float pauseDelay = 0.3f;

	private float timeScale;

	public void OnUnPause() {
		Debug.Log ("TestPause.OnUnPause");
		Time.timeScale = timeScale;
	}
	
	public void OnPause() {
		Debug.Log ("TestPause.OnPause");
		timeScale = Time.timeScale;
		Invoke ("StopTime", pauseDelay ); 
	}

	void StopTime() {
		Time.timeScale = 0;
	}
}
