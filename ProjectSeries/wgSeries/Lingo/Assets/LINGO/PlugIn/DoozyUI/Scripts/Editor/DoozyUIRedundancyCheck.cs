// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

#if (UNITY_5_1 == false && UNITY_5_2 == false)
using UnityEditor.SceneManagement;
#endif

namespace DoozyUI
{
    //[InitializeOnLoad]
    public class DoozyUIRedundancyCheck
    {
        #region MENU ITEMS
        [MenuItem("DoozyUI/Tools/Redundancy Checks/UIElements Redundancy Check", false, 1)]
        static void UIElementsRedundancyCheck()
        {
            Debug.Log("[DoozyUI] Executing UIElements redundancy check.");
            CheckAllTheUIElements();
            Debug.Log("[DoozyUI] Finished UIElements redundancy check.");
        }

        [MenuItem("DoozyUI/Tools/Redundancy Checks/UIButtons Redundancy Check", false, 1)]
        static void UIButtonsRedundancyCheck()
        {
            Debug.Log("[DoozyUI] Executing UIButtons redundancy check.");
            CheckAllTheUIButtons();
            Debug.Log("[DoozyUI] Finished UIButtons redundancy check.");
        }
        #endregion

        private static string currentScene;

        static DoozyUIRedundancyCheck()
        {
            //we want the scene to be fully loaded before the startup operation
            //(for example to be able to use Object.FindObjectsOfType)
            //we defer the logic until the first editor update
            EditorApplication.update += RunOnce; //runs on Editor update (after the editor compiled the asemblies)

            UpdateCurrentScene();

            //On complex UI's this would cause lags in the editor. Disabled in 2.4.1
            EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged; //we use this to run the code on scene change (in the editor); this is useful if the user (dev) loads a new scene and we need to check our component's integrity
        }

        private static void OnHierarchyWindowChanged()
        {
            if (string.IsNullOrEmpty(currentScene)) //we just started Unity so we do not run this because the EditorApplication.update will handle it
            {
                UpdateCurrentScene();
            }

#if (UNITY_5_1 == false && UNITY_5_2 == false)
            if (currentScene != EditorSceneManager.GetActiveScene().name) //a scene change has happened
#else
        if (currentScene != EditorApplication.currentScene) //a scene change has happened
#endif
            {
                //Debug.Log("Previous Scene Name: " + currentScene);
                UpdateCurrentScene();

                StartTheRedundancyCheck();
            }
        }

        /// <summary>
        /// Updates the currentScene name by taking into account the Unity version
        /// </summary>
        static void UpdateCurrentScene()
        {
#if (UNITY_5_1 == false && UNITY_5_2 == false)
            currentScene = EditorSceneManager.GetActiveScene().name;
#else
        currentScene = EditorApplication.currentScene;
#endif
        }

        static void RunOnce()
        {

            //Debug.Log("RunOnce!");
            EditorApplication.update -= RunOnce; //we stop listening for editor update (it will be run again only after the editor has recomplied the assemblies)

            StartTheRedundancyCheck();
        }

        static void StartTheRedundancyCheck()
        {
            if (Application.isPlaying)
            {
                return;
            }

            //Debug.Log("[DoozyUI] Checking the database...");
            CheckAllTheUIElements();
            CheckAllTheUIButtons();
            //Debug.Log("[DoozyUI] is ready!");
        }

        #region UIElement
        public static void CheckAllTheUIElements()
        {
            UIElement[] uiElements = Object.FindObjectsOfType<UIElement>(); //get all the UIElements in the current scene

            if (uiElements != null && uiElements.Length > 0)
            {
                for (int i = 0; i < uiElements.Length; i++)
                {
                    UIElementRedundancyCheck(uiElements[i]); //run the redundancy check on each element
                }
            }
        }

