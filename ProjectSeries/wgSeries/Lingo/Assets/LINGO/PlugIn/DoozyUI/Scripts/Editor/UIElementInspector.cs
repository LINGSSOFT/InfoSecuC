// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DoozyUI;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(UIElement), true)]
public class UIElementInspector : Editor
{
    #region SerializedProperties
    SerializedProperty sp_elementName;

#if dUI_UseOrientationManager
    SerializedProperty sp_LANDSCAPE;
    SerializedProperty sp_PORTRAIT;
#endif

    SerializedProperty sp_startHidden;
    SerializedProperty sp_animateAtStart;
    SerializedProperty sp_disableWhenHidden;

    SerializedProperty sp_selectedButton;
    SerializedProperty sp_useCustomStartAnchoredPosition;
    SerializedProperty sp_customStartAnchoredPosition;

    SerializedProperty sp_useInAnimations;
    SerializedProperty sp_useLoopAnimations;
    SerializedProperty sp_useOutAnimations;

    SerializedProperty sp_activeInAnimationsPresetIndex;
    SerializedProperty sp_activeLoopAnimationsPresetIndex;
    SerializedProperty sp_activeOutAnimationsPresetIndex;

    //EVENTS
    SerializedProperty sp_useInAnimationsStartEvents;
    SerializedProperty sp_useInAnimationsFinishEvents;
    SerializedProperty sp_useOutAnimationsStartEvents;
    SerializedProperty sp_useOutAnimationsFinishEvents;
    SerializedProperty sp_onInAnimationsStart;
    SerializedProperty sp_onInAnimationsFinish;
    SerializedProperty sp_onOutAnimationsStart;
    SerializedProperty sp_onOutAnimationsFinish;

    //MoveIn
    SerializedProperty sp_moveIn_enabled;
    SerializedProperty sp_moveIn_moveFrom;
    SerializedProperty sp_moveIn_positionAdjustment;
    SerializedProperty sp_moveIn_positionFrom;
    SerializedProperty sp_moveIn_easeType;
    SerializedProperty sp_moveIn_time;
    SerializedProperty sp_moveIn_delay;

    //RotationIn
    SerializedProperty sp_rotationIn_enabled;
    SerializedProperty sp_rotationIn_rotateFrom;
    SerializedProperty sp_rotationIn_easeType;
    SerializedProperty sp_rotationIn_time;
    SerializedProperty sp_rotationIn_delay;

    //ScaleIn
    SerializedProperty sp_scaleIn_enabled;
    SerializedProperty sp_scaleIn_scaleBegin;
    SerializedProperty sp_scaleIn_easeType;
    SerializedProperty sp_scaleIn_time;
    SerializedProperty sp_scaleIn_delay;

    //FadeIn
    SerializedProperty sp_fadeIn_enabled;
    SerializedProperty sp_fadeIn_easeType;
    SerializedProperty sp_fadeIn_time;
    SerializedProperty sp_fadeIn_delay;

    //MoveLoop
    SerializedProperty sp_moveLoop_enabled;
    SerializedProperty sp_moveLoop_autoStart;
    SerializedProperty sp_moveLoop_movement;
    SerializedProperty sp_moveLoop_easeType;
    SerializedProperty sp_moveLoop_loops;
    SerializedProperty sp_moveLoop_loopType;
    SerializedProperty sp_moveLoop_time;
    SerializedProperty sp_moveLoop_delay;

    //RotationLoop
    SerializedProperty sp_rotationLoop_enabled;
    SerializedProperty sp_rotationLoop_autoStart;
    SerializedProperty sp_rotationLoop_rotation;
    SerializedProperty sp_rotationLoop_easeType;
    SerializedProperty sp_rotationLoop_loops;
    SerializedProperty sp_rotationLoop_loopType;
    SerializedProperty sp_rotationLoop_time;
    SerializedProperty sp_rotationLoop_delay;

    //ScaleLoop
    SerializedProperty sp_scaleLoop_enabled;
    SerializedProperty sp_scaleLoop_autoStart;
    SerializedProperty sp_scaleLoop_min;
    SerializedProperty sp_scaleLoop_max;
    SerializedProperty sp_scaleLoop_easeType;
    SerializedProperty sp_scaleLoop_loops;
    SerializedProperty sp_scaleLoop_loopType;
    SerializedProperty sp_scaleLoop_time;
    SerializedProperty sp_scaleLoop_delay;

    //FadeLoop
    SerializedProperty sp_fadeLoop_enabled;
    SerializedProperty sp_fadeLoop_autoStart;
    SerializedProperty sp_fadeLoop_min;
    SerializedProperty sp_fadeLoop_max;
    SerializedProperty sp_fadeLoop_easeType;
    SerializedProperty sp_fadeLoop_loops;
    SerializedProperty sp_fadeLoop_loopType;
    SerializedProperty sp_fadeLoop_time;
    SerializedProperty sp_fadeLoop_delay;

    //MoveOut
    SerializedProperty sp_moveOut_enabled;
    SerializedProperty sp_moveOut_moveTo;
    SerializedProperty sp_moveOut_positionAdjustment;
    SerializedProperty sp_moveOut_positionTo;
    SerializedProperty sp_moveOut_easeType;
    SerializedProperty sp_moveOut_time;
    SerializedProperty sp_moveOut_delay;

    //RotationOut
    SerializedProperty sp_rotationOut_enabled;
    SerializedProperty sp_rotationOut_rotateTo;
    SerializedProperty sp_rotationOut_easeType;
    SerializedProperty sp_rotationOut_time;
    SerializedProperty sp_rotationOut_delay;

    //ScaleOut
    SerializedProperty sp_scaleOut_enabled;
    SerializedProperty sp_scaleOut_scaleEnd;
    SerializedProperty sp_scaleOut_easeType;
    SerializedProperty sp_scaleOut_time;
    SerializedProperty sp_scaleOut_delay;

    //FadeOut
    SerializedProperty sp_fadeOut_enabled;
    SerializedProperty sp_fadeOut_easeType;
    SerializedProperty sp_fadeOut_time;
    SerializedProperty sp_fadeOut_delay;
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_elementName = serializedObject.FindProperty("elementName");

#if dUI_UseOrientationManager
        sp_LANDSCAPE = serializedObject.FindProperty("LANDSCAPE");
        sp_PORTRAIT = serializedObject.FindProperty("PORTRAIT");
#endif

        sp_startHidden = serializedObject.FindProperty("startHidden");
        sp_animateAtStart = serializedObject.FindProperty("animateAtStart");
        sp_disableWhenHidden = serializedObject.FindProperty("disableWhenHidden");

        sp_selectedButton = serializedObject.FindProperty("selectedButton");
        sp_useCustomStartAnchoredPosition = serializedObject.FindProperty("useCustomStartAnchoredPosition");
        sp_customStartAnchoredPosition = serializedObject.FindProperty("customStartAnchoredPosition");

        sp_useInAnimations = serializedObject.FindProperty("useInAnimations");
        sp_useLoopAnimations = serializedObject.FindProperty("useLoopAnimations");
        sp_useOutAnimations = serializedObject.FindProperty("useOutAnimations");

        sp_activeInAnimationsPresetIndex = serializedObject.FindProperty("activeInAnimationsPresetIndex");
        sp_activeLoopAnimationsPresetIndex = serializedObject.FindProperty("activeLoopAnimationsPresetIndex");
        sp_activeOutAnimationsPresetIndex = serializedObject.FindProperty("activeOutAnimationsPresetIndex");

        //EVENTS
        sp_useInAnimationsStartEvents = serializedObject.FindProperty("useInAnimationsStartEvents");
        sp_useInAnimationsFinishEvents = serializedObject.FindProperty("useInAnimationsFinishEvents");
        sp_useOutAnimationsStartEvents = serializedObject.FindProperty("useOutAnimationsStartEvents");
        sp_useOutAnimationsFinishEvents = serializedObject.FindProperty("useOutAnimationsFinishEvents");
        sp_onInAnimationsStart = serializedObject.FindProperty("onInAnimationsStart");
        sp_onInAnimationsFinish = serializedObject.FindProperty("onInAnimationsFinish");
        sp_onOutAnimationsStart = serializedObject.FindProperty("onOutAnimationsStart");
        sp_onOutAnimationsFinish = serializedObject.FindProperty("onOutAnimationsFinish");

        //MoveIn
        sp_moveIn_enabled = serializedObject.FindProperty("moveIn").FindPropertyRelative("enabled");
        sp_moveIn_moveFrom = serializedObject.FindProperty("moveIn").FindPropertyRelative("moveFrom");
        sp_moveIn_positionAdjustment = serializedObject.FindProperty("moveIn").FindPropertyRelative("positionAdjustment");
        sp_moveIn_positionFrom = serializedObject.FindProperty("moveIn").FindPropertyRelative("positionFrom");
        sp_moveIn_easeType = serializedObject.FindProperty("moveIn").FindPropertyRelative("easeType");
        sp_moveIn_time = serializedObject.FindProperty("moveIn").FindPropertyRelative("time");
        sp_moveIn_delay = serializedObject.FindProperty("moveIn").FindPropertyRelative("delay");

        //RotationIn
        sp_rotationIn_enabled = serializedObject.FindProperty("rotationIn").FindPropertyRelative("enabled");
        sp_rotationIn_rotateFrom = serializedObject.FindProperty("rotationIn").FindPropertyRelative("rotateFrom");
        sp_rotationIn_easeType = serializedObject.FindProperty("rotationIn").FindPropertyRelative("easeType");
        sp_rotationIn_time = serializedObject.FindProperty("rotationIn").FindPropertyRelative("time");
        sp_rotationIn_delay = serializedObject.FindProperty("rotationIn").FindPropertyRelative("delay");

        //ScaleIn
        sp_scaleIn_enabled = serializedObject.FindProperty("scaleIn").FindPropertyRelative("enabled");
        sp_scaleIn_scaleBegin = serializedObject.FindProperty("scaleIn").FindPropertyRelative("scaleBegin");
        sp_scaleIn_easeType = serializedObject.FindProperty("scaleIn").FindPropertyRelative("easeType");
        sp_scaleIn_time = serializedObject.FindProperty("scaleIn").FindPropertyRelative("time");
        sp_scaleIn_delay = serializedObject.FindProperty("scaleIn").FindPropertyRelative("delay");

        //FadeIn
        sp_fadeIn_enabled = serializedObject.FindProperty("fadeIn").FindPropertyRelative("enabled");
        sp_fadeIn_easeType = serializedObject.FindProperty("fadeIn").FindPropertyRelative("easeType");
        sp_fadeIn_time = serializedObject.FindProperty("fadeIn").FindPropertyRelative("time");
        sp_fadeIn_delay = serializedObject.FindProperty("fadeIn").FindPropertyRelative("delay");

        //MoveLoop
        sp_moveLoop_enabled = serializedObject.FindProperty("moveLoop").FindPropertyRelative("enabled");
        sp_moveLoop_autoStart = serializedObject.FindProperty("moveLoop").FindPropertyRelative("autoStart");
        sp_moveLoop_movement = serializedObject.FindProperty("moveLoop").FindPropertyRelative("movement");
        sp_moveLoop_easeType = serializedObject.FindProperty("moveLoop").FindPropertyRelative("easeType");
        sp_moveLoop_loops = serializedObject.FindProperty("moveLoop").FindPropertyRelative("loops");
        sp_moveLoop_loopType = serializedObject.FindProperty("moveLoop").FindPropertyRelative("loopType");
        sp_moveLoop_time = serializedObject.FindProperty("moveLoop").FindPropertyRelative("time");
        sp_moveLoop_delay = serializedObject.FindProperty("moveLoop").FindPropertyRelative("delay");

        //RotationLoop
        sp_rotationLoop_enabled = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("enabled");
        sp_rotationLoop_autoStart = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("autoStart");
        sp_rotationLoop_rotation = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("rotation");
        sp_rotationLoop_easeType = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("easeType");
        sp_rotationLoop_loops = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("loops");
        sp_rotationLoop_loopType = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("loopType");
        sp_rotationLoop_time = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("time");
        sp_rotationLoop_delay = serializedObject.FindProperty("rotationLoop").FindPropertyRelative("delay");

        //ScaleLoop
        sp_scaleLoop_enabled = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("enabled");
        sp_scaleLoop_autoStart = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("autoStart");
        sp_scaleLoop_min = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("min");
        sp_scaleLoop_max = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("max");
        sp_scaleLoop_easeType = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("easeType");
        sp_scaleLoop_loops = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("loops");
        sp_scaleLoop_loopType = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("loopType");
        sp_scaleLoop_time = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("time");
        sp_scaleLoop_delay = serializedObject.FindProperty("scaleLoop").FindPropertyRelative("delay");

        //FadeLoop
        sp_fadeLoop_enabled = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("enabled");
        sp_fadeLoop_autoStart = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("autoStart");
        sp_fadeLoop_min = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("min");
        sp_fadeLoop_max = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("max");
        sp_fadeLoop_easeType = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("easeType");
        sp_fadeLoop_loops = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("loops");
        sp_fadeLoop_loopType = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("loopType");
        sp_fadeLoop_time = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("time");
        sp_fadeLoop_delay = serializedObject.FindProperty("fadeLoop").FindPropertyRelative("delay");

        //MoveOut
        sp_moveOut_enabled = serializedObject.FindProperty("moveOut").FindPropertyRelative("enabled");
        sp_moveOut_moveTo = serializedObject.FindProperty("moveOut").FindPropertyRelative("moveTo");
        sp_moveOut_positionAdjustment = serializedObject.FindProperty("moveOut").FindPropertyRelative("positionAdjustment");
        sp_moveOut_positionTo = serializedObject.FindProperty("moveOut").FindPropertyRelative("positionTo");
        sp_moveOut_easeType = serializedObject.FindProperty("moveOut").FindPropertyRelative("easeType");
        sp_moveOut_time = serializedObject.FindProperty("moveOut").FindPropertyRelative("time");
        sp_moveOut_delay = serializedObject.FindProperty("moveOut").FindPropertyRelative("delay");

        //RotationOut
        sp_rotationOut_enabled = serializedObject.FindProperty("rotationOut").FindPropertyRelative("enabled");
        sp_rotationOut_rotateTo = serializedObject.FindProperty("rotationOut").FindPropertyRelative("rotateTo");
        sp_rotationOut_easeType = serializedObject.FindProperty("rotationOut").FindPropertyRelative("easeType");
        sp_rotationOut_time = serializedObject.FindProperty("rotationOut").FindPropertyRelative("time");
        sp_rotationOut_delay = serializedObject.FindProperty("rotationOut").FindPropertyRelative("delay");

        //ScaleOut
        sp_scaleOut_enabled = serializedObject.FindProperty("scaleOut").FindPropertyRelative("enabled");
        sp_scaleOut_scaleEnd = serializedObject.FindProperty("scaleOut").FindPropertyRelative("scaleEnd");
        sp_scaleOut_easeType = serializedObject.FindProperty("scaleOut").FindPropertyRelative("easeType");
        sp_scaleOut_time = serializedObject.FindProperty("scaleOut").FindPropertyRelative("time");
        sp_scaleOut_delay = serializedObject.FindProperty("scaleOut").FindPropertyRelative("delay");

