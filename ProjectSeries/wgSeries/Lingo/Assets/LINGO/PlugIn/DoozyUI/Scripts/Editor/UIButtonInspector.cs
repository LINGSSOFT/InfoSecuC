// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using DoozyUI;
using System.Collections.Generic;

[CustomEditor(typeof(DoozyUI.UIButton), true)]
public class UIButtonInspector : Editor
{
    #region SerializedProperties
    SerializedProperty sp_buttonName;
    SerializedProperty sp_onClickSound;

    SerializedProperty sp_showHelp;
    SerializedProperty sp_allowMultipleClicks;
    SerializedProperty sp_disableButtonInterval;
    SerializedProperty sp_waitForOnClickAnimation;

    SerializedProperty sp_addToNavigationHistory;
    SerializedProperty sp_backButton;

    SerializedProperty sp_useOnClickAnimations;
    SerializedProperty sp_useNormalStateAnimations;
    SerializedProperty sp_useHighlightedStateAnimations;

    SerializedProperty sp_activeOnclickAnimationsPresetIndex;
    SerializedProperty sp_activeNormalAnimationsPresetIndex;
    SerializedProperty sp_activeHighlightedAnimationsPresetIndex;

    //MOVE PUNCH
    SerializedProperty sp_punchPositionEnabled;
    SerializedProperty sp_punchPositionPunch;
    SerializedProperty sp_punchPositionSnapping;
    SerializedProperty sp_punchPositionDuration;
    SerializedProperty sp_punchPositionVibrato;
    SerializedProperty sp_punchPositionElasticity;
    SerializedProperty sp_punchPositionDelay;

    //ROTATE PUNCH
    SerializedProperty sp_punchRotationEnabled;
    SerializedProperty sp_punchRotationPunch;
    SerializedProperty sp_punchRotationDuration;
    SerializedProperty sp_punchRotationVibrato;
    SerializedProperty sp_punchRotationElasticity;
    SerializedProperty sp_punchRotationDelay;

    //SCALE PUNCH
    SerializedProperty sp_punchScaleEnabled;
    SerializedProperty sp_punchScalePunch;
    SerializedProperty sp_punchScaleDuration;
    SerializedProperty sp_punchScaleVibrato;
    SerializedProperty sp_punchScaleElasticity;
    SerializedProperty sp_punchScaleDelay;

    SerializedProperty sp_showElements;
    SerializedProperty sp_hideElements;
    SerializedProperty sp_gameEvents;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_buttonName = serializedObject.FindProperty("buttonName");
        sp_onClickSound = serializedObject.FindProperty("onClickSound");

        sp_showHelp = serializedObject.FindProperty("showHelp");
        sp_allowMultipleClicks = serializedObject.FindProperty("allowMultipleClicks");
        sp_disableButtonInterval = serializedObject.FindProperty("disableButtonInterval");
        sp_waitForOnClickAnimation = serializedObject.FindProperty("waitForOnClickAnimation");

        sp_addToNavigationHistory = serializedObject.FindProperty("addToNavigationHistory");
        sp_backButton = serializedObject.FindProperty("backButton");

        sp_useOnClickAnimations = serializedObject.FindProperty("useOnClickAnimations");
        sp_useNormalStateAnimations = serializedObject.FindProperty("useNormalStateAnimations");
        sp_useHighlightedStateAnimations = serializedObject.FindProperty("useHighlightedStateAnimations");

        sp_activeOnclickAnimationsPresetIndex = serializedObject.FindProperty("activeOnclickAnimationsPresetIndex");
        sp_activeNormalAnimationsPresetIndex = serializedObject.FindProperty("activeNormalAnimationsPresetIndex");
        sp_activeHighlightedAnimationsPresetIndex = serializedObject.FindProperty("activeHighlightedAnimationsPresetIndex");

        //MOVE PUNCH
        sp_punchPositionEnabled = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionEnabled");
        sp_punchPositionPunch = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionPunch");
        sp_punchPositionSnapping = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionSnapping");
        sp_punchPositionDuration = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionDuration");
        sp_punchPositionVibrato = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionVibrato");
        sp_punchPositionElasticity = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionElasticity");
        sp_punchPositionDelay = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchPositionDelay");

        //ROTATE PUNCH
        sp_punchRotationEnabled = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationEnabled");
        sp_punchRotationPunch = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationPunch");
        sp_punchRotationDuration = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationDuration");
        sp_punchRotationVibrato = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationVibrato");
        sp_punchRotationElasticity = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationElasticity");
        sp_punchRotationDelay = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchRotationDelay");

        //SCALE PUNCH
        sp_punchScaleEnabled = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScaleEnabled");
        sp_punchScalePunch = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScalePunch");
        sp_punchScaleDuration = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScaleDuration");
        sp_punchScaleVibrato = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScaleVibrato");
        sp_punchScaleElasticity = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScaleElasticity");
        sp_punchScaleDelay = serializedObject.FindProperty("onClickAnimationSettings").FindPropertyRelative("punchScaleDelay");

