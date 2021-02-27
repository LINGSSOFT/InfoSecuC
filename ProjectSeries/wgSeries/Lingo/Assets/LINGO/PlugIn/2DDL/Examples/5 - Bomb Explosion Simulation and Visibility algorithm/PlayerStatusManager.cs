using UnityEngine;
using System.Collections;

public class PlayerStatusManager : MonoBehaviour {

	[SerializeField] GUIText statustext;

	public void player_onEnterWave(GameObject go){
		//Filter by Hash
		if (gameObject.GetHashCode () == go.GetHashCode ()) {
			if(statustext != null)
			{
				statustext.text = "Status: HIT";
			}
		}
	}
	
	public void player_onExitWave(GameObject go){
		if (gameObject.GetHashCode () == go.GetHashCode ()) {
			if(statustext != null)
			{
				statustext.text = "Status: out of range";
			}
		}
	}
}
