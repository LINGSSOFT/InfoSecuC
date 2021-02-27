// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DoozyUI
{
    [InitializeOnLoad]
    public class DoozyUIControlPanel : EditorWindow
    {
        #region Enums
        private enum Page
        {
            Main = 0,
            ElementNames = 1,
            ElementSounds = 2,
            ButtonNames = 3,
            ButtonSounds = 4,
            QuickHelp = 5,
            VideoTutorials = 6
        }
        #endregion

        #region Const
        private const float windowWidth = 600;
        private const float windowHeight = 600;
        private const float pageTop = 80;
        private const float pagePadding = 16;
        #endregion

        #region Private Variables
        private Page currentPage = Page.Main;
        private Page nextPage;

        private Rect currentPageRect;
        private Rect nextPageRect;
        private float currentPageMoveTo;

        private bool pageInTransition;
        private float transitionStartTime;
        private const float transitionDuration = 0.5f;

        UIElement[] uiElementsInTheScene;
        UIButton[] uiButtonsInTheScene;

        string[] elementNames;
        string[] elementSounds;
        string[] buttonNames;
        string[] buttonSounds;

        int elementNamesDeleteIndex = -1;
        int elementSoundsDeleteIndex = -1;
        int buttonNamesDeleteIndex = -1;
        int buttonSoundsDeleteIndex = -1;

        bool elementNamesNew = false;
        bool elementSoundsNew = false;
        bool buttonNamesNew = false;
        bool buttonSoundsNew = false;

        string elementNamesNewString = string.Empty;
        string elementSoundsNewString = string.Empty;
        string buttonNamesNewString = string.Empty;
        string buttonSoundsNewString = string.Empty;

        int elementNamesRenameIndex = -1;
        int elementSoundsRenameIndex = -1;
        int buttonNamesRenameIndex = -1;
        int buttonSoundsRenameIndex = -1;

        string elementNamesRenameString = string.Empty;
        string elementSoundsRenameString = string.Empty;
        string buttonNamesRenameString = string.Empty;
        string buttonSoundsRenameString = string.Empty;

        int elementNamesFinderIndex = 0;
        int elementSoundsFinderIndex = 0;
        int buttonNamesFinderIndex = 0;
        int buttonSoundsFinderIndex = 0;

        private Vector2 elementNamesScrollPosition = Vector2.zero;
        private Vector2 elementSoundsScrollPosition = Vector2.zero;
        private Vector2 buttonNamesScrollPosition = Vector2.zero;
        private Vector2 buttonSoundsScrollPosition = Vector2.zero;
        private Vector2 quickHelpScrollPosition = Vector2.zero;
        private Vector2 videoTutorialsScrollPosition = Vector2.zero;

        List<UIElement> elementNamesFilterList;
        List<UIElement> elementSoundsFilterList;
        List<UIButton> buttonNamesFilterList;
        List<UIButton> buttonSoundsFilterList;

        private Vector2 elementNamesFilterScrollPosition = Vector2.zero;
        private Vector2 elementSoundsFilterScrollPosition = Vector2.zero;
        private Vector2 buttonNamesFilterScrollPosition = Vector2.zero;
        private Vector2 buttonSoundsFilterScrollPosition = Vector2.zero;

        int numberOfFilteredButtonsPerRow = 3;

        bool resetElementNames = false;
        bool resetElementSounds = false;
        bool resetButtonNames = false;
        bool resetButtonSounds = false;
        #endregion

        [MenuItem("DoozyUI/Control Panel", false)]
        static void Init()
        {
            GetWindow<DoozyUIControlPanel>(true);
        }

        void OnEnable()
        {
            UIManager.InitDoozyUIData();

            UpdateViewableDatabaseArrays();

            UpdateUIElementsArray();
            UpdateUIButtonsArray();

            titleContent = new GUIContent("");

            maxSize = new Vector2(windowWidth, windowHeight);
            minSize = maxSize;

            // Setup pages
            currentPageRect = new Rect(0, pageTop, windowWidth, windowHeight - pagePadding);
            nextPageRect = new Rect(0, pageTop, windowWidth, windowHeight - pagePadding);

            currentPage = Page.Main;
            SetPage(currentPage);
        }

        public void OnGUI()
        {
            UpdateHeader();

            GUILayout.Space(-19);

            #region Check if application is compiling
            if (EditorApplication.isCompiling)
            {
                DoozyUIHelper.DrawTexture(DoozyUIResources.WindowMessageWaitCompile, 600, 535);

                Repaint();
                return;
            }
            #endregion

            #region Check if application is in play mode
            if (Application.isPlaying)
            {
                DoozyUIHelper.DrawTexture(DoozyUIResources.WindowWarningPlayMode, 600, 535);

                Repaint();
                return;
            }
            #endregion

            #region Check if DoozyUI is in the scene
            if (FindObjectOfType<UIManager>() == null)
            {
                if (GUILayout.Button(DoozyUIResources.WindowMessageAddDoozy, GUIStyle.none, GUILayout.Height(535), GUILayout.Width(600)))
                {
                    var go = UIManager.CreateDoozyUI();
                    Undo.RegisterCreatedObjectUndo(go, "Create DoozyUI prefab");
                    Selection.activeObject = go;
                    Repaint();
                }
            }
            #endregion
            else
            {
                #region Show Default Background
                DoozyUIHelper.DrawTexture(DoozyUIResources.WindowBackgroundLightGrey, 600, 535);
                #endregion

                UpdateViewableDatabaseArrays();

                DoPage(currentPage, currentPageRect);

                if (pageInTransition)
                {
                    DoPage(nextPage, nextPageRect);
                }

                Repaint();
            }

        }

        void Update()
        {
            if (pageInTransition)
            {
                DoPageTransition();
            }
        }

        #region Update viewable database arrays
        private void UpdateViewableDatabaseArrays()
        {
            elementNames = UIManager.GetElementNames();
            elementSounds = UIManager.GetElementSounds();
            buttonNames = UIManager.GetButtonNames();
            buttonSounds = UIManager.GetButtonSounds();
        }
        #endregion

        #region Scene related methods - UpdateUIElementsArray, GetUIElementNameCount, UpdateUIButtonsArray, GetUIButtonsNameCount

        #region UIElement - ElementNames & ElementSounds
        /// <summary>
        /// Gets an array of all the UIElements in the current scene
        /// </summary>
        void UpdateUIElementsArray()
        {
            uiElementsInTheScene = FindObjectsOfType<UIElement>();
        }

        /// <summary>
        /// Return a list of all the UIElements in the scene with the eName
        /// </summary>
        /// <param name="eName"></param>
        /// <returns></returns>
        List<UIElement> GetAllTheUIElementWithName(string eName)
        {
            List<UIElement> tempList = new List<UIElement>();

            if (uiElementsInTheScene == null)
            {
                uiElementsInTheScene = FindObjectsOfType<UIElement>();
            }
            else
            {
                for (int i = 0; i < uiElementsInTheScene.Length; i++)
                {
                    if (uiElementsInTheScene[i].linkedToNotification == false && eName.Equals(uiElementsInTheScene[i].elementName))
                    {
                        tempList.Add(uiElementsInTheScene[i]);
                    }
                }
                if (tempList.Count > 0)
                {
                    return tempList;
                }
            }
            return null;
        }

        /// <summary>
        /// Return a list of all the UIElements in the scene with the eSound
        /// </summary>
        /// <param name="eSound"></param>
        /// <returns></returns>
        List<UIElement> GetAllTheUIElementWithSound(string eSound)
        {
            List<UIElement> tempList = new List<UIElement>();

            if (uiElementsInTheScene == null)
            {
                uiElementsInTheScene = FindObjectsOfType<UIElement>();
            }
            else
            {
                for (int i = 0; i < uiElementsInTheScene.Length; i++)
                {
                    if (
                    #region IN
                        eSound.Equals(uiElementsInTheScene[i].moveIn.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].moveIn.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationIn.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationIn.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleIn.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleIn.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeIn.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeIn.soundAtFinishReference.soundName)
                    #endregion
                    #region LOOP
                        || eSound.Equals(uiElementsInTheScene[i].moveLoop.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].moveLoop.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationLoop.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationLoop.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleLoop.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleLoop.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeLoop.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeLoop.soundAtFinishReference.soundName)
                    #endregion
                    #region OUT
                        || eSound.Equals(uiElementsInTheScene[i].moveOut.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].moveOut.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationOut.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].rotationOut.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleOut.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].scaleOut.soundAtFinishReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeOut.soundAtStartReference.soundName)
                       || eSound.Equals(uiElementsInTheScene[i].fadeOut.soundAtFinishReference.soundName)
                    #endregion
                        )
                    {
                        tempList.Add(uiElementsInTheScene[i]);
                    }
                }
                if (tempList.Count > 0)
                {
                    return tempList;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns how many UIElements have the same eName in the current scene
        /// </summary>
        /// <param name="eName"></param>
        /// <returns></returns>
        int GetUIElementNameCount(string eName)
        {
            int count = 0;
            if (uiElementsInTheScene != null)
            {
                for (int i = 0; i < uiElementsInTheScene.Length; i++)
                {
                    if (uiElementsInTheScene[i].linkedToNotification == false && eName.Equals(uiElementsInTheScene[i].elementName))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Returns how many UIElements have the same eSound in the current scene
        /// </summary>
        /// <param name="eSound"></param>
        /// <returns></returns>
        int GetUIElementSoundCount(string eSound)
        {
            int count = 0;
            if (uiElementsInTheScene != null)
            {
                for (int i = 0; i < uiElementsInTheScene.Length; i++)
                {
                    if (
                    #region IN
                        eSound.Equals(uiElementsInTheScene[i].moveIn.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].moveIn.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationIn.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationIn.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleIn.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleIn.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeIn.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeIn.soundAtFinishReference.soundName)
                    #endregion
                    #region LOOP
                        || eSound.Equals(uiElementsInTheScene[i].moveLoop.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].moveLoop.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationLoop.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationLoop.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleLoop.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleLoop.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeLoop.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeLoop.soundAtFinishReference.soundName)
                    #endregion
                    #region OUT
                        || eSound.Equals(uiElementsInTheScene[i].moveOut.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].moveOut.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationOut.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].rotationOut.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleOut.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].scaleOut.soundAtFinishReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeOut.soundAtStartReference.soundName)
                        || eSound.Equals(uiElementsInTheScene[i].fadeOut.soundAtFinishReference.soundName)
                    #endregion
                        )
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        #endregion

        #region UIButton - ButtonNams & ButtonSounds
        /// <summary>
        /// Gets an array of all the UIButtons in the current scene
        /// </summary>
        void UpdateUIButtonsArray()
        {
            uiButtonsInTheScene = FindObjectsOfType<UIButton>();
        }

        /// <summary>
        /// Return a list of all the UIButtons in the scene with the bName
        /// </summary>
        /// <param name="bName"></param>
        /// <returns></returns>
        List<UIButton> GetAllTheUIButtonWithName(string bName)
        {
            List<UIButton> tempList = new List<UIButton>();

            if (uiButtonsInTheScene == null)
            {
                uiButtonsInTheScene = FindObjectsOfType<UIButton>();
            }
            else
            {
                for (int i = 0; i < uiButtonsInTheScene.Length; i++)
                {
                    if (bName.Equals(uiButtonsInTheScene[i].buttonName))
                    {
                        tempList.Add(uiButtonsInTheScene[i]);
                    }
                }
                if (tempList.Count > 0)
                {
                    return tempList;
                }
            }
            return null;
        }

        /// <summary>
        /// Return a list of all the UIButtons in the scene with the bSound
        /// </summary>
        /// <param name="bSound"></param>
        /// <returns></returns>
        List<UIButton> GetAllTheUIButtonWithSound(string bSound)
        {
            List<UIButton> tempList = new List<UIButton>();

            if (uiButtonsInTheScene == null)
            {
                uiButtonsInTheScene = FindObjectsOfType<UIButton>();
            }
            else
            {
                for (int i = 0; i < uiButtonsInTheScene.Length; i++)
                {
                    if (bSound.Equals(uiButtonsInTheScene[i].onClickSound))
                    {
                        tempList.Add(uiButtonsInTheScene[i]);
                    }
                }
                if (tempList.Count > 0)
                {
                    return tempList;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns how many UIButtons have the same bName in the current scene
        /// </summary>
        /// <param name="bName"></param>
        /// <returns></returns>
        int GetUIButtonsNameCount(string bName)
        {
            int count = 0;
            if (uiButtonsInTheScene != null)
            {
                for (int i = 0; i < uiButtonsInTheScene.Length; i++)
                {
                    if (bName.Equals(uiButtonsInTheScene[i].buttonName))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Returns how many UIButtons have the same bSound in the current scene
        /// </summary>
        /// <param name="bSound"></param>
        /// <returns></returns>
        int GetUIButtonsSoundCount(string bSound)
        {
            int count = 0;
            if (uiButtonsInTheScene != null)
            {
                for (int i = 0; i < uiButtonsInTheScene.Length; i++)
                {
                    if (bSound.Equals(uiButtonsInTheScene[i].onClickSound))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        #endregion

        #endregion

        #region Header
        void UpdateHeader()
        {
            EditorGUILayout.BeginVertical();

            DoozyUIHelper.DrawTexture(DoozyUIResources.WindowHeaderDoozy);

            GUILayout.Space(-1);

            switch (nextPage)
            {
                case Page.Main:
                    DoozyUIHelper.DrawTexture(DoozyUIResources.WindowSubheaderControlPanel, 600, 32);
                    break;

                case Page.ElementNames:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderElementNames, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;

                case Page.ElementSounds:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderElementSounds, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;

                case Page.ButtonNames:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderButtonNames, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;

                case Page.ButtonSounds:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderButtonSounds, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;

                case Page.QuickHelp:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderQuickHelp, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;

                case Page.VideoTutorials:
                    if (GUILayout.Button(DoozyUIResources.WindowSubheaderVideoTutorials, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(600)))
                    {
                        GotoPage(Page.Main);
                    }
                    break;
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(16);
        }
        #endregion

        #region Page Methods

        #region DoPage
        private void DoPage(Page page, Rect pageRect)
        {
            pageRect.height = position.height - pagePadding;
            GUILayout.BeginArea(pageRect);

            switch (page)
            {
                case Page.Main:
                    ShowMainPage();
                    break;
                case Page.ElementNames:
                    ShowElementNames();
                    break;
                case Page.ElementSounds:
                    ShowElementSounds();
                    break;
                case Page.ButtonNames:
                    ShowButtonNames();
                    break;
                case Page.ButtonSounds:
                    ShowButtonSounds();
                    break;
                case Page.QuickHelp:
                    ShowQuickHelp();
                    break;
                case Page.VideoTutorials:
                    ShowVideoTutorials();
                    break;
            }

            GUILayout.EndArea();
        }
        #endregion

        #region SetPage
        private void SetPage(Page page)
        {
            currentPage = page;
            nextPage = page;
            pageInTransition = false;
            currentPageRect.x = 0;
            Repaint();
        }
        #endregion

        #region GotoPage
        private void GotoPage(object userData)
        {
            nextPage = (Page)userData;
            pageInTransition = true;
            transitionStartTime = Time.realtimeSinceStartup;

            // next page slides in from the right
            // main screen slides offscreen left
            // reversed if returning to the main screen

            if (nextPage == Page.Main)
            {
                nextPageRect.x = -windowWidth;
                currentPageMoveTo = windowWidth;
            }
            else
            {
                nextPageRect.x = windowWidth;
                currentPageMoveTo = -windowWidth;
            }

            GUIUtility.ExitGUI();
        }
        #endregion

        #region DoPageTransition
        void DoPageTransition()
        {
            var t = (Time.realtimeSinceStartup - transitionStartTime) / transitionDuration;
            if (t > 1f)
            {
                SetPage(nextPage);
                return;
            }

            var nextPageX = Mathf.SmoothStep(nextPageRect.x, 0, t);
            var currentPageX = Mathf.SmoothStep(currentPageRect.x, currentPageMoveTo, t);
            currentPageRect.Set(currentPageX, pageTop, windowWidth, position.height);
            nextPageRect.Set(nextPageX, pageTop, windowWidth, position.height);

            Repaint();
        }
        #endregion

        #endregion

        #region Pages

        #region Show - MainPage
        void ShowMainPage()
        {
            int spacing = 12;

            EditorGUILayout.BeginVertical(GUILayout.Height(535));

            GUILayout.Space(8);

            #region Extension Buttons - MasterAudio, TextMeshPro, EnergyBarToolkit, Playmaker, Navigation System, Orientation Manager

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(14);
#if dUI_MasterAudio
            if (GUILayout.Button(DoozyUIResources.WindowButtonMAEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable support for MasterAudio?", "This will remove 'dUI_MasterAudio' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_MasterAudio");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonMADisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable support for MasterAudio?", "Enable this only if you have MasterAudio already installed. This will add 'dUI_MasterAudio' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_MasterAudio");
            }
#endif

            GUILayout.Space(spacing);

#if dUI_TextMeshPro
            if (GUILayout.Button(DoozyUIResources.WindowButtonTMPEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable support for TextMeshPro?", "This will remove 'dUI_TextMeshPro' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_TextMeshPro");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonTMPDisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable support for TextMeshPro?", "Enable this only if you have TextMeshPro already installed. This will add 'dUI_TextMeshPro' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_TextMeshPro");
            }
#endif

            GUILayout.Space(spacing);

#if dUI_EnergyBarToolkit
            if (GUILayout.Button(DoozyUIResources.WindowButtonEBTEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable support for EnergyBarToolkit?", "This will remove 'dUI_EnergyBarToolkit' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_EnergyBarToolkit");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonEBTDisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable support for EnergyBarToolkit?", "Enable this only if you have EnergyBarToolkit already installed. This will add 'dUI_EnergyBarToolkit' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_EnergyBarToolkit");
            }
#endif

            GUILayout.Space(spacing);

#if dUI_PlayMaker
            if (GUILayout.Button(DoozyUIResources.WindowButtonPMEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable support for PlayMaker?", "This will remove 'dUI_PlayMaker' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_PlayMaker");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonPMDisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable support for PlayMaker?", "Enable this only if you have PlayMaker already installed. This will add 'dUI_PlayMaker' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_PlayMaker");
            }
#endif

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(16);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

#if dUI_UseOrientationManager
            if (GUILayout.Button(DoozyUIResources.WindowButtonOMEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable the Orientation Manager?", "The LANDSCAPE and PORTRAIT options from UIElement components will no longer be available. This will remove 'dUI_UseOrientationManager' from Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_UseOrientationManager");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonOMDisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable the Orientation Manager?", "Enable this only if you want to create different UI's for each orientation. This will add 'dUI_UseOrientationManager' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_UseOrientationManager");
            }
#endif

            GUILayout.Space(spacing);

#if dUI_NavigationDisabled
            if (GUILayout.Button(DoozyUIResources.WindowButtonNavDisabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Enable the Navigation Manager?", "This will add 'dUI_NavigationDisabled' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    RemoveScriptingDefineSymbol("dUI_NavigationDisabled");
            }
#else
            if (GUILayout.Button(DoozyUIResources.WindowButtonNavEnabled, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                if (EditorUtility.DisplayDialog("Disable the Navigation Manager?", "Do this if you intend to handle the navigation yourself (maybe use Playmaker to do it?). This will add 'dUI_NavigationDisabled' to Scripting Define Symbols in Player Settings.", "Ok", "Cancel"))
                    AddScriptingDefineSymbol("dUI_NavigationDisabled");
            }
#endif
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(24);

            DoozyUIHelper.DrawTexture(DoozyUIResources.WindowMiniheaderEditDatabase);

            GUILayout.Space(16);

            #region Database Buttons - ElementNames, ElementSounds, ButtonNames, ButtonSounds

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(14);

            #region ElementNames
            EditorGUILayout.BeginVertical(GUILayout.Width(134));
            if (resetElementNames == false)
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonElementNames, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    GotoPage(Page.ElementNames);
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetRed, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetElementNames = true;
                }
            }
            else
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetElementNames, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    UIManager.ResetDoozyUIDataElementNames();
                    resetElementNames = false;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonCancelGreen, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetElementNames = false;
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(spacing);

            #region ElementSounds
            EditorGUILayout.BeginVertical(GUILayout.Width(134));
            if (resetElementSounds == false)
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonElementSounds, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    GotoPage(Page.ElementSounds);
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetRed, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetElementSounds = true;
                }
            }
            else
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetElementsounds, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    UIManager.ResetDoozyUIDataElementSounds();
                    resetElementSounds = false;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonCancelGreen, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetElementSounds = false;
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(spacing);

            #region ButtonNames
            EditorGUILayout.BeginVertical(GUILayout.Width(134));
            if (resetButtonNames == false)
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonButtonNames, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    GotoPage(Page.ButtonNames);
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetRed, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetButtonNames = true;
                }
            }
            else
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetButtonNames, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    UIManager.ResetDoozyUIDataButtonNames();
                    resetButtonNames = false;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonCancelGreen, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetButtonNames = false;
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(spacing);

            #region ButtonSounds
            EditorGUILayout.BeginVertical(GUILayout.Width(134));
            if (resetButtonSounds == false)
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonButtonSounds, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    GotoPage(Page.ButtonSounds);
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetRed, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetButtonSounds = true;
                }
            }
            else
            {
                if (GUILayout.Button(DoozyUIResources.WindowButtonResetButtonSounds, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
                {
                    UIManager.ResetDoozyUIDataButtonSounds();
                    resetButtonSounds = false;
                }
                GUILayout.Space(4);
                if (GUILayout.Button(DoozyUIResources.WindowButtonCancelGreen, GUIStyle.none, GUILayout.Height(24), GUILayout.Width(134)))
                {
                    resetButtonSounds = false;
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(24);

            DoozyUIHelper.DrawTexture(DoozyUIResources.WindowMiniheaderHelpAndTutorials);

            GUILayout.Space(16);

            #region QuickHelp, VideoTutorials

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            #region QuickHelp
            if (GUILayout.Button(DoozyUIResources.WindowButtonQuickHelp, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                GotoPage(Page.QuickHelp);
            }
            #endregion

            GUILayout.Space(spacing);

            #region VideoTutorials
            if (GUILayout.Button(DoozyUIResources.WindowButtonVideoTutorials, GUIStyle.none, GUILayout.Height(48), GUILayout.Width(134)))
            {
                GotoPage(Page.VideoTutorials);
            }
            #endregion

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            #endregion

            GUILayout.FlexibleSpace();

            #region Upgrade Scene - DISABLED FEATURE in 2.3.1
            /*
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(DoozyUIResources.WindowButtonUpgradeScene, GUIStyle.none, GUILayout.Width(160), GUILayout.Height(40)))
            {
                UpgradeCurentScene();
                Repaint();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            */
            #endregion

            GUILayout.Space(16);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("DoozyUI " + UIManager.VERSION, DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.Doozy, TextAnchor.MiddleCenter, FontStyle.Italic, 12), GUILayout.Width(600));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(UIManager.COPYRIGHT, DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.Doozy, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 8), GUILayout.Width(600));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            DoozyUIHelper.VerticalSpace(20);
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Show - ElementNames
        void ShowElementNames()
        {
            DoozyUIHelper.ResetColors();

            #region New ElementName
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (elementNamesNew == false)
            {
                if (GUILayout.Button("New Element Name", GUILayout.Height(20), GUILayout.Width(392)))
                {
                    elementNamesNew = true;
                }
            }
            else
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                elementNamesNewString = EditorGUILayout.TextField(elementNamesNewString, GUILayout.Height(20), GUILayout.Width(276));
                if (GUILayout.Button("create", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                {
                    if (elementNamesNewString.Equals(string.Empty) == false
                        && elementNamesNewString.Equals(UIManager.DEFAULT_ELEMENT_NAME) == false)
                    {
                        UIManager.NewElementName(elementNamesNewString);
                        elementNames = UIManager.GetElementNames();
                        DoozyUIRedundancyCheck.CheckAllTheUIElements();
                    }
                    elementNamesNewString = string.Empty;
                    elementNamesNew = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                {
                    elementNamesNewString = string.Empty;
                    elementNamesNew = false;
                }
                DoozyUIHelper.ResetColors();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            if (elementNamesNew == false)
            {
                DoozyUIHelper.VerticalSpace(8);
            }
            else
            {
                DoozyUIHelper.VerticalSpace(9);
            }

            #region Table Headers
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Used", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(50));
            EditorGUILayout.LabelField("Index", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(70));
            EditorGUILayout.LabelField("Element Name", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(320));
            EditorGUILayout.LabelField("Options", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(130));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.VerticalSpace(2);

            #region Table
            elementNamesScrollPosition = EditorGUILayout.BeginScrollView(elementNamesScrollPosition, GUILayout.Height(345));
            for (int i = 0; i < elementNames.Length; i++) //show the all the elementNames entries
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //Used
                if (elementNamesFinderIndex == i)
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
                }
                else
                {
                    DoozyUIHelper.ResetColors();
                }

                if (GUILayout.Button(GetUIElementNameCount(elementNames[i]).ToString(), GUILayout.Height(18), GUILayout.Width(60)))
                {
                    elementNamesFinderIndex = i;
                }
                DoozyUIHelper.ResetColors();
                //Index
                EditorGUILayout.LabelField(i.ToString(), DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(60));
                if (elementNames[i].Equals(UIManager.DEFAULT_ELEMENT_NAME)) //no options for the default element name
                {
                    //Element Name
                    EditorGUILayout.LabelField(elementNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                    //Options
                    GUILayout.Space(120);
                }
                else
                {
                    if (elementNamesRenameIndex == -1 && elementNamesDeleteIndex == -1)
                    {
                        //Element Name
                        EditorGUILayout.LabelField(elementNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                        //Options
                        if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                        {
                            elementNamesRenameIndex = i;
                            elementNamesRenameString = elementNames[i];
                        }
                        if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                        {
                            elementNamesDeleteIndex = i;
                        }
                    }
                    #region Rename ElementName
                    else if (elementNamesRenameIndex != -1) //RENAME
                    {
                        if (i == elementNamesRenameIndex) //this is the index we want to rename
                        {
                            //Rename ElementName
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                            elementNamesRenameString = EditorGUILayout.TextField(elementNamesRenameString, GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.TrimStartAndEndSpaces(elementNamesRenameString);
                                if (elementNamesRenameString.Equals(string.Empty) == false                      //we make sure the new name is not empty
                                    && elementNamesRenameString.Equals(UIManager.DEFAULT_ELEMENT_NAME) == false //we check that is not the default name
                                    && UIManager.GetIndexForElementName(elementNamesRenameString) == -1)         //we make sure there are no duplicates
                                {
                                    UIManager.RenameElementName(elementNamesRenameIndex, elementNamesRenameString); //we rename the element name
                                    elementNames = UIManager.GetElementNames(); //we update the string array for this list
                                    DoozyUIRedundancyCheck.CheckAllTheUIElements();
                                }
                                elementNamesRenameString = string.Empty; //we clear the temp string
                                elementNamesRenameIndex = -1; //we set the rename index to the default value
                                UpdateUIElementsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                elementNamesRenameString = string.Empty; //we clear the temp string
                                elementNamesRenameIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                        }
                        else
                        {
                            //Element Name
                            EditorGUILayout.LabelField(elementNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                    #region Delete ElementName
                    else if (elementNamesDeleteIndex != -1) //DELETE
                    {
                        if (i == elementNamesDeleteIndex) //this is the index we want to delete
                        {
                            //Delete ElementName
                            EditorGUILayout.LabelField(elementNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightRed, TextAnchor.MiddleLeft, FontStyle.Bold), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.DeleteElementName(elementNamesDeleteIndex); //we delete the list entry
                                elementNames = UIManager.GetElementNames(); //we update the string array for this list
                                DoozyUIRedundancyCheck.CheckAllTheUIElements();
                                elementNamesDeleteIndex = -1; //we set the rename index to the default value
                                elementNamesFinderIndex = 0; //we reset the finder filter in case we are deleting the currently selected entry
                                UpdateUIElementsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                elementNamesDeleteIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.ResetColors();
                        }
                        else
                        {
                            //Element Name
                            EditorGUILayout.LabelField(elementNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            DoozyUIHelper.VerticalSpace(2);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);

            #region Finder
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 2f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.LabelField("SHOWING all the UIElements with the", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Bold, 10), GUILayout.Width(200));
            EditorGUILayout.LabelField(elementNames[elementNamesFinderIndex], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic, 12), GUILayout.Width(260));
            EditorGUILayout.LabelField("Element Name", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleLeft, FontStyle.Bold, 10), GUILayout.Width(70));
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 2f);
            DoozyUIHelper.VerticalSpace(2);
            elementNamesFilterScrollPosition = EditorGUILayout.BeginScrollView(elementNamesFilterScrollPosition, GUILayout.Height(85));
            elementNamesFilterList = GetAllTheUIElementWithName(elementNames[elementNamesFinderIndex]);
            if (elementNamesFilterList != null && elementNamesFilterList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int i = 0; i < elementNamesFilterList.Count; i++)
                {
                    if (GUILayout.Button(elementNamesFilterList[i].gameObject.name, GUILayout.Height(18), GUILayout.Width(180)))
                    {
                        Selection.activeObject = elementNamesFilterList[i].gameObject;
                    }
                    if (((i + 2) % numberOfFilteredButtonsPerRow) == 1)
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("There are no UIElements in the scene with the selected element name!", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
            #endregion
        }
        #endregion

        #region Show - ElementSounds
        void ShowElementSounds()
        {
            DoozyUIHelper.ResetColors();

            #region New ElementSound
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (elementSoundsNew == false)
            {
                if (GUILayout.Button("New Element Sound", GUILayout.Height(20), GUILayout.Width(392)))
                {
                    elementSoundsNew = true;
                }
            }
            else
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                elementSoundsNewString = EditorGUILayout.TextField(elementSoundsNewString, GUILayout.Height(20), GUILayout.Width(276));
                if (GUILayout.Button("create", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                {
                    if (elementSoundsNewString.Equals(string.Empty) == false
                        && elementSoundsNewString.Equals(UIManager.DEFAULT_SOUND_NAME) == false)
                    {
                        UIManager.NewElementSound(elementSoundsNewString);
                        elementSounds = UIManager.GetElementSounds();
                        DoozyUIRedundancyCheck.CheckAllTheUIElements();
                    }
                    elementSoundsNewString = string.Empty;
                    elementSoundsNew = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                {
                    elementSoundsNewString = string.Empty;
                    elementSoundsNew = false;
                }
                DoozyUIHelper.ResetColors();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            if (elementSoundsNew == false)
            {
                DoozyUIHelper.VerticalSpace(8);
            }
            else
            {
                DoozyUIHelper.VerticalSpace(9);
            }

            #region Table Headers
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Used", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(50));
            EditorGUILayout.LabelField("Index", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(70));
            EditorGUILayout.LabelField("Element Sound", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(320));
            EditorGUILayout.LabelField("Options", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(130));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.VerticalSpace(2);

            #region Table
            elementSoundsScrollPosition = EditorGUILayout.BeginScrollView(elementSoundsScrollPosition, GUILayout.Height(345));
            for (int i = 0; i < elementSounds.Length; i++) //show the all the elementSounds entries
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //Used
                if (elementSoundsFinderIndex == i)
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
                }
                else
                {
                    DoozyUIHelper.ResetColors();
                }

                if (GUILayout.Button(GetUIElementSoundCount(elementSounds[i]).ToString(), GUILayout.Height(18), GUILayout.Width(60)))
                {
                    elementSoundsFinderIndex = i;
                }
                DoozyUIHelper.ResetColors();
                //Index
                EditorGUILayout.LabelField(i.ToString(), DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(60));
                if (elementSounds[i].Equals(UIManager.DEFAULT_SOUND_NAME)) //no options for the default element sound
                {
                    //Element Sound
                    EditorGUILayout.LabelField(elementSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                    //Options
                    GUILayout.Space(120);
                }
                else
                {
                    if (elementSoundsRenameIndex == -1 && elementSoundsDeleteIndex == -1)
                    {
                        //Element Sound
                        EditorGUILayout.LabelField(elementSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                        //Options
                        if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                        {
                            elementSoundsRenameIndex = i;
                            elementSoundsRenameString = elementSounds[i];
                        }
                        if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                        {
                            elementSoundsDeleteIndex = i;
                        }
                    }
                    #region Rename ElementSound
                    else if (elementSoundsRenameIndex != -1) //RENAME
                    {
                        if (i == elementSoundsRenameIndex) //this is the index we want to rename
                        {
                            //Rename ElementName
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                            elementSoundsRenameString = EditorGUILayout.TextField(elementSoundsRenameString, GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.TrimStartAndEndSpaces(elementSoundsRenameString);
                                if (elementSoundsRenameString.Equals(string.Empty) == false                      //we make sure the new name is not empty
                                    && elementSoundsRenameString.Equals(UIManager.DEFAULT_SOUND_NAME) == false //we check that is not the default name
                                    && UIManager.GetIndexForElementSound(elementSoundsRenameString) == -1)         //we make sure there are no duplicates
                                {
                                    UIManager.RenameElementSound(elementSoundsRenameIndex, elementSoundsRenameString); //we rename the element name
                                    elementSounds = UIManager.GetElementSounds(); //we update the string array for this list
                                    DoozyUIRedundancyCheck.CheckAllTheUIElements();
                                }
                                elementSoundsRenameString = string.Empty; //we clear the temp string
                                elementSoundsRenameIndex = -1; //we set the rename index to the default value
                                UpdateUIElementsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                elementSoundsRenameString = string.Empty; //we clear the temp string
                                elementSoundsRenameIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                        }
                        else
                        {
                            //Element Name
                            EditorGUILayout.LabelField(elementSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                    #region Delete ElementSound
                    else if (elementSoundsDeleteIndex != -1) //DELETE
                    {
                        if (i == elementSoundsDeleteIndex) //this is the index we want to delete
                        {
                            //Delete ElementSound
                            EditorGUILayout.LabelField(elementSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightRed, TextAnchor.MiddleLeft, FontStyle.Bold), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.DeleteElementSound(elementSoundsDeleteIndex); //we delete the list entry
                                elementSounds = UIManager.GetElementSounds(); //we update the string array for this 
                                DoozyUIRedundancyCheck.CheckAllTheUIElements();
                                elementSoundsDeleteIndex = -1; //we set the rename index to the default value
                                elementSoundsFinderIndex = 0; //we reset the finder filter in case we are deleting the currently selected entry
                                UpdateUIElementsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                elementSoundsDeleteIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.ResetColors();
                        }
                        else
                        {
                            //Element Name
                            EditorGUILayout.LabelField(elementSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            DoozyUIHelper.VerticalSpace(2);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);

            #region Finder
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.LabelField("SHOWING all the UIElements with the", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Bold, 10), GUILayout.Width(200));
            EditorGUILayout.LabelField(elementSounds[elementSoundsFinderIndex], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic, 12), GUILayout.Width(260));
            EditorGUILayout.LabelField("Element Sound", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleLeft, FontStyle.Bold, 10), GUILayout.Width(70));
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            DoozyUIHelper.VerticalSpace(2);
            elementSoundsFilterScrollPosition = EditorGUILayout.BeginScrollView(elementSoundsFilterScrollPosition, GUILayout.Height(85));
            elementSoundsFilterList = GetAllTheUIElementWithSound(elementSounds[elementSoundsFinderIndex]);
            if (elementSoundsFilterList != null && elementSoundsFilterList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int i = 0; i < elementSoundsFilterList.Count; i++)
                {
                    if (GUILayout.Button(elementSoundsFilterList[i].gameObject.name, GUILayout.Height(18), GUILayout.Width(180)))
                    {
                        Selection.activeObject = elementSoundsFilterList[i].gameObject;
                    }
                    if (((i + 2) % numberOfFilteredButtonsPerRow) == 1)
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("There are no gameObjects in the scene with the selected element sound!", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
            #endregion
        }
        #endregion

        #region Show - ButtonNames
        void ShowButtonNames()
        {
            DoozyUIHelper.ResetColors();

            #region New ButtonNames
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (buttonNamesNew == false)
            {
                if (GUILayout.Button("New Button Name", GUILayout.Height(20), GUILayout.Width(392)))
                {
                    buttonNamesNew = true;
                }
            }
            else
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                buttonNamesNewString = EditorGUILayout.TextField(buttonNamesNewString, GUILayout.Height(20), GUILayout.Width(276));
                if (GUILayout.Button("create", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                {
                    if (buttonNamesNewString.Equals(string.Empty) == false
                        && buttonNamesNewString.Equals(UIManager.DEFAULT_BUTTON_NAME) == false
                        && buttonNamesNewString.Equals("Back") == false
                        && UIManager.GetIndexForButtonName(buttonNamesNewString) == -1)
                    {
                        UIManager.NewButtonName(buttonNamesNewString);
                        buttonNames = UIManager.GetButtonNames();
                        DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                    }
                    buttonNamesNewString = string.Empty;
                    buttonNamesNew = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                {
                    buttonNamesNewString = string.Empty;
                    buttonNamesNew = false;
                }
                DoozyUIHelper.ResetColors();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            if (elementNamesNew == false)
            {
                DoozyUIHelper.VerticalSpace(8);
            }
            else
            {
                DoozyUIHelper.VerticalSpace(9);
            }

            #region Table Headers
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Used", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(50));
            EditorGUILayout.LabelField("Index", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(70));
            EditorGUILayout.LabelField("Button Name", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(320));
            EditorGUILayout.LabelField("Options", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(130));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.VerticalSpace(2);

            #region Table
            buttonNamesScrollPosition = EditorGUILayout.BeginScrollView(buttonNamesScrollPosition, GUILayout.Height(345));
            for (int i = 0; i < buttonNames.Length; i++) //show the all the buttonNames entries
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //Used
                if (buttonNamesFinderIndex == i)
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
                }
                else
                {
                    DoozyUIHelper.ResetColors();
                }

                if (GUILayout.Button(GetUIButtonsNameCount(buttonNames[i]).ToString(), GUILayout.Height(18), GUILayout.Width(60)))
                {
                    buttonNamesFinderIndex = i;
                }
                DoozyUIHelper.ResetColors();
                //Index
                EditorGUILayout.LabelField(i.ToString(), DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(60));
                if (buttonNames[i].Equals(UIManager.DEFAULT_BUTTON_NAME) //no options for the default button name
                    || buttonNames[i].Equals("Back") //no options for the the 'Back' button
                    || buttonNames[i].Equals("ToggleSound") //no options for the the 'ToggleSound' button
                    || buttonNames[i].Equals("ToggleMusic") //no options for the the 'ToggleMusic' button
                    || buttonNames[i].Equals("TogglePause") //no options for the the 'TogglePause' button
                    || buttonNames[i].Equals("ApplicationQuit")) //no options for the the 'ApplicationQuit' button
                {
                    //Button Name
                    EditorGUILayout.LabelField(buttonNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                    //Options
                    GUILayout.Space(120);
                }
                else
                {
                    if (buttonNamesRenameIndex == -1 && buttonNamesDeleteIndex == -1)
                    {
                        //Element Name
                        EditorGUILayout.LabelField(buttonNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                        //Options
                        if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                        {
                            buttonNamesRenameIndex = i;
                            buttonNamesRenameString = buttonNames[i];
                        }
                        if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                        {
                            buttonNamesDeleteIndex = i;
                        }
                    }
                    #region Rename ButtonName
                    else if (buttonNamesRenameIndex != -1) //RENAME
                    {
                        if (i == buttonNamesRenameIndex) //this is the index we want to rename
                        {
                            //Rename ButtonName
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                            buttonNamesRenameString = EditorGUILayout.TextField(buttonNamesRenameString, GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.TrimStartAndEndSpaces(buttonNamesRenameString);
                                if (buttonNamesRenameString.Equals(string.Empty) == false                      //we make sure the new name is not empty
                                    && buttonNamesRenameString.Equals(UIManager.DEFAULT_BUTTON_NAME) == false //we check that is not the default name
                                    && buttonNamesRenameString.Equals("Back") == false
                                    && UIManager.GetIndexForButtonName(buttonNamesRenameString) == -1)         //we make sure there are no duplicates
                                {
                                    UIManager.RenameButtonName(buttonNamesRenameIndex, buttonNamesRenameString); //we rename the button name
                                    buttonNames = UIManager.GetButtonNames(); //we update the string array for this list
                                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                                }
                                buttonNamesRenameString = string.Empty; //we clear the temp string
                                buttonNamesRenameIndex = -1; //we set the rename index to the default value
                                UpdateUIButtonsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                buttonNamesRenameString = string.Empty; //we clear the temp string
                                buttonNamesRenameIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                        }
                        else
                        {
                            //Button Name
                            EditorGUILayout.LabelField(buttonNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                    #region Delete ButtonName
                    else if (buttonNamesDeleteIndex != -1) //DELETE
                    {
                        if (i == buttonNamesDeleteIndex) //this is the index we want to delete
                        {
                            //Delete ButtonName
                            EditorGUILayout.LabelField(buttonNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightRed, TextAnchor.MiddleLeft, FontStyle.Bold), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.DeleteButtonName(buttonNamesDeleteIndex); //we delete the list entry
                                buttonNames = UIManager.GetButtonNames(); //we update the string array for this list
                                buttonNamesDeleteIndex = -1; //we set the rename index to the default value
                                buttonNamesFinderIndex = 0; //we reset the finder filter in case we are deleting the currently selected entry
                                UpdateUIButtonsArray();
                                DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                buttonNamesDeleteIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.ResetColors();
                        }
                        else
                        {
                            //Button Name
                            EditorGUILayout.LabelField(buttonNames[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            DoozyUIHelper.VerticalSpace(2);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);

            #region Finder
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.LabelField("SHOWING all the UIButtons with the", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Bold, 10), GUILayout.Width(200));
            EditorGUILayout.LabelField(buttonNames[buttonNamesFinderIndex], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic, 12), GUILayout.Width(260));
            EditorGUILayout.LabelField("Button Name", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleLeft, FontStyle.Bold, 10), GUILayout.Width(70));
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            DoozyUIHelper.VerticalSpace(2);
            buttonNamesFilterScrollPosition = EditorGUILayout.BeginScrollView(buttonNamesFilterScrollPosition, GUILayout.Height(85));
            buttonNamesFilterList = GetAllTheUIButtonWithName(buttonNames[buttonNamesFinderIndex]);
            if (buttonNamesFilterList != null && buttonNamesFilterList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int i = 0; i < buttonNamesFilterList.Count; i++)
                {
                    if (GUILayout.Button(buttonNamesFilterList[i].gameObject.name, GUILayout.Height(18), GUILayout.Width(180)))
                    {
                        Selection.activeObject = buttonNamesFilterList[i].gameObject;
                    }
                    if (((i + 2) % numberOfFilteredButtonsPerRow) == 1)
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("There are no gameObjects in the scene with the selected button name!", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
            #endregion
        }
        #endregion

        #region Show - ButtonSounds
        void ShowButtonSounds()
        {
            DoozyUIHelper.ResetColors();

            #region New ButtonSound
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (buttonSoundsNew == false)
            {
                if (GUILayout.Button("New Button Sound", GUILayout.Height(20), GUILayout.Width(392)))
                {
                    buttonSoundsNew = true;
                }
            }
            else
            {
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                buttonSoundsNewString = EditorGUILayout.TextField(buttonSoundsNewString, GUILayout.Height(20), GUILayout.Width(276));
                if (GUILayout.Button("create", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                {
                    if (buttonSoundsNewString.Equals(string.Empty) == false
                        && buttonSoundsNewString.Equals(UIManager.DEFAULT_SOUND_NAME) == false)
                    {
                        UIManager.NewButtonSound(buttonSoundsNewString);
                        buttonSounds = UIManager.GetButtonSounds();
                        DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                    }
                    buttonSoundsNewString = string.Empty;
                    buttonSoundsNew = false;
                }
                DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                {
                    buttonSoundsNewString = string.Empty;
                    buttonSoundsNew = false;
                }
                DoozyUIHelper.ResetColors();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            if (buttonSoundsNew == false)
            {
                DoozyUIHelper.VerticalSpace(8);
            }
            else
            {
                DoozyUIHelper.VerticalSpace(9);
            }

            #region Table Headers
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Used", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(50));
            EditorGUILayout.LabelField("Index", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(70));
            EditorGUILayout.LabelField("Button Sound", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(320));
            EditorGUILayout.LabelField("Options", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Bold), GUILayout.Height(20), GUILayout.Width(130));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.VerticalSpace(2);

            #region Table
            buttonSoundsScrollPosition = EditorGUILayout.BeginScrollView(buttonSoundsScrollPosition, GUILayout.Height(345));
            for (int i = 0; i < buttonSounds.Length; i++) //show the all the buttonSounds entries
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //Used
                if (buttonSoundsFinderIndex == i)
                {
                    DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
                }
                else
                {
                    DoozyUIHelper.ResetColors();
                }

                if (GUILayout.Button(GetUIButtonsSoundCount(buttonSounds[i]).ToString(), GUILayout.Height(18), GUILayout.Width(60)))
                {
                    buttonSoundsFinderIndex = i;
                }
                DoozyUIHelper.ResetColors();
                //Index
                EditorGUILayout.LabelField(i.ToString(), DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleCenter, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(60));
                if (buttonSounds[i].Equals(UIManager.DEFAULT_SOUND_NAME)) //no options for the default button sound
                {
                    //Button Sound
                    EditorGUILayout.LabelField(buttonSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                    //Options
                    GUILayout.Space(120);
                }
                else
                {
                    if (buttonSoundsRenameIndex == -1 && buttonSoundsDeleteIndex == -1)
                    {
                        //Button Sound
                        EditorGUILayout.LabelField(buttonSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                        //Options
                        if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56))) //we add a rename button for each list entry
                        {
                            buttonSoundsRenameIndex = i;
                            buttonSoundsRenameString = buttonSounds[i];
                        }
                        if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56))) //we add a delete button for each list entry
                        {
                            buttonSoundsDeleteIndex = i;
                        }
                    }
                    #region Rename ButtonSound
                    else if (buttonSoundsRenameIndex != -1) //RENAME
                    {
                        if (i == buttonSoundsRenameIndex) //this is the index we want to rename
                        {
                            //Rename ButtonSound
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightOranage);
                            buttonSoundsRenameString = EditorGUILayout.TextField(buttonSoundsRenameString, GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            if (GUILayout.Button("rename", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.TrimStartAndEndSpaces(buttonSoundsRenameString);
                                if (buttonSoundsRenameString.Equals(string.Empty) == false                      //we make sure the new name is not empty
                                    && buttonSoundsRenameString.Equals(UIManager.DEFAULT_SOUND_NAME) == false //we check that is not the default name
                                    && UIManager.GetIndexForButtonName(buttonSoundsRenameString) == -1)         //we make sure there are no duplicates
                                {
                                    UIManager.RenameButtonSound(buttonSoundsRenameIndex, buttonSoundsRenameString); //we rename the button sound
                                    buttonSounds = UIManager.GetButtonSounds(); //we update the string array for this list
                                    DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                                }
                                buttonSoundsRenameString = string.Empty; //we clear the temp string
                                buttonSoundsRenameIndex = -1; //we set the rename index to the default value
                                UpdateUIButtonsArray();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                buttonSoundsRenameString = string.Empty; //we clear the temp string
                                buttonSoundsRenameIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                        }
                        else
                        {
                            //Button Sound
                            EditorGUILayout.LabelField(buttonSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                    #region Delete ButtonSound
                    else if (buttonSoundsDeleteIndex != -1) //DELETE
                    {
                        if (i == buttonSoundsDeleteIndex) //this is the index we want to delete
                        {
                            //Delete ButtonSound
                            EditorGUILayout.LabelField(buttonSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightRed, TextAnchor.MiddleLeft, FontStyle.Bold), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightRed);
                            if (GUILayout.Button("delete", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                UIManager.DeleteButtonSound(buttonSoundsDeleteIndex); //we delete the list entry
                                buttonSounds = UIManager.GetButtonSounds(); //we update the string array for this list
                                buttonSoundsDeleteIndex = -1; //we set the rename index to the default value
                                buttonSoundsFinderIndex = 0; //we reset the finder filter in case we are deleting the currently selected entry
                                UpdateUIButtonsArray();
                                DoozyUIRedundancyCheck.CheckAllTheUIButtons();
                            }
                            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightGreen);
                            if (GUILayout.Button("cancel", GUILayout.Height(18), GUILayout.Width(56)))
                            {
                                buttonSoundsDeleteIndex = -1; //we set the rename index to the default value
                            }
                            DoozyUIHelper.ResetColors();
                        }
                        else
                        {
                            //Button Name
                            EditorGUILayout.LabelField(buttonSounds[i], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightGrey, TextAnchor.MiddleLeft, FontStyle.Normal), GUILayout.Height(18), GUILayout.Width(330));
                            //Options
                            GUILayout.Space(120);
                        }
                    }
                    #endregion
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                DoozyUIHelper.VerticalSpace(2);
                DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            DoozyUIHelper.VerticalSpace(2);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightGrey, 600f, 1f);

            #region Finder
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.LightBlue);
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.LabelField("SHOWING all the UIButtons with the", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Bold, 10), GUILayout.Width(200));
            EditorGUILayout.LabelField(buttonSounds[buttonSoundsFinderIndex], DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic, 12), GUILayout.Width(260));
            EditorGUILayout.LabelField("Button Sound", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleLeft, FontStyle.Bold, 10), GUILayout.Width(70));
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            DoozyUIHelper.DrawTexture(DoozyUIResources.MiniBarLightBlue, 600f, 2f);
            DoozyUIHelper.VerticalSpace(2);
            buttonSoundsFilterScrollPosition = EditorGUILayout.BeginScrollView(buttonSoundsFilterScrollPosition, GUILayout.Height(85));
            buttonSoundsFilterList = GetAllTheUIButtonWithSound(buttonSounds[buttonSoundsFinderIndex]);
            if (buttonSoundsFilterList != null && buttonSoundsFilterList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int i = 0; i < buttonSoundsFilterList.Count; i++)
                {
                    if (GUILayout.Button(buttonSoundsFilterList[i].gameObject.name, GUILayout.Height(18), GUILayout.Width(180)))
                    {
                        Selection.activeObject = buttonSoundsFilterList[i].gameObject;
                    }
                    if (((i + 2) % numberOfFilteredButtonsPerRow) == 1)
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("There are no gameObjects in the scene with the selected button sound!", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.LightBlue, TextAnchor.MiddleCenter, FontStyle.Italic));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
            #endregion
        }
        #endregion

        #region Show - QuickHelp
        void ShowQuickHelp()
        {
            DoozyUIHelper.ResetColors();
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.DarkGrey);

            int spaceBeforeEachIntem = 128;
            int spaceBetweenEachItem = 4;

            #region Disclaimer
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("You will find up to date informations by accesing the online documentation", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.Orange, TextAnchor.MiddleCenter, FontStyle.Italic, 9), GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(16);

            #region Open Online Documentation
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(DoozyUIResources.WindowButtonOpenDocumentation, GUIStyle.none, GUILayout.Width(192), GUILayout.Height(48)))
            {
                Application.OpenURL("https://dl.dropboxusercontent.com/u/82405904/DoozyUI/Documentation/DoozyUI%202.6%20-%20Documentation.pdf");
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(16);

            quickHelpScrollPosition = EditorGUILayout.BeginScrollView(quickHelpScrollPosition, GUILayout.Height(401), GUILayout.Width(590));

            ShowTitle(spaceBetweenEachItem, "About the DoozyUI components");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "The Control Panel", "https://www.youtube.com/watch?v=7q1rN-mIT88");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Manager", "https://www.youtube.com/watch?v=pjRwJeVQxgM");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Button", "https://www.youtube.com/watch?v=zJwGyFLh5_0");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Element", "https://www.youtube.com/watch?v=CEd_gQsh-p4");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Trigger", "https://www.youtube.com/watch?v=omQCWiMI2tg");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Effect", "https://www.youtube.com/watch?v=WygtDgmbVZQ");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "UI Notification", "https://www.youtube.com/watch?v=WlBcFz7CNmE");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "Scene Loader", "https://www.youtube.com/watch?v=QE24nljfqpk");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "Playmaker Event Dispatcher", "https://www.youtube.com/watch?v=h_Z7HUPbZx4");

            GUILayout.Space(spaceBetweenEachItem);

            ShowTitle(spaceBetweenEachItem, "3rd Party Plugins");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to enable support for MasterAudio", "https://www.youtube.com/watch?v=IxE4krN5-xo");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to enable support for TextMeshPro", "https://www.youtube.com/watch?v=Cwlf4L0chvU");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to enable support for EnergyBarToolkit", "https://www.youtube.com/watch?v=53FiqbrmoXo");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to enable support for Playmaker", "https://www.youtube.com/watch?v=lqJIRQXa7q0");

            GUILayout.Space(spaceBetweenEachItem);

            ShowTitle(spaceBetweenEachItem, "The Navigation System");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to disable the Navigation System", "https://www.youtube.com/watch?v=Afcq4hvT5OE");

            GUILayout.Space(spaceBetweenEachItem);

            ShowTitle(spaceBetweenEachItem, "The Orientation Manager");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to use the Orientation Manager", "https://youtu.be/XBGzmxqWA2c");

            GUILayout.Space(spaceBetweenEachItem);
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
        }


        #endregion

        #region Show - VideoTutorials
        void ShowVideoTutorials()
        {
            DoozyUIHelper.ResetColors();
            DoozyUIHelper.SetZoneColor(DoozyUIHelper.DoozyColor.DarkGrey);

            int spaceBeforeEachIntem = 128;
            int spaceBetweenEachItem = 4;

            #region Disclaimer
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("You will find more tutorials on our YouTube Channel", DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.Orange, TextAnchor.MiddleCenter, FontStyle.Italic, 9), GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(16);

            #region YouTube Channel Link
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(DoozyUIResources.WindowButtonYoutube, GUIStyle.none, GUILayout.Width(136), GUILayout.Height(64)))
            {
                Application.OpenURL("https://www.youtube.com/playlist?list=PLszfX2HJpT-JwTq16SqkyGzA1TRWSstdI");
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(16);

            videoTutorialsScrollPosition = EditorGUILayout.BeginScrollView(videoTutorialsScrollPosition, GUILayout.Height(385), GUILayout.Width(590));

            ShowTitle(spaceBetweenEachItem, "Release Notes");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.1 - New Features", "https://www.youtube.com/watch?v=ItrBJPtdsA4");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.2 - New Features", "https://youtu.be/DI7bSHSA2A8");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.3 - New Features", "https://youtu.be/fphA5T7SlHs");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.4 - New Features", "https://youtu.be/Mg_8PQ8ZlpI");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.5 - New Features", "https://youtu.be/wjBiONqijF4");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "DoozyUI 2.6 - New Features", "https://youtu.be/sAcLyHRNKi0");

            GUILayout.Space(spaceBetweenEachItem);

            ShowTitle(spaceBetweenEachItem, "Design Techniques");
            GUILayout.Space(spaceBetweenEachItem);
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to create a Game UI with Playmaker", "https://youtu.be/n_SdleiC10s");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How To create a Simple UI", "https://youtu.be/x5vubVxAv8I");
            ShowYouTubePlayButton(spaceBetweenEachItem, spaceBeforeEachIntem, "How to create a Parchment styled UI", "https://www.youtube.com/watch?v=quruI9a85vg");

            GUILayout.Space(spaceBetweenEachItem);
            EditorGUILayout.EndScrollView();
            DoozyUIHelper.ResetColors();
        }
        #endregion

        #endregion

        void ShowYouTubePlayButton(int spaceBetweenEachItem, int spaceBeforeEachIntem, string label, string link)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(spaceBeforeEachIntem);
            if (GUILayout.Button(DoozyUIResources.WindowButtonPlay, GUIStyle.none, GUILayout.Width(64), GUILayout.Height(24)))
            {
                Application.OpenURL(link);
            }
            GUILayout.Space(4);
            EditorGUILayout.LabelField(label, DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.White, TextAnchor.MiddleLeft, FontStyle.BoldAndItalic, 12), GUILayout.Height(22));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(spaceBetweenEachItem);
        }

        void ShowTitle(int spaceBetweenEachItem, string title)
        {
            GUILayout.Space(spaceBetweenEachItem * 3);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(title, DoozyUIHelper.CreateTextStyle(DoozyUIHelper.DoozyColor.Orange, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 14), GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(spaceBetweenEachItem);
        }

        #region Search Hierarchy Filter
        public const int FILTERMODE_ALL = 0;
        public const int FILTERMODE_NAME = 1;
        public const int FILTERMODE_TYPE = 2;
        static SearchableEditorWindow hierarchy;

        public static void SetSearchFilter(string filter, int filterMode)
        {
            SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));

            foreach (SearchableEditorWindow window in windows)
            {

                if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
                {

                    hierarchy = window;
                    break;
                }
            }

            if (hierarchy == null)
                return;

            MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { filter, filterMode, false };

            setSearchType.Invoke(hierarchy, parameters);
        }
        #endregion

        #region Scripting Define Symbols Mehtods - Add & Remove
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

        #region Sorting Layer Management Methods - GetSortingLayerNames, GetSortingLayerUniqueIDs
        /// <summary>
        ///  Get the sorting layer names
        /// </summary>
        public string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        /// <summary>
        ///  Get the unique sorting layer IDs
        /// </summary>
        public int[] GetSortingLayerUniqueIDs()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
        }
        #endregion
    }
}
#endif
