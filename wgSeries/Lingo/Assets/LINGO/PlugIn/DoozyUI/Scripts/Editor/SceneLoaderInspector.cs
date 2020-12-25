// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DoozyUI;

[CustomEditor(typeof(DoozyUI.SceneLoader), true)]
public class SceneLoaderInspector : Editor
{
    #region SerializedProperties
    SerializedProperty sp_command_LoadSceneAsync_SceneName;
    SerializedProperty sp_command_LoadSceneAsync_SceneBuildIndex;
    SerializedProperty sp_command_LoadSceneAdditiveAsync_SceneName;
    SerializedProperty sp_command_LoadSceneAdditiveAsync_SceneBuildIndex;
    SerializedProperty sp_command_LoadLevel;
    SerializedProperty sp_levelSceneName;

#if (UNITY_5_1 == false && UNITY_5_2 == false)
    SerializedProperty sp_command_UnloadScene_SceneName;
    SerializedProperty sp_command_UnloadScene_SceneBuildIndex;
    SerializedProperty sp_command_UnloadLevel;
#endif
    #endregion

    #region Update Serialized Properties
    void UpdateSerializedProperties()
    {
        sp_command_LoadSceneAsync_SceneName = serializedObject.FindProperty("command_LoadSceneAsync_SceneName");
        sp_command_LoadSceneAsync_SceneBuildIndex = serializedObject.FindProperty("command_LoadSceneAsync_SceneBuildIndex");
        sp_command_LoadSceneAdditiveAsync_SceneName = serializedObject.FindProperty("command_LoadSceneAdditiveAsync_SceneName");
        sp_command_LoadSceneAdditiveAsync_SceneBuildIndex = serializedObject.FindProperty("command_LoadSceneAdditiveAsync_SceneBuildIndex");
        sp_command_LoadLevel = serializedObject.FindProperty("command_LoadLevel");
        sp_levelSceneName = serializedObject.FindProperty("levelSceneName");

#if (UNITY_5_1 == false && UNITY_5_2 == false)
        sp_command_UnloadScene_SceneName = serializedObject.FindProperty("command_UnloadScene_SceneName");
        sp_command_UnloadScene_SceneBuildIndex = serializedObject.FindProperty("command_UnloadScene_SceneBuildIndex");
        sp_command_UnloadLevel = serializedObject.FindProperty("command_UnloadLevel");
#endif
    }
    #endregion

    #region Variables
    DoozyUI.SceneLoader sceneLoader;
    string unityVersion;

#if dUI_EnergyBarToolkit
    List<EnergyBar> iEnergyBars;
#endif

    #endregion

    #region Properties

#if dUI_EnergyBarToolkit
    List<EnergyBar> GetEnergyBars
    {
        get
        {
            if(iEnergyBars == null)
            {
                iEnergyBars = new List<EnergyBar>();
            }

            return iEnergyBars;
        }

        set
        {
            if(iEnergyBars == null)
            {
                iEnergyBars = new List<EnergyBar>();
            }

            iEnergyBars = value;
        }
    }
#endif

    #endregion

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    void OnEnable()
    {
        sceneLoader = (DoozyUI.SceneLoader)target;
        unityVersion = Application.unityVersion;

#if dUI_EnergyBarToolkit
        if(sceneLoader.energyBars == null)
        {
            sceneLoader.energyBars = new List<EnergyBar>();
        }
        GetEnergyBars = sceneLoader.energyBars;
#endif
    }

    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();

        if (sceneLoader == null)
            sceneLoader = (DoozyUI.SceneLoader)target;

        UpdateSerializedProperties();

        serializedObject.Update();

        DoozyUIHelper.VerticalSpace(8);

        #region Header
        DoozyUIHelper.DrawTexture(DoozyUIResources.BarSceneLoader);
        #endregion

        #region InfoBox
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);

        EditorGUILayout.HelpBox("Here you can customize the game event commands that trigger different methods for scene loading.", MessageType.Info);

