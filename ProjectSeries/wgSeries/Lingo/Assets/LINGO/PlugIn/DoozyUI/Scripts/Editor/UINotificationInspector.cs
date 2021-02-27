// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using DoozyUI;

[CustomEditor(typeof(UINotification), true)]
public class UINotificationInspector : Editor
{
    #region Serialized Properties
    SerializedProperty sp_notificationContainer;
    SerializedProperty sp_overlay;
    SerializedProperty sp_title;
    SerializedProperty sp_message;
    SerializedProperty sp_icon;
    SerializedProperty sp_buttons;
    SerializedProperty sp_closeButton;
    SerializedProperty sp_specialElements;
    SerializedProperty sp_effects;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_notificationContainer = serializedObject.FindProperty("notificationContainer");
        sp_overlay = serializedObject.FindProperty("overlay");
        sp_title = serializedObject.FindProperty("title");
        sp_message = serializedObject.FindProperty("message");
        sp_icon = serializedObject.FindProperty("icon");
        sp_closeButton = serializedObject.FindProperty("closeButton");
        sp_buttons = serializedObject.FindProperty("buttons");
        sp_specialElements = serializedObject.FindProperty("specialElements");
        sp_effects = serializedObject.FindProperty("effects");
    }
    #endregion

    #region Variables
    UINotification uiNotification;
    #endregion

    void OnEnable()
    {
        uiNotification = (UINotification)target;
        UpdateSerializedProperties();
    }

    public override void OnInspectorGUI()
    {

        if (uiNotification == null)
            uiNotification = (UINotification)target;

        //base.OnInspectorGUI();

        serializedObject.Update();

        LinkChildUIElementsToNotification();

        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiNotification);
        #endregion
        if (uiNotification.notificationContainer != null) //we check if this notification has at least a notification container attached (if not, we disable it)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnabled);
        }
        else
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarDisabled);
        }
        DoozyUIHelper.VerticalSpace(8);
        EditorGUILayout.BeginHorizontal();
        {
            #region Show Help
            DoozyUIHelper.ResetColors();
            uiNotification.showHelp = EditorGUILayout.ToggleLeft("Show Help", uiNotification.showHelp, GUILayout.Width(80));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            #endregion
            #region Listen for Back Button
            uiNotification.listenForBackButton = EditorGUILayout.ToggleLeft("Listen for Back Button", uiNotification.listenForBackButton);
            #endregion
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        DoozyUIHelper.VerticalSpace(8);
        #region Notification Container
        if (uiNotification.notificationContainer != null)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
        }
        else
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(sp_notificationContainer, true, GUILayout.Width(410));
        EditorGUILayout.EndHorizontal();
        DoozyUIHelper.ResetColors();

        if (uiNotification.showHelp)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            EditorGUILayout.HelpBox("required", MessageType.None);
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("This is a chld gameObject of the notification, with an UIElement component attached, that will serve as the main container of all the other notification elements. It should contain everything besides the overlay.", MessageType.None);
            EditorGUILayout.HelpBox("To play sounds when it appears and when it dissapears you can add them in the attached UIElement IN and OUT animations.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        #endregion
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Overlay
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_overlay, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("This is the fullscreen overlay / color tint.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Title
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_title, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("This gameobject should have either a Text or a TextMeshProUGUI component attached.", MessageType.None);
                EditorGUILayout.HelpBox("If you used a TextMeshProUGUI componenet make sure TextMeshPro it is enabled in the UIManager or the Control Panel.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Message
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_message, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("This gameobject should have either a Text or a TextMeshProUGUI component attached.", MessageType.None);
                EditorGUILayout.HelpBox("If you used a TextMeshProUGUI componenet make sure TextMeshPro it is enabled in the UIManager or the Control Panel.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Icon
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_icon, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Every notification can have a customized icon or you can leave it null. Link the Image component here and pass in the icon Sprite when you call the ShowNotification method.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region CloseButton
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_closeButton, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("This is a special button as it coveres the entire notification area. This allows the user to dismiss at once the notification, just by touching/clicking on it.", MessageType.None);
                EditorGUILayout.HelpBox("TIP: you can attach a Button component to the Overlay. link it here and have a full screen close button", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Buttons
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_buttons, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("These are the buttons you would like to be available for this notification. They can be turned on or off when you call the ShowNotification method.", MessageType.None);
                EditorGUILayout.HelpBox("See documentation for more details.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Special Elements
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_specialElements, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("If you added stars or anything else with an UIElement attached, you need to link it here", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);
        if (uiNotification.notificationContainer != null)
        {
            #region Effects
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sp_effects, true, GUILayout.Width(410));
            EditorGUILayout.EndHorizontal();

            if (uiNotification.showHelp)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                EditorGUILayout.HelpBox("optional", MessageType.None);
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("If you added any effects with UIEffect attached, you need to link them here so that they can work as intended", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion
        }
        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    #region Link Child UIElements To Notification
    void LinkChildUIElementsToNotification()
    {
        UIElement[] childUIElements = uiNotification.GetComponentsInChildren<UIElement>(true);
        if (childUIElements != null && childUIElements.Length > 0)
        {
            for (int i = 0; i < childUIElements.Length; i++)
            {
                childUIElements[i].linkedToNotification = true;
            }
        }
    }
    #endregion
}
