// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DoozyUI;

[CustomEditor(typeof(UITrigger))]
public class UITriggerInspector : Editor
{
    #region Serialized Properties
    SerializedProperty sp_triggerOnGameEvent;
    SerializedProperty sp_gameEvent;

    SerializedProperty sp_triggerOnButtonClick;
    SerializedProperty sp_buttonName;

    SerializedProperty sp_dispatchAll;

    SerializedProperty sp_onTriggerEvent;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_triggerOnGameEvent = serializedObject.FindProperty("triggerOnGameEvent");
        sp_gameEvent = serializedObject.FindProperty("gameEvent");

        sp_triggerOnButtonClick = serializedObject.FindProperty("triggerOnButtonClick");
        sp_buttonName = serializedObject.FindProperty("buttonName");

        sp_dispatchAll = serializedObject.FindProperty("dispatchAll");

        sp_onTriggerEvent = serializedObject.FindProperty("onTriggerEvent");
    }
    #endregion

    #region Variables
    UITrigger uiTrigger;
    bool isTriggerEnabled = false;

    string[] buttonNames;
    int buttonNameCurrentIndex = 0;
    #endregion

    #region Update ButtonNames Popup
    void UpdateButtonNamesPopup()
    {
        //we create the string array that we use for the gui popup
        buttonNames = UIManager.GetButtonNames();
    }
    #endregion

    #region Show Trigger Event
    void ShowTriggerEvent()
    {
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
        EditorGUILayout.PropertyField(sp_onTriggerEvent, GUILayout.Width(410));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);

        DoozyUIHelper.VerticalSpace(4);

        #region Send GameEvents
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarSendGameEvents);

        if (uiTrigger.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Type in the game events that you want this trigger to send", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        if (GUILayout.Button("add game event", GUILayout.Width(224)))
        {
            if (uiTrigger.gameEvents == null)
            {
                uiTrigger.gameEvents = new List<string>();
            }

            uiTrigger.gameEvents.Add(string.Empty);
        }

        if (uiTrigger.gameEvents != null)  //we check if the gameEvents list has any items in it
        {
            for (int i = 0; i < uiTrigger.gameEvents.Count; i++)    //we show the list of elements
            {
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                uiTrigger.gameEvents[i] = EditorGUILayout.TextField(uiTrigger.gameEvents[i], GUILayout.Width(200));
                if (GUILayout.Button("x", GUILayout.Height(12)))
                {
                    uiTrigger.gameEvents.RemoveAt(i);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
        DoozyUIHelper.ResetColors();
        #endregion
    }
    #endregion

    #region Show Help Checkbox
    void ShowHelpCheckbox()
    {
        DoozyUIHelper.ResetColors();
        uiTrigger.showHelp = EditorGUILayout.ToggleLeft("Show Help", uiTrigger.showHelp, GUILayout.Width(160));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
    }
    #endregion

    #region Show Help Instructions
    void ShowHelpInstructions()
    {
        if (isTriggerEnabled)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);

            if (sp_triggerOnGameEvent.boolValue)
            {
                if (sp_dispatchAll.boolValue)
                {
                    EditorGUILayout.HelpBox("Dispatching ALL Game Events...", MessageType.None);

                    if (uiTrigger.showHelp)
                    {
                        DoozyUIHelper.ResetColors();
                        EditorGUILayout.HelpBox("Because you selected the Dispatch ALL option, you should be aware that in order for it to work as intended you must call a function that has a string as a paramater and...", MessageType.Info);
                        EditorGUILayout.HelpBox("It is VERY IMPORTANT that you select, in the 'On Trigger Event (String)' section, from the right drop down list, a function that is located in the 'Dynamic string' section (on top) and not the 'Static parameters' section (on bottom). See the documentation for mode details.", MessageType.Warning);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Dispatching the [" + sp_gameEvent.stringValue + "] Game Event...", MessageType.None);
                }

            }
            else if (sp_triggerOnButtonClick.boolValue)
            {
                if (sp_dispatchAll.boolValue)
                {
                    EditorGUILayout.HelpBox("Dispatching ALL Button Clicks...", MessageType.None);

                    if(uiTrigger.showHelp)
                    {
                        DoozyUIHelper.ResetColors();
                        EditorGUILayout.HelpBox("Because you selected the Dispatch ALL option, you should be aware that in order for it to work as intended you must call a function that has a string as a paramater and...", MessageType.Info);
                        EditorGUILayout.HelpBox("It is VERY IMPORTANT that you select, in the 'On Trigger Event (String)' section, from the right drop down list, a function that is located in the 'Dynamic string' section (on top) and not the 'Static parameters' section (on bottom). See the documentation for mode details.", MessageType.Warning);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Dispatching Button Clicks for the [" + sp_buttonName.stringValue + "] button...", MessageType.None);
                }
            }
        }
        else if (uiTrigger.showHelp)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);

            if (sp_triggerOnGameEvent.boolValue)
            {
                EditorGUILayout.HelpBox("Enter the game event you want this trigger to listen for. Or select Dispatch ALL to send all game events.", MessageType.None);
            }
            else if (sp_triggerOnButtonClick.boolValue)
            {
                EditorGUILayout.HelpBox("Select the button name you want this trigger to listen for. Or select Dispatch ALL to send all button clicks.", MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox("You can activate this trigger by listening for either a game event or a button name.", MessageType.None);
            }
        }

        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
    }
    #endregion

    void OnEnable()
    {
        uiTrigger = (UITrigger)target;

        UpdateSerializedProperties();

        UpdateButtonNamesPopup();
    }

    public override void OnInspectorGUI()
    {
        if (uiTrigger == null)
            uiTrigger = (UITrigger)target;

        //base.OnInspectorGUI();

        serializedObject.Update();

        DoozyUIHelper.VerticalSpace(8);

        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiTrigger);
        #endregion

        if (isTriggerEnabled)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnabled);
        }
        else
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarDisabled);
        }

        if (sp_triggerOnGameEvent.boolValue == false && sp_triggerOnButtonClick.boolValue == false)
        {
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarSelectListener);

            ShowHelpInstructions();

            DoozyUIHelper.VerticalSpace(8);

            ShowHelpCheckbox();

            DoozyUIHelper.VerticalSpace(8);

            sp_gameEvent.stringValue = string.Empty;
            sp_buttonName.stringValue = string.Empty;
            isTriggerEnabled = false;
            buttonNameCurrentIndex = 0;
            sp_dispatchAll.boolValue = false;

            #region Listen Game Events
            sp_triggerOnGameEvent.boolValue = EditorGUILayout.ToggleLeft("Listen for Game Events", sp_triggerOnGameEvent.boolValue);
            #endregion

            DoozyUIHelper.VerticalSpace(4);

            #region Listen Button Clicks
            sp_triggerOnButtonClick.boolValue = EditorGUILayout.ToggleLeft("Listen for Button Clicks", sp_triggerOnButtonClick.boolValue);
            #endregion
        }
        else
        {
            if (sp_triggerOnGameEvent.boolValue)
            {
                if (string.IsNullOrEmpty(sp_gameEvent.stringValue) && sp_dispatchAll.boolValue == false)
                {
                    DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnterGameEvent);
                    isTriggerEnabled = false;
                }
                else
                {
                    isTriggerEnabled = true;
                }

                ShowHelpInstructions();

                DoozyUIHelper.VerticalSpace(8);

                ShowHelpCheckbox();

                DoozyUIHelper.VerticalSpace(8);

                EditorGUILayout.BeginHorizontal();
                #region Listen Game Events
                sp_triggerOnGameEvent.boolValue = EditorGUILayout.ToggleLeft("Listen for Game Events", sp_triggerOnGameEvent.boolValue, GUILayout.Width(180));
                #endregion
                GUILayout.Space(8);
                #region DipatchAll
                sp_dispatchAll.boolValue = EditorGUILayout.ToggleLeft("Dispatch All Game Events", sp_dispatchAll.boolValue, GUILayout.Width(180));
                #endregion
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (sp_dispatchAll.boolValue == false)
                {
                    #region Game Event
                    DoozyUIHelper.VerticalSpace(4);

                    if(string.IsNullOrEmpty(sp_gameEvent.stringValue))
                    {
                        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    }
                    else
                    {
                        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Game Event", GUILayout.Width(80));
                    sp_gameEvent.stringValue = EditorGUILayout.TextField(sp_gameEvent.stringValue, GUILayout.Width(320));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    DoozyUIHelper.ResetColors();
                    #endregion
                }
            }
            else if (sp_triggerOnButtonClick.boolValue)
            {
                UpdateButtonNamesPopup();

                if (string.IsNullOrEmpty(sp_buttonName.stringValue)
                    || sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME)
                    && sp_dispatchAll.boolValue == false)
                {
                    DoozyUIHelper.DrawTexture(DoozyUIResources.BarEnterButtonName);
                    isTriggerEnabled = false;
                }
                else
                {
                    isTriggerEnabled = true;
                }

                ShowHelpInstructions();

                DoozyUIHelper.VerticalSpace(8);

                ShowHelpCheckbox();

                DoozyUIHelper.VerticalSpace(8);

                EditorGUILayout.BeginHorizontal();
                #region Listen Button Clicks
                sp_triggerOnButtonClick.boolValue = EditorGUILayout.ToggleLeft("Listen for Button Clicks", sp_triggerOnButtonClick.boolValue, GUILayout.Width(180));
                #endregion
                GUILayout.Space(8);
                #region DipatchAll
                sp_dispatchAll.boolValue = EditorGUILayout.ToggleLeft("Dispatch All Button Clicks", sp_dispatchAll.boolValue, GUILayout.Width(180));
                #endregion
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (sp_dispatchAll.boolValue == false)
                {
                    #region Button Name

                    if(UIManager.GetIndexForButtonName(sp_buttonName.stringValue) == -1) //we have a button name that is not in the database; we reset this value
                    {
                        sp_buttonName.stringValue = UIManager.DEFAULT_BUTTON_NAME;
                    }

                    if (sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME))
                    {
                        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    }
                    else
                    {
                        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    }

                    DoozyUIHelper.VerticalSpace(4);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Button Name", GUILayout.Width(80));
                    buttonNameCurrentIndex = UIManager.GetIndexForButtonName(sp_buttonName.stringValue);
                    buttonNameCurrentIndex = EditorGUILayout.Popup(buttonNameCurrentIndex, buttonNames, GUILayout.Width(320));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    sp_buttonName.stringValue = buttonNames[buttonNameCurrentIndex];

                    DoozyUIHelper.ResetColors();
                    #endregion
                }
            }

            DoozyUIHelper.VerticalSpace(8);

            if (isTriggerEnabled)
            {
                ShowTriggerEvent();
            }
        }

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