#if (UNITY_5_1 || UNITY_5_2)
        EditorGUILayout.HelpBox("Your Unity version is [" + unityVersion + "] and for scene loading Application.LoadLevel methods will be used.", MessageType.None);
#endif

#if (UNITY_5_1 == false && UNITY_5_2 == false)
        EditorGUILayout.HelpBox("Your Unity version is [" + unityVersion + "] and for scene loading SceneManager.LoadSceneAsync methods will be used.", MessageType.None);
#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region ShowHelp
        DoozyUIHelper.ResetColors();
        sceneLoader.showHelp = EditorGUILayout.ToggleLeft("Show Help", sceneLoader.showHelp, GUILayout.Width(160));
        DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region EnergyBars
#if dUI_EnergyBarToolkit
        EditorGUILayout.BeginHorizontal();
        DoozyUIHelper.DrawTexture(DoozyUIResources.LogoEBT);
        GUILayout.Space(4);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add bar", GUILayout.Width(366)))
        {
            GetEnergyBars.Add(new EnergyBar());
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (GetEnergyBars.Count > 0)
        {
            for (int i = 0; i < GetEnergyBars.Count; i++)
            {
                DoozyUIHelper.VerticalSpace(2);
                EditorGUILayout.BeginHorizontal();
                GetEnergyBars[i] = EditorGUILayout.ObjectField(GetEnergyBars[i], typeof(EnergyBar), true, GUILayout.Width(340)) as EnergyBar;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(12)))
                {
                    GetEnergyBars.RemoveAt(i);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            sceneLoader.energyBars = GetEnergyBars;
        }
        else
        {
            DoozyUIHelper.VerticalSpace(20);
        }
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("You should link the EnergyBars you want to update on when you load a scene. They will show the load progress of that scene.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

#endif
        #endregion

        DoozyUIHelper.VerticalSpace(8);

        #region Interface for Unity 5.1 and Unity 5.2
#if (UNITY_5_1 || UNITY_5_2)
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel Async by Name", GUILayout.Width(210));
        sp_command_LoadSceneAsync_SceneName.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAsync_SceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the scene named 'MySceneName_5' you need to send a game event with the command 'LoadSceneAsync_Name_MySceneName_5', where 'LoadSceneAsync_Name_' is the command and 'MySceneName_5' is the name of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel Async by ID", GUILayout.Width(210));
        sp_command_LoadSceneAsync_SceneBuildIndex.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAsync_SceneBuildIndex.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the 5th scene in your build index you need to send a game event with the command 'LoadSceneAsync_ID_5', where 'LoadSceneAsync_ID_' is the command and '5' is the name of the index number of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel Additive Async by Name", GUILayout.Width(210));
        sp_command_LoadSceneAdditiveAsync_SceneName.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAdditiveAsync_SceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the scene named 'MySceneName_5' you need to send a game event with the command 'LoadSceneAdditiveAsync_Name_MySceneName_5', where 'LoadSceneAdditiveAsync_Name_' is the command and 'MySceneName_5' is the name of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel Additive Async by ID", GUILayout.Width(210));
        sp_command_LoadSceneAdditiveAsync_SceneBuildIndex.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAdditiveAsync_SceneBuildIndex.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the 5th scene in your build index you need to send a game event with the command 'LoadSceneAdditiveAsync_ID_5', where 'LoadSceneAdditiveAsync_ID_' is the command and '5' is the build index number of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel shortcut command", GUILayout.Width(210));
        sp_command_LoadLevel.stringValue = EditorGUILayout.TextField(sp_command_LoadLevel.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load level 5 you need to send a game event with the command 'LoadLevel_5', where 'LoadLevel_' is the shortcut command and '5' is the level you want to load.", MessageType.None);
            EditorGUILayout.HelpBox("This will load the level by using the LoadLevelAdditiveAsync method.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Scene Name", GUILayout.Width(210));
        sp_levelSceneName.stringValue = EditorGUILayout.TextField(sp_levelSceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("This is the name of your level scenes in build. Example: 'Level_1', 'Level_2' ... 'Level_100'", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }
#endif
        #endregion

        #region Interface for Unity 5.3 and up
#if (UNITY_5_1 == false && UNITY_5_2 == false)
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadScene Async by Name", GUILayout.Width(210));
        sp_command_LoadSceneAsync_SceneName.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAsync_SceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the scene named 'MySceneName_5' you need to send a game event with the command 'LoadSceneAsync_Name_MySceneName_5', where 'LoadSceneAsync_Name_' is the first part of the command and 'MySceneName_5' is the name of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadScene Async by ID", GUILayout.Width(210));
        sp_command_LoadSceneAsync_SceneBuildIndex.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAsync_SceneBuildIndex.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the 5th scene in your build index you need to send a game event with the command LoadSceneAsync_ID_5', where 'LoadSceneAsync_ID_' is the first part of the command and '5' is the build index number of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadScene Additive Async by Name", GUILayout.Width(210));
        sp_command_LoadSceneAdditiveAsync_SceneName.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAdditiveAsync_SceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the scene named 'MySceneName_5' you need to send a game event with the command 'LoadSceneAdditiveAsync_Name_MySceneName_5', where 'LoadSceneAdditiveAsync_Name_' is the first part of the comman and 'MySceneName_5' is the name of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadScene Additive Async by ID", GUILayout.Width(210));
        sp_command_LoadSceneAdditiveAsync_SceneBuildIndex.stringValue = EditorGUILayout.TextField(sp_command_LoadSceneAdditiveAsync_SceneBuildIndex.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load the 5th scene in your build index you need to send a game event with the command 'LoadSceneAdditiveAsync_ID_5', where 'LoadSceneAdditiveAsync_ID_' is the first part of the command and '5' is the build index number of the scene you want to load.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UnloadScene by Name", GUILayout.Width(210));
        sp_command_UnloadScene_SceneName.stringValue = EditorGUILayout.TextField(sp_command_UnloadScene_SceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo unload a scene named 'MySceneName_5' you need to send a game event with the command 'UnloadScene_Name_MyScene_5', where 'UnloadScene_Name_' is the first part of the command and 'MySceneName_5' is the name of the scene you want to unload.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UnloadScene by ID", GUILayout.Width(210));
        sp_command_UnloadScene_SceneBuildIndex.stringValue = EditorGUILayout.TextField(sp_command_UnloadScene_SceneBuildIndex.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo unload the 5th scene in your build index you need to send a game event with the command 'UnloadScene_ID_5', where 'UnloadScene_ID_' is the first part of the command and '5' is the build index number of the scene you want to unload.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LoadLevel shortcut command", GUILayout.Width(210));
        sp_command_LoadLevel.stringValue = EditorGUILayout.TextField(sp_command_LoadLevel.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo load level 5 you need to send a game event with the command 'LoadLevel_5', where 'LoadLevel_' is the shortcut command and '5' is the level you want to load.", MessageType.None);
            EditorGUILayout.HelpBox("This will load the level by using the SceneManager.LoadSceneAsync method with LoadSceneMode.Additive option.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UnloadLevel shortcut command", GUILayout.Width(210));
        sp_command_UnloadLevel.stringValue = EditorGUILayout.TextField(sp_command_UnloadLevel.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("Usage example: \nTo unload level 5 you need to send a game event with the command 'UnloadLevel_5', where 'UnloadLevel_' is the shortcut command and '5' is the level you want to unload.", MessageType.None);
            EditorGUILayout.HelpBox("This will unload the level by using the SceneManager.UnloadScene method.", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

        DoozyUIHelper.VerticalSpace(8);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Scene Name", GUILayout.Width(210));
        sp_levelSceneName.stringValue = EditorGUILayout.TextField(sp_levelSceneName.stringValue, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (sceneLoader.showHelp)
        {
            DoozyUIHelper.ResetColors();
            EditorGUILayout.HelpBox("This is the name for your level scenes in build. Example: 'Level_1', 'Level_2' ... 'Level_100'", MessageType.None);
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.Doozy);
        }

#endif
        #endregion

        DoozyUIHelper.ResetColors();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
