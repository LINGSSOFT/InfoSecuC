namespace DynamicLight2D
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	
	public class DynamicLightDrawGizmo{

		[DrawGizmo(GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy)]
		private static void drawGizmoNow(DynamicLight dl, GizmoType gizmoType)
		{

			PostProcessMethods.checkIconTexture();

			//Texture2D t = AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/logo2DDL_gizmos.png", typeof(Texture2D)) as Texture2D;

			//Debug.Log(EditorUtils.getMainRelativepath() + "2DLight/Misc/logo2DDL_gizmos.png");
			//float v = 0.08f;// Camera.current.orthographicSize * .003f;
			Gizmos.DrawIcon(dl.transform.position, "logo2DDL_gizmos.png", false);
			//Gizmos.DrawGUITexture(new Rect(dl.transform.position.x - t.width*.5f * v, dl.transform.position.y + t.height*.5f * v, t.width * v, -1*t.height * v), t);
		}

	}
}
