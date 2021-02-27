// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using UnityEditor;
using DoozyUI;

[CustomEditor(typeof(PlaymakerEventDispatcher), true)]
public class PlaymakerEventDispatcherInspector : Editor
{
    #region SerializedProperties
    SerializedProperty sp_dispatchGameEvents;
    SerializedProperty sp_dispatchButtonClicks;
    SerializedProperty sp_targetFSM;
    SerializedProperty sp_overrideTargetFSM;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_dispatchGameEvents = serializedObject.FindProperty("dispatchGameEvents");
        sp_dispatchButtonClicks = serializedObject.FindProperty("dispatchButtonClicks");
        sp_targetFSM = serializedObject.FindProperty("targetFSM");
        sp_overrideTargetFSM = serializedObject.FindProperty("overrideTargetFSM");
    }
    #endregion

    #region Variables
    PlaymakerEventDispatcher playmakerEventDispacher;
    #endregion

    void OnEnable()
    {
        playmakerEventDispacher = (PlaymakerEventDispatcher)target;
        UpdateSerializedProperties();
    }

    public override void OnInspectorGUI()
    {

        if (playmakerEventDispacher == null)
            playmakerEventDispacher = (PlaymakerEventDispatcher)target;

        //base.OnInspectorGUI();

        serializedObject.Update();

        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);

        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarPmEventDispatcher);
        #endregion

        if (sp_dispatchGameEvents.boolValue == false && sp_dispatchButtonClicks.boolValue == false)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarDisabled);
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarSelectListener);
            if (playmakerEventDispacher.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("Select at least one listener in order to activate the event dispatcher.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
        }
        else if (playmakerEventDispacher.targetFSM == null)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarDisabled);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            EditorGUILayout.HelpBox("Please reference a Target FSM", MessageType.Error);
        }
        else
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnabled);

            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);

            if (sp_dispatchGameEvents.boolValue && sp_dispatchButtonClicks.boolValue)
            {
                EditorGUILayout.HelpBox("Dispatching Game Events and Button Clicks...", MessageType.None);
            }
            else if (sp_dispatchGameEvents.boolValue)
            {
                EditorGUILayout.HelpBox("Dispatching Game Events...", MessageType.None);
            }
            else if (sp_dispatchButtonClicks.boolValue)
            {
                EditorGUILayout.HelpBox("Dispatching Button Clicks...", MessageType.None);
            }

            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();

        #region Show Help
        DoozyUIHelper.ResetColors();
        playmakerEventDispacher.showHelp = EditorGUILayout.ToggleLeft("Show Help", playmakerEventDispacher.showHelp, GUILayout.Width(100));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        #region Debug This
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        playmakerEventDispacher.debugThis = EditorGUILayout.ToggleLeft("Debug This", playmakerEventDispacher.debugThis, GUILayout.Width(100));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();

        #region Override Target FSM
        sp_overrideTargetFSM.boolValue = EditorGUILayout.ToggleLeft("Override Target FSM", sp_overrideTargetFSM.boolValue, GUILayout.Width(140));
        #endregion

        if (sp_overrideTargetFSM.boolValue == false)
        {
            playmakerEventDispacher.targetFSM = playmakerEventDispacher.gameObject.GetComponent<PlayMakerFSM>();
        }
        else
        {
            EditorGUILayout.PropertyField(sp_targetFSM, GUIContent.none, GUILayout.Width(270));
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (playmakerEventDispacher.targetFSM != null)
        {
            EditorGUILayout.HelpBox("Target FSM Name: " + playmakerEventDispacher.targetFSM.FsmName, MessageType.None);
        }

        DoozyUIHelper.VerticalSpace(8);

        #region Dispatch Game Events
        sp_dispatchGameEvents.boolValue = EditorGUILayout.ToggleLeft("Listen for Game Events", sp_dispatchGameEvents.boolValue);
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region Dispatch Button Clicks
        sp_dispatchButtonClicks.boolValue = EditorGUILayout.ToggleLeft("Listen for Button Clicks", sp_dispatchButtonClicks.boolValue);
        #endregion

        if (playmakerEventDispacher.showHelp)
        {
            DoozyUIHelper.VerticalSpace(8);
            DoozyUIHelper.ResetColors();
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarGeneralInfo);
            DoozyUIHelper.VerticalSpace(1);
            EditorGUILayout.HelpBox("This dispatcher auto targets the first FSM on this GameObject. You can override that and reference the FSM you want to target.", MessageType.None);
            EditorGUILayout.HelpBox("For this dispatcher to work in Playmaker you have to create FSM events named exactly as the Game Event commands or buttonNames that you want to listen for and react to, in the FSM. The event names are case sensitive.", MessageType.None);
            EditorGUILayout.HelpBox("To dispatch Game Events, you have to create, in the FSM, events named exactly as the Game Event commands you wants to catch.", MessageType.None);
            EditorGUILayout.HelpBox("To dispatch Button Clicks, you have to create, in the FSM, events named exactly as the buttonNames you wants to catch.", MessageType.None);
            EditorGUILayout.HelpBox("Debug This will print to Debug.Log all the dispatched game events and/or button clicks.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
#endif