        //FadeOut
        sp_fadeOut_enabled = serializedObject.FindProperty("fadeOut").FindPropertyRelative("enabled");
        sp_fadeOut_easeType = serializedObject.FindProperty("fadeOut").FindPropertyRelative("easeType");
        sp_fadeOut_time = serializedObject.FindProperty("fadeOut").FindPropertyRelative("time");
        sp_fadeOut_delay = serializedObject.FindProperty("fadeOut").FindPropertyRelative("delay");
    }
    #endregion

    #region Variables
    UIElement uiElement;
    UIAnimationManager animationManager;
    Texture tex;

    bool inEditorShowHideOnlyThisUIElement = false;

    int elementNameCurrentIndex = 0;

    UIAnimator.MoveDetails moveFrom;
    UIAnimator.MoveDetails moveTo;

    DG.Tweening.Ease moveInEaseType;
    DG.Tweening.Ease rotationInEaseType;
    DG.Tweening.Ease scaleInEaseType;
    DG.Tweening.Ease fadeInEaseType;

    DG.Tweening.Ease moveLoopEaseType;
    DG.Tweening.Ease rotationLoopEaseType;
    DG.Tweening.Ease scaleLoopEaseType;
    DG.Tweening.Ease fadeLoopEaseType;

    DG.Tweening.LoopType moveLoopLoopType;
    DG.Tweening.LoopType rotationLoopLoopType;
    DG.Tweening.LoopType scaleLoopLoopType;
    DG.Tweening.LoopType fadeLoopLoopType;

    DG.Tweening.Ease moveOutEaseType;
    DG.Tweening.Ease rotationOutEaseType;
    DG.Tweening.Ease scaleOutEaseType;
    DG.Tweening.Ease fadeOutEaseType;

    bool saveInAnimationsPreset = false;
    bool deleteInAnimationsPreset = false;

    bool saveLoopAnimationsPreset = false;
    bool deleteLoopAnimationsPreset = false;

    bool saveOutAnimationsPreset = false;
    bool deleteOutAnimationsPreset = false;

    string[] inAnimationPresets;
    string[] loopAnimationPresets;
    string[] outAnimationPresets;

    string newInAnimationsPresetName = "";
    string newLoopAnimationsPresetName = "";
    string newOutAnimationsPresetName = "";

    string[] elementNames;
    string[] elementSounds;

    string tempElementNameString = string.Empty;
    bool newElementName = false;
    bool renameElementName = false;
    bool deleteElementName = false;

    int[] inAnimationSoundIndex = new int[8]
    {
        0,  //moveIn_soundAtStart_index
        0,  //moveIn_soundAtFinish_index
        0,  //rotateIn_soundAtStart_index
        0,  //rotateIn_soundAtFinish_index
        0,  //scaleIn_soundAtStart_index
        0,  //scaleIn_soundAtFinish_index
        0,  //fadeIn_soundAtStart_index
        0   //fadeIn_soundAtFinish_index
    };

    int[] loopAnimationSoundIndex = new int[8]
   {
        0,  //moveLoop_soundAtStart_index
        0,  //moveLoop_soundAtFinish_index
        0,  //rotateLoop_soundAtStart_index
        0,  //rotateLoop_soundAtFinish_index
        0,  //scaleLoop_soundAtStart_index
        0,  //scaleLoop_soundAtFinish_index
        0,  //fadeLoop_soundAtStart_index
        0   //fadeLoop_soundAtFinish_index
   };

    int[] outAnimationSoundIndex = new int[8]
   {
        0,  //moveOut_soundAtStart_index
        0,  //moveOut_soundAtFinish_index
        0,  //rotateOut_soundAtStart_index
        0,  //rotateOut_soundAtFinish_index
        0,  //scaleOut_soundAtStart_index
        0,  //scaleOut_soundAtFinish_index
        0,  //fadeOut_soundAtStart_index
        0   //fadeOut_soundAtFinish_index
   };
    #endregion

    #region Properties
    UIElement GetUIElement { get { if (uiElement == null) uiElement = (UIElement)target; return uiElement; } }
    UIAnimationManager GetAnimationManager { get { if (animationManager == null) animationManager = GetUIElement.GetAnimationManager; return animationManager; } }
    #endregion

    #region Update ElementNames and ElementSounds Popup

    void UpdateElementNamesPopup()
    {
        //we create the string array that we use for the gui popup
        elementNames = UIManager.GetElementNames();
    }

    void UpdateElementSoundsPopus()
    {
        //we create the string array that we use for the gui popup
        elementSounds = UIManager.GetElementSounds();
    }

    #endregion

    #region Animation Preset Methods
    void UpdateAnimationPresetsFromFiles()
    {
        inAnimationPresets = GetUIElement.GetInAnimationsPresetNames;
        if (UIManager.IsStringInArray(inAnimationPresets, GetUIElement.inAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIElement.inAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIElement.activeInAnimationsPresetIndex = UIManager.GetIndexForStringInArray(inAnimationPresets, GetUIElement.inAnimationsPresetName);

        loopAnimationPresets = GetUIElement.GetLoopAnimationsPresetNames;
        if (UIManager.IsStringInArray(loopAnimationPresets, GetUIElement.loopAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIElement.loopAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIElement.activeLoopAnimationsPresetIndex = UIManager.GetIndexForStringInArray(loopAnimationPresets, GetUIElement.loopAnimationsPresetName);

        outAnimationPresets = GetUIElement.GetOutAnimationsPresetNames;
        if (UIManager.IsStringInArray(outAnimationPresets, GetUIElement.outAnimationsPresetName) == false) //Check the the current peset name exists in the presets array
        {
            GetUIElement.outAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME; //this should not happen; it means that the preset name does not exist as a preset file; we reset to the default preset name
        }
        GetUIElement.activeOutAnimationsPresetIndex = UIManager.GetIndexForStringInArray(outAnimationPresets, GetUIElement.outAnimationsPresetName);
    }
    #endregion

    #region Check if UIElement should be linked to a UINotification
    /// <summary>
    /// This checks up the Hierarchy for any parents that might have a UINotification component attached and if that happens, it sets the linkedToNotification as true.
    /// We do this so that we can hide the Element Name settings from the user, because we know that the UINotification will auto generate it's own elementName and that it will get assigned to this UIElement as well.
    /// </summary>
    void CheckForNotificationLink()
    {
        if (GetUIElement.GetComponentInParent<UINotification>() != null) //we check if one of this UIElement's parents might have a UINotification component
        {
            GetUIElement.linkedToNotification = true;
        }
    }
    #endregion

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    void OnEnable()
    {
        uiElement = (UIElement)target;

        UpdateAnimationPresetsFromFiles();

        UpdateElementNamesPopup();
        UpdateElementSoundsPopus();

        CheckForNotificationLink();

        if (GetUIElement.GetComponent<Canvas>() == null)
        {
            GetUIElement.gameObject.AddComponent<Canvas>();
            Debug.Log("[DoozyUI] [" + GetUIElement.name + "] The selected UIElement didn't have a <Canvas> component attached and it was just added automatically. The missing <Canvas> might cause visibility issues.");
        }

        if (GetUIElement.GetComponent<GraphicRaycaster>() == null)
        {
            GetUIElement.gameObject.AddComponent<GraphicRaycaster>();
            Debug.Log("[DoozyUI] [" + GetUIElement.name + "] The selected UIElement didn't have a <GraphicRaycaster> component attached and it was just added automatically. The missing <GraphicRaycaster> might make this UIElement not be able to receive clicks/touches.");
        }
    }

    public override void OnInspectorGUI()
    {
        UpdateSerializedProperties();
        serializedObject.Update();
        //UpdateElementNamesPopup();
        //UpdateElementSoundsPopus();
        DoozyUIHelper.VerticalSpace(8);
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiElement);
        #endregion
        DoozyUIHelper.VerticalSpace(4);
        if (Application.isPlaying == false)
        {
            #region Main Settings
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            #region Show Help
            DoozyUIHelper.ResetColors();
            GetUIElement.showHelp = EditorGUILayout.ToggleLeft("Show Help", GetUIElement.showHelp, GUILayout.Width(160));
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            #endregion
            GUILayout.Space(8);

#if dUI_UseOrientationManager
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(4);
                if (GUILayout.Button(sp_LANDSCAPE.boolValue ? DoozyUIResources.ButtonLandscapeEnabled : DoozyUIResources.ButtonLandscapeDisabled, GUIStyle.none, GUILayout.Width(87), GUILayout.Height(21.75f)))
                {
                    sp_LANDSCAPE.boolValue = !sp_LANDSCAPE.boolValue;
                }
                GUILayout.Space(8);
                if (GUILayout.Button(sp_PORTRAIT.boolValue ? DoozyUIResources.ButtonPortraitEnabled : DoozyUIResources.ButtonPortraitDisabled, GUIStyle.none, GUILayout.Width(87), GUILayout.Height(21.75f)))
                {
                    sp_PORTRAIT.boolValue = !sp_PORTRAIT.boolValue;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8);
#endif

            #region Element Name
            if (GetUIElement.linkedToNotification)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    DoozyUIHelper.DrawTexture(DoozyUIResources.MessageLinkedToNotification, 128, 56);
                    GUILayout.Space(72);
                    if (GUILayout.Button(DoozyUIResources.ButtonUnlinkFromNotification, GUIStyle.none, GUILayout.Width(56), GUILayout.Height(56)))
                    {
                        GetUIElement.linkedToNotification = false;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (newElementName == false && renameElementName == false && deleteElementName == false)
                {
                    EditorGUILayout.LabelField("Element Name", GUILayout.Width(182));
                    if (UIManager.GetIndexForElementName(sp_elementName.stringValue) == -1)
                    {
                        DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                    }
                    elementNameCurrentIndex = UIManager.GetIndexForElementName(sp_elementName.stringValue);
                    elementNameCurrentIndex = EditorGUILayout.Popup(elementNameCurrentIndex, elementNames, GUILayout.Width(182));
                    sp_elementName.stringValue = UIManager.GetDoozyUIData.elementNames[elementNameCurrentIndex].elementName;
                }
                else if (newElementName == true)
                {
                    EditorGUILayout.LabelField("Create new element name", GUILayout.Width(182));
                    DoozyUIHelper.ResetColors();
                    tempElementNameString = EditorGUILayout.TextField(tempElementNameString, GUILayout.Width(182));
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                }
                else if (renameElementName == true)
                {
                    EditorGUILayout.LabelField("Rename element?", GUILayout.Width(182));
                    DoozyUIHelper.ResetColors();
                    tempElementNameString = EditorGUILayout.TextField(tempElementNameString, GUILayout.Width(182));
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                }
                else if (deleteElementName == true)
                {
                    EditorGUILayout.LabelField("Do you want to delete?", GUILayout.Width(182));
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    EditorGUILayout.LabelField(tempElementNameString, GUILayout.Width(182));
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                }

                EditorGUILayout.BeginHorizontal();
                if (newElementName == false && renameElementName == false && deleteElementName == false)
                {
                    if (sp_elementName.stringValue.Equals(UIManager.DEFAULT_ELEMENT_NAME))
                    {
                        if (GUILayout.Button("new", GUILayout.Width(182), GUILayout.Height(16)))
                        {
                            tempElementNameString = string.Empty;
                            newElementName = true;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("new", GUILayout.Width(58), GUILayout.Height(16)))
                        {
                            tempElementNameString = string.Empty;
                            newElementName = true;
                        }
                        if (GUILayout.Button("rename", GUILayout.Width(58), GUILayout.Height(16)))
                        {
                            if (tempElementNameString.Equals(UIManager.DEFAULT_ELEMENT_NAME))
                            {
                                Debug.Log("[DoozyUI] You cannot (and should not) rename the default element name.");
                                return;
                            }
                            tempElementNameString = sp_elementName.stringValue;
                            renameElementName = true;
                        }
                        if (GUILayout.Button("delete", GUILayout.Width(58), GUILayout.Height(16)))
                        {
                            if (sp_elementName.stringValue.Equals(UIManager.DEFAULT_ELEMENT_NAME))
                            {
                                Debug.Log("[DoozyUI] You cannot (and should not) delete the default element name.");
                                return;
                            }
                            tempElementNameString = sp_elementName.stringValue;
                            deleteElementName = true;
                        }
                    }
                }
                else if (newElementName == true)
                {
                    #region New ElementName
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    if (GUILayout.Button("confirm", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        UIManager.TrimStartAndEndSpaces(tempElementNameString);
                        if (tempElementNameString.Equals(string.Empty) == false                         //we make sure the new name is not empty
                            && tempElementNameString.Equals(UIManager.DEFAULT_ELEMENT_NAME) == false    //we check that is not the default name
                            && UIManager.GetIndexForElementName(tempElementNameString) == -1)            //we make sure there are no duplicates
                        {
                            UIManager.NewElementName(tempElementNameString);
                        }

                        sp_elementName.stringValue = UIManager.GetDoozyUIData.elementNames[UIManager.GetIndexForElementName(tempElementNameString)].elementName;
                        UpdateElementNamesPopup();                  //we update the string array that shows the list of element names in the inspector
                        tempElementNameString = string.Empty;       //we clear the temporary name holder
                        newElementName = false;                     //we show the initial menu for the element name
                    }

                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    if (GUILayout.Button("cancel", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        tempElementNameString = string.Empty;
                        newElementName = false;
                    }
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    #endregion
                }
                else if (renameElementName == true)
                {
                    #region Rename ElementName
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    if (GUILayout.Button("confirm", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        UIManager.TrimStartAndEndSpaces(tempElementNameString);
                        if (tempElementNameString.Equals(string.Empty) == false                          //we make sure the new name is not empty
                             && tempElementNameString.Equals(UIManager.DEFAULT_ELEMENT_NAME) == false    //we check that is not the default name
                             && UIManager.GetIndexForElementName(tempElementNameString) == -1)            //we make sure there are no duplicates
                        {
                            UIManager.RenameElementName(elementNameCurrentIndex, tempElementNameString);
                            UpdateElementNamesPopup();
                            sp_elementName.stringValue = tempElementNameString;
                            DoozyUIRedundancyCheck.CheckAllTheUIElements();
                        }
                        tempElementNameString = string.Empty;
                        renameElementName = false;
                    }

                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    if (GUILayout.Button("cancel", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        tempElementNameString = string.Empty;
                        renameElementName = false;
                    }
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    #endregion
                }
                else if (deleteElementName == true)
                {
                    #region Delete ElementName
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    if (GUILayout.Button("yes", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        if (sp_elementName.stringValue.Equals(UIManager.DEFAULT_ELEMENT_NAME) == false)   //we delete the entry only if it's not the first one or the default one, that we are keeping as an empty string entry
                        {
                            UIManager.DeleteElementName(elementNameCurrentIndex);   //we remove the entry with the current index
                            elementNameCurrentIndex = UIManager.GetIndexForElementName(UIManager.DEFAULT_ELEMENT_NAME); //we set the current index to the default name
                            sp_elementName.stringValue = UIManager.DEFAULT_ELEMENT_NAME;
                            DoozyUIRedundancyCheck.CheckAllTheUIElements();
                        }
                        else
                        {
                            Debug.Log("[DoozyUI] You cannot (and should not) delete the default element name '" + UIManager.DEFAULT_ELEMENT_NAME + "'.");
                        }

                        UpdateElementNamesPopup();
                        tempElementNameString = string.Empty;
                        deleteElementName = false;
                    }

                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    if (GUILayout.Button("no", GUILayout.Width(89), GUILayout.Height(16)))
                    {
                        tempElementNameString = string.Empty;
                        deleteElementName = false;
                    }
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                    #endregion
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            GUILayout.Space(4);

            #region hide @START, animate @START, disable when hidden
            sp_startHidden.boolValue = EditorGUILayout.ToggleLeft("hide @START", sp_startHidden.boolValue, GUILayout.Width(160));
            sp_animateAtStart.boolValue = EditorGUILayout.ToggleLeft("animate @START", sp_animateAtStart.boolValue, GUILayout.Width(160));
            if (sp_animateAtStart.boolValue) { sp_startHidden.boolValue = false; }
            sp_disableWhenHidden.boolValue = EditorGUILayout.ToggleLeft("disable when hidden", sp_disableWhenHidden.boolValue, GUILayout.Width(160));
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(16);

            EditorGUILayout.BeginVertical();
            DoozyUIHelper.VerticalSpace(2);

            #region InAnimationsOverview
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_useInAnimations.boolValue ? DoozyUIResources.IconInEnabled : DoozyUIResources.IconInDisabled, GUIStyle.none, GUILayout.Width(48), GUILayout.Height(24)))
                {
                    sp_useInAnimations.boolValue = !sp_useInAnimations.boolValue; //Toggle visibility of the InAnimations Zone
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_moveIn_enabled.boolValue ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate MoveIn
                    sp_moveIn_enabled.boolValue = !sp_moveIn_enabled.boolValue;
                    if (sp_moveIn_enabled.boolValue && !sp_useInAnimations.boolValue) //If InAnimations Zone is hidden, we show it
                    {
                        sp_useInAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_rotationIn_enabled.boolValue ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate RotationIn
                    sp_rotationIn_enabled.boolValue = !sp_rotationIn_enabled.boolValue;
                    if (sp_rotationIn_enabled.boolValue && !sp_useInAnimations.boolValue) //If InAnimations Zone is hidden, we show it
                    {
                        sp_useInAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_scaleIn_enabled.boolValue ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate ScaleIn
                    sp_scaleIn_enabled.boolValue = !sp_scaleIn_enabled.boolValue;
                    if (sp_scaleIn_enabled.boolValue && !sp_useInAnimations.boolValue) //If InAnimations Zone is hidden, we show it
                    {
                        sp_useInAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_fadeIn_enabled.boolValue ? DoozyUIResources.IconFadeEnabled : DoozyUIResources.IconFadeDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate FadeIn
                    sp_fadeIn_enabled.boolValue = !sp_fadeIn_enabled.boolValue;
                    if (sp_fadeIn_enabled.boolValue && !sp_useInAnimations.boolValue) //If InAnimations Zone is hidden, we show it
                    {
                        sp_useInAnimations.boolValue = true;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.VerticalSpace(4);

            #region LoopAnimationsOverview
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_useLoopAnimations.boolValue ? DoozyUIResources.IconLoopEnabled : DoozyUIResources.IconLoopDisabled, GUIStyle.none, GUILayout.Width(48), GUILayout.Height(24)))
                {
                    //Toggle visibility of the LoopAnimations Zone
                    sp_useLoopAnimations.boolValue = !sp_useLoopAnimations.boolValue;
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_moveLoop_enabled.boolValue ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate MoveLoop
                    sp_moveLoop_enabled.boolValue = !sp_moveLoop_enabled.boolValue;
                    if (sp_moveLoop_enabled.boolValue && !sp_useLoopAnimations.boolValue) //If LoopAnimations Zone is hidden, we show it
                    {
                        sp_useLoopAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_rotationLoop_enabled.boolValue ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate RotationLoop
                    sp_rotationLoop_enabled.boolValue = !sp_rotationLoop_enabled.boolValue;
                    if (sp_rotationLoop_enabled.boolValue && !sp_useLoopAnimations.boolValue) //If LoopAnimations Zone is hidden, we show it
                    {
                        sp_useLoopAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_scaleLoop_enabled.boolValue ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate ScaleLoop
                    sp_scaleLoop_enabled.boolValue = !sp_scaleLoop_enabled.boolValue;
                    if (sp_scaleLoop_enabled.boolValue && !sp_useLoopAnimations.boolValue) //If LoopAnimations Zone is hidden, we show it
                    {
                        sp_useLoopAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_fadeLoop_enabled.boolValue ? DoozyUIResources.IconFadeEnabled : DoozyUIResources.IconFadeDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate FadeLoop
                    sp_fadeLoop_enabled.boolValue = !sp_fadeLoop_enabled.boolValue;
                    if (sp_fadeLoop_enabled.boolValue && !sp_useLoopAnimations.boolValue) //If LoopAnimations Zone is hidden, we show it
                    {
                        sp_useLoopAnimations.boolValue = true;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.VerticalSpace(4);

            #region OutAnimationsOverview
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sp_useOutAnimations.boolValue ? DoozyUIResources.IconOutEnabled : DoozyUIResources.IconOutDisabled, GUIStyle.none, GUILayout.Width(48), GUILayout.Height(24)))
                {
                    //Toggle visibility of the OutAnimations Zone
                    sp_useOutAnimations.boolValue = !sp_useOutAnimations.boolValue;
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_moveOut_enabled.boolValue ? DoozyUIResources.IconMoveEnabled : DoozyUIResources.IconMoveDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate MoveOut
                    sp_moveOut_enabled.boolValue = !sp_moveOut_enabled.boolValue;
                    if (sp_moveOut_enabled.boolValue && !sp_useOutAnimations.boolValue) //If OutAnimations Zone is hidden, we show it
                    {
                        sp_useOutAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_rotationOut_enabled.boolValue ? DoozyUIResources.IconRotationEnabled : DoozyUIResources.IconRotationDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate RotationOut
                    sp_rotationOut_enabled.boolValue = !sp_rotationOut_enabled.boolValue;
                    if (sp_rotationOut_enabled.boolValue && !sp_useOutAnimations.boolValue) //If OutAnimations Zone is hidden, we show it
                    {
                        sp_useOutAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_scaleOut_enabled.boolValue ? DoozyUIResources.IconScaleEnabled : DoozyUIResources.IconScaleDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate ScaleOut
                    sp_scaleOut_enabled.boolValue = !sp_scaleOut_enabled.boolValue;
                    if (sp_scaleOut_enabled.boolValue && !sp_useOutAnimations.boolValue) //If OutAnimations Zone is hidden, we show it
                    {
                        sp_useOutAnimations.boolValue = true;
                    }
                }
                GUILayout.Space(6);
                if (GUILayout.Button(sp_fadeOut_enabled.boolValue ? DoozyUIResources.IconFadeEnabled : DoozyUIResources.IconFadeDisabled, GUIStyle.none, GUILayout.Width(36), GUILayout.Height(24)))
                {
                    //Activate/Deactivate FadeOut
                    sp_fadeOut_enabled.boolValue = !sp_fadeOut_enabled.boolValue;
                    if (sp_fadeOut_enabled.boolValue && !sp_useOutAnimations.boolValue) //If OutAnimations Zone is hidden, we show it
                    {
                        sp_useOutAnimations.boolValue = true;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion
            DoozyUIHelper.VerticalSpace(8);
            #region Selected Button
            EditorGUILayout.LabelField("Default Button Selected");
            EditorGUILayout.PropertyField(sp_selectedButton, GUIContent.none, GUILayout.Width(210));
            #endregion
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (GetUIElement.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("Element Name: the name that triggers the Show or Hide of this element", MessageType.None);
                EditorGUILayout.HelpBox("hide @START: if enabled, it will auto hide this element when the app/game starts (an IN and an OUT animation should be setup beforehand) - this was previously known as 'start hidden'", MessageType.None);
                EditorGUILayout.HelpBox("animate @START: if enabled, it will trigger automatically the IN animations OnAwake (an IN and an OUT animation should be setup beforehand)", MessageType.None);
                EditorGUILayout.HelpBox("disable when hidden: if enabled, it will disable - SetActive(false) - the UIElement when it is hidden (or set to Hide). This will help lower the draw calls for the UI", MessageType.None);
                EditorGUILayout.HelpBox("Default Button Selected: if a button is referenced, it will get selected automatically when the UIElement is shown. This feature has been introduced to facilitate the control of the UI through controllers and keyboard", MessageType.None);
                EditorGUILayout.HelpBox("The panel on the right shows/hides each animation group (IN, LOOP, OUT - the blue buttons) and enables/disables each individual animation type (MOVE, ROTATE, SCALE, FADE - the colored buttons)", MessageType.None);
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }

            DoozyUIHelper.VerticalSpace(4);

            #region Custon Start Anchored Position
            EditorGUILayout.BeginHorizontal();
            if (sp_useCustomStartAnchoredPosition.boolValue)
            {
                GUILayout.Space(-4);
            }
            EditorGUILayout.BeginVertical();
            sp_useCustomStartAnchoredPosition.boolValue = EditorGUILayout.ToggleLeft("use Custom Start Position", sp_useCustomStartAnchoredPosition.boolValue, GUILayout.Width(182));
            if (sp_useCustomStartAnchoredPosition.boolValue)
            {
                DoozyUIHelper.VerticalSpace(2);
                sp_customStartAnchoredPosition.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_customStartAnchoredPosition.vector3Value, GUILayout.Width(182));
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical();
            if (sp_useCustomStartAnchoredPosition.boolValue)
            {
                if (GUILayout.Button("set as current position", GUILayout.Width(216), GUILayout.Height(16)))
                {
                    sp_customStartAnchoredPosition.vector3Value = GetUIElement.GetRectTransform.anchoredPosition3D;
                }

                if (GUILayout.Button("move to position", GUILayout.Width(216), GUILayout.Height(16)))
                {
                    GetUIElement.GetRectTransform.anchoredPosition3D = sp_customStartAnchoredPosition.vector3Value;
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (GetUIElement.showHelp)
            {
                DoozyUIHelper.ResetColors();
                EditorGUILayout.HelpBox("The Custom Start Position allows you to move the UIElement (window) anywhere in the scene. On Awake, it will automatically move to this position in order for the IN and OUT animations to work.", MessageType.None);
                if (sp_useCustomStartAnchoredPosition.boolValue)
                {
                    EditorGUILayout.HelpBox("set as current position: sets the Custom Start Position as the UIElement's current anchoredPosition3D", MessageType.None);
                    EditorGUILayout.HelpBox("move to position: moves the UIElement to the Custom Start Position using the anchoredPosition3D", MessageType.None);
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            }
            #endregion

            #endregion
        }
        else
        {
            #region PlayMode Menu - SHOW & HIDE buttons
            EditorGUILayout.BeginVertical(GUILayout.Width(416));

            if (GetUIElement.AreInAnimationsEnabled == false) //if the IN animations are disabled we disable the SHOW button and we show the user (developer) what is the problem
            {
                EditorGUILayout.HelpBox(
                  "There are no IN animations enabled, thus the SHOW UIElement will not work. Enable at least one IN animation."
                  , MessageType.Error);
            }
            if (GetUIElement.AreOutAnimationsEnabled == false) //if the OUT animations are disabled we disable the HIDE button and we show the user (developer) what is the problem
            {
                EditorGUILayout.HelpBox(
                  "There are no OUT animations enabled, thus the HIDE UIElement will not work. Enable at least one OUT animation."
                  , MessageType.Error);
            }
            EditorGUILayout.BeginHorizontal();
            if (GetUIElement.AreInAnimationsEnabled == false)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            }
            if (GUILayout.Button("SHOW", GUILayout.Width(206), GUILayout.Height(24))) //this is the SHOW UIELement in PlayMode button
            {
                if (GetUIElement.AreInAnimationsEnabled)
                {
                    if (inEditorShowHideOnlyThisUIElement)
                    {
                        if (GetUIElement.gameObject.activeInHierarchy == false)
                        {
                            GetUIElement.gameObject.SetActive(true);
                        }
                        GetUIElement.Show(false);
                    }
                    else
                    {
                        UIManager.ShowUiElement(sp_elementName.stringValue, false); //we play the IN animations
                    }
                }
                else
                {
                    Debug.Log("[DoozyUI] There are no IN animations enabled for the '" + sp_elementName.stringValue + "' UIElement. The SHOW method will not work. Enable at least one IN animation to fix this issue.");
                }
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);

            if (GetUIElement.AreOutAnimationsEnabled == false)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            }
            if (GUILayout.Button("HIDE", GUILayout.Width(206), GUILayout.Height(24))) //this is the HIDE UIElement in PlayMode button
            {
                if (GetUIElement.AreOutAnimationsEnabled)
                {
                    if (inEditorShowHideOnlyThisUIElement)
                    {
                        GetUIElement.Hide(false);
                    }
                    else
                    {
                        UIManager.HideUiElement(sp_elementName.stringValue, false); //we play the OUT animations
                    }
                }
                else
                {
                    Debug.Log("[DoozyUI] There are no OUT animations enabled for the '" + sp_elementName.stringValue + "' UIElement. The HIDE method will not work. Enable at least one OUT animation to fix this issue.");
                }
            }
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            EditorGUILayout.EndHorizontal();

            inEditorShowHideOnlyThisUIElement = EditorGUILayout.ToggleLeft("trigger IN and OUT animations only for this UIElement", inEditorShowHideOnlyThisUIElement);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(416));
            string selectedButtonName = "None";
            if (GetUIElement.selectedButton != null)
            {
                selectedButtonName = GetUIElement.selectedButton.name;
            }
            EditorGUILayout.HelpBox(
               "Element Name: " + sp_elementName.stringValue + "\n" +
               "hide @START: " + GetUIElement.startHidden + "\n" +
               "animate @START: " + GetUIElement.animateAtStart + "\n" +
               "disable when hidden: " + GetUIElement.disableWhenHidden + "\n" +
               "Default Button Selected: " + selectedButtonName
               , MessageType.None);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            #endregion
        }
        DoozyUIHelper.VerticalSpace(8);

        #region InAnimations
        EditorGUILayout.BeginHorizontal();
        {
            tex = DoozyUIResources.LabelInAnimationsDisabled;
            if (sp_useInAnimations.boolValue)
            {
                tex = DoozyUIResources.LabelInAnimations;
            }
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Width(198), GUILayout.Height(30)))
            {
                //Toggle visibility of the InAnimations Zone
                sp_useInAnimations.boolValue = !sp_useInAnimations.boolValue;
            }

            #region InAnimations - PRESETS
            if (sp_useInAnimations.boolValue)
            {
                EditorGUILayout.BeginVertical();
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                if (saveInAnimationsPreset == false && deleteInAnimationsPreset == false)
                {
                    sp_activeInAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeInAnimationsPresetIndex.intValue, inAnimationPresets, GUILayout.Width(214));

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("load", GUILayout.Width(69), GUILayout.Height(16)))
                    {
                        GetAnimationManager.LoadPreset(inAnimationPresets[sp_activeInAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.IN);
                        OnInspectorGUI();
                    }

                    if (GUILayout.Button("save", GUILayout.Width(69), GUILayout.Height(16)))
                    {
                        saveInAnimationsPreset = !saveInAnimationsPreset;
                        newInAnimationsPresetName = string.Empty;
                    }

                    if (GUILayout.Button("delete", GUILayout.Width(68), GUILayout.Height(16)))
                    {
                        deleteInAnimationsPreset = true;
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
                else if (saveInAnimationsPreset == true)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                    newInAnimationsPresetName = EditorGUILayout.TextField(newInAnimationsPresetName, GUILayout.Width(130));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    if (GUILayout.Button("save", GUILayout.Width(105), GUILayout.Height(16)))
                    {
                        GetAnimationManager.SavePreset(newInAnimationsPresetName, UIAnimationManager.AnimationType.IN);
                        UpdateAnimationPresetsFromFiles();
                        saveInAnimationsPreset = false;
                    }
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    if (GUILayout.Button("cancel", GUILayout.Width(105), GUILayout.Height(16)))
                    {
                        saveInAnimationsPreset = false;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
                else if (deleteInAnimationsPreset == true)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Delete Preset '" + inAnimationPresets[GetUIElement.activeInAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                    if (GUILayout.Button("yes", GUILayout.Width(105), GUILayout.Height(16)))
                    {
                        GetAnimationManager.DeletePreset(inAnimationPresets[GetUIElement.activeInAnimationsPresetIndex], UIAnimationManager.AnimationType.IN);
                        UpdateAnimationPresetsFromFiles();
                        deleteInAnimationsPreset = false;
                    }
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                    if (GUILayout.Button("no", GUILayout.Width(105), GUILayout.Height(16)))
                    {
                        deleteInAnimationsPreset = false;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                GUILayout.FlexibleSpace();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GetUIElement.showHelp && sp_useInAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for IN animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The IN animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/IN", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.ResetColors();
        #endregion

        if (sp_useInAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);

            #region MoveIn

            EditorGUILayout.BeginHorizontal();

            tex = DoozyUIResources.LabelMoveInDisabled;
            if (sp_moveIn_enabled.boolValue)
                tex = DoozyUIResources.LabelMoveInEnabled;

            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_moveIn_enabled.boolValue = !sp_moveIn_enabled.boolValue;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (sp_moveIn_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_moveIn_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_moveIn_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_moveIn_time.floatValue = EditorGUILayout.FloatField(sp_moveIn_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_moveIn_delay.floatValue = EditorGUILayout.FloatField(sp_moveIn_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                moveInEaseType = GetUIElement.moveIn.easeType;
                moveInEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(moveInEaseType, GUILayout.Width(140));
                sp_moveIn_easeType.enumValueIndex = (int)moveInEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("from", GUILayout.Width(30));
                moveFrom = GetUIElement.moveIn.moveFrom;
                moveFrom = (UIAnimator.MoveDetails)EditorGUILayout.EnumPopup(moveFrom, GUILayout.Width(130));
                sp_moveIn_moveFrom.enumValueIndex = (int)moveFrom;
                EditorGUILayout.LabelField("adjust position", GUILayout.Width(90));
                sp_moveIn_positionAdjustment.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_moveIn_positionAdjustment.vector3Value, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (moveFrom == UIAnimator.MoveDetails.LocalPosition)
                {
                    EditorGUILayout.LabelField("from localposition", GUILayout.Width(120));
                    sp_moveIn_positionFrom.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_moveIn_positionFrom.vector3Value, GUILayout.Width(150));
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region MoveIn Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.moveInSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[0] = UIManager.GetIndexForElementSound(GetUIElement.moveInSoundAtStart);
                inAnimationSoundIndex[0] = EditorGUILayout.Popup(inAnimationSoundIndex[0], elementSounds, GUILayout.Width(140));
                GetUIElement.moveIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[0]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveInSoundAtStart = GetUIElement.moveIn.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region MoveIN sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.moveInSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[1] = UIManager.GetIndexForElementSound(GetUIElement.moveInSoundAtFinish);
                inAnimationSoundIndex[1] = EditorGUILayout.Popup(inAnimationSoundIndex[1], elementSounds, GUILayout.Width(140));
                GetUIElement.moveIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[1]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveInSoundAtFinish = GetUIElement.moveIn.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region RotationIn
            tex = DoozyUIResources.LabelRotateInDisabled;
            if (sp_rotationIn_enabled.boolValue)
                tex = DoozyUIResources.LabelRotateInEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_rotationIn_enabled.boolValue = !sp_rotationIn_enabled.boolValue;
            }
            if (sp_rotationIn_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_rotationIn_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_rotationIn_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_rotationIn_time.floatValue = EditorGUILayout.FloatField(sp_rotationIn_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_rotationIn_delay.floatValue = EditorGUILayout.FloatField(sp_rotationIn_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                rotationInEaseType = GetUIElement.rotationIn.easeType;
                rotationInEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(rotationInEaseType, GUILayout.Width(140));
                sp_rotationIn_easeType.enumValueIndex = (int)rotationInEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("from", GUILayout.Width(30));
                sp_rotationIn_rotateFrom.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_rotationIn_rotateFrom.vector3Value, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);

                EditorGUILayout.BeginHorizontal();

                #region RotateIN Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationInSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[2] = UIManager.GetIndexForElementSound(GetUIElement.rotationInSoundAtStart);
                inAnimationSoundIndex[2] = EditorGUILayout.Popup(inAnimationSoundIndex[2], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[2]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationInSoundAtStart = GetUIElement.rotationIn.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region RotateIN sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationInSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[3] = UIManager.GetIndexForElementSound(GetUIElement.rotationInSoundAtFinish);
                inAnimationSoundIndex[3] = EditorGUILayout.Popup(inAnimationSoundIndex[3], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[3]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationInSoundAtFinish = GetUIElement.rotationIn.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region ScaleIn
            tex = DoozyUIResources.LabelScaleInDisabled;
            if (sp_scaleIn_enabled.boolValue)
                tex = DoozyUIResources.LabelScaleInEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_scaleIn_enabled.boolValue = !sp_scaleIn_enabled.boolValue;
            }
            if (sp_scaleIn_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_scaleIn_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_scaleIn_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_scaleIn_time.floatValue = EditorGUILayout.FloatField(sp_scaleIn_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_scaleIn_delay.floatValue = EditorGUILayout.FloatField(sp_scaleIn_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                scaleInEaseType = GetUIElement.scaleIn.easeType;
                scaleInEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(scaleInEaseType, GUILayout.Width(140));
                sp_scaleIn_easeType.enumValueIndex = (int)scaleInEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("from", GUILayout.Width(30));
                sp_scaleIn_scaleBegin.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_scaleIn_scaleBegin.vector3Value, GUILayout.Width(150));
#if dUI_TextMeshPro
                sp_scaleIn_scaleBegin.vector3Value = new Vector3(sp_scaleIn_scaleBegin.vector3Value.x, sp_scaleIn_scaleBegin.vector3Value.y, 1);
#endif
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region ScaleIN Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleInSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[4] = UIManager.GetIndexForElementSound(GetUIElement.scaleInSoundAtStart);
                inAnimationSoundIndex[4] = EditorGUILayout.Popup(inAnimationSoundIndex[4], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[4]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleInSoundAtStart = GetUIElement.scaleIn.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region ScaleIN sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleInSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[5] = UIManager.GetIndexForElementSound(GetUIElement.scaleInSoundAtFinish);
                inAnimationSoundIndex[5] = EditorGUILayout.Popup(inAnimationSoundIndex[5], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[5]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleInSoundAtFinish = GetUIElement.scaleIn.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region FadeIn
            tex = DoozyUIResources.LabelFadeInDisabled;
            if (sp_fadeIn_enabled.boolValue)
                tex = DoozyUIResources.LabelFadeInEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_fadeIn_enabled.boolValue = !sp_fadeIn_enabled.boolValue;
            }
            if (sp_fadeIn_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Fade);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_fadeIn_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_fadeIn_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_fadeIn_time.floatValue = EditorGUILayout.FloatField(sp_fadeIn_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_fadeIn_delay.floatValue = EditorGUILayout.FloatField(sp_fadeIn_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                fadeInEaseType = GetUIElement.fadeIn.easeType;
                fadeInEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(fadeInEaseType, GUILayout.Width(140));
                sp_fadeIn_easeType.enumValueIndex = (int)fadeInEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region FadeIN Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeInSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[6] = UIManager.GetIndexForElementSound(GetUIElement.fadeInSoundAtStart);
                inAnimationSoundIndex[6] = EditorGUILayout.Popup(inAnimationSoundIndex[6], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[6]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeInSoundAtStart = GetUIElement.fadeIn.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region FadeIN sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeInSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                inAnimationSoundIndex[7] = UIManager.GetIndexForElementSound(GetUIElement.fadeInSoundAtFinish);
                inAnimationSoundIndex[7] = EditorGUILayout.Popup(inAnimationSoundIndex[7], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[inAnimationSoundIndex[7]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeInSoundAtFinish = GetUIElement.fadeIn.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region Events
            if (sp_moveIn_enabled.boolValue || sp_rotationIn_enabled.boolValue || sp_scaleIn_enabled.boolValue || sp_fadeIn_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                EditorGUILayout.BeginHorizontal();
                sp_useInAnimationsStartEvents.boolValue = EditorGUILayout.ToggleLeft("IN Animations Events @START", sp_useInAnimationsStartEvents.boolValue, GUILayout.Width(205));
                sp_useInAnimationsFinishEvents.boolValue = EditorGUILayout.ToggleLeft("IN Animations Events @FINISH", sp_useInAnimationsFinishEvents.boolValue, GUILayout.Width(205));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (GetUIElement.showHelp)
                {
                    DoozyUIHelper.ResetColors();
                    DoozyUIHelper.VerticalSpace(1);
                    EditorGUILayout.HelpBox("If you want to trigger anything when the IN Animations START or FINISH you can do it here, using the native UnityEvent system", MessageType.None);
                    EditorGUILayout.HelpBox("All the triggers take into account the delay times of all the IN Animations on this UIElement", MessageType.None);
                    EditorGUILayout.HelpBox("The events @START, are triggered after MIN (delay)", MessageType.None);
                    EditorGUILayout.HelpBox("The events @FINISH, are triggered after MAX (delay + time)", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                }

                if (sp_useInAnimationsStartEvents.boolValue)
                {
                    EditorGUILayout.PropertyField(sp_onInAnimationsStart, GUILayout.Width(416));
                }
                if (sp_useInAnimationsFinishEvents.boolValue)
                {
                    EditorGUILayout.PropertyField(sp_onInAnimationsFinish, GUILayout.Width(416));
                }
                DoozyUIHelper.ResetColors();
            }
            else
            {
                sp_useInAnimationsStartEvents.boolValue = false;
                sp_useInAnimationsFinishEvents.boolValue = false;
            }
            #endregion
        }
        #endregion


        if (sp_useInAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(8);
        }
        else
        {
            DoozyUIHelper.VerticalSpace(2);
        }

        #region LoopAnimations
        EditorGUILayout.BeginHorizontal();

        tex = DoozyUIResources.LabelLoopAnimationsDisabled;
        if (sp_useLoopAnimations.boolValue)
        {
            tex = DoozyUIResources.LabelLoopAnimations;
        }
        if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Width(198), GUILayout.Height(24)))
        {
            //Toggle visibility of the LoopAnimations Zone
            sp_useLoopAnimations.boolValue = !sp_useLoopAnimations.boolValue;
        }

        #region LoopAnimations - PRESETS

        if (sp_useLoopAnimations.boolValue)
        {
            EditorGUILayout.BeginVertical();
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            if (saveLoopAnimationsPreset == false && deleteLoopAnimationsPreset == false)
            {
                sp_activeLoopAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeLoopAnimationsPresetIndex.intValue, loopAnimationPresets, GUILayout.Width(214));

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("load", GUILayout.Width(69), GUILayout.Height(16)))
                {
                    GetAnimationManager.LoadPreset(loopAnimationPresets[sp_activeLoopAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.LOOP);
                    OnInspectorGUI();
                }

                if (GUILayout.Button("save", GUILayout.Width(69), GUILayout.Height(16)))
                {
                    saveLoopAnimationsPreset = !saveLoopAnimationsPreset;
                    newLoopAnimationsPresetName = string.Empty;
                }

                if (GUILayout.Button("delete", GUILayout.Width(68), GUILayout.Height(16)))
                {
                    deleteLoopAnimationsPreset = true;
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (saveLoopAnimationsPreset == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                newLoopAnimationsPresetName = EditorGUILayout.TextField(newLoopAnimationsPresetName, GUILayout.Width(130));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("save", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    GetAnimationManager.SavePreset(newLoopAnimationsPresetName, UIAnimationManager.AnimationType.LOOP);
                    UpdateAnimationPresetsFromFiles();
                    saveLoopAnimationsPreset = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    saveLoopAnimationsPreset = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (deleteLoopAnimationsPreset == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Delete Preset '" + loopAnimationPresets[GetUIElement.activeLoopAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("yes", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    GetAnimationManager.DeletePreset(loopAnimationPresets[GetUIElement.activeLoopAnimationsPresetIndex], UIAnimationManager.AnimationType.LOOP);
                    UpdateAnimationPresetsFromFiles();
                    deleteLoopAnimationsPreset = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("no", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    deleteLoopAnimationsPreset = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.EndHorizontal();

        if (GetUIElement.showHelp && sp_useLoopAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for LOOP animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The LOOP animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/LOOP", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.ResetColors();
        #endregion

        if (sp_useLoopAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);

            #region MoveLoop
            tex = DoozyUIResources.LabelMoveLoopDisabled;
            if (sp_moveLoop_enabled.boolValue)
                tex = DoozyUIResources.LabelMoveLoopEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_moveLoop_enabled.boolValue = !sp_moveLoop_enabled.boolValue;
            }
            if (sp_moveLoop_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_moveLoop_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_moveLoop_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_moveLoop_time.floatValue = EditorGUILayout.FloatField(sp_moveLoop_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_moveLoop_delay.floatValue = EditorGUILayout.FloatField(sp_moveLoop_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                moveLoopEaseType = GetUIElement.moveLoop.easeType;
                moveLoopEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(moveLoopEaseType, GUILayout.Width(140));
                sp_moveLoop_easeType.enumValueIndex = (int)moveLoopEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_moveLoop_autoStart.boolValue = EditorGUILayout.ToggleLeft("auto start", sp_moveLoop_autoStart.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("movement", GUILayout.Width(70));
                sp_moveLoop_movement.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_moveLoop_movement.vector3Value, GUILayout.Width(254));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                sp_moveLoop_loops.intValue = EditorGUILayout.IntField(sp_moveLoop_loops.intValue, GUILayout.Width(93));
                EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                moveLoopLoopType = GetUIElement.moveLoop.loopType;
                moveLoopLoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(moveLoopLoopType, GUILayout.Width(138));
                sp_moveLoop_loopType.enumValueIndex = (int)moveLoopLoopType;
                EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                switch (moveLoopLoopType)
                {
                    case DG.Tweening.LoopType.Yoyo:
                        EditorGUILayout.HelpBox("LoopType.Yoyo - the tween moves forward and backwards at alternate cycles", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Restart:
                        EditorGUILayout.HelpBox("LoopType.Restart - each loop cycle restarts from the beginning", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Incremental:
                        EditorGUILayout.HelpBox("LoopType.Incremental - continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving 'onward'", MessageType.None);
                        break;
                }

                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region MoveLOOP Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.moveLoopSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[0] = UIManager.GetIndexForElementSound(GetUIElement.moveLoopSoundAtStart);
                loopAnimationSoundIndex[0] = EditorGUILayout.Popup(loopAnimationSoundIndex[0], elementSounds, GUILayout.Width(140));
                GetUIElement.moveLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[0]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveLoopSoundAtStart = GetUIElement.moveLoop.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region MoveLOOP sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.moveLoopSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[1] = UIManager.GetIndexForElementSound(GetUIElement.moveLoopSoundAtFinish);
                loopAnimationSoundIndex[1] = EditorGUILayout.Popup(loopAnimationSoundIndex[1], elementSounds, GUILayout.Width(140));
                GetUIElement.moveLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[1]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveLoopSoundAtFinish = GetUIElement.moveLoop.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region RotationLoop
            tex = DoozyUIResources.LabelRotateLoopDisabled;
            if (sp_rotationLoop_enabled.boolValue)
                tex = DoozyUIResources.LabelRotateLoopEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_rotationLoop_enabled.boolValue = !sp_rotationLoop_enabled.boolValue;
            }
            if (sp_rotationLoop_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_rotationLoop_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_rotationLoop_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_rotationLoop_time.floatValue = EditorGUILayout.FloatField(sp_rotationLoop_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_rotationLoop_delay.floatValue = EditorGUILayout.FloatField(sp_rotationLoop_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                rotationLoopEaseType = GetUIElement.rotationLoop.easeType;
                rotationLoopEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(rotationLoopEaseType, GUILayout.Width(140));
                sp_rotationLoop_easeType.enumValueIndex = (int)rotationLoopEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_rotationLoop_autoStart.boolValue = EditorGUILayout.ToggleLeft("auto start", sp_rotationLoop_autoStart.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("rotation", GUILayout.Width(50));
                sp_rotationLoop_rotation.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_rotationLoop_rotation.vector3Value, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                sp_rotationLoop_loops.intValue = EditorGUILayout.IntField(sp_rotationLoop_loops.intValue, GUILayout.Width(93));
                EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                rotationLoopLoopType = GetUIElement.rotationLoop.loopType;
                rotationLoopLoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(rotationLoopLoopType, GUILayout.Width(138));
                sp_rotationLoop_loopType.enumValueIndex = (int)rotationLoopLoopType;
                EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                switch (rotationLoopLoopType)
                {
                    case DG.Tweening.LoopType.Yoyo:
                        EditorGUILayout.HelpBox("LoopType.Yoyo - the tween moves forward and backwards at alternate cycles", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Restart:
                        EditorGUILayout.HelpBox("LoopType.Restart - each loop cycle restarts from the beginning", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Incremental:
                        EditorGUILayout.HelpBox("LoopType.Incremental - continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving 'onward'", MessageType.None);
                        break;
                }

                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region RotateLOOP Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationLoopSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[2] = UIManager.GetIndexForElementSound(GetUIElement.rotationLoopSoundAtStart);
                loopAnimationSoundIndex[2] = EditorGUILayout.Popup(loopAnimationSoundIndex[2], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[2]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationLoopSoundAtStart = GetUIElement.rotationLoop.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region RotateLOOP sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationLoopSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[3] = UIManager.GetIndexForElementSound(GetUIElement.rotationLoopSoundAtFinish);
                loopAnimationSoundIndex[3] = EditorGUILayout.Popup(loopAnimationSoundIndex[3], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[3]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationLoopSoundAtFinish = GetUIElement.rotationLoop.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region ScaleLoop
            tex = DoozyUIResources.LabelScaleLoopDisabled;
            if (sp_scaleLoop_enabled.boolValue)
                tex = DoozyUIResources.LabelScaleLoopEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_scaleLoop_enabled.boolValue = !sp_scaleLoop_enabled.boolValue;
            }
            if (sp_scaleLoop_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_scaleLoop_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_scaleLoop_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_scaleLoop_time.floatValue = EditorGUILayout.FloatField(sp_scaleLoop_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_scaleLoop_delay.floatValue = EditorGUILayout.FloatField(sp_scaleLoop_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                scaleLoopEaseType = GetUIElement.scaleLoop.easeType;
                scaleLoopEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(scaleLoopEaseType, GUILayout.Width(140));
                sp_scaleLoop_easeType.enumValueIndex = (int)scaleLoopEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_scaleLoop_autoStart.boolValue = EditorGUILayout.ToggleLeft("auto start", sp_scaleLoop_autoStart.boolValue, GUILayout.Width(80));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("min", GUILayout.Width(30));
                sp_scaleLoop_min.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_scaleLoop_min.vector3Value, GUILayout.Width(150));
#if dUI_TextMeshPro
                sp_scaleLoop_min.vector3Value = new Vector3(sp_scaleLoop_min.vector3Value.x, sp_scaleLoop_min.vector3Value.y, 1);
#endif
                EditorGUILayout.LabelField("max", GUILayout.Width(30));
                sp_scaleLoop_max.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_scaleLoop_max.vector3Value, GUILayout.Width(150));
#if dUI_TextMeshPro
                sp_scaleLoop_max.vector3Value = new Vector3(sp_scaleLoop_max.vector3Value.x, sp_scaleLoop_max.vector3Value.y, 1);
#endif
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                sp_scaleLoop_loops.intValue = EditorGUILayout.IntField(sp_scaleLoop_loops.intValue, GUILayout.Width(93));
                EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                scaleLoopLoopType = GetUIElement.scaleLoop.loopType;
                scaleLoopLoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(scaleLoopLoopType, GUILayout.Width(138));
                sp_scaleLoop_loopType.enumValueIndex = (int)scaleLoopLoopType;
                EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                switch (scaleLoopLoopType)
                {
                    case DG.Tweening.LoopType.Yoyo:
                        EditorGUILayout.HelpBox("LoopType.Yoyo - the tween moves forward and backwards at alternate cycles", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Restart:
                        EditorGUILayout.HelpBox("LoopType.Restart - each loop cycle restarts from the beginning", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Incremental:
                        EditorGUILayout.HelpBox("LoopType.Incremental - continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving 'onward'", MessageType.None);
                        break;
                }

                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region ScaleLOOP Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleLoopSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[4] = UIManager.GetIndexForElementSound(GetUIElement.scaleLoopSoundAtStart);
                loopAnimationSoundIndex[4] = EditorGUILayout.Popup(loopAnimationSoundIndex[4], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[4]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleLoopSoundAtStart = GetUIElement.scaleLoop.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region ScaleLOOP sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleLoopSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[5] = UIManager.GetIndexForElementSound(GetUIElement.scaleLoopSoundAtFinish);
                loopAnimationSoundIndex[5] = EditorGUILayout.Popup(loopAnimationSoundIndex[5], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[5]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleLoopSoundAtFinish = GetUIElement.scaleLoop.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region FadeLoop
            tex = DoozyUIResources.LabelFadeLoopDisabled;
            if (sp_fadeLoop_enabled.boolValue)
                tex = DoozyUIResources.LabelFadeLoopEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_fadeLoop_enabled.boolValue = !sp_fadeLoop_enabled.boolValue;
            }
            if (sp_fadeLoop_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Fade);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_fadeLoop_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_fadeLoop_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_fadeLoop_time.floatValue = EditorGUILayout.FloatField(sp_fadeLoop_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_fadeLoop_delay.floatValue = EditorGUILayout.FloatField(sp_fadeLoop_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                fadeLoopEaseType = GetUIElement.fadeLoop.easeType;
                fadeLoopEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(fadeLoopEaseType, GUILayout.Width(140));
                sp_fadeLoop_easeType.enumValueIndex = (int)fadeLoopEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_fadeLoop_autoStart.boolValue = EditorGUILayout.ToggleLeft("auto start", sp_fadeLoop_autoStart.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("min", GUILayout.Width(30));
                sp_fadeLoop_min.floatValue = EditorGUILayout.FloatField(sp_fadeLoop_min.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("max", GUILayout.Width(30));
                sp_fadeLoop_max.floatValue = EditorGUILayout.FloatField(sp_fadeLoop_max.floatValue, GUILayout.Width(40));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("number of loops", GUILayout.Width(105));
                sp_fadeLoop_loops.intValue = EditorGUILayout.IntField(sp_fadeLoop_loops.intValue, GUILayout.Width(93));
                EditorGUILayout.HelpBox("number of cycles to play (-1 for infinite)", MessageType.None);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("loop type", GUILayout.Width(60));
                fadeLoopLoopType = GetUIElement.fadeLoop.loopType;
                fadeLoopLoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup(fadeLoopLoopType, GUILayout.Width(138));
                sp_fadeLoop_loopType.enumValueIndex = (int)fadeLoopLoopType;
                EditorGUILayout.HelpBox("default: LoopType.Yoyo", MessageType.None, true);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                switch (fadeLoopLoopType)
                {
                    case DG.Tweening.LoopType.Yoyo:
                        EditorGUILayout.HelpBox("LoopType.Yoyo - the tween moves forward and backwards at alternate cycles", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Restart:
                        EditorGUILayout.HelpBox("LoopType.Restart - each loop cycle restarts from the beginning", MessageType.None);
                        break;

                    case DG.Tweening.LoopType.Incremental:
                        EditorGUILayout.HelpBox("LoopType.Incremental - continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving 'onward'", MessageType.None);
                        break;
                }

                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region FadeLOOP Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeLoopSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[6] = UIManager.GetIndexForElementSound(GetUIElement.fadeLoopSoundAtStart);
                loopAnimationSoundIndex[6] = EditorGUILayout.Popup(loopAnimationSoundIndex[6], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[6]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeLoopSoundAtStart = GetUIElement.fadeLoop.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region FadeLOOP sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeLoopSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                loopAnimationSoundIndex[7] = UIManager.GetIndexForElementSound(GetUIElement.fadeLoopSoundAtFinish);
                loopAnimationSoundIndex[7] = EditorGUILayout.Popup(loopAnimationSoundIndex[7], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[loopAnimationSoundIndex[7]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeLoopSoundAtFinish = GetUIElement.fadeLoop.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion
        }
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region OutAnimations
        EditorGUILayout.BeginHorizontal();

        tex = DoozyUIResources.LabelOutAnimationsDisabled;
        if (sp_useOutAnimations.boolValue)
        {
            tex = DoozyUIResources.LabelOutAnimations;
        }
        if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Width(198), GUILayout.Height(30)))
        {
            //Toggle visibility of the OutAnimations Zone
            sp_useOutAnimations.boolValue = !sp_useOutAnimations.boolValue;
        }

        #region OutAnimations - PRESETS

        if (sp_useOutAnimations.boolValue)
        {
            EditorGUILayout.BeginVertical();
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
            if (saveOutAnimationsPreset == false && deleteOutAnimationsPreset == false)
            {
                sp_activeOutAnimationsPresetIndex.intValue = EditorGUILayout.Popup(sp_activeOutAnimationsPresetIndex.intValue, outAnimationPresets, GUILayout.Width(214));

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("load", GUILayout.Width(69), GUILayout.Height(16)))
                {
                    GetAnimationManager.LoadPreset(outAnimationPresets[sp_activeOutAnimationsPresetIndex.intValue], UIAnimationManager.AnimationType.OUT);
                    OnInspectorGUI();
                }

                if (GUILayout.Button("save", GUILayout.Width(69), GUILayout.Height(16)))
                {
                    saveOutAnimationsPreset = !saveOutAnimationsPreset;
                    newOutAnimationsPresetName = string.Empty;
                }

                if (GUILayout.Button("delete", GUILayout.Width(68), GUILayout.Height(16)))
                {
                    deleteOutAnimationsPreset = true;
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (saveOutAnimationsPreset == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Preset Name", GUILayout.Width(80));
                newOutAnimationsPresetName = EditorGUILayout.TextField(newOutAnimationsPresetName, GUILayout.Width(130));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("save", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    GetAnimationManager.SavePreset(newOutAnimationsPresetName, UIAnimationManager.AnimationType.OUT);
                    UpdateAnimationPresetsFromFiles();
                    saveOutAnimationsPreset = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    saveOutAnimationsPreset = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (deleteOutAnimationsPreset == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Delete Preset '" + outAnimationPresets[GetUIElement.activeOutAnimationsPresetIndex] + "' ?", GUILayout.Width(210));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("yes", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    GetAnimationManager.DeletePreset(outAnimationPresets[GetUIElement.activeOutAnimationsPresetIndex], UIAnimationManager.AnimationType.OUT);
                    UpdateAnimationPresetsFromFiles();
                    deleteOutAnimationsPreset = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                if (GUILayout.Button("no", GUILayout.Width(105), GUILayout.Height(16)))
                {
                    deleteOutAnimationsPreset = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.EndHorizontal();

        if (GetUIElement.showHelp && sp_useOutAnimations.boolValue)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Here you can select, load, save and delete any preset for OUT animations.", MessageType.None, true);
            EditorGUILayout.HelpBox("The OUT animations presets can be found in .xml format in DoozyUI/Presets/UIAnimations/OUT", MessageType.None);
            EditorGUILayout.HelpBox("All the animations can be reused in other projects by copying the .xml files", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.ResetColors();
        #endregion

        if (sp_useOutAnimations.boolValue)
        {
            DoozyUIHelper.VerticalSpace(4);

            #region MoveOut
            tex = DoozyUIResources.LabelMoveOutDisabled;
            if (sp_moveOut_enabled.boolValue)
                tex = DoozyUIResources.LabelMoveOutEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_moveOut_enabled.boolValue = !sp_moveOut_enabled.boolValue;
            }
            if (sp_moveOut_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Move);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_moveOut_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_moveOut_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_moveOut_time.floatValue = EditorGUILayout.FloatField(sp_moveOut_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_moveOut_delay.floatValue = EditorGUILayout.FloatField(sp_moveOut_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                moveOutEaseType = GetUIElement.moveOut.easeType;
                moveOutEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(moveOutEaseType, GUILayout.Width(140));
                sp_moveOut_easeType.enumValueIndex = (int)moveOutEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("to", GUILayout.Width(30));
                moveTo = GetUIElement.moveOut.moveTo;
                moveTo = (UIAnimator.MoveDetails)EditorGUILayout.EnumPopup(moveTo, GUILayout.Width(130));
                sp_moveOut_moveTo.enumValueIndex = (int)moveTo;
                EditorGUILayout.LabelField("adjust position", GUILayout.Width(90));
                sp_moveOut_positionAdjustment.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_moveOut_positionAdjustment.vector3Value, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                if (moveTo == UIAnimator.MoveDetails.LocalPosition)
                {
                    EditorGUILayout.LabelField("to localposition", GUILayout.Width(120));
                    sp_moveOut_positionTo.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_moveOut_positionTo.vector3Value, GUILayout.Width(150));
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region MoveOUT Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.moveOutSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[0] = UIManager.GetIndexForElementSound(GetUIElement.moveOutSoundAtStart);
                outAnimationSoundIndex[0] = EditorGUILayout.Popup(outAnimationSoundIndex[0], elementSounds, GUILayout.Width(140));
                GetUIElement.moveOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[0]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveOutSoundAtStart = GetUIElement.moveOut.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region MoveOUT sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.moveOutSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[1] = UIManager.GetIndexForElementSound(GetUIElement.moveOutSoundAtFinish);
                outAnimationSoundIndex[1] = EditorGUILayout.Popup(outAnimationSoundIndex[1], elementSounds, GUILayout.Width(140));
                GetUIElement.moveOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[1]]; //we reference the class not the value (we need a reference)
                GetUIElement.moveOutSoundAtFinish = GetUIElement.moveOut.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region RotationOut
            tex = DoozyUIResources.LabelRotateOutDisabled;
            if (sp_rotationOut_enabled.boolValue)
                tex = DoozyUIResources.LabelRotateOutEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_rotationOut_enabled.boolValue = !sp_rotationOut_enabled.boolValue;
            }
            if (sp_rotationOut_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Rotate);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_rotationOut_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_rotationOut_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_rotationOut_time.floatValue = EditorGUILayout.FloatField(sp_rotationOut_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_rotationOut_delay.floatValue = EditorGUILayout.FloatField(sp_rotationOut_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                rotationOutEaseType = GetUIElement.rotationOut.easeType;
                rotationOutEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(rotationOutEaseType, GUILayout.Width(140));
                sp_rotationOut_easeType.enumValueIndex = (int)rotationOutEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("to", GUILayout.Width(30));
                sp_rotationOut_rotateTo.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_rotationOut_rotateTo.vector3Value, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region RotateOUT Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationOutSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[2] = UIManager.GetIndexForElementSound(GetUIElement.rotationOutSoundAtStart);
                outAnimationSoundIndex[2] = EditorGUILayout.Popup(outAnimationSoundIndex[2], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[2]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationOutSoundAtStart = GetUIElement.rotationOut.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region RotateOUT sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.rotationOutSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[3] = UIManager.GetIndexForElementSound(GetUIElement.rotationOutSoundAtFinish);
                outAnimationSoundIndex[3] = EditorGUILayout.Popup(outAnimationSoundIndex[3], elementSounds, GUILayout.Width(140));
                GetUIElement.rotationOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[3]]; //we reference the class not the value (we need a reference)
                GetUIElement.rotationOutSoundAtFinish = GetUIElement.rotationOut.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region ScaleOut
            tex = DoozyUIResources.LabelScaleOutDisabled;
            if (sp_scaleOut_enabled.boolValue)
                tex = DoozyUIResources.LabelScaleOutEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_scaleOut_enabled.boolValue = !sp_scaleOut_enabled.boolValue;
            }
            if (sp_scaleOut_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Scale);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_scaleOut_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_scaleOut_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_scaleOut_time.floatValue = EditorGUILayout.FloatField(sp_scaleOut_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_scaleOut_delay.floatValue = EditorGUILayout.FloatField(sp_scaleOut_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                scaleOutEaseType = GetUIElement.scaleOut.easeType;
                scaleOutEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(scaleOutEaseType, GUILayout.Width(140));
                sp_scaleOut_easeType.enumValueIndex = (int)scaleOutEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("to", GUILayout.Width(30));
                sp_scaleOut_scaleEnd.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, sp_scaleOut_scaleEnd.vector3Value, GUILayout.Width(150));
#if dUI_TextMeshPro
                sp_scaleOut_scaleEnd.vector3Value = new Vector3(sp_scaleOut_scaleEnd.vector3Value.x, sp_scaleOut_scaleEnd.vector3Value.y, 1);
#endif
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region ScaleOUT Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleOutSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[4] = UIManager.GetIndexForElementSound(GetUIElement.scaleOutSoundAtStart);
                outAnimationSoundIndex[4] = EditorGUILayout.Popup(outAnimationSoundIndex[4], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[4]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleOutSoundAtStart = GetUIElement.scaleOut.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region ScaleOUT sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.scaleOutSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[5] = UIManager.GetIndexForElementSound(GetUIElement.scaleOutSoundAtFinish);
                outAnimationSoundIndex[5] = EditorGUILayout.Popup(outAnimationSoundIndex[5], elementSounds, GUILayout.Width(140));
                GetUIElement.scaleOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[5]]; //we reference the class not the value (we need a reference)
                GetUIElement.scaleOutSoundAtFinish = GetUIElement.scaleOut.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region FadeOut
            tex = DoozyUIResources.LabelFadeOutDisabled;
            if (sp_fadeOut_enabled.boolValue)
                tex = DoozyUIResources.LabelFadeOutEnabled;
            if (GUILayout.Button(tex, GUIStyle.none, GUILayout.Height(14)))
            {
                sp_fadeOut_enabled.boolValue = !sp_fadeOut_enabled.boolValue;
            }
            if (sp_fadeOut_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Fade);
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                sp_fadeOut_enabled.boolValue = EditorGUILayout.ToggleLeft("enabled", sp_fadeOut_enabled.boolValue, GUILayout.Width(80));
                EditorGUILayout.LabelField("time", GUILayout.Width(28));
                sp_fadeOut_time.floatValue = EditorGUILayout.FloatField(sp_fadeOut_time.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("delay", GUILayout.Width(34));
                sp_fadeOut_delay.floatValue = EditorGUILayout.FloatField(sp_fadeOut_delay.floatValue, GUILayout.Width(40));
                EditorGUILayout.LabelField("ease", GUILayout.Width(30));
                fadeOutEaseType = GetUIElement.fadeOut.easeType;
                fadeOutEaseType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(fadeOutEaseType, GUILayout.Width(140));
                sp_fadeOut_easeType.enumValueIndex = (int)fadeOutEaseType;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();

                #region FadeOUT Sound at START
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeOutSoundAtStart) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[6] = UIManager.GetIndexForElementSound(GetUIElement.fadeOutSoundAtStart);
                outAnimationSoundIndex[6] = EditorGUILayout.Popup(outAnimationSoundIndex[6], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[6]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeOutSoundAtStart = GetUIElement.fadeOut.soundAtStartReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@START", GUILayout.Width(60));
                #endregion

                GUILayout.Space(10);

                #region FadeOUT sound at FINISH
                if (UIManager.GetIndexForElementSound(GetUIElement.fadeOutSoundAtFinish) == -1)
                {
                    DoozyUIRedundancyCheck.UIElementRedundancyCheck(GetUIElement);
                }
                outAnimationSoundIndex[7] = UIManager.GetIndexForElementSound(GetUIElement.fadeOutSoundAtFinish);
                outAnimationSoundIndex[7] = EditorGUILayout.Popup(outAnimationSoundIndex[7], elementSounds, GUILayout.Width(140));
                GetUIElement.fadeOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[outAnimationSoundIndex[7]]; //we reference the class not the value (we need a reference)
                GetUIElement.fadeOutSoundAtFinish = GetUIElement.fadeOut.soundAtFinishReference.soundName; //we save the backup
                EditorGUILayout.LabelField("@FINISH", GUILayout.Width(60));
                #endregion

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.ResetColors();

                DoozyUIHelper.VerticalSpace(8);
            }
            #endregion

            DoozyUIHelper.VerticalSpace(2);

            #region Events
            if (sp_moveOut_enabled.boolValue || sp_rotationOut_enabled.boolValue || sp_scaleOut_enabled.boolValue || sp_fadeOut_enabled.boolValue)
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                EditorGUILayout.BeginHorizontal();
                sp_useOutAnimationsStartEvents.boolValue = EditorGUILayout.ToggleLeft("OUT Animations Events @START", sp_useOutAnimationsStartEvents.boolValue, GUILayout.Width(205));
                sp_useOutAnimationsFinishEvents.boolValue = EditorGUILayout.ToggleLeft("OUT Animations Events @FINISH", sp_useOutAnimationsFinishEvents.boolValue, GUILayout.Width(205));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (GetUIElement.showHelp)
                {
                    DoozyUIHelper.ResetColors();
                    DoozyUIHelper.VerticalSpace(1);
                    EditorGUILayout.HelpBox("If you want to trigger anything when the OUT Animations START or FINISH you can do it here, using the native UnityEvent system", MessageType.None);
                    EditorGUILayout.HelpBox("All the triggers take into account the delay times of all the OUT Animations on this UIElement", MessageType.None);
                    EditorGUILayout.HelpBox("The events @START, are triggered after MIN (delay)", MessageType.None);
                    EditorGUILayout.HelpBox("The events @FINISH, are triggered after MAX (delay + time)", MessageType.None);
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
                }

                if (sp_useOutAnimationsStartEvents.boolValue)
                {
                    EditorGUILayout.PropertyField(sp_onOutAnimationsStart, GUILayout.Width(416));
                }
                if (sp_useOutAnimationsFinishEvents.boolValue)
                {
                    EditorGUILayout.PropertyField(sp_onOutAnimationsFinish, GUILayout.Width(416));
                }
                DoozyUIHelper.ResetColors();
            }
            else
            {
                sp_useOutAnimationsStartEvents.boolValue = false;
                sp_useOutAnimationsFinishEvents.boolValue = false;
            }
            #endregion
        }
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        if (GetUIElement.showHelp)
        {
            DoozyUIHelper.ResetColors();
            DoozyUIHelper.DrawTexture(DoozyUIResources.BarGeneralInfo);
            DoozyUIHelper.VerticalSpace(1);
            EditorGUILayout.HelpBox("To load a preset: select it from the dropdown list and then press load", MessageType.None);
            EditorGUILayout.HelpBox("To save a preset: press save, enter the preset name and press save again (if you enter the name of an existing preset, it will be overwritten)", MessageType.None);
            EditorGUILayout.HelpBox("TIP: You can change, test and then save any IN or OUT animation settings, as a preset, while in Play Mode. After exiting Play Mode, just load the saved preset and you are good to go.", MessageType.None);
            EditorGUILayout.HelpBox("To delete a preset: select it from the dropdown list and then press delete", MessageType.None);
            EditorGUILayout.HelpBox("sound at start: the sound filename (without extension) that will play when the animation starts", MessageType.None);
            EditorGUILayout.HelpBox("sound at finish: the sound filename (without extension) that will play when the animation ended", MessageType.None);
            EditorGUILayout.HelpBox("The sound file has to be in a 'Resources' folder (for example: Sounds/UI/Resources/)", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}

