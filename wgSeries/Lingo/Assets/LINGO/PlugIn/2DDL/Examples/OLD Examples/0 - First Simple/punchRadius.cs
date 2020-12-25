using UnityEngine;
using System.Collections;

public class punchRadius : MonoBehaviour {

	// Use this for initialization
	[Header ("Random variation on each frame")]
	public float mag = 0.5f;

	[Header ("Delay flicker")]
	public float delay = 0.081f;

	[Header ("Bright intensity")]
	public byte bright = 255;

	private float lastOffset;
	private Vector2 _initUVScale;
	private Vector3 _initPos;
	private DynamicLight2D.DynamicLight dl;

	void Start () {
		dl = GetComponent<DynamicLight2D.DynamicLight>();
		//lastOffset = dl.LightRadius;
		_initUVScale = dl.uv_Scale;
		_initPos = transform.position;
		_deltaPos = _initPos;

		StartCoroutine(updateLoop());

	}

	int cycles = 0;
	Vector2 _deltaPos;
	IEnumerator updateLoop(){

		while(true){
			yield return new WaitForSeconds(delay);
			float chance = Random.Range(0f,1f);



			dl.LightColor = new Color(dl.LightColor.r, dl.LightColor.g, dl.LightColor.b, bright);
			float rnd = Random.Range(-0.1f,0.1f) * mag;

			float descentralizationMag = Random.Range(-0.1f,0.1f) * .8f;

			if(cycles == 30){
				cycles *=0;
				_deltaPos = _initPos;
			}

			if(chance > .5f) //Only happend in 50% chance
			{
				_deltaPos = new Vector2(_deltaPos.x + descentralizationMag, _deltaPos.y + descentralizationMag);
				transform.position = new Vector3(_deltaPos.x, _deltaPos.y, _initPos.z);
			}

			yield return new WaitForEndOfFrame();

			dl.uv_Scale = new Vector2(_initUVScale.x + rnd ,_initUVScale.y + rnd );
			dl.Refresh();
			cycles++;
		}
	}

}
