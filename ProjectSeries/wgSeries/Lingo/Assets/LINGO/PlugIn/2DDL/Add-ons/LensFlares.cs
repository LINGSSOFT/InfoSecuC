namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	using System.IO;
	using System.Reflection;
	
	[ExecuteInEditMode]
	public class LensFlares : AddOnBase {


		// Tags array is used for search results in inspector
		public static string []tags = {"lens", "glare", "halo", "camera effect"};
		
		// Brief description of behavior in this Add-on
		public static string description = "Add lens flare effect";


		[SerializeField]Texture2D PrimaryFlare;
		[SerializeField]Texture2D FirstHalo;
		[SerializeField]Texture2D SmallBurst;
		[SerializeField]Texture2D NextHalo;
		//[SerializeField]Texture2D NextBurst;
		[Space(20)] 
		[SerializeField][Range(.5f, 2f)]float scale = 1f;
		[SerializeField][Range(.01f, 1.5f)] float separation = .4f;
		[SerializeField]bool Use2DDLColor = false;
		[SerializeField]Color color = Color.white;



		Vector2 lightPos;
		Vector2 cameraPos;
		Vector2 dir;
		float lenght;
		Vector2 cameraScreen;
		float maxCameraLenght;
		Vector2 texCentre;
		Vector2 position;
		Vector2 texSize;
		Rect rec;
		float alpha = 1f;
		
		
		// Use this for initialization
		public override void Start () {
		
			cameraScreen = new Vector2(Screen.width*.5f, Screen.height*.5f);
			maxCameraLenght = 1 / (new Vector2 (Screen.width, Screen.height)).magnitude;

			if(PrimaryFlare == null)
				PrimaryFlare = Resources2DDL.getTexture2D(1);

			if(FirstHalo == null)
				FirstHalo = Resources2DDL.getTexture2D(2);

			if(SmallBurst == null)
				SmallBurst = Resources2DDL.getTexture2D(3);

			if (NextHalo == null)
				NextHalo = Resources2DDL.getTexture2D (4);



		}
		


		void OnGUI()
		{

			lightPos = (Vector2)dynamicLightInstance.transform.position;

			if (Camera.current != null) {
				if (!Camera.current.pixelRect.Contains (Camera.current.WorldToScreenPoint (lightPos)))
					return;
			}




			if (!Camera.main) {
				Debug.LogError("NO MAIN CAMERA SET IN THIS PROJECT");
				return;
			}

			cameraPos = (Vector2)Camera.main.transform.position;
			dir = cameraPos - lightPos;
			lenght = dir.magnitude;
			alpha = lenght * maxCameraLenght * 50;

			if (alpha > 1f)
				alpha = 1f;

			Color guiColor = GUI.color;
			guiColor = Use2DDLColor ? dynamicLightInstance.LightColor : color;
			guiColor.a = alpha;
			GUI.color = guiColor;

			dir.Normalize();
			dir.x = -dir.x; 


			if(PrimaryFlare == null)
				return;


			Color lastGUIColor = GUI.color;
			Color newColor = guiColor;

			// Primary Flare //
			texCentre = new Vector2 (-PrimaryFlare.width * .5f, -PrimaryFlare.height * .5f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-10f * separation;
			texSize = new Vector2(PrimaryFlare.width, PrimaryFlare.height)* scale;
			rec = new Rect(position, texSize);
			GUI.DrawTexture(rec,(Texture) PrimaryFlare, ScaleMode.ScaleAndCrop);


			// First Halo //
			texCentre = new Vector2 (-FirstHalo.width * .5f, -FirstHalo.height * .5f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-15f * separation;
			texSize = new Vector2(FirstHalo.width, FirstHalo.height)* scale;
			rec = new Rect(position, texSize);
			GUI.DrawTexture(rec,(Texture) FirstHalo, ScaleMode.ScaleAndCrop);


			// Mini Flare //
			texCentre = new Vector2 (-PrimaryFlare.width * .5f * .6f, -PrimaryFlare.height * .5f * .6f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-17f * separation;
			texSize = new Vector2(PrimaryFlare.width, PrimaryFlare.height)* scale * .6f;
			rec = new Rect(position, texSize);
			GUI.color = new Color (1f,1f,0f, newColor.a);
			GUI.DrawTexture(rec,(Texture) PrimaryFlare, ScaleMode.ScaleAndCrop);
			GUI.color = lastGUIColor;

			// Mini Next Halo //
			texCentre = new Vector2 (-NextHalo.width * .5f * .4f, -NextHalo.height * .5f * .4f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-19f * separation;
			texSize = new Vector2(NextHalo.width, NextHalo.height)* scale * .4f;
			rec = new Rect(position, texSize);
			//newColor.r = .4f;
			newColor.a *= .3f;
			GUI.color = newColor;
			GUI.DrawTexture(rec,(Texture) NextHalo, ScaleMode.ScaleAndCrop);
			GUI.color = lastGUIColor;


			// Small Burts //
			texCentre = new Vector2 (-SmallBurst.width * .5f, -SmallBurst.height * .5f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-24f*separation;
			texSize = new Vector2(SmallBurst.width, SmallBurst.height)* scale;
			rec = new Rect(position, texSize);
			GUI.color = new Color (36/255f,173/255,1f, newColor.a);
			GUI.DrawTexture(rec,(Texture) SmallBurst, ScaleMode.ScaleAndCrop);
			GUI.color = newColor;

			// Mini Flare2 //
			texCentre = new Vector2 (-PrimaryFlare.width * .5f * .3f, -PrimaryFlare.height * .5f * .3f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-28f * separation;
			texSize = new Vector2(PrimaryFlare.width, PrimaryFlare.height)* scale * .3f;
			rec = new Rect(position, texSize);
			GUI.DrawTexture(rec,(Texture) PrimaryFlare, ScaleMode.ScaleAndCrop);



			// First Halo 2//
			texCentre = new Vector2 (-FirstHalo.width * .5f, -FirstHalo.height * .5f) * scale;
			position = cameraScreen + texCentre;
			position += dir * lenght*-34f * separation;
			texSize = new Vector2(FirstHalo.width, FirstHalo.height)* scale;
			rec = new Rect(position, texSize);
			GUI.DrawTexture(rec,(Texture) FirstHalo, ScaleMode.ScaleAndCrop);

		}
	}

}