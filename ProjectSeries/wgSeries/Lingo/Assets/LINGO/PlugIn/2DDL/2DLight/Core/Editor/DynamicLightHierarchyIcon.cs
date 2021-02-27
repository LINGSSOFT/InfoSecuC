using UnityEngine;
using UnityEditor;
using DynamicLight2D;

namespace DynamicLight2D
{
    [InitializeOnLoad]
    public class DynamicLightHierarchyIcon : MonoBehaviour
    {
        static readonly Texture2D _icon;
      
		static DynamicLightHierarchyIcon()
        {
			_icon = AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "2DLight/Misc/icon.png", typeof(Texture2D)) as Texture2D;
           
            if (_icon == null)
            {
                return;
            } 

			EditorApplication.hierarchyWindowItemOnGUI += hierarchyItemOnGUI;
            EditorApplication.RepaintHierarchyWindow();
        }

		static void hierarchyItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go == null)
            {
                return;
            }   

			if (_icon != null && go.GetComponent<DynamicLight>() != null)
            {
                Rect r = new Rect(selectionRect);
                r.x = r.width - 5;

                GUI.Label(r, _icon);
                return;
            }

           
        }
    }
}
