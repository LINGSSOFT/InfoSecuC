// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using UnityEditor;
using DoozyUI;
using System.Collections.Generic;

[CustomEditor(typeof(UIManager))]
public class UIManagerInspector : Editor
{
    #region SerializedProperties
    private SerializedProperty sp_autoDisableButtonClicks;

#if dUI_MasterAudio
    private SerializedProperty sp_useMasterAudio_PlaySoundAndForget;
    private SerializedProperty sp_useMasterAudio_FireCustomEvent;
#endif

#if dUI_TextMeshPro
    private SerializedProperty sp_useTextMeshPro;
#endif
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_autoDisableButtonClicks = serializedObject.FindProperty("_autoDisableButtonClicks");

#if dUI_MasterAudio
        sp_useMasterAudio_PlaySoundAndForget = serializedObject.FindProperty("useMasterAudio_PlaySoundAndForget");
        sp_useMasterAudio_FireCustomEvent = serializedObject.FindProperty("useMasterAudio_FireCustomEvent");
#endif

#if dUI_TextMeshPro
        sp_useTextMeshPro = serializedObject.FindProperty("useTextMeshPro");
#endif
    }
    #endregion

    #region Variables
    private UIManager uiManager;
    #endregion

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    void OnEnable()
    {
        uiManager = (UIManager)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        UpdateSerializedProperties();

        uiManager = target as UIManager;

        serializedObject.Update();

        DoozyUIHelper.VerticalSpace(8);

        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarUiManager);
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        if (EditorApplication.isCompiling)
        {
            EditorGUILayout.BeginHorizontal();
            DoozyUIHelper.DrawTexture(DoozyUIResources.MessageWaitCompile);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            DoozyUIHelper.VerticalSpace(158);

            Repaint();
            return;
        }

        EditorGUILayout.BeginHorizontal();

        #region Show Help
        DoozyUIHelper.ResetColors();
        uiManager.showHelp = EditorGUILayout.ToggleLeft("Show Help", uiManager.showHelp, GUILayout.Width(80));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        #region Debug Events
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        uiManager._debugEvents = EditorGUILayout.ToggleLeft("Debug Events", uiManager._debugEvents, GUILayout.Width(90));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        #region Debug Buttons
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        uiManager._debugButtons = EditorGUILayout.ToggleLeft("Debug Buttons", uiManager._debugButtons, GUILayout.Width(100));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        #region Debug Notifications
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
        uiManager._debugNotifications = EditorGUILayout.ToggleLeft("Debug Notifications", uiManager._debugNotifications, GUILayout.Width(130));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        DoozyUIHelper.VerticalSpace(8);

        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);

        sp_autoDisableButtonClicks.boolValue = EditorGUILayout.ToggleLeft("Auto disable Button Clicks when an UIElement is in trasition", sp_autoDisableButtonClicks.boolValue);

        DoozyUIHelper.VerticalSpace(8);

        #region MasterAudio Settings
