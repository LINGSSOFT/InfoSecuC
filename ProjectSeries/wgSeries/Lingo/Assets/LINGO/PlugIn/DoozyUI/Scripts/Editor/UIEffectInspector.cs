// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using DoozyUI;

[CustomEditor(typeof(UIEffect), true)]
public class UIEffectInspector : Editor
{
    #region SerializedProperties
    SerializedProperty sp_targetUIElement;
    SerializedProperty sp_playOnAwake;
    SerializedProperty sp_startDelay;
    SerializedProperty sp_stopInstantly;
    SerializedProperty sp_effectPosition;
    SerializedProperty sp_sortingOrderStep;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_targetUIElement = serializedObject.FindProperty("targetUIElement");
        sp_playOnAwake = serializedObject.FindProperty("playOnAwake");
        sp_startDelay = serializedObject.FindProperty("startDelay");
        sp_stopInstantly = serializedObject.FindProperty("stopInstantly");
        sp_effectPosition = serializedObject.FindProperty("effectPosition");
        sp_sortingOrderStep = serializedObject.FindProperty("sortingOrderStep");
    }
    #endregion

    #region Variables
    UIEffect uiEffect;
    UIEffect.EffectPosition effectPosition;
    #endregion

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    void OnEnable()
    {
        uiEffect = (UIEffect)target;
    }

    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();

        if (uiEffect == null)
            uiEffect = (UIEffect)target;

        UpdateSerializedProperties();

        serializedObject.Update();

        DoozyUIHelper.VerticalSpace(8);

        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiEffect);

        if (uiEffect.targetUIElement != null)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnabled);
        }
        else
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarDisabled);
            if (uiEffect.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                EditorGUILayout.HelpBox("Link a target UIElement...", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
        }

        DoozyUIHelper.VerticalSpace(4);

        #region Show Help
        DoozyUIHelper.ResetColors();
        uiEffect.showHelp = EditorGUILayout.ToggleLeft("Show Help", uiEffect.showHelp, GUILayout.Width(160));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        DoozyUIHelper.VerticalSpace(4);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        if(uiEffect.targetUIElement == null)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
        }
        else
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
        }
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Target UIElement", GUILayout.Width(110));
        EditorGUILayout.PropertyField(sp_targetUIElement, GUIContent.none, GUILayout.Width(260));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        DoozyUIHelper.ResetColors();


        if (uiEffect.targetUIElement == null && uiEffect.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Target UIElement: this effect will be linked to the show and hide events of the target element.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        if (uiEffect.targetUIElement == null)
        {
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("start delay", GUILayout.Width(65));
            sp_startDelay.floatValue = EditorGUILayout.FloatField(sp_startDelay.floatValue, GUILayout.Width(40));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (uiEffect.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Target UIElement: this effect will be linked to the show and hide events of the target element.", MessageType.None);
                EditorGUILayout.HelpBox("start delay: after the show event, for the target element, has been issued, you can set a start delay before the effect plays.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }

            GUILayout.Space(8);

            sp_playOnAwake.boolValue = EditorGUILayout.ToggleLeft("play on awake", sp_playOnAwake.boolValue, GUILayout.Width(160));
            sp_stopInstantly.boolValue = EditorGUILayout.ToggleLeft("stop instantly on hide", sp_stopInstantly.boolValue, GUILayout.Width(160));

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (uiEffect.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("play on awake: just like the UIElement's 'start hidden' option, the play on awake should be used if this effect will be visible when the scene strts. Otherwise you should let this option unchecked.", MessageType.None);
                EditorGUILayout.HelpBox("stop instantly on hide: when the hide event, for the target element, has been issued, you can stop and clear the effect instantly (it will dissapear) or you can let it fade out (for the lifetime duration of the particles).", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }

            GUILayout.Space(8);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("effect position", GUILayout.Width(90));
            effectPosition = uiEffect.effectPosition;
            effectPosition = (UIEffect.EffectPosition)EditorGUILayout.EnumPopup(effectPosition, GUILayout.Width(125));
            sp_effectPosition.enumValueIndex = (int)effectPosition;
            DoozyUIHelper.VerticalSpace(8);
            EditorGUILayout.LabelField("sorting order step", GUILayout.Width(105));
            sp_sortingOrderStep.intValue = EditorGUILayout.IntField(sp_sortingOrderStep.intValue, GUILayout.Width(40));
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            if (uiEffect.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("effect position: this option adjusts the sorting order 'moving' the effect in front or behind the target UIElement's canvas sorting order.", MessageType.None);
                EditorGUILayout.HelpBox("sorting order step: this option goes hand in hand with the previous one, by selecting how many steps/levels should the effect sorting order be ajusted with. Example: if the target UIElement's canvas sorting order is 100 and we have a sorting order step of 5 then the effect can be either at 95 (if behind) or 105 (if in front).", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }

            DoozyUIHelper.VerticalSpace(16);

            if (GUILayout.Button("Update UIEffect SortingOrder"))
            {
                uiEffect.UpdateEffectSortingOrder();
            }

            if (uiEffect.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Update UI Effect: this option helps you adjust/update the sorting order on the fly whenever you want.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
        }

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
