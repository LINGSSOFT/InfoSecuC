using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SightListenerTemplate : MonoBehaviour {

	// * NOTE: WHEN YOU'RE TESTING EVENTS IN EDITOR, BE SURE THAT GAME WINDOW IS ACTIVE AND VISIBLE 

	// ** DECLARE PUBLIC METHODS FOR LISTENERS

	// *** READ FIRST: http://jacksondunstan.com/articles/3335  (UnityEvents vs C# native Events performance)

	private bool check = false;
	public void Start()
	{
		if(!check)
		{
			check = true;
			BoxCollider2D box2d = GetComponent<BoxCollider2D> ();
			if (box2d == null) {
				PolygonCollider2D pol2d = GetComponent<PolygonCollider2D>();
				if(pol2d == null)
					gameObject.AddComponent<BoxCollider2D>();
			}

		}

	}



	public void myListener_onEnter(GameObject go){
		//Filter by Hash
		if (gameObject.GetHashCode () == go.GetHashCode ()) {
			print (go.name + " --> OnEnter() event");
			go.GetComponent<SpriteRenderer>().color = Color.green;
		}
	}

	public void myListener_onExit(GameObject go){
		if (gameObject.GetHashCode () == go.GetHashCode ()) {
			print (go.name + " --> OnExit() event");
			go.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	public void myListener_onInside(GameObject go){
		if(gameObject.GetHashCode() == go.GetHashCode())
			print (go.name + " --> OnInside() event");
	}

	public void myListener_TT(){
		print("tt--list");
	}

}