        sp_showElements = serializedObject.FindProperty("showElements");
        sp_hideElements = serializedObject.FindProperty("hideElements");
        sp_gameEvents = serializedObject.FindProperty("gameEvents");
    }
    #endregion

    #region Variables
    DoozyUI.UIButton uiButton;
    UIAnimationManager uiAnimationManager;

    string[] elementNames;
    string[] buttonNames;
    string[] buttonSounds;

    int buttonNameCurrentIndex = 0;
    int buttonSoundCurrentIndex = 0;

    string tempButtonNameString = string.Empty;
    string tempButtonSoundString = string.Empty;

    bool newButtonName = false;
    bool renameButtonName = false;
    bool deleteButtonName = false;

    bool newButtonSound = false;
    bool renameButtonSound = false;
    bool deleteButtonSound = false;

    List<int> showElementsIndex;
    List<int> hideElementsIndex;

    bool saveOnClickAnimationPreset = false;
    bool deleteOnClickAnimationPreset = false;
    string[] onClickAnimationPresets;
    string newOnClickAnimationPresetName = "";

    bool saveNormalAnimationPreset = false;
    bool deleteNormalAnimationPreset = false;
    string newNormalAnimationPresetName = "";

    bool saveHighlightedAnimationPreset = false;
    bool deleteHighlightedAnimationPreset = false;
    string newHighlightedAnimationPresetName = "";

    string[] buttonLoopsAnimationPresets;
    #endregion

    #region Properties
    DoozyUI.UIButton GetUIButton { get { if (uiButton == null) uiButton = (DoozyUI.UIButton)target; return uiButton; } }
    #endregion

    #region Update ElementNames, ButtonNames and ButtonSounds Popup
    void UpdateElementNamesPopup()
    {
        //we create the string array that we use for the gui popup
        elementNames = UIManager.GetElementNames();
    }
    void UpdateButtonNamesPopup()
    {
        //we create the string array that we use for the gui popup
        buttonNames = UIManager.GetButtonNames();
    }
    void UpdateButtonSoundsPopup()
    {
        //we create the string array that we use fro the gui popup
        buttonSounds = UIManager.GetButtonSounds();
    }
    #endregion

    #region Show Elements, Hide Elements, GameEvents
    void UpdateShowElementsIndex()
    {
        if (showElementsIndex == null)
        {
            showElementsIndex = new List<int>();
        }
        else
        {
            showElementsIndex.Clear();
        }

        if (GetUIButton.showElements == null)
        {
            GetUIButton.showElements = new List<string>();
        }
        else if (GetUIButton.showElements.Count > 0)
        {
            for (int i = 0; i < GetUIButton.showElements.Count; i++)
            {
                showElementsIndex.Add(UIManager.GetIndexForElementName(GetUIButton.showElements[i]));
            }
        }
    }
    void UpdateHideElementsIndex()
    {
        if (hideElementsIndex == null)
        {
            hideElementsIndex = new List<int>();
        }
        else
        {
            hideElementsIndex.Clear();
        }

        if (GetUIButton.hideElements == null)
        {
            GetUIButton.hideElements = new List<string>();
        }
        else if (GetUIButton.hideElements.Count > 0)
        {
            for (int i = 0; i < GetUIButton.hideElements.Count; i++)
            {
                hideElementsIndex.Add(UIManager.GetIndexForElementName(GetUIButton.hideElements[i]));
            }
        }
    }
    void UpdateGameEvents()
    {
        if (GetUIButton.gameEvents == null)
        {
            GetUIButton.gameEvents = new List<string>();
        }
    }
    #endregion

    #region Update - OnCllick, Normal and Highlighted Presets
    void UpdateAnimationPresetsFromFiles()
    {
        onClickAnimationPresets = GetUIButton.GetOnClickAnimationsPresetNames; //preset names for OnClick Animations
        buttonLoopsAnimationPresets = GetUIButton.GetButtonLoopsAnimationsPresetNames; //preset named for Normal and Highlighted Animations

        //OnClick Animations
        if (UIManager.IsStringInArray(onClickAnimationPresets, GetUIButton.onClickAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIButton.onClickAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIButton.activeOnclickAnimationsPresetIndex = UIManager.GetIndexForStringInArray(onClickAnimationPresets, GetUIButton.onClickAnimationsPresetName);

        //Normal Animations
        if (UIManager.IsStringInArray(buttonLoopsAnimationPresets, GetUIButton.normalAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIButton.normalAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIButton.activeNormalAnimationsPresetIndex = UIManager.GetIndexForStringInArray(buttonLoopsAnimationPresets, GetUIButton.normalAnimationsPresetName);

        //Highlighted Animations
        if (UIManager.IsStringInArray(buttonLoopsAnimationPresets, GetUIButton.highlightedAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIButton.highlightedAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIButton.activeHighlightedAnimationsPresetIndex = UIManager.GetIndexForStringInArray(buttonLoopsAnimationPresets, GetUIButton.highlightedAnimationsPresetName);
    }
    #endregion

    public override bool RequiresConstantRepaint() { return true; }

    void OnEnable()
    {
        uiButton = (DoozyUI.UIButton)target;
        uiAnimationManager = GetUIButton.GetAnimationManager;
        UpdateAnimationPresetsFromFiles();
        UpdateElementNamesPopup();
        UpdateButtonNamesPopup();
        UpdateButtonSoundsPopup();
        UpdateShowElementsIndex();
        UpdateHideElementsIndex();
        UpdateGameEvents();
    }

    public override void OnInspectorGUI()
    {
        UpdateSerializedProperties();
        serializedObject.Update();
        UpdateButtonNamesPopup();
        UpdateButtonSoundsPopup();
        UpdateElementNamesPopup();
        UpdateShowElementsIndex();
        UpdateHideElementsIndex();
        UpdateGameEvents();
        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiButton);
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        #region Show Help
        DoozyUIHelper.ResetColors();
        sp_showHelp.boolValue = EditorGUILayout.ToggleLeft("Show Help", sp_showHelp.boolValue, GUILayout.Width(160));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        #region Allow Multiple Clicks
        sp_allowMultipleClicks.boolValue = EditorGUILayout.ToggleLeft("Allow Multiple Clicks", sp_allowMultipleClicks.boolValue, GUILayout.Width(160));
        if (sp_showHelp.boolValue && sp_allowMultipleClicks.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("It allows the user to press the button multiple times without restrictions.", MessageType.None);
            EditorGUILayout.HelpBox("If you want to disable the button after each click for a set interval then disable the allow multiple clicks option.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        if (sp_allowMultipleClicks.boolValue == false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Disable Button Interval", GUILayout.Width(140));
            sp_disableButtonInterval.floatValue = EditorGUILayout.FloatField(sp_disableButtonInterval.floatValue, GUILayout.Width(56));
            EditorGUILayout.EndHorizontal();
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("After each click the button is disabled for the set interval. Default is 0.5 seconds.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
        }
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        #region Wait for OnClick Animations
        if (GetUIButton.AreOnClickAnimationsEnabled)
        {
            sp_waitForOnClickAnimation.boolValue = EditorGUILayout.ToggleLeft("Wait for OnClick Animation to finish", sp_waitForOnClickAnimation.boolValue, GUILayout.Width(256));
            DoozyUIHelper.VerticalSpace(4);
        }
        #endregion
        #region Button Name
        if (newButtonName == false && renameButtonName == false && deleteButtonName == false)
        {
            EditorGUILayout.LabelField("Button Name", GUILayout.Width(200));
            EditorGUILayout.BeginHorizontal();
            {
                if (sp_backButton.boolValue == true && sp_buttonName.stringValue.Equals("Back") == false) //CASE: we just ticked the 'Is Back Button' and the buttonNameCurrentIndex is not set to the 'Back' button --> we set the index to the 'Back' button
                {
                    //we are looking for the 'Back' button index in the database
                    int backButtonIndex = UIManager.GetIndexForButtonName("Back");
                    if (backButtonIndex == -1)   //we didn't not find a 'Back' button --> something went wrong --> we create it now
                    {
                        UIManager.NewButtonName("Back");
                        UpdateButtonNamesPopup(); //we update the popup list that we show in the inspector
                        for (int i = 0; i < UIManager.GetDoozyUIData.buttonNames.Count; i++)
                        {
                            if (UIManager.GetDoozyUIData.buttonNames[i].buttonName.Equals("Back")) //because we sorted the list, we need to find the 'Back' button index again, as we do not know where it is
                            {
                                backButtonIndex = i;
                                break;
                            }
                        }
                    }
                    sp_buttonName.stringValue = "Back";
                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                }
                else if (sp_backButton.boolValue == false && sp_buttonName.stringValue.Equals("Back")) //CASE: we just unticked 'Is Back Button' and the buttonNameCurrentIndes is set to the 'Back' button --> se set the index to the default button name
                {
                    sp_buttonName.stringValue = UIManager.DEFAULT_BUTTON_NAME;
                }
                if (UIManager.GetIndexForButtonName(sp_buttonName.stringValue) == -1)
                {
                    DoozyUIRedundancyCheck.UIButtonRedundancyCheck(GetUIButton);
                }
                buttonNameCurrentIndex = UIManager.GetIndexForButtonName(sp_buttonName.stringValue); //we get the index for the current button name (if there is an error, it will be set to the default button name)
                buttonNameCurrentIndex = EditorGUILayout.Popup(buttonNameCurrentIndex, buttonNames, GUILayout.Width(200));
                sp_buttonName.stringValue = UIManager.GetDoozyUIData.buttonNames[buttonNameCurrentIndex].buttonName;
                GUILayout.Space(16);
                sp_backButton.boolValue = EditorGUILayout.ToggleLeft("Is Back Button", sp_backButton.boolValue, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }
        else if (newButtonName == true)
        {
            EditorGUILayout.LabelField("Create new button name", GUILayout.Width(200));
            DoozyUIHelper.ResetColors();
            tempButtonNameString = EditorGUILayout.TextField(tempButtonNameString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        else if (renameButtonName == true)
        {
            EditorGUILayout.LabelField("Rename button?", GUILayout.Width(200));
            DoozyUIHelper.ResetColors();
            tempButtonNameString = EditorGUILayout.TextField(tempButtonNameString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        else if (deleteButtonName == true)
        {
            EditorGUILayout.LabelField("Do you want to delete?", GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            EditorGUILayout.LabelField(tempButtonNameString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        EditorGUILayout.BeginHorizontal();
        {
            if (newButtonName == false && renameButtonName == false && deleteButtonName == false)
            {
                if (sp_buttonName.stringValue.Equals("Back") == false
                    && sp_buttonName.stringValue.Equals("ToggleSound") == false
                    && sp_buttonName.stringValue.Equals("ToggleMusic") == false
                    && sp_buttonName.stringValue.Equals("TogglePause") == false
                    && sp_buttonName.stringValue.Equals("ApplicationQuit") == false)
                {
                    if (sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME))
                    {
                        if (GUILayout.Button("new", GUILayout.Width(200), GUILayout.Height(16)))
                        {
                            tempButtonNameString = string.Empty;
                            newButtonName = true;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("new", GUILayout.Width(64), GUILayout.Height(16)))
                        {
                            tempButtonNameString = string.Empty;
                            newButtonName = true;
                        }
                        if (GUILayout.Button("rename", GUILayout.Width(64), GUILayout.Height(16)))
                        {
                            if (tempButtonNameString.Equals(UIManager.DEFAULT_BUTTON_NAME))
                            {
                                Debug.Log("[DoozyUI] You cannot (and should not) rename the default button name.");
                                return;
                            }
                            tempButtonNameString = sp_buttonName.stringValue;
                            renameButtonName = true;
                        }
                        if (GUILayout.Button("delete", GUILayout.Width(64), GUILayout.Height(16)))
                        {
                            if (tempButtonNameString.Equals(UIManager.DEFAULT_BUTTON_NAME))
                            {
                                Debug.Log("[DoozyUI] You cannot (and should not) delete the default button name.");
                                return;
                            }
                            tempButtonNameString = sp_buttonName.stringValue;
                            deleteButtonName = true;
                        }
                    }
                    GUILayout.Space(16);
                    if (UIManager.isNavigationEnabled)
                    {
                        if (sp_backButton.boolValue == false
                            && sp_buttonName.stringValue.Equals("GoToMainMenu") == false
                            && sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME) == false)
                        {
                            sp_addToNavigationHistory.boolValue = EditorGUILayout.ToggleLeft("Add to Navigation History", sp_addToNavigationHistory.boolValue, GUILayout.Width(120));
                        }
                        else
                        {
                            EditorGUILayout.LabelField("~ Not Available ~", GUILayout.Width(120));
                            sp_addToNavigationHistory.boolValue = false;
                        }
                    }
                }
                else if (sp_buttonName.stringValue.Equals("ToggleSound")
                        || sp_buttonName.stringValue.Equals("ToggleMusic")
                        || sp_buttonName.stringValue.Equals("TogglePause"))
                {
                    GUILayout.Space(220);
                    if (UIManager.isNavigationEnabled)
                    {
                        if (sp_backButton.boolValue == false
                            && sp_buttonName.stringValue.Equals("GoToMainMenu") == false
                            && sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME) == false
                            && sp_buttonName.stringValue.Equals("ToggleSound") == false
                            && sp_buttonName.stringValue.Equals("ToggleMusic") == false
                            && sp_buttonName.stringValue.Equals("ApplicationQuit") == false)
                        {
                            sp_addToNavigationHistory.boolValue = EditorGUILayout.ToggleLeft("Add to Navigation History", sp_addToNavigationHistory.boolValue, GUILayout.Width(120));
                        }
                        else
                        {
                            EditorGUILayout.LabelField("~ Not Available ~", GUILayout.Width(120));
                            sp_addToNavigationHistory.boolValue = false;
                        }
                    }
                }
            }
            else if (newButtonName == true)
            {
                #region New ButtonName
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("confirm", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    UIManager.TrimStartAndEndSpaces(tempButtonNameString);
                    if (tempButtonNameString.Equals(string.Empty) == false                      //we make sure the new name is not empty
                        && tempButtonNameString.Equals(UIManager.DEFAULT_BUTTON_NAME) == false  //we check that is not the default name
                        && UIManager.GetIndexForButtonName(tempButtonNameString) == -1           //we make sure there are no duplicates
                        && tempButtonNameString.Equals("Back") == false)                        //we make sure the name is not 'Back' as that is a reserved name
                    {
                        UIManager.NewButtonName(tempButtonNameString);
                    }
                    sp_buttonName.stringValue = tempButtonNameString;
                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                    UpdateButtonNamesPopup();                   //we update the string array that shows the list of button names in the inspector
                    tempButtonNameString = string.Empty;        //we clear the temporary name holder
                    newButtonName = false;                      //we show the initial menu for the button name
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    tempButtonNameString = string.Empty;
                    newButtonName = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                #endregion
            }
            else if (renameButtonName == true)
            {
                #region Rename ButtonName
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("confirm", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    UIManager.TrimStartAndEndSpaces(tempButtonNameString);
                    if (tempButtonNameString.Equals(string.Empty) == false                     //we make sure the new name is not empty
                       && tempButtonNameString.Equals(UIManager.DEFAULT_BUTTON_NAME) == false  //we check that is not the default name
                       && UIManager.GetIndexForButtonName(tempButtonNameString) == -1           //we make sure there are no duplicates
                       && tempButtonNameString.Equals("Back") == false)                        //we make sure the name is not 'Back' as that is a reserved name
                    {
                        UIManager.RenameButtonName(buttonNameCurrentIndex, tempButtonNameString);
                        UpdateButtonNamesPopup();
                        sp_buttonName.stringValue = tempButtonNameString;
                        DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                    }
                    tempButtonNameString = string.Empty;
                    renameButtonName = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    tempButtonNameString = string.Empty;
                    renameButtonName = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                #endregion
            }
            else if (deleteButtonName == true)
            {
                #region Delete ButtonName
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("yes", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    if (sp_buttonName.stringValue.Equals(UIManager.DEFAULT_BUTTON_NAME))
                    {
                        Debug.Log("[DoozyUI] You cannot (and should not) delete the default button name '" + UIManager.DEFAULT_BUTTON_NAME + "'.");
                    }
                    else if (sp_buttonName.stringValue.Equals("Back"))
                    {
                        Debug.Log("[DoozyUI] You cannot (and should not) delete the 'Back' button.");
                    }
                    else
                    {
                        UIManager.DeleteButtonName(buttonNameCurrentIndex); //we remove the entry whith the current index
                        buttonNameCurrentIndex = UIManager.GetIndexForButtonName(UIManager.DEFAULT_BUTTON_NAME); //we set the current index to the default value
                        sp_buttonName.stringValue = UIManager.DEFAULT_BUTTON_NAME;
                        DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                    }
                    UpdateButtonNamesPopup();
                    tempButtonNameString = string.Empty;
                    deleteButtonName = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("no", GUILayout.Width(98), GUILayout.Height(16)))
                {
                    tempButtonNameString = string.Empty;
                    deleteButtonName = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                #endregion
            }
        }
        EditorGUILayout.EndHorizontal();
        if (sp_showHelp.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Button Name: the string that you can listen for to trigger anything", MessageType.None);
            EditorGUILayout.HelpBox("You can create a new button name, rename the current one or delete it from the database", MessageType.None);
            EditorGUILayout.HelpBox("If the current button should perform a 'Back' action, check 'Is Back Button'", MessageType.None);
            if (UIManager.isNavigationEnabled)
            {
                EditorGUILayout.HelpBox("In order for the back button to work (to know how to go back), you should check 'Add to Navigation History' if the current button performs an action that needs a return option available.", MessageType.None);
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        #region OnClick Sound
        if (newButtonSound == false && renameButtonSound == false && deleteButtonSound == false)
        {
            EditorGUILayout.LabelField("OnClick Sound", GUILayout.Width(200));
            if (UIManager.GetIndexForButtonSound(sp_onClickSound.stringValue) == -1)
            {
                DoozyUIRedundancyCheck.UIButtonRedundancyCheck(GetUIButton);
            }
            buttonSoundCurrentIndex = UIManager.GetIndexForButtonSound(sp_onClickSound.stringValue);
            buttonSoundCurrentIndex = EditorGUILayout.Popup(buttonSoundCurrentIndex, buttonSounds, GUILayout.Width(200));
            sp_onClickSound.stringValue = UIManager.GetDoozyUIData.buttonSounds[buttonSoundCurrentIndex].onClickSound;
        }
        else if (newButtonSound == true)
        {
            EditorGUILayout.LabelField("Create new button sound", GUILayout.Width(200));
            DoozyUIHelper.ResetColors();
            tempButtonSoundString = EditorGUILayout.TextField(tempButtonSoundString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        else if (renameButtonSound == true)
        {
            EditorGUILayout.LabelField("Rename button sound?", GUILayout.Width(200));
            DoozyUIHelper.ResetColors();
            tempButtonSoundString = EditorGUILayout.TextField(tempButtonSoundString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        else if (deleteButtonSound == true)
        {
            EditorGUILayout.LabelField("Do you want to delete?", GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            EditorGUILayout.LabelField(tempButtonSoundString, GUILayout.Width(200));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        EditorGUILayout.BeginHorizontal();
        if (newButtonSound == false && renameButtonSound == false && deleteButtonSound == false)
        {
            if (sp_onClickSound.stringValue.Equals(UIManager.DEFAULT_SOUND_NAME))
            {
                if (GUILayout.Button("new", GUILayout.Width(200), GUILayout.Height(16)))
                {
                    tempButtonSoundString = string.Empty;
                    newButtonSound = true;
                }
            }
            else
            {
                if (GUILayout.Button("new", GUILayout.Width(64), GUILayout.Height(16)))
                {
                    tempButtonSoundString = string.Empty;
                    newButtonSound = true;
                }
                if (GUILayout.Button("rename", GUILayout.Width(64), GUILayout.Height(16)))
                {
                    if (tempButtonSoundString.Equals(UIManager.DEFAULT_BUTTON_NAME))
                    {
                        Debug.Log("[DoozyUI] You cannot (and should not) rename the default button sound.");
                        return;
                    }
                    tempButtonSoundString = sp_onClickSound.stringValue;
                    renameButtonSound = true;
                }
                if (GUILayout.Button("delete", GUILayout.Width(64), GUILayout.Height(16)))
                {
                    if (tempButtonSoundString.Equals(UIManager.DEFAULT_BUTTON_NAME))
                    {
                        Debug.Log("[DoozyUI] You cannot (and should not) delete the default button sound.");
                        return;
                    }
                    tempButtonSoundString = sp_onClickSound.stringValue;
                    deleteButtonSound = true;
                }
            }
        }
        else if (newButtonSound == true)
        {
            #region New ButtonSound
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
            if (GUILayout.Button("confirm", GUILayout.Width(98), GUILayout.Height(16)))
            {
                UIManager.TrimStartAndEndSpaces(tempButtonSoundString);
                if (tempButtonSoundString.Equals(string.Empty) == false                          //we make sure the new name is not empty
                    && tempButtonSoundString.Equals(UIManager.DEFAULT_SOUND_NAME) == false      //we check that is not the default name
                    && UIManager.GetIndexForButtonSound(tempButtonSoundString) == -1)            //we make sure there are no duplicates
                {
                    UIManager.NewButtonSound(tempButtonSoundString);
                }
                sp_onClickSound.stringValue = UIManager.GetDoozyUIData.buttonSounds[UIManager.GetIndexForButtonSound(tempButtonSoundString)].onClickSound;
                UpdateButtonSoundsPopup();              //we update the string array that shows the list of button sounds in the inspector
                tempButtonSoundString = string.Empty;   //we clear the temporary name holder
                newButtonSound = false;                 //we show the initial menu for the element name
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            if (GUILayout.Button("cancel", GUILayout.Width(98), GUILayout.Height(16)))
            {
                newButtonSound = false;
                tempButtonSoundString = string.Empty;
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            #endregion
        }
        else if (renameButtonSound == true)
        {
            #region Rename ButtonSound
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
            if (GUILayout.Button("confirm", GUILayout.Width(98), GUILayout.Height(16)))
            {
                UIManager.TrimStartAndEndSpaces(tempButtonSoundString);
                if (tempButtonSoundString.Equals(string.Empty) == false                          //we make sure the new name is not empty
                    && tempButtonSoundString.Equals(UIManager.DEFAULT_SOUND_NAME) == false      //we check that is not the default name
                    && UIManager.GetIndexForButtonSound(tempButtonSoundString) == -1)            //we make sure there are no duplicates
                {
                    UIManager.RenameButtonSound(buttonSoundCurrentIndex, tempButtonSoundString);
                    UpdateButtonSoundsPopup();
                    sp_onClickSound.stringValue = tempButtonSoundString;
                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                }
                tempButtonSoundString = string.Empty;
                renameButtonSound = false;
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            if (GUILayout.Button("cancel", GUILayout.Width(98), GUILayout.Height(16)))
            {
                tempButtonSoundString = string.Empty;
                renameButtonSound = false;
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            #endregion
        }
        else if (deleteButtonSound == true)
        {
            #region Delete ButtonSound
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            if (GUILayout.Button("yes", GUILayout.Width(98), GUILayout.Height(16)))
            {
                if (sp_onClickSound.stringValue.Equals(UIManager.DEFAULT_SOUND_NAME))
                {
                    Debug.Log("[DoozyUI] You cannot (and should not) delete the default sound name '" + UIManager.DEFAULT_SOUND_NAME + "'.");
                }
                else
                {
                    UIManager.DeleteButtonSound(buttonSoundCurrentIndex); //we remove the entry from the current index
                    buttonSoundCurrentIndex = UIManager.GetIndexForButtonSound(UIManager.DEFAULT_SOUND_NAME); //we set the current index to the default value
                    sp_onClickSound.stringValue = UIManager.GetDoozyUIData.buttonSounds[buttonSoundCurrentIndex].onClickSound;
                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                }
                UpdateButtonSoundsPopup();
                tempButtonSoundString = string.Empty;
                deleteButtonSound = false;
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
            if (GUILayout.Button("no", GUILayout.Width(98), GUILayout.Height(16)))
            {
                tempButtonSoundString = string.Empty;
                deleteButtonSound = false;
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            #endregion
        }
        EditorGUILayout.EndHorizontal();

        if (sp_showHelp.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("This is the sound you trigger on button click.", MessageType.None);
            EditorGUILayout.HelpBox("You can create a new button sound, rename the current one or delete it from the database.", MessageType.None);
            EditorGUILayout.HelpBox("If MasterAudio is enabled it will trigger the selected method (PlaySoundAndForget or FireCustomEvent).", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        #endregion
        DoozyUIHelper.VerticalSpace(8);
        #region OnClick Animations
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(sp_useOnClickAnimations.boolValue ? DoozyUIResources.LabelOnClickAnimations : DoozyUIResources.LabelOnClickAnimationsDisabled, GUIStyle.none, GUILayout.Width(216), GUILayout.Height(30)))
            {
                //Toggle visibility of the OnClick Animations Zone
                sp_useOnClickAnimations.boolValue = !sp_useOnClickAnimations.boolValue;
            }
            #region OnClick Animations Presets
            if (sp_useOnClickAnimations.boolValue)
            {
                EditorGUILayout.BeginVertical();
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    if (saveOnClickAnimationPreset == false && deleteOnClickAnimationPreset == false)
                    {
                        sp_activeOnclickAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeOnclickAnimationsPresetIndex.intValue, onClickAnimationPresets, GUILayout.Width(192));
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("load", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                uiAnimationManager.LoadPreset(onClickAnimationPresets[sp_activeOnclickAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.OnClick);
                                OnInspectorGUI();
                            }
                            if (GUILayout.Button("save", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                saveOnClickAnimationPreset = !saveOnClickAnimationPreset;
                                newOnClickAnimationPresetName = string.Empty;
                            }
                            if (GUILayout.Button("delete", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                deleteOnClickAnimationPreset = true;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (saveOnClickAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            newOnClickAnimationPresetName = EditorGUILayout.TextField(newOnClickAnimationPresetName, GUILayout.Width(192));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("save", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.SavePreset(newOnClickAnimationPresetName, UIAnimationManager.AnimationType.OnClick);
                                UpdateAnimationPresetsFromFiles();
                                saveOnClickAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                saveOnClickAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (deleteOnClickAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Delete Preset '" + onClickAnimationPresets[GetUIButton.activeOnclickAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("yes", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.DeletePreset(onClickAnimationPresets[GetUIButton.activeOnclickAnimationsPresetIndex], UIAnimationManager.AnimationType.OnClick);
                                UpdateAnimationPresetsFromFiles();
                                deleteOnClickAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("no", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                deleteOnClickAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            else
            {
                #region ACTIVE OnClick Animations QuickView
                GUILayout.Space(6);
                if (GUILayout.Button(sp_punchPositionEnabled.boolValue ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (sp_punchPositionEnabled.boolValue == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useOnClickAnimations.boolValue = true;
                    }
                    sp_punchPositionEnabled.boolValue = !sp_punchPositionEnabled.boolValue;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(sp_punchRotationEnabled.boolValue ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (sp_punchRotationEnabled.boolValue == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useOnClickAnimations.boolValue = true;
                    }
                    sp_punchRotationEnabled.boolValue = !sp_punchRotationEnabled.boolValue;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(sp_punchScaleEnabled.boolValue ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (sp_punchScaleEnabled.boolValue == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useOnClickAnimations.boolValue = true;
                    }
                    sp_punchScaleEnabled.boolValue = !sp_punchScaleEnabled.boolValue;
                }
                #endregion
            }
        }
        EditorGUILayout.EndHorizontal();
        if (sp_showHelp.boolValue && sp_useOnClickAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for OnClick animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The OnClick animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/OnClick", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        DoozyUIHelper.ResetColors();
        #endregion
        if (sp_showHelp.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("The animations that happen on button click", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        if (sp_useOnClickAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);
            #region MOVE PUNCH
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_punchPositionEnabled.boolValue ? DoozyUIResources.LabelMovePunchEnabled : DoozyUIResources.LabelMovePunchDisabled, GUIStyle.none, GUILayout.Height(14)))
                {
                    sp_punchPositionEnabled.boolValue = !sp_punchPositionEnabled.boolValue;
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Punches the button's anchoredPosition towards the given direction and then back to the starting one as if it was connected to the starting position via an elastic.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
            }
            if (sp_punchPositionEnabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    sp_punchPositionEnabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_punchPositionEnabled.boolValue, GUILayout.Width(80));
                    EditorGUILayout.LabelField("direction", GUILayout.Width(54));
                    sp_punchPositionPunch.vector2Value = EditorGUILayout.Vector2Field(GUIContent.none, sp_punchPositionPunch.vector2Value, GUILayout.Width(110));
                    GUILayout.Space(10);
                    sp_punchPositionSnapping.boolValue = EditorGUILayout.ToggleLeft("snap", sp_punchPositionSnapping.boolValue, GUILayout.Width(46));
                    EditorGUILayout.LabelField("duration", GUILayout.Width(52));
                    sp_punchPositionDuration.floatValue = EditorGUILayout.FloatField(sp_punchPositionDuration.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.VerticalSpace(2);
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("direction: The direction and strength of the punch (added to the Transform's current position)", MessageType.None);
                    EditorGUILayout.HelpBox("snap: If TRUE the tween will smoothly snap all values to integers", MessageType.None);
                    EditorGUILayout.HelpBox("duration: The duration of the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("vibrato", GUILayout.Width(44));
                    sp_punchPositionVibrato.intValue = EditorGUILayout.IntField(sp_punchPositionVibrato.intValue, GUILayout.Width(40));
                    EditorGUILayout.LabelField("elasticity", GUILayout.Width(58));
                    sp_punchPositionElasticity.floatValue = EditorGUILayout.Slider(sp_punchPositionElasticity.floatValue, 0f, 1f, GUILayout.Width(258));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("vibrato: Indicates how much will the punch vibrate", MessageType.None);
                    EditorGUILayout.HelpBox("elasticity: Represents how much (0 to 1) the vector will go beyond the starting position when bouncing backwards. 1 creates a full oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start position", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("delay", GUILayout.Width(44));
                    sp_punchPositionDelay.floatValue = EditorGUILayout.FloatField(sp_punchPositionDelay.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("delay: Start delay for the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                }
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region ROTATE PUNCH
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_punchRotationEnabled.boolValue ? DoozyUIResources.LabelRotatePunchEnabled : DoozyUIResources.LabelRotatePunchDisabled, GUIStyle.none, GUILayout.Height(14)))
                {
                    sp_punchRotationEnabled.boolValue = !sp_punchRotationEnabled.boolValue;
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Punches a button's localRotation towards the given size and then back to the starting one as if it was connected to the starting rotation via an elastic.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
            }
            if (sp_punchRotationEnabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    sp_punchRotationEnabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_punchRotationEnabled.boolValue, GUILayout.Width(80));
                    EditorGUILayout.LabelField("rotation", GUILayout.Width(54));
                    sp_punchRotationPunch.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_punchRotationPunch.vector3Value, GUILayout.Width(170));
                    EditorGUILayout.LabelField("duration", GUILayout.Width(52));
                    sp_punchRotationDuration.floatValue = EditorGUILayout.FloatField(sp_punchRotationDuration.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("rotation: The punch strength (added to the Transform's current rotation)", MessageType.None);
                    EditorGUILayout.HelpBox("duration: The duration of the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("vibrato", GUILayout.Width(44));
                    sp_punchRotationVibrato.intValue = EditorGUILayout.IntField(sp_punchRotationVibrato.intValue, GUILayout.Width(40));
                    EditorGUILayout.LabelField("elasticity", GUILayout.Width(58));
                    sp_punchRotationElasticity.floatValue = EditorGUILayout.Slider(sp_punchRotationElasticity.floatValue, 0f, 1f, GUILayout.Width(258));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("vibrato: Indicates how much will the punch vibrate", MessageType.None);
                    EditorGUILayout.HelpBox("elasticity: Represents how much (0 to 1) the vector will go beyond the starting rotation when bouncing backwards. 1 creates a full oscillation between the punch rotation and the opposite rotation, while 0 oscillates only between the punch and the start rotation", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("delay", GUILayout.Width(44));
                    sp_punchRotationDelay.floatValue = EditorGUILayout.FloatField(sp_punchRotationDelay.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("delay: Start delay for the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                }
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region SCALE PUNCH
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_punchScaleEnabled.boolValue ? DoozyUIResources.LabelScalePunchEnabled : DoozyUIResources.LabelScalePunchDisabled, GUIStyle.none, GUILayout.Height(14)))
                {
                    sp_punchScaleEnabled.boolValue = !sp_punchScaleEnabled.boolValue;
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Punches a button's localScale towards the given size and then back to the starting one as if it was connected to the starting scale via an elastic.", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
            }
            if (sp_punchScaleEnabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    sp_punchScaleEnabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_punchScaleEnabled.boolValue, GUILayout.Width(80));
                    EditorGUILayout.LabelField("scale", GUILayout.Width(54));
                    sp_punchScalePunch.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_punchScalePunch.vector3Value, GUILayout.Width(170));
                    EditorGUILayout.LabelField("duration", GUILayout.Width(52));
                    sp_punchScaleDuration.floatValue = EditorGUILayout.FloatField(sp_punchScaleDuration.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("scale: The punch strength (added to the Transform's current scale)", MessageType.None);
                    EditorGUILayout.HelpBox("duration: The duration of the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("vibrato", GUILayout.Width(44));
                    sp_punchScaleVibrato.intValue = EditorGUILayout.IntField(sp_punchScaleVibrato.intValue, GUILayout.Width(40));
                    EditorGUILayout.LabelField("elasticity", GUILayout.Width(58));
                    sp_punchScaleElasticity.floatValue = EditorGUILayout.Slider(sp_punchScaleElasticity.floatValue, 0f, 1f, GUILayout.Width(258));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("vibrato: Indicates how much will the punch vibrate", MessageType.None);
                    EditorGUILayout.HelpBox("elasticity: Represents how much (0 to 1) the vector will go beyond the starting size when bouncing backwards. 1 creates a full oscillation between the punch scale and the opposite scale, while 0 oscillates only between the punch scale and the start scale", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                }
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("delay", GUILayout.Width(44));
                    sp_punchScaleDelay.floatValue = EditorGUILayout.FloatField(sp_punchScaleDelay.floatValue, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                if (sp_showHelp.boolValue)
                {
                    DoozyUIHelper.ResetColors();
                    EditorGUILayout.HelpBox("delay: Start delay for the tween", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                }
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
        }
        #endregion
        DoozyUIHelper.VerticalSpace(sp_useNormalStateAnimations.boolValue ? 8 : 2);
        #region Normal State Animations
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(sp_useNormalStateAnimations.boolValue ? DoozyUIResources.LabelNormalAnimations : DoozyUIResources.LabelNormalAnimationsDisabled, GUIStyle.none, GUILayout.Width(216), GUILayout.Height(30)))
            {
                sp_useNormalStateAnimations.boolValue = !sp_useNormalStateAnimations.boolValue;
            }
            #region Normal State Animations - PRESETS
            if (sp_useNormalStateAnimations.boolValue)
            {
                EditorGUILayout.BeginVertical();
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    if (saveNormalAnimationPreset == false && deleteNormalAnimationPreset == false)
                    {
                        sp_activeNormalAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeNormalAnimationsPresetIndex.intValue, buttonLoopsAnimationPresets, GUILayout.Width(192));
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("load", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                uiAnimationManager.LoadPreset(buttonLoopsAnimationPresets[sp_activeNormalAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Normal);
                                OnInspectorGUI();
                            }
                            if (GUILayout.Button("save", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                saveNormalAnimationPreset = !saveNormalAnimationPreset;
                                newNormalAnimationPresetName = string.Empty;
                            }
                            if (GUILayout.Button("delete", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                deleteNormalAnimationPreset = true;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (saveNormalAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            newNormalAnimationPresetName = EditorGUILayout.TextField(newNormalAnimationPresetName, GUILayout.Width(192));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("save", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.SavePreset(newNormalAnimationPresetName, UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Normal);
                                UpdateAnimationPresetsFromFiles();
                                saveNormalAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                saveNormalAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (deleteNormalAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Delete Preset '" + buttonLoopsAnimationPresets[GetUIButton.activeNormalAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("yes", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.DeletePreset(buttonLoopsAnimationPresets[GetUIButton.activeNormalAnimationsPresetIndex], UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Normal);
                                UpdateAnimationPresetsFromFiles();
                                deleteNormalAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("no", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                deleteNormalAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            else
            {
                #region ACTIVE Normal Animations QuickView
                GUILayout.Space(6);
                if (GUILayout.Button(GetUIButton.normalAnimationSettings.moveLoop.enabled ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.normalAnimationSettings.moveLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useNormalStateAnimations.boolValue = true;
                    }
                    GetUIButton.normalAnimationSettings.moveLoop.enabled = !GetUIButton.normalAnimationSettings.moveLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.normalAnimationSettings.rotationLoop.enabled ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.normalAnimationSettings.rotationLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useNormalStateAnimations.boolValue = true;
                    }
                    GetUIButton.normalAnimationSettings.rotationLoop.enabled = !GetUIButton.normalAnimationSettings.rotationLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.normalAnimationSettings.scaleLoop.enabled ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.normalAnimationSettings.scaleLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useNormalStateAnimations.boolValue = true;
                    }
                    GetUIButton.normalAnimationSettings.scaleLoop.enabled = !GetUIButton.normalAnimationSettings.scaleLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.normalAnimationSettings.fadeLoop.enabled ? DoozyUIResources.IconFadeEnabled : DoozyUIResources.IconFadeDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.normalAnimationSettings.fadeLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useNormalStateAnimations.boolValue = true;
                    }
                    GetUIButton.normalAnimationSettings.fadeLoop.enabled = !GetUIButton.normalAnimationSettings.fadeLoop.enabled;
                }
                #endregion
            }
        }
        EditorGUILayout.EndHorizontal();
        if (sp_showHelp.boolValue && sp_useNormalStateAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for Normal state animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The Normal state animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/ButtonLoops", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        DoozyUIHelper.ResetColors();
        #endregion
        if (sp_useNormalStateAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);
            #region MoveLoop
            if (GUILayout.Button(GetUIButton.normalAnimationSettings.moveLoop.enabled ? DoozyUIResources.LabelMoveLoopEnabled : DoozyUIResources.LabelMoveLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.normalAnimationSettings.moveLoop.enabled = !GetUIButton.normalAnimationSettings.moveLoop.enabled;
            }
            if (GetUIButton.normalAnimationSettings.moveLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.normalAnimationSettings.moveLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.normalAnimationSettings.moveLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.normalAnimationSettings.moveLoop.time = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.moveLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.normalAnimationSettings.moveLoop.delay = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.moveLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.moveLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.moveLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("movement", GUILayout.Width(70));
                    GetUIButton.normalAnimationSettings.moveLoop.movement = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.normalAnimationSettings.moveLoop.movement, GUILayout.Width(254));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.normalAnimationSettings.moveLoop.loops = EditorGUILayout.IntField(GetUIButton.normalAnimationSettings.moveLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.normalAnimationSettings.moveLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.moveLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.normalAnimationSettings.moveLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region RotationLoop
            if (GUILayout.Button(GetUIButton.normalAnimationSettings.rotationLoop.enabled ? DoozyUIResources.LabelRotateLoopEnabled : DoozyUIResources.LabelRotateLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.normalAnimationSettings.rotationLoop.enabled = !GetUIButton.normalAnimationSettings.rotationLoop.enabled;
            }
            if (GetUIButton.normalAnimationSettings.rotationLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.normalAnimationSettings.rotationLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.normalAnimationSettings.rotationLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.normalAnimationSettings.rotationLoop.time = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.rotationLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.normalAnimationSettings.rotationLoop.delay = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.rotationLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.rotationLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.rotationLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("rotation", GUILayout.Width(50));
                    GetUIButton.normalAnimationSettings.rotationLoop.rotation = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.normalAnimationSettings.rotationLoop.rotation, GUILayout.Width(150));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.normalAnimationSettings.rotationLoop.loops = EditorGUILayout.IntField(GetUIButton.normalAnimationSettings.rotationLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.normalAnimationSettings.rotationLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.rotationLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.normalAnimationSettings.rotationLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region ScaleLoop
            if (GUILayout.Button(GetUIButton.normalAnimationSettings.scaleLoop.enabled ? DoozyUIResources.LabelScaleLoopEnabled : DoozyUIResources.LabelScaleLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.normalAnimationSettings.scaleLoop.enabled = !GetUIButton.normalAnimationSettings.scaleLoop.enabled;
            }
            if (GetUIButton.normalAnimationSettings.scaleLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.normalAnimationSettings.scaleLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.normalAnimationSettings.scaleLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.normalAnimationSettings.scaleLoop.time = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.scaleLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.normalAnimationSettings.scaleLoop.delay = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.scaleLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.scaleLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.scaleLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("min", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.scaleLoop.min = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.normalAnimationSettings.scaleLoop.min, GUILayout.Width(150));
#if dUI_TextMeshPro
                    GetUIButton.normalAnimationSettings.scaleLoop.min = new Vector3(GetUIButton.normalAnimationSettings.scaleLoop.min.x, GetUIButton.normalAnimationSettings.scaleLoop.min.y, 1);
#endif
                    EditorGUILayout.LabelField("max", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.scaleLoop.max = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.normalAnimationSettings.scaleLoop.max, GUILayout.Width(150));
#if dUI_TextMeshPro
                    GetUIButton.normalAnimationSettings.scaleLoop.max = new Vector3(GetUIButton.normalAnimationSettings.scaleLoop.max.x, GetUIButton.normalAnimationSettings.scaleLoop.max.y, 1);
#endif
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.normalAnimationSettings.scaleLoop.loops = EditorGUILayout.IntField(GetUIButton.normalAnimationSettings.scaleLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.normalAnimationSettings.scaleLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.scaleLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.normalAnimationSettings.scaleLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region FadeLoop
            if (GUILayout.Button(GetUIButton.normalAnimationSettings.fadeLoop.enabled ? DoozyUIResources.LabelFadeLoopEnabled : DoozyUIResources.LabelFadeLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.normalAnimationSettings.fadeLoop.enabled = !GetUIButton.normalAnimationSettings.fadeLoop.enabled;
            }
            if (GetUIButton.normalAnimationSettings.fadeLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Fade);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.normalAnimationSettings.fadeLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.normalAnimationSettings.fadeLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.normalAnimationSettings.fadeLoop.time = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.fadeLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.normalAnimationSettings.fadeLoop.delay = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.fadeLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.fadeLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.fadeLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("min", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.fadeLoop.min = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.fadeLoop.min, GUILayout.Width(40));
                    EditorGUILayout.LabelField("max", GUILayout.Width(30));
                    GetUIButton.normalAnimationSettings.fadeLoop.max = EditorGUILayout.FloatField(GetUIButton.normalAnimationSettings.fadeLoop.max, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.normalAnimationSettings.fadeLoop.loops = EditorGUILayout.IntField(GetUIButton.normalAnimationSettings.fadeLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.normalAnimationSettings.fadeLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.normalAnimationSettings.fadeLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.normalAnimationSettings.fadeLoop.loopType);
                DoozyUIHelper.ResetColors();
            }
            DoozyUIHelper.VerticalSpace(8);
            #endregion
        }
        #endregion
        DoozyUIHelper.VerticalSpace(sp_useHighlightedStateAnimations.boolValue ? 8 : 2);
        #region Highlighted State Animations
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(sp_useHighlightedStateAnimations.boolValue ? DoozyUIResources.LabelHighlightedAnimations : DoozyUIResources.LabelHighlightedAnimationsDisabled, GUIStyle.none, GUILayout.Width(216), GUILayout.Height(30)))
            {
                sp_useHighlightedStateAnimations.boolValue = !sp_useHighlightedStateAnimations.boolValue;
            }
            #region Highlighted State Animations - PRESETS
            if (sp_useHighlightedStateAnimations.boolValue)
            {
                EditorGUILayout.BeginVertical();
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    if (saveHighlightedAnimationPreset == false && deleteHighlightedAnimationPreset == false)
                    {
                        sp_activeHighlightedAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeHighlightedAnimationsPresetIndex.intValue, buttonLoopsAnimationPresets, GUILayout.Width(192));

                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("load", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                uiAnimationManager.LoadPreset(buttonLoopsAnimationPresets[sp_activeHighlightedAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Highlighted);
                                OnInspectorGUI();
                            }
                            if (GUILayout.Button("save", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                saveHighlightedAnimationPreset = !saveHighlightedAnimationPreset;
                                newHighlightedAnimationPresetName = string.Empty;
                            }
                            if (GUILayout.Button("delete", GUILayout.Width(61), GUILayout.Height(16)))
                            {
                                deleteHighlightedAnimationPreset = true;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (saveHighlightedAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            newHighlightedAnimationPresetName = EditorGUILayout.TextField(newHighlightedAnimationPresetName, GUILayout.Width(192));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("save", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.SavePreset(newHighlightedAnimationPresetName, UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Highlighted);
                                UpdateAnimationPresetsFromFiles();
                                saveHighlightedAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                saveHighlightedAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (deleteHighlightedAnimationPreset == true)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Delete Preset '" + buttonLoopsAnimationPresets[GetUIButton.activeHighlightedAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("yes", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                uiAnimationManager.DeletePreset(buttonLoopsAnimationPresets[GetUIButton.activeHighlightedAnimationsPresetIndex], UIAnimationManager.AnimationType.ButtonLoops, UIAnimationManager.ButtonLoopType.Highlighted);
                                UpdateAnimationPresetsFromFiles();
                                deleteHighlightedAnimationPreset = false;
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("no", GUILayout.Width(94), GUILayout.Height(16)))
                            {
                                deleteHighlightedAnimationPreset = false;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            else
            {
                #region ACTIVE Highlighted Animations QuickView
                GUILayout.Space(6);
                if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.moveLoop.enabled ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.highlightedAnimationSettings.moveLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useHighlightedStateAnimations.boolValue = true;
                    }
                    GetUIButton.highlightedAnimationSettings.moveLoop.enabled = !GetUIButton.highlightedAnimationSettings.moveLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.rotationLoop.enabled ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.highlightedAnimationSettings.rotationLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useHighlightedStateAnimations.boolValue = true;
                    }
                    GetUIButton.highlightedAnimationSettings.rotationLoop.enabled = !GetUIButton.highlightedAnimationSettings.rotationLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.scaleLoop.enabled ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.highlightedAnimationSettings.scaleLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useHighlightedStateAnimations.boolValue = true;
                    }
                    GetUIButton.highlightedAnimationSettings.scaleLoop.enabled = !GetUIButton.highlightedAnimationSettings.scaleLoop.enabled;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.fadeLoop.enabled ? DoozyUIResources.IconFadeEnabled : DoozyUIResources.IconFadeDisabled, GUIStyle.none, GUILayout.Width(28), GUILayout.Height(24)))
                {
                    if (GetUIButton.highlightedAnimationSettings.fadeLoop.enabled == false) //if the user enables an animation type, then we open the animations tabs
                    {
                        sp_useHighlightedStateAnimations.boolValue = true;
                    }
                    GetUIButton.highlightedAnimationSettings.fadeLoop.enabled = !GetUIButton.highlightedAnimationSettings.fadeLoop.enabled;
                }
                #endregion
            }
        }
        EditorGUILayout.EndHorizontal();

        if (sp_showHelp.boolValue && sp_useHighlightedStateAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for Highlighted state animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The Highlighted state animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/ButtonLoops", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        DoozyUIHelper.ResetColors();
        #endregion
        if (sp_useHighlightedStateAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);
            #region MoveLoop
            if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.moveLoop.enabled ? DoozyUIResources.LabelMoveLoopEnabled : DoozyUIResources.LabelMoveLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.highlightedAnimationSettings.moveLoop.enabled = !GetUIButton.highlightedAnimationSettings.moveLoop.enabled;
            }
            if (GetUIButton.highlightedAnimationSettings.moveLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.highlightedAnimationSettings.moveLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.highlightedAnimationSettings.moveLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.highlightedAnimationSettings.moveLoop.time = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.moveLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.highlightedAnimationSettings.moveLoop.delay = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.moveLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.moveLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.moveLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("movement", GUILayout.Width(70));
                    GetUIButton.highlightedAnimationSettings.moveLoop.movement = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.highlightedAnimationSettings.moveLoop.movement, GUILayout.Width(254));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.highlightedAnimationSettings.moveLoop.loops = EditorGUILayout.IntField(GetUIButton.highlightedAnimationSettings.moveLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.highlightedAnimationSettings.moveLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.moveLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.highlightedAnimationSettings.moveLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region RotationLoop
            if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.rotationLoop.enabled ? DoozyUIResources.LabelRotateLoopEnabled : DoozyUIResources.LabelRotateLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.highlightedAnimationSettings.rotationLoop.enabled = !GetUIButton.highlightedAnimationSettings.rotationLoop.enabled;
            }
            if (GetUIButton.highlightedAnimationSettings.rotationLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.highlightedAnimationSettings.rotationLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.highlightedAnimationSettings.rotationLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.time = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.rotationLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.delay = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.rotationLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.rotationLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("rotation", GUILayout.Width(50));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.rotation = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.highlightedAnimationSettings.rotationLoop.rotation, GUILayout.Width(150));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.loops = EditorGUILayout.IntField(GetUIButton.highlightedAnimationSettings.rotationLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.highlightedAnimationSettings.rotationLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.rotationLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.highlightedAnimationSettings.rotationLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region ScaleLoop
            if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.scaleLoop.enabled ? DoozyUIResources.LabelScaleLoopEnabled : DoozyUIResources.LabelScaleLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.highlightedAnimationSettings.scaleLoop.enabled = !GetUIButton.highlightedAnimationSettings.scaleLoop.enabled;
            }
            if (GetUIButton.highlightedAnimationSettings.scaleLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.highlightedAnimationSettings.scaleLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.highlightedAnimationSettings.scaleLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.time = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.scaleLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.delay = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.scaleLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.scaleLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("min", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.min = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.highlightedAnimationSettings.scaleLoop.min, GUILayout.Width(150));
#if dUI_TextMeshPro
                    GetUIButton.normalAnimationSettings.scaleLoop.min = new Vector3(GetUIButton.normalAnimationSettings.scaleLoop.min.x, GetUIButton.normalAnimationSettings.scaleLoop.min.y, 1);
#endif
                    EditorGUILayout.LabelField("max", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.max = EditorGUILayout.Vector3Field(GUIContent.none, GetUIButton.highlightedAnimationSettings.scaleLoop.max, GUILayout.Width(150));
#if dUI_TextMeshPro
                    GetUIButton.normalAnimationSettings.scaleLoop.max = new Vector3(GetUIButton.normalAnimationSettings.scaleLoop.max.x, GetUIButton.normalAnimationSettings.scaleLoop.max.y, 1);
#endif
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.loops = EditorGUILayout.IntField(GetUIButton.highlightedAnimationSettings.scaleLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.highlightedAnimationSettings.scaleLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.scaleLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.highlightedAnimationSettings.scaleLoop.loopType);
                DoozyUIHelper.ResetColors();
                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
            DoozyUIHelper.VerticalSpace(2);
            #region FadeLoop
            if (GUILayout.Button(GetUIButton.highlightedAnimationSettings.fadeLoop.enabled ? DoozyUIResources.LabelFadeLoopEnabled : DoozyUIResources.LabelFadeLoopDisabled, GUIStyle.none, GUILayout.Height(14)))
            {
                GetUIButton.highlightedAnimationSettings.fadeLoop.enabled = !GetUIButton.highlightedAnimationSettings.fadeLoop.enabled;
            }
            if (GetUIButton.highlightedAnimationSettings.fadeLoop.enabled)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Fade);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    GetUIButton.highlightedAnimationSettings.fadeLoop.enabled = EditorGUILayout.ToggleLeft("enabled", GetUIButton.highlightedAnimationSettings.fadeLoop.enabled, GUILayout.Width(80));
                    EditorGUILayout.LabelField("time", GUILayout.Width(28));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.time = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.fadeLoop.time, GUILayout.Width(40));
                    EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.delay = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.fadeLoop.delay, GUILayout.Width(40));
                    EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.fadeLoop.easeType, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("min", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.min = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.fadeLoop.min, GUILayout.Width(40));
                    EditorGUILayout.LabelField("max", GUILayout.Width(30));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.max = EditorGUILayout.FloatField(GetUIButton.highlightedAnimationSettings.fadeLoop.max, GUILayout.Width(40));
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.loops = EditorGUILayout.IntField(GetUIButton.highlightedAnimationSettings.fadeLoop.loops, GUILayout.Width(93));
                    EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                    GetUIButton.highlightedAnimationSettings.fadeLoop.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(GetUIButton.highlightedAnimationSettings.fadeLoop.loopType, GUILayout.Width(138));
                    EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                DrawLoopTypeHelpBox(GetUIButton.highlightedAnimationSettings.fadeLoop.loopType);
                DoozyUIHelper.ResetColors();
            }
            DoozyUIHelper.VerticalSpace(8);
            #endregion
        }
        #endregion
        DoozyUIHelper.VerticalSpace(8);
        if (UIManager.isNavigationEnabled && sp_backButton.boolValue == false)
        {
            #region Show Elements
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
            DoozyUIHelper.VerticalSpace(8);
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarShowElements);
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Select the elements that you want this button to show", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            if (GUILayout.Button("add element", GUILayout.Width(224)))
            {
                if (GetUIButton.showElements == null)
                {
                    GetUIButton.showElements = new List<string>();
                    showElementsIndex = new List<int>();
                }
                sp_showElements.InsertArrayElementAtIndex(sp_showElements.arraySize);
                sp_showElements.GetArrayElementAtIndex(sp_showElements.arraySize - 1).stringValue = UIManager.DEFAULT_ELEMENT_NAME;
                showElementsIndex.Add(UIManager.GetIndexForElementName(sp_showElements.GetArrayElementAtIndex(sp_showElements.arraySize - 1).stringValue));
            }
            if (GetUIButton.showElements != null)  //we check if the showElements list has any items in it
            {
                for (int i = 0; i < GetUIButton.showElements.Count; i++)    //we show the list of elements
                {
                    DoozyUIHelper.VerticalSpace(2);
                    EditorGUILayout.BeginHorizontal();
                    {
                        showElementsIndex[i] = EditorGUILayout.Popup(showElementsIndex[i], elementNames, GUILayout.Width(200));
                        sp_showElements.GetArrayElementAtIndex(i).stringValue = elementNames[showElementsIndex[i]];
                        if (GUILayout.Button("x", GUILayout.Height(12))) //we add a delete button for each list entry
                        {
                            sp_showElements.DeleteArrayElementAtIndex(i);
                            showElementsIndex.RemoveAt(i);
                            serializedObject.ApplyModifiedProperties();
                            EditorGUIUtility.ExitGUI();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            DoozyUIHelper.ResetColors();
            #endregion
            #region Hide Elements
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            DoozyUIHelper.VerticalSpace(8);
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarHideElements);
            if (sp_showHelp.boolValue)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Select the elements that you want this button to hide", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            if (GUILayout.Button("add element", GUILayout.Width(224)))
            {
                if (GetUIButton.hideElements == null)
                {
                    GetUIButton.hideElements = new List<string>();
                    hideElementsIndex = new List<int>();
                }
                sp_hideElements.InsertArrayElementAtIndex(sp_hideElements.arraySize);
                sp_hideElements.GetArrayElementAtIndex(sp_hideElements.arraySize - 1).stringValue = UIManager.DEFAULT_ELEMENT_NAME;
                hideElementsIndex.Add(UIManager.GetIndexForElementName(sp_hideElements.GetArrayElementAtIndex(sp_hideElements.arraySize - 1).stringValue));
            }
            if (GetUIButton.hideElements != null)  //we check if the hideElements list has any items in it
            {
                for (int i = 0; i < GetUIButton.hideElements.Count; i++)    //we show the list of elements
                {
                    DoozyUIHelper.VerticalSpace(2);
                    EditorGUILayout.BeginHorizontal();
                    {
                        hideElementsIndex[i] = EditorGUILayout.Popup(hideElementsIndex[i], elementNames, GUILayout.Width(200));
                        sp_hideElements.GetArrayElementAtIndex(i).stringValue = elementNames[hideElementsIndex[i]];
                        if (GUILayout.Button("x", GUILayout.Height(12)))
                        {
                            sp_hideElements.DeleteArrayElementAtIndex(i);
                            hideElementsIndex.RemoveAt(i);
                            serializedObject.ApplyModifiedProperties();
                            EditorGUIUtility.ExitGUI();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            DoozyUIHelper.ResetColors();
            #endregion
        }
        else if (UIManager.isNavigationEnabled == false)
        {
            DoozyUIHelper.VerticalSpace(8);
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarNavigationDisabled);
        }
        #region Send GameEvents
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarSendGameEvents);
        if (sp_showHelp.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Type in the game events that you want this button to send", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        if (GUILayout.Button("add game event", GUILayout.Width(224)))
        {
            if (GetUIButton.gameEvents == null)
            {
                GetUIButton.gameEvents = new List<string>();
            }
            sp_gameEvents.InsertArrayElementAtIndex(sp_gameEvents.arraySize);
            sp_gameEvents.GetArrayElementAtIndex(sp_gameEvents.arraySize - 1).stringValue = string.Empty;
        }
        if (GetUIButton.gameEvents != null)  //we check if the gameEvents list has any items in it
        {
            for (int i = 0; i < GetUIButton.gameEvents.Count; i++)    //we show the list of elements
            {
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                {
                    sp_gameEvents.GetArrayElementAtIndex(i).stringValue = EditorGUILayout.TextField(sp_gameEvents.GetArrayElementAtIndex(i).stringValue, GUILayout.Width(200));
                    if (GUILayout.Button("x", GUILayout.Height(12)))
                    {
                        sp_gameEvents.DeleteArrayElementAtIndex(i);
                        serializedObject.ApplyModifiedProperties();
                        EditorGUIUtility.ExitGUI();
                    }
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        DoozyUIHelper.ResetColors();
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    void DrawLoopTypeHelpBox(DG.Tweening.LoopType loopType)
    {
        switch (loopType)
        {
            case DG.Tweening.LoopType.Yoyo: EditorGUILayout.HelpBox("LoopType.Yoyo - the tween moves forward and backwards at alternate cycles", MessageType.None); break;
            case DG.Tweening.LoopType.Restart: EditorGUILayout.HelpBox("LoopType.Restart - each loop cycle restarts from the beginning", MessageType.None); break;
            case DG.Tweening.LoopType.Incremental: EditorGUILayout.HelpBox("LoopType.Incremental - continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving 'onward'", MessageType.None); break;
        }
    }
}
