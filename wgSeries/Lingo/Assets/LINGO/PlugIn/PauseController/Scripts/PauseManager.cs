using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseManager : MonoBehaviour {
	public GameObject pausable;
	public Canvas pauseCanvas;

	private bool isPaused = false;
	private Animator anim;
	private Component[] pausableInterfaces;
	private Component[] quittableInterfaces;

	void Start() 
	{
		// PauseManager requires the EventSystem - make sure there is one
		if (FindObjectOfType<EventSystem>() == null)
		{
			var es = new GameObject("EventSystem", typeof(EventSystem));
			es.AddComponent<StandaloneInputModule>();
		}

		pausableInterfaces = pausable.GetComponents (typeof(IPausable));
		quittableInterfaces = pausable.GetComponents (typeof(IQuittable));
		anim = pauseCanvas.GetComponent<Animator> ();

		pauseCanvas.enabled = false;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if( isPaused ) {
				OnUnPause();
			} else {
				OnPause();
			}
		}

		pauseCanvas.enabled = isPaused;
		anim.SetBool( "IsPaused", isPaused );
	}
		
	public void OnQuit() {
		Debug.Log ("PauseManager.OnQuit");

		foreach (var quittableComponent in quittableInterfaces) {		
			IQuittable quittableInterface = (IQuittable)quittableComponent;
			if( quittableInterface != null )
				quittableInterface.OnQuit ();
		}		
	}
	
	public void OnUnPause() {
		Debug.Log ("PauseManager.OnUnPause");	
		isPaused = false;

		foreach (var pausableComponent in pausableInterfaces) {		
			IPausable pausableInterface = (IPausable)pausableComponent;
			if( pausableInterface != null )
				pausableInterface.OnUnPause ();
		}
	}

	public void OnPause() {
		Debug.Log ("PauseManager.OnPause");
		isPaused = true;

		foreach (var pausableComponent in pausableInterfaces) {		
			IPausable pausableInterface = (IPausable)pausableComponent;
			if( pausableInterface != null )
				pausableInterface.OnPause ();
		}
	}
}