#if dUI_MasterAudio
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarMaEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Disable support for MasterAudio?", "This will remove 'dUI_MasterAudio' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                RemoveScriptingDefineSymbol("dUI_MasterAudio");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If disabled, it removes 'dUI_MasterAudio' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        sp_useMasterAudio_PlaySoundAndForget.boolValue = EditorGUILayout.ToggleLeft("Use PlaySoundAndForget method", sp_useMasterAudio_PlaySoundAndForget.boolValue);
        sp_useMasterAudio_FireCustomEvent.boolValue = EditorGUILayout.ToggleLeft("Use FireCustomEvent method", sp_useMasterAudio_FireCustomEvent.boolValue);
        if (sp_useMasterAudio_PlaySoundAndForget.boolValue && sp_useMasterAudio_FireCustomEvent.boolValue)
        {
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
            EditorGUILayout.HelpBox("Use either PlaySoundAndForget or FireCustomEvent, but do not enable both of them at once.", MessageType.Error);
        }
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarMaDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Enable support for MasterAudio?", "Enable this only if you have MasterAudio already installed. This will add 'dUI_MasterAudio' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_MasterAudio");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it adds 'dUI_MasterAudio' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region TextMeshPro Settings
#if dUI_TextMeshPro
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarTmpEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Disable support for TextMeshPro?", "This will remove 'dUI_TextMeshPro' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                RemoveScriptingDefineSymbol("dUI_TextMeshPro");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If disabled, it removes 'dUI_TextMeshPro' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
        }
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        sp_useTextMeshPro.boolValue = EditorGUILayout.ToggleLeft("Use TextMeshPro in UINotifications", sp_useTextMeshPro.boolValue);
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarTmpDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Enable support for TextMeshPro?", "Enable this only if you have TextMeshPro already installed. This will add 'dUI_TextMeshPro' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_TextMeshPro");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it adds 'dUI_TextMeshPro' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region EnergyBarToolkit Settings
#if dUI_EnergyBarToolkit
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarEbtEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Disable support for EnergyBarToolkit?", "This will remove 'dUI_EnergyBarToolkit' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                RemoveScriptingDefineSymbol("dUI_EnergyBarToolkit");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If disabled, it removes 'dUI_EnergyBarToolkit' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarEbtDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Enable support for EnergyBarToolkit?", "Enable this only if you have EnergyBarToolkit already installed. This will add 'dUI_EnergyBarToolkit' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_EnergyBarToolkit");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it adds 'dUI_EnergyBarToolkit' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region Playmaker Settings
#if dUI_PlayMaker
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarPmEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Disable support for PlayMaker?", "This will remove 'dUI_PlayMaker' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                RemoveScriptingDefineSymbol("dUI_PlayMaker");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If disabled, it removes 'dUI_PlayMaker' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarPmDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Enable support for PlayMaker?", "Enable this only if you have PlayMaker already installed. This will add 'dUI_PlayMaker' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_PlayMaker");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it adds 'dUI_PlayMaker' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region Orientation Manager Settings
#if dUI_UseOrientationManager
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarOmEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
             if (EditorUtility.DisplayDialog("Disable the Orientation Manager?", "The LANDSCAPE and PORTRAIT options from UIElement components will no longer be available. This will remove 'dUI_UseOrientationManager' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_UseOrientationManager");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it removes 'dUI_NavigationDisabled' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarOmDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Enable the Orientation Manager?", "Enable this only if you want to create different UI's for each orientation. This will add 'dUI_UseOrientationManager' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_UseOrientationManager");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
#endif
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it adds 'dUI_UseOrientationManager' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
        }
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region Navigation System Settings
#if dUI_NavigationDisabled
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarNavDisabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonEnable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
             if (EditorUtility.DisplayDialog("Enable the Navigation Manager?", "This will add 'dUI_NavigationDisabled' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_NavigationDisabled");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If enabled, it removes 'dUI_NavigationDisabled' from PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#else
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarNavEnabled);
        GUILayout.Space(318);
        if (GUILayout.Button(DoozyUIResources.BarButtonDisable, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(32)))
        {
            if (EditorUtility.DisplayDialog("Disable the Navigation Manager?", "Do this if you intend to handle the navigation yourself (maybe use Playmaker to do it?). This will add 'dUI_NavigationDisabled' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                AddScriptingDefineSymbol("dUI_NavigationDisabled");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
#endif
        if (uiManager.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("If disabled, it adds 'dUI_NavigationDisabled' to PlayerSettings --> Scripting Define Symbols. It takes a few seconds to update.", MessageType.None);
            EditorGUILayout.HelpBox("Enable it if you want to handle the navigation yourself (the back button and navigation options). This is useful if you are using a FSM system like PlayMaker in order to have a visual control over the UI. This switch will also modify the UIButton component and disable several other options. (See the documentation for more details)", MessageType.None);
        }
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    #region Scripting Define Symbols Mehtods
    private static void AddScriptingDefineSymbol(string symbol)
    {
        string currentDefinedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        if (currentDefinedSymbols.Contains(symbol) == false)
        {
            currentDefinedSymbols += ";" + symbol;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinedSymbols);
        }
    }

    private static void RemoveScriptingDefineSymbol(string symbol)
    {
        string currentDefinedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        if (currentDefinedSymbols.Contains(symbol) == true)
        {
            currentDefinedSymbols = currentDefinedSymbols.Replace(symbol, "");

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinedSymbols);
        }
    }
    #endregion

}
