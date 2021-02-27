using UnityEngine;
using UnityEditor;

namespace DoozyUI
{
    [CustomEditor(typeof(UpdateSortingLayerName), true)]
    public class UpdateSortingLayerNameInspector : Editor
    {
        UpdateSortingLayerName updateSortingLayerName;

        void OnEnable()
        {
            updateSortingLayerName = (UpdateSortingLayerName)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(8);
            if (GUILayout.Button("Update Canvases & Renderers", GUILayout.Height(EditorGUIUtility.singleLineHeight * 3)))
            {
                updateSortingLayerName.UpdateCanvases();
                updateSortingLayerName.UpdateRenderers();
            }

            EditorGUILayout.HelpBox("This updates the all the children's canvases and renderers sorting layer name to the new sorting layer name", MessageType.Info);
        }
    }
}
