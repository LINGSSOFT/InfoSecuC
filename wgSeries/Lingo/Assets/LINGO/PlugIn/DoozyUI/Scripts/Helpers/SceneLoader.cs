// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;

#if (UNITY_5_1 == false && UNITY_5_2 == false)
using UnityEngine.SceneManagement;
#endif

#if dUI_EnergyBarToolkit
using EnergyBarToolkit;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{

    #region Context Menu Methods

    [AddComponentMenu("DoozyUI/Scene Loader", 6)]
    public class SceneLoader : Singleton<SceneLoader>
    {
#if UNITY_EDITOR
        [MenuItem("DoozyUI/Components/Scene Loader", false, 6)]
        [MenuItem("GameObject/DoozyUI/Scene Loader", false, 6)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            if (GameObject.Find("UIManager") == null)
            {
                Debug.LogError("[DoozyUI] The DoozyUI system was not found in the scene. Please add it before trying to create a Scene Loader.");
                return;
            }

            SceneLoader sl = FindObjectOfType<SceneLoader>();
            if (sl != null)
            {
                Selection.activeGameObject = sl.gameObject;
                Debug.Log("[DoozyUI] A Scene Loader is already present in this scene.");
                return;
            }

            GameObject go = new GameObject("Scene Loader");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
            go.AddComponent<SceneLoader>();
        }
#endif

        #endregion

        #region Public Variables
        [HideInInspector]
        public bool showHelp = false;

#if dUI_EnergyBarToolkit
        public List<EnergyBar> energyBars;
#endif

        public string command_LoadSceneAsync_SceneName = "LoadSceneAsync_Name_";
        public string command_LoadSceneAsync_SceneBuildIndex = "LoadSceneAsync_ID_";
        public string command_LoadSceneAdditiveAsync_SceneName = "LoadSceneAdditiveAsync_Name_";
        public string command_LoadSceneAdditiveAsync_SceneBuildIndex = "LoadSceneAdditiveAsync_ID_";
        public string command_UnloadScene_SceneName = "UnloadScene_Name_";
        public string command_UnloadScene_SceneBuildIndex = "UnloadScene_ID_";
        public string command_UnloadLevel = "UnloadLevel_";
        public string command_LoadLevel = "LoadLevel_";

        public string levelSceneName = "Level_";
        #endregion

        #region Private Variables
        private AsyncOperation async = null; // When assigned, load is in progress.
        private int sceneBuildIndex = -1;
        private string sceneName = "";
        #endregion

        void OnEnable()
        {
            RegisterSceneLoader();
        }

        void OnDisable()
        {
            UnregisterSceneLoader();
        }

        void Update()
        {
            CheckIfLevelLoaded();
            UpdateEnergyBars();
        }

        void CheckIfLevelLoaded()
        {
            if (async != null)
            {
                if (async.isDone)
                {
                    UIManager.SendGameEvent("LevelLoaded");
                    async = null;
                }
            }
        }

        #region UPdate EnergyBars
        void UpdateEnergyBars()
        {
#if dUI_EnergyBarToolkit
            // If if we are loading a level and we have energyBars linkd then we update their values
            if (async != null && energyBars != null && energyBars.Count > 0)
            {
                for (int i = 0; i < energyBars.Count; i++)
                {
                    if (energyBars[i] != null)
                        energyBars[i].SetValueF(async.progress);
                }
            }
#endif
        }
        #endregion

        #region Register and Unregister SceneLoader
        void RegisterSceneLoader()
        {
            if (UIManager.sceneLoader == null)
            {
                UIManager.sceneLoader = this;
            }
            else
            {
                gameObject.name = "SceneLoader_DUPLICATE";
                Debug.LogWarning("[DoozyUI] An instance of a SceneLoader is already registared to the UIManager. There should never be more than 1 SceneLoader in the Hierarcy! This gameObject has been renamed to 'SceneLoader_DUPLICATE'. Look for it in the Hierarchy, while 'Play Mode', then select it, exit 'Play Mode' and then delete it.");
            }
        }

        void UnregisterSceneLoader()
        {
            if (UIManager.sceneLoader != null)
            {
                UIManager.sceneLoader = null;
            }
        }
        #endregion

        #region SceneLoader for Unity 5.1 and Unity 5.2
#if (UNITY_5_1 || UNITY_5_2)
        #region Event Listeners - uses Application.LoadLevel...

        public void OnGameEvent(GameEventMessage m)
        {
            if (m.command.Contains(command_LoadSceneAsync_SceneName))
            {
                sceneName = m.command.Split('_')[2];
                LoadSceneAsync(sceneName);

            }
            else if (m.command.Contains(command_LoadSceneAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(m.command.Split('_')[2]);
                LoadSceneAsync(sceneBuildIndex);
            }
            else if (m.command.Contains(command_LoadSceneAdditiveAsync_SceneName))
            {
                sceneName = m.command.Split('_')[2];
                LoadLevelAdditiveAsync(sceneName);
            }
            else if (m.command.Contains(command_LoadSceneAdditiveAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(m.command.Split('_')[2]);
                LoadLevelAdditiveAsync(sceneBuildIndex);
            }
            else if (m.command.Contains(command_LoadLevel))   //SHORTCUT VARIANT - we just call LoadLevel_{LevelNumber} and we load additive async the level data
            {
                sceneName = levelSceneName + m.command.Split('_')[1];
                async = Application.LoadLevelAdditiveAsync(sceneName);
            }
        }

        public void LoadSceneAsync(string sceneName)
        {
            async = Application.LoadLevelAsync(sceneName);
        }

        public void LoadSceneAsync(int sceneBuildIndex)
        {
            async = Application.LoadLevelAsync(sceneBuildIndex);
        }

        public void LoadLevelAdditiveAsync(string sceneName)
        {
            async = Application.LoadLevelAdditiveAsync(sceneName);
        }

        public void LoadLevelAdditiveAsync(int sceneBuildIndex)
        {
            async = Application.LoadLevelAdditiveAsync(sceneBuildIndex);
        }

        public void LoadLevel(int levelNumber)
        {
            sceneName = levelSceneName + levelNumber;
            async = Application.LoadLevelAdditiveAsync(sceneName);
        }
        #endregion
#endif
        #endregion

        #region SceneLoader for Unity 5.3 and up
#if (UNITY_5_1 == false && UNITY_5_2 == false)
        #region Event Listeners - uses SceneManager.LoadScene...

        public void OnGameEvent(GameEventMessage m)
        {
            if (m.command.Contains(command_LoadSceneAsync_SceneName))
            {
                sceneName = m.command.Split('_')[2];
                LoadSceneAsync(sceneName);
            }
            else if (m.command.Contains(command_LoadSceneAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(m.command.Split('_')[2]);
                LoadSceneAsync(sceneBuildIndex);
            }
            else if (m.command.Contains(command_LoadSceneAdditiveAsync_SceneName))
            {
                sceneName = m.command.Split('_')[2];
                LoadLevelAdditiveAsync(sceneName);
            }
            else if (m.command.Contains(command_LoadSceneAdditiveAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(m.command.Split('_')[2]);
                LoadLevelAdditiveAsync(sceneBuildIndex);
            }
            else if (m.command.Contains(command_UnloadScene_SceneName))
            {
                sceneName = m.command.Split('_')[2];
                UnloadScene(sceneName);
            }
            else if (m.command.Contains(command_UnloadScene_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(m.command.Split('_')[2]);
                UnloadScene(sceneBuildIndex);
            }
            else if (m.command.Contains(command_LoadLevel))  //SHORTCUT VARIANT - we just call LoadLevel_{LevelNumber} and we load additive async the level data
            {
                sceneName = levelSceneName + m.command.Split('_')[1];
                async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else if (m.command.Contains(command_UnloadLevel))    ///SHORTCUT VARIANT - we just call UnloadLevel_{LevelNumber} and we unload the level data
            {
                sceneName = levelSceneName + m.command.Split('_')[1];
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }

        public void LoadSceneAsync(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public void LoadSceneAsync(int sceneBuildIndex)
        {
            async = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        }

        public void LoadLevelAdditiveAsync(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void LoadLevelAdditiveAsync(int sceneBuildIndex)
        {
            async = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        public void UnloadScene(string sceneName)
        {
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
        }

        public void UnloadScene(int sceneBuildIndex)
        {
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneBuildIndex);
#else
                SceneManager.UnloadScene(sceneBuildIndex);
#endif
        }

        public void LoadLevel(int levelNumber)
        {
            sceneName = levelSceneName + levelNumber;
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void UnloadLevel(int levelNumber)
        {
            sceneName = levelSceneName + levelNumber;
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
        }

        #endregion
#endif
        #endregion
    }
}