        static string CheckIfElementSoundIsInDatabase(string s)
        {
            if (string.IsNullOrEmpty(s)) //check if the string is null or empty
            {
                s = UIManager.DEFAULT_SOUND_NAME; //set default value
            }
            else if (UIManager.GetIndexForElementSound(s) == -1) //the sound name does not exist in the database it got corrupted; we reset it to the default value
            {
                s = UIManager.DEFAULT_SOUND_NAME; //set default value
            }
            return s;
        }

        public static void UIElementRedundancyCheck(UIElement uiElement, bool debugThis = false)
        {
            if (debugThis)
                Debug.Log("[DoozyUI] [UIElement] Starting a redundancy check for '" + uiElement.gameObject.name + "'");

            if (uiElement.GetComponent<Canvas>() == null)
            {
                uiElement.gameObject.AddComponent<Canvas>();
                Debug.Log("[DoozyUI] [RedundancyCheck] [" + uiElement.name + "] The UIElement didn't have a <Canvas> component attached and it was just added automatically. The missing <Canvas> might cause visibility issues.");
            }

            if (uiElement.GetComponent<GraphicRaycaster>() == null)
            {
                uiElement.gameObject.AddComponent<GraphicRaycaster>();
                Debug.Log("[DoozyUI] [RedundancyCheck] [" + uiElement.name + "] The UIElement didn't have a <GraphicRaycaster> component attached and it was just added automatically. The missing <GraphicRaycaster> might make this UIElement not be able to receive clicks/touches.");
            }

            #region ELEMENT NAME
            if (string.IsNullOrEmpty(uiElement.elementName))
            {
                uiElement.elementName = UIManager.DEFAULT_ELEMENT_NAME;
            }
            else if (UIManager.GetIndexForElementName(uiElement.elementName) == -1) //the backup name does not exist in the database it got corrupted; we reset it to the default value
            {
                uiElement.elementName = UIManager.DEFAULT_ELEMENT_NAME;
            }

            //if (uiElement.elementNameReference == null)
            //{
            //    uiElement.elementNameReference = UIManager.GetDoozyUIData.elementNames[UIManager.GetIndexForElementName(UIManager.DEFAULT_ELEMENT_NAME)];
            //}

            //if (uiElement.elementName.Equals(uiElement.elementNameReference.elementName) == false) //the referenced name is not the same as the backup name, we check if a rollbakck is needed
            //{
            //    if (UIManager.GetIndexForElementName(uiElement.elementNameReference.elementName) == -1) //if TRUE, the referenced name does not exist in the database or it's the default value (we might have an error); we need to check that the backup name is not corrupted as well
            //    {
            //        if (UIManager.GetIndexForElementName(uiElement.elementName) == -1) //the backup name does not exist in the database; we perform the rollback
            //        {
            //            uiElement.elementName = UIManager.DEFAULT_ELEMENT_NAME; //we set the backup name as the default name
            //            uiElement.elementNameReference = UIManager.GetDoozyUIData.elementNames[UIManager.GetIndexForElementName(UIManager.DEFAULT_ELEMENT_NAME)]; //we reference the default name from the database
            //        }
            //        else //the referenced name does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
            //        {
            //            uiElement.elementNameReference = UIManager.GetDoozyUIData.elementNames[UIManager.GetIndexForElementName(uiElement.elementName)]; //we update the reference from the database
            //        }
            //    }
            //    else //the referenced name exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup name)
            //    {
            //        uiElement.elementName = uiElement.elementNameReference.elementName; //we update the backup name to the referenced name
            //    }
            //}
            //else
            //{
            //    if (debugThis)
            //    {
            //        Debug.Log("eName [" + uiElement.elementName + "] | eNameRef [" + uiElement.elementNameReference.elementName + "] | index [" + UIManager.GetIndexForElementName(uiElement.elementNameReference.elementName) + "]");
            //    }
            //}
            #endregion

            #region  MOVE IN @START
            uiElement.moveInSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.moveInSoundAtStart);
            if (uiElement.moveInSoundAtStart.Equals(uiElement.moveIn.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveIn.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveInSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveInSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveInSoundAtStart)]; //we update the reference from the database
                    }

                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveInSoundAtStart = uiElement.moveIn.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveIn.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  MOVE IN @FINISH
            uiElement.moveInSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.moveInSoundAtFinish);
            if (uiElement.moveInSoundAtFinish.Equals(uiElement.moveIn.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveIn.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveInSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveInSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveInSoundAtFinish = uiElement.moveIn.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveIn.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  ROTATION IN @START
            uiElement.rotationInSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.rotationInSoundAtStart);
            if (uiElement.rotationInSoundAtStart.Equals(uiElement.rotationIn.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationIn.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationInSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationInSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationInSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationInSoundAtStart = uiElement.rotationIn.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationIn.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  ROTATION IN @FINISH
            uiElement.rotationInSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.rotationInSoundAtFinish);
            if (uiElement.rotationInSoundAtFinish.Equals(uiElement.rotationIn.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationIn.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationInSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationInSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationInSoundAtFinish = uiElement.rotationIn.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationIn.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  SCALE IN @START
            uiElement.scaleInSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.scaleInSoundAtStart);
            if (uiElement.scaleInSoundAtStart.Equals(uiElement.scaleIn.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleIn.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleInSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleInSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleInSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleInSoundAtStart = uiElement.scaleIn.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleIn.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  SCALE IN @FINISH
            uiElement.scaleInSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.scaleInSoundAtFinish);
            if (uiElement.scaleInSoundAtFinish.Equals(uiElement.scaleIn.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleIn.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleInSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleInSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleInSoundAtFinish = uiElement.scaleIn.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleIn.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  FADE IN @START
            uiElement.fadeInSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.fadeInSoundAtStart);
            if (uiElement.fadeInSoundAtStart.Equals(uiElement.fadeIn.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeIn.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeInSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeInSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeIn.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeInSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeInSoundAtStart = uiElement.fadeIn.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeIn.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  FADE IN @FINISH
            uiElement.fadeInSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.fadeInSoundAtFinish);
            if (uiElement.fadeInSoundAtFinish.Equals(uiElement.fadeIn.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeIn.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeInSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeIn.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeInSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeInSoundAtFinish = uiElement.fadeIn.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeIn.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  MOVE LOOP @START
            uiElement.moveLoopSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.moveLoopSoundAtStart);
            if (uiElement.moveLoopSoundAtStart.Equals(uiElement.moveLoop.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveLoop.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveLoopSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveLoopSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveLoopSoundAtStart = uiElement.moveLoop.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveLoop.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  MOVE LOOP @FINISH
            uiElement.moveLoopSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.moveLoopSoundAtFinish);
            if (uiElement.moveLoopSoundAtFinish.Equals(uiElement.moveLoop.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveLoop.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveLoopSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveLoopSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveLoopSoundAtFinish = uiElement.moveLoop.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveLoop.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  ROTATION LOOP @START
            uiElement.rotationLoopSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.rotationLoopSoundAtStart);
            if (uiElement.rotationLoopSoundAtStart.Equals(uiElement.rotationLoop.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationLoop.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationLoopSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationLoopSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationLoopSoundAtStart = uiElement.rotationLoop.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationLoop.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  ROTATION LOOP @FINISH
            uiElement.rotationLoopSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.rotationLoopSoundAtFinish);
            if (uiElement.rotationLoopSoundAtFinish.Equals(uiElement.rotationLoop.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationLoop.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationLoopSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationLoopSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationLoopSoundAtFinish = uiElement.rotationLoop.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationLoop.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  SCALE LOOP @START
            uiElement.scaleLoopSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.scaleLoopSoundAtStart);
            if (uiElement.scaleLoopSoundAtStart.Equals(uiElement.scaleLoop.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleLoop.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleLoopSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleLoopSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleLoopSoundAtStart = uiElement.scaleLoop.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleLoop.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  SCALE LOOP @FINISH
            uiElement.scaleLoopSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.scaleLoopSoundAtFinish);
            if (uiElement.scaleLoopSoundAtFinish.Equals(uiElement.scaleLoop.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleLoop.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleLoopSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleLoopSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleLoopSoundAtFinish = uiElement.scaleLoop.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleLoop.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  FADE LOOP @START
            uiElement.fadeLoopSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.fadeLoopSoundAtStart);
            if (uiElement.fadeLoopSoundAtStart.Equals(uiElement.fadeLoop.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeLoop.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeLoopSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeLoop.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeLoopSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeLoopSoundAtStart = uiElement.fadeLoop.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeLoop.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  FADE LOOP @FINISH
            uiElement.fadeLoopSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.fadeLoopSoundAtFinish);
            if (uiElement.fadeLoopSoundAtFinish.Equals(uiElement.fadeLoop.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeLoop.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeLoopSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeLoop.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeLoopSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeLoopSoundAtFinish = uiElement.fadeLoop.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeLoop.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  MOVE OUT @START
            uiElement.moveOutSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.moveOutSoundAtStart);
            if (uiElement.moveOutSoundAtStart.Equals(uiElement.moveOut.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveOut.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveOutSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveOutSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveOutSoundAtStart = uiElement.moveOut.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveOut.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  MOVE OUT @FINISH
            uiElement.moveOutSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.moveOutSoundAtFinish);
            if (uiElement.moveOutSoundAtFinish.Equals(uiElement.moveOut.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.moveOut.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.moveOutSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.moveOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.moveOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.moveOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.moveOutSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.moveOutSoundAtFinish = uiElement.moveOut.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.moveOut.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  ROTATION OUT @START
            uiElement.rotationOutSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.rotationOutSoundAtStart);
            if (uiElement.rotationOutSoundAtStart.Equals(uiElement.rotationOut.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationOut.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationOutSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationOutSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationOutSoundAtStart = uiElement.rotationOut.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationOut.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  ROTATION OUT @FINISH
            uiElement.rotationOutSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.rotationOutSoundAtFinish);
            if (uiElement.rotationOutSoundAtFinish.Equals(uiElement.rotationOut.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.rotationOut.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.rotationOutSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.rotationOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.rotationOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.rotationOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.rotationOutSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.rotationOutSoundAtFinish = uiElement.rotationOut.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.rotationOut.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  SCALE OUT @START
            uiElement.scaleOutSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.scaleOutSoundAtStart);
            if (uiElement.scaleOutSoundAtStart.Equals(uiElement.scaleOut.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleOut.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleOutSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleOutSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleOutSoundAtStart = uiElement.scaleOut.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleOut.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  SCALE OUT @FINISH
            uiElement.scaleOutSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.scaleOutSoundAtFinish);
            if (uiElement.scaleOutSoundAtFinish.Equals(uiElement.scaleOut.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.scaleOut.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.scaleOutSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.scaleOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.scaleOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.scaleOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.scaleOutSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.scaleOutSoundAtFinish = uiElement.scaleOut.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.scaleOut.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            #region  FADE OUT @START
            uiElement.fadeOutSoundAtStart = CheckIfElementSoundIsInDatabase(uiElement.fadeOutSoundAtStart);
            if (uiElement.fadeOutSoundAtStart.Equals(uiElement.fadeOut.soundAtStartReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeOut.soundAtStartReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeOutSoundAtStart) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeOut.soundAtStartReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeOutSoundAtStart)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeOutSoundAtStart = uiElement.fadeOut.soundAtStartReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeOut.soundAtStart = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion
            #region  FADE OUT @FINISH
            uiElement.fadeOutSoundAtFinish = CheckIfElementSoundIsInDatabase(uiElement.fadeOutSoundAtFinish);
            if (uiElement.fadeOutSoundAtFinish.Equals(uiElement.fadeOut.soundAtFinishReference.soundName) == false) //the referenced sound is not the same as the backup name, we check if a rollbakck is needed
            {
                if (UIManager.GetIndexForElementSound(uiElement.fadeOut.soundAtFinishReference.soundName) == -1) //if TRUE, the referenced sound does not exist in the database (we might have an error); we need to check that the backup sound is not corrupted as well
                {
                    if (UIManager.GetIndexForElementSound(uiElement.fadeOutSoundAtFinish) == -1) //the backup sound does not exist in the database or it is the default sound; we perform the rollback
                    {
                        uiElement.fadeOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound as the default sound
                        uiElement.fadeOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(UIManager.DEFAULT_SOUND_NAME)];  //we reference the default sound from the database
                    }
                    else //the referenced sound does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
                    {
                        uiElement.fadeOut.soundAtFinishReference = UIManager.GetDoozyUIData.elementSounds[UIManager.GetIndexForElementSound(uiElement.fadeOutSoundAtFinish)]; //we update the reference from the database
                    }
                }
                else //the referenced sound exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup sound)
                {
                    uiElement.fadeOutSoundAtFinish = uiElement.fadeOut.soundAtFinishReference.soundName; //we update the backup sound to the referenced sound
                }
            }
            uiElement.fadeOut.soundAtFinish = string.Empty; //this is left from DoozyUI 1.2 we clear it out
            #endregion

            if (debugThis)
            {
                Debug.Log("[DoozyUI] [UIElement] Redundancy check for '" + uiElement.gameObject.name + "' finished!");
            }
        }
        #endregion

        #region UIButton

        public static void CheckAllTheUIButtons()
        {
            UIButton[] uiButtons = Object.FindObjectsOfType<UIButton>(); //get all the UIButtons in the current scene

            if (uiButtons != null && uiButtons.Length > 0)
            {
                for (int i = 0; i < uiButtons.Length; i++)
                {
                    UIButtonRedundancyCheck(uiButtons[i]); //run the redundancy check on each element
                }
            }
        }

        public static void UIButtonRedundancyCheck(UIButton uiButton, bool debugThis = false)
        {
            if (debugThis)
            {
                Debug.Log("[DoozyUI] [UIButton] Starting a redundancy check for '" + uiButton.gameObject.name + "'");
            }

            #region BUTTON NAME
            if (string.IsNullOrEmpty(uiButton.buttonName))
            {
                uiButton.buttonName = UIManager.DEFAULT_BUTTON_NAME;
            }
            else if (UIManager.GetIndexForButtonName(uiButton.buttonName) == -1) //the backup name does not exist in the database it got corrupted; we reset it to the default value
            {
                uiButton.buttonName = UIManager.DEFAULT_BUTTON_NAME;
            }

            //if (uiButton.buttonNameReference == null)
            //{
            //    uiButton.buttonNameReference = UIManager.GetDoozyUIData.buttonNames[UIManager.GetIndexForButtonName(UIManager.DEFAULT_BUTTON_NAME)];
            //}

            //if (uiButton.buttonName.Equals(uiButton.buttonNameReference.buttonName) == false) //the referenced name is not the same as the backup name, we check if a rollbakck is needed
            //{
            //    if (UIManager.GetIndexForButtonName(uiButton.buttonNameReference.buttonName) == -1) //if TRUE, the referenced name does not exist in the database (we might have an error); we need to check that the backup name is not corrupted as well
            //    {
            //        if (UIManager.GetIndexForButtonName(uiButton.buttonName) == -1) //the backup name does not exist in the database; we perform the rollback
            //        {
            //            uiButton.buttonName = UIManager.DEFAULT_BUTTON_NAME; //we set the backup name as the default name
            //            uiButton.buttonNameReference = UIManager.GetDoozyUIData.buttonNames[UIManager.GetIndexForButtonName(UIManager.DEFAULT_BUTTON_NAME)]; //we reference the default name from the database
            //        }
            //        else //the referenced name does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
            //        {
            //            uiButton.buttonNameReference = UIManager.GetDoozyUIData.buttonNames[UIManager.GetIndexForButtonName(uiButton.buttonName)]; //we update the reference from the database
            //        }
            //    }
            //    else //the referenced name exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup name)
            //    {
            //        uiButton.buttonName = uiButton.buttonNameReference.buttonName; //we update the backup name to the referenced name
            //    }
            //}
            //else
            //{
            //    if (debugThis)
            //    {
            //        Debug.Log("bName [" + uiButton.buttonName + "] | bNameRef [" + uiButton.buttonNameReference.buttonName + "] | index [" + UIManager.GetIndexForButtonName(uiButton.buttonNameReference.buttonName) + "]");
            //    }
            //}
            #endregion

            #region OnClick Sound
            if (string.IsNullOrEmpty(uiButton.onClickSound))
            {
                uiButton.onClickSound = UIManager.DEFAULT_SOUND_NAME;
            }
            else if (UIManager.GetIndexForButtonSound(uiButton.onClickSound) == -1) //the backup sound name does not exist in the database it got corrupted; we reset it to the default value
            {
                uiButton.onClickSound = UIManager.DEFAULT_SOUND_NAME;
            }

            //if (uiButton.onClickSoundReference == null)
            //{
            //    uiButton.onClickSoundReference = UIManager.GetDoozyUIData.buttonSounds[UIManager.GetIndexForButtonSound(UIManager.DEFAULT_SOUND_NAME)];
            //}

            //if (uiButton.onClickSound.Equals(uiButton.onClickSoundReference.onClickSound) == false) //the referenced sound name is not the same as the backup name, we check if a rollbakck is needed
            //{
            //    if (UIManager.GetIndexForButtonSound(uiButton.onClickSoundReference.onClickSound) == -1) //if TRUE, the referenced button name does not exist in the database (we might have an error); we need to check that the backup name is not corrupted as well
            //    {
            //        if (UIManager.GetIndexForButtonSound(uiButton.onClickSound) == -1) //the backup sound name does not exist in the database; we perform the rollback
            //        {
            //            uiButton.onClickSound = UIManager.DEFAULT_SOUND_NAME; //we set the backup sound name as the default name
            //            uiButton.onClickSoundReference = UIManager.GetDoozyUIData.buttonSounds[UIManager.GetIndexForButtonSound(UIManager.DEFAULT_SOUND_NAME)]; //we reference the default sound name from the database
            //        }
            //        else //the referenced sound name does not exit (we had a serialization error), but the backup is fine (we found it in the database) --> we update the reference
            //        {
            //            uiButton.onClickSoundReference = UIManager.GetDoozyUIData.buttonSounds[UIManager.GetIndexForButtonSound(uiButton.onClickSound)]; //we update the reference from the database
            //        }
            //    }
            //    else //the referenced sound name exists in the database, so we update the backup (this is for upgrade purposes, so that we get the proper backup name)
            //    {
            //        uiButton.onClickSound = uiButton.onClickSoundReference.onClickSound; //we update the backup sound name to the referenced name
            //    }
            //}
            //else
            //{
            //    if (debugThis)
            //    {
            //        Debug.Log("onClickSound [" + uiButton.onClickSound + "] | onClickSoundRef [" + uiButton.onClickSoundReference.onClickSound + "] | index [" + UIManager.GetIndexForButtonSound(uiButton.onClickSoundReference.onClickSound) + "]");
            //    }
            //}
            #endregion

            #region NORMAL ANIMATIONS
            uiButton.normalAnimationSettings.moveLoop.soundAtStartReference = null;
            uiButton.normalAnimationSettings.moveLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.moveLoop.soundAtStart = string.Empty;
            uiButton.normalAnimationSettings.moveLoop.soundAtFinishReference = null;
            uiButton.normalAnimationSettings.moveLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.moveLoop.soundAtFinish = string.Empty;

            uiButton.normalAnimationSettings.rotationLoop.soundAtStartReference = null;
            uiButton.normalAnimationSettings.rotationLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.rotationLoop.soundAtStart = string.Empty;
            uiButton.normalAnimationSettings.rotationLoop.soundAtFinishReference = null;
            uiButton.normalAnimationSettings.rotationLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.rotationLoop.soundAtFinish = string.Empty;

            uiButton.normalAnimationSettings.scaleLoop.soundAtStartReference = null;
            uiButton.normalAnimationSettings.scaleLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.scaleLoop.soundAtStart = string.Empty;
            uiButton.normalAnimationSettings.scaleLoop.soundAtFinishReference = null;
            uiButton.normalAnimationSettings.scaleLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.scaleLoop.soundAtFinish = string.Empty;

            uiButton.normalAnimationSettings.fadeLoop.soundAtStartReference = null;
            uiButton.normalAnimationSettings.fadeLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.fadeLoop.soundAtStart = string.Empty;
            uiButton.normalAnimationSettings.fadeLoop.soundAtFinishReference = null;
            uiButton.normalAnimationSettings.fadeLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.normalAnimationSettings.fadeLoop.soundAtFinish = string.Empty;
            #endregion

            #region HIGHLIGHTED ANIMATIONS
            uiButton.highlightedAnimationSettings.moveLoop.soundAtStartReference = null;
            uiButton.highlightedAnimationSettings.moveLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.moveLoop.soundAtStart = string.Empty;
            uiButton.highlightedAnimationSettings.moveLoop.soundAtFinishReference = null;
            uiButton.highlightedAnimationSettings.moveLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.moveLoop.soundAtFinish = string.Empty;

            uiButton.highlightedAnimationSettings.rotationLoop.soundAtStartReference = null;
            uiButton.highlightedAnimationSettings.rotationLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.rotationLoop.soundAtStart = string.Empty;
            uiButton.highlightedAnimationSettings.rotationLoop.soundAtFinishReference = null;
            uiButton.highlightedAnimationSettings.rotationLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.rotationLoop.soundAtFinish = string.Empty;

            uiButton.highlightedAnimationSettings.scaleLoop.soundAtStartReference = null;
            uiButton.highlightedAnimationSettings.scaleLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.scaleLoop.soundAtStart = string.Empty;
            uiButton.highlightedAnimationSettings.scaleLoop.soundAtFinishReference = null;
            uiButton.highlightedAnimationSettings.scaleLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.scaleLoop.soundAtFinish = string.Empty;

            uiButton.highlightedAnimationSettings.fadeLoop.soundAtStartReference = null;
            uiButton.highlightedAnimationSettings.fadeLoop.soundAtStartReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.fadeLoop.soundAtStart = string.Empty;
            uiButton.highlightedAnimationSettings.fadeLoop.soundAtFinishReference = null;
            uiButton.highlightedAnimationSettings.fadeLoop.soundAtFinishReference = new UIAnimator.SoundDetails();
            uiButton.highlightedAnimationSettings.fadeLoop.soundAtFinish = string.Empty;
            #endregion

            //#region SHOW ELEMENTS
            //if (uiButton.showElements != null && uiButton.showElements.Count > 0)
            //{
            //    for (int i = 0; i < uiButton.showElements.Count; i++)
            //    {
            //        if (UIManager.GetIndexForElementName(uiButton.showElements[i]) == -1) //something went wrong a we are asking to show an element name that is not in the database
            //        {
            //            uiButton.showElements[i] = UIManager.DEFAULT_ELEMENT_NAME;
            //        }
            //    }
            //}
            //#endregion

            //#region HIDE ELEMENTS
            //if (uiButton.hideElements != null && uiButton.hideElements.Count > 0)
            //{
            //    for (int i = 0; i < uiButton.hideElements.Count; i++)
            //    {
            //        if (UIManager.GetIndexForElementName(uiButton.hideElements[i]) == -1) //something went wrong a we are asking to hide an element name that is not in the database
            //        {
            //            uiButton.hideElements[i] = UIManager.DEFAULT_ELEMENT_NAME;
            //        }
            //    }
            //}
            //#endregion

            if (debugThis)
            {
                Debug.Log("[DoozyUI] [UIButton] Redundancy check for '" + uiButton.gameObject.name + "' finished!");
            }
        }

        #endregion
    }
}
