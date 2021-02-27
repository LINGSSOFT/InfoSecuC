using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

	[SerializeField] DynamicLight2D.DynamicLight Light2DDL;
	[SerializeField] GameObject player;
	TimerClass timer;

	float tmpValue = 0;

	Vector3 pos;


	IEnumerator Start () {
		if(player == null)
		{
			Debug.LogError("Player not selected");
			yield break;
		}
		if(Light2DDL == null)
		{
			Debug.LogError("Light2DDL not selected");
			yield break;
		}

		Light2DDL.Segments = 80;

		timer = gameObject.AddComponent<TimerClass>();


		// Subscribe timer events //
		timer.OnUpdateTimerEvent += timerUpdate;
		timer.OnTargetTimerEvent += tick;

		timer.InitTimer(1.2f, true);

		StartCoroutine(LoopUpdate());
	
		yield return  null;

	}


		IEnumerator LoopUpdate(){

		while(true){
			pos.x += Input.GetAxis ("Horizontal") * 20f * Time.deltaTime;
			pos.y += Input.GetAxis ("Vertical") * 20f * Time.deltaTime;
			

			Vector3 martinPos = Input.mousePosition;
			martinPos = Camera.main.ScreenToWorldPoint(martinPos);
			martinPos.z = 0;


			yield return new WaitForEndOfFrame();
			Light2DDL.gameObject.transform.position = pos;
			player.transform.position = martinPos;
		}

		



	}

	
	void tick(){
		Light2DDL.LightRadius = 1.7f;
		tmpValue *=0;
	}
	void timerUpdate(float value){
		Light2DDL.LightRadius += value*.8f;
	}
}
