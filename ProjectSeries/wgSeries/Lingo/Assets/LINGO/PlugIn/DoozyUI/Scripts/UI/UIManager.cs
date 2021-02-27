// Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class UIManager : Singleton<UIManager>
    {
        protected UIManager()
        { }

        #region UIManager Classes - Navigation, UIScreenRect
        [System.Serializable]
        public class Navigation
        {
            public List<string> showElements;
            public List<string> hideElements;
        }

        [System.Serializable]
        public class UIScreenRect
        {
            public Vector2 size = Vector2.zero;
            public Vector2 position = Vector2.zero;
        }
        #endregion

        #region Default DoozyUIData Values - the reset values for ElementNames and ButtonNames
        private static string[] INIT_ELEMENT_NAMES = new string[] { "MainMenu", "QuitMenu", "PauseMenu", "InGameHud", "SettingsMenu", "ShopMenu", "SoundON", "SoundOFF", "MusicON", "MusicOFF", "Tab_1", "Tab_2", "Tab_3" };
        private static string[] INIT_BUTTON_NAMES = new string[] { "Back", "GoToMainMenu", "GoToQuitMenu", "GoToPauseMenu", "GoToShopMenu", "GoToSettingsMenu", "GoToInGameHud", "TogglePause", "ToggleSound", "ToggleMusic", "ApplicationQuit", "Button_1", "Button_2", "Button_3" };
        #endregion

        #region Const - Default Values
        public const string DEFAULT_ELEMENT_NAME = "~Element Name~";
        public const string DEFAULT_BUTTON_NAME = "~Button Name~";
        public const string DEFAULT_SOUND_NAME = "~No Sound~";
        public const string DISPATCH_ALL = "~Dispatch All~";

        public const string COPYRIGHT = "Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.";
        public const string VERSION = "Version 2.7p6";
        #endregion

        #region Enums - EventType, Orientation
        public enum EventType { GameEvent, ButtonClick }
        public enum Orientation { Landscape, Portrait, Unknown }
        #endregion

        #region StaticVariables
#if dUI_UseOrientationManager
        public static bool useOrientationManager = true; //activates the checks for screen orientation - Landscape or Portrait
#else
        public static bool useOrientationManager = false; //activates the checks for screen orientation - Landscape or Portrait
#endif
        public static Orientation currentOrientation = Orientation.Unknown;

        /// <summary>
        /// Reference to Unity's EventSystem
        /// </summary>
        public static EventSystem eventSystem = null;
        /// <summary>
        /// Reference to the currently selected button in the EventSystem
        /// </summary>
        public static GameObject currentlySelectedGameObject = null;
        /// <summary>
        /// Reference to the UICamera that comes with DoozyUI
        /// </summary>
        public static Camera uiCamera = null;

        public static bool debugEvents;  //Option to debug GameEvents
        public static bool debugButtons;  //Option to debug ButtonClicks
        public static bool debugNotifications;  //Option to debug Notifications

        public static bool autoDisableButtonClicks = true; //Option to disable button lcick during an UIElement transition (IN/OUT animation) (default: TRUE)

        public static bool backButtonDisabled  //There are cases when we want to disable the 'Back' button functionality
        {
            get
            {
                if (backButtonDisableLevel > 0) { return true; }
                else if (backButtonDisableLevel == 0) { return false; }
                else
                {
                    Debug.LogWarning("[DoozyUI] The backButtonDisableLevel has a negative value. This means that the variable was changed by you in code. This should not happen. You should not handle this variable yourself.");
                    return false;
                }
            }
        }
        private static int backButtonDisableLevel = 0;              //if == 0 --> false (the back button is not disabled); if > 0 --> true (back button is disabled); this is used to create an additive bool

        public static bool buttonClicksDisabled        //The buttons will get disabled when an UIElement is in transition
        {
            get
            {
                if (buttonClicksDisableLevel > 0) { return true; }
                else if (buttonClicksDisableLevel == 0) { return false; }
                else
                {
                    Debug.LogWarning("[DoozyUI] The buttonClicksDisableLevel has a negative value. This means that the variable was changed by you in code. This should not happen. You should not handle this variable yourself.");
                    return false;
                }
            }
        }
        private static int buttonClicksDisableLevel = 0;        //if == 0 --> false (button clicks are not disabled); if > 0 --> true (button clicks are disabled); this is used to create an additive bool

        public static bool gamePaused = false;                              //Check if the game is paused or not
        public static float currentGameTimeScale = 1;                      //We presume 1, but we check every time the player presses the pause button
        public static float transitionTimeForTimeScaleChange = 0.25f;       //This is the transition time, in seconds, when we pause or unpause the game (looks nicer instead of instant stopping the game)
        private static Stack<Navigation> navStack;                          //This is a stack used for the navigation history

        public static bool isTimeScaleIndependent = true;           //Should the UI ignore game timescale?
        public static bool firstPass = true;                        //Is this the first frame in runtime
        private static Transform uiContainer;                        //The container that has all the ui elements ("UI Container")

        public static bool isSoundOn = true;                        //Sound state
        public static bool isMusicOn = true;                        //Music state

        public static bool usesMA_PlaySoundAndForget = false;       //Should the Play Sound method use MasterAudio method PlaySoundAndForget when trying to play a sound?
        public static bool usesMA_FireCustomEvent = false;          //Should the Play Sound method use MasterAudio method FireCustomEvent when trying to play a sound?
        public static bool usesTMPro = false;                       //Should the UINotification look for TextMeshProUGUI component instead of a Text componenet when looking for text

#if dUI_NavigationDisabled
        public static bool isNavigationEnabled = false;              //Should the UIManager handle the UI Navigation or are you using your own custom navition solution. This also disables the 'Back' button listener
#else
        public static bool isNavigationEnabled = true;              //Should the UIManager handle the UI Navigation or are you using your own custom navition solution. This also disables the 'Back' button listener
#endif
        private static UIScreenRect uiScreenRect;                   //The screen size in pixels (the mesurements are made only once and on the second frame on runtime)

        private static DoozyUI_Data doozyUIData;                       //The data object reference
        private static string dataObjectPath;                        //The path to the data object

        private static Dictionary<string, List<UIElement>> uiElementRegistry = new Dictionary<string, List<UIElement>>();   //A registry that contains all the UIElements in the Hierarchy (they register themselves on OnEnable)
        private static List<UIElement> showElementsList = new List<UIElement>();                                            //When you want to show an UIElement by elementName we need to retrieve the list of all the UIElements, that have the same elementName, from uiElementRegistry and store them in this list. After that we call the Show() method on each of them.
        private static List<UIElement> hideElementsList = new List<UIElement>();                                            //When you want to hide an UIElement by elementName we need to retrieve the list of all the UIElements, that have the same elementName, from uiElementRegistry and store them in this list. After that we call the Hide() method on each of them.
        private static List<string> visibleHideElementsList = new List<string>();                                           //used by the ui navigation system to manage the proper show/hide of UIElements

        private static Dictionary<string, List<UIEffect>> uiEffectRegistry = new Dictionary<string, List<UIEffect>>();      //A registry that contains all the UIEffects in the Hierarchy (they register themselves on OnEnable)
        private static List<UIEffect> showEffectsList = new List<UIEffect>();                                               //When you want to show an UIElement by elementName we also need to retrieve the list of all the UIEffects, that have the same elementName, from uiEffectRegistry and store them in this list. After that we call the Show() method on each of them.
        private static List<UIEffect> hideEffectsList = new List<UIEffect>();                                               //When you want to hide an UIElement by elementName we also need to retrieve the list of all the UIEffects, that have the same elementName, from uiEffectRegistry and store them in this list. After that we call the Hide() method on each of them.

        private static List<UINotification.NotificationData> notificationQueue = new List<UINotification.NotificationData>();      //A registry that contains all the instantiated UINotification in the Hierarchy. It is used to handle the Notifications Queue.

        private static Dictionary<string, List<UITrigger>> gameEventsTriggerRegistry = new Dictionary<string, List<UITrigger>>();           //A registry that contains all the UITriggers in the Hierarchy that listen for GameEvents (they register themselvs on OnEnable)
        private static Dictionary<string, List<UITrigger>> buttonClicksTriggerRegistry = new Dictionary<string, List<UITrigger>>();         //A registry that contains all the UITriggers in the Hierarchy that listen for ButtonClicks (they register themselvs on OnEnable)
        private static List<UITrigger> triggerList = new List<UITrigger>();                                                                 //When you want to trigger an UITrigger by triggerValue this stores the list of triggers that need to be notified. We created this variable so that we don't generate unnecessary garbage collection every time we need a list of UITriggers.

        public static SceneLoader sceneLoader = null;                      //A reference to the scene loader in the Hierarchy. The SceneLoader registeres itself on OnEnable and if you should have 2 scene loaders in the Hierarchy it will send a warning in the console. (nothing will break)
#if dUI_PlayMaker
        private static List<PlaymakerEventDispatcher> playmakerEventDispatcherRegistry = new List<PlaymakerEventDispatcher>();      //A registry that contains all the active PlaymakerEventDispatchers in the Hierarchy (they auto register).
#endif
        #endregion

        #region PublicVariables
        public bool showHelp = false;
        public bool _debugEvents = false;
        public bool _debugButtons = false;
        public bool _debugNotifications = false;
        public bool _autoDisableButtonClicks = true;
        public bool useMasterAudio_PlaySoundAndForget = false;      //Used to change in the inspector the settings for the static variable
        public bool useMasterAudio_FireCustomEvent = false;         //Used to change in the inspector the settings for the static variable
        public bool useTextMeshPro = false;                         //Used to change in the inspector the settings for the static variable
        #endregion

        #region Properties
        /// <summary>
        /// Returns the UICamera that comes with DoozyUI
        /// </summary>
        public static Camera GetUICamera
        {
            get
            {
                if (uiCamera == null)
                {
                    uiCamera = GetUiContainer.transform.parent.GetComponentInChildren<Camera>();
                }
                return uiCamera;
            }
        }

        /// <summary>
        /// Returns Unity's default EventSystem that is active in the current scene
        /// </summary>
        public static EventSystem GetEventSystem
        {
            get
            {
                if (eventSystem == null)
                {
                    eventSystem = FindObjectOfType<EventSystem>() as EventSystem;

                    if (eventSystem == null)
                    {
                        Debug.Log("[DoozyUI] Could not find the EventSystem in the Hierarchy. As Unity's default EventSystem is missing, please add it to fix this issue.");
                    }
                }
                return eventSystem;
            }
        }

        /// <summary>
        /// Returns the UIContainer reference
        /// </summary>
        public static Transform GetUiContainer
        {
            get
            {
                if (uiContainer == null)
                {
                    UpdateUiContainer();
                }
                return uiContainer;
            }
        }

        /// <summary>
        /// Returns the current screens size
        /// </summary>
        public static UIScreenRect GetUIScreenRect
        {
            get
            {
                UpdateUIScreenRect();
                if (firstPass)  //this check is needed since in the first frame of the application the uiScreenRect is (0,0); only from the second frame can we get the screen size values
                {
                    firstPass = false;
                }
                return uiScreenRect;
            }
        }

        /// <summary>
        /// Returns the path to the doozyUIData file
        /// </summary>
        private static string GetDataObjectPath
        {
            get
            {
                if (string.IsNullOrEmpty(dataObjectPath))
                {
                    dataObjectPath = FileHelper.GetRelativeFolderPath("DoozyUI") + "/Data/DoozyUI_Data.asset";
                }
                return dataObjectPath;
            }
        }

        /// <summary>
        /// Retrurns the reference to the doozyUIData scriptable object
        /// </summary>
        public static DoozyUI_Data GetDoozyUIData
        {
            get
            {
#if UNITY_EDITOR
                if (doozyUIData == null)
                {
                    doozyUIData = (DoozyUI_Data)AssetDatabase.LoadAssetAtPath(GetDataObjectPath, typeof(DoozyUI_Data));
                }
#else
                  if (doozyUIData == null)
                {
                    doozyUIData = (DoozyUI_Data) Resources.Load(UIManager.GetDataObjectPath, typeof(DoozyUI_Data));
                }
#endif
                return doozyUIData;
            }
        }
        #endregion

        void Awake()
        {
            //GetUICamera.enabled = false; //Because we can't get the screen size on the first frame, all the UIElements are on the screen. We hide the entire UI for 1 frame in order to fix the issue when the game starts and the player sees the entire UI on the screen.
            InitDoTween();
            UpdateSettings();
            UpdateUiContainer();
            StartCoroutine("GetScreenSize");
            StartCoroutine("GetOrientation");
        }

        void Start()
        {
            SoundCheck();
            MusicCheck();

            currentGameTimeScale = Time.timeScale;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && backButtonDisabled == false) //The listener for the 'Back' button event
            {
                SendButtonClick("Back", false, true);
            }

            if (GetEventSystem.currentSelectedGameObject != currentlySelectedGameObject) //if the user selected a new button
            {
                if (currentlySelectedGameObject != null && currentlySelectedGameObject.GetComponent<UIButton>() != null) //if the previous selected button was not null and had a UIButton component
                {
                    currentlySelectedGameObject.GetComponent<UIButton>().StopHighlightedSteateAnimations();  //if a Highlighted state animation was running we stop it
                    currentlySelectedGameObject.GetComponent<UIButton>().StartNormalStateAnimations();  //if the button has a normal state animation, we start it
                }

                if (GetEventSystem.currentSelectedGameObject != null && GetEventSystem.currentSelectedGameObject.GetComponent<UIButton>() != null) //if the new selection is not null and the object has an UIButton component attached
                {
                    GetEventSystem.currentSelectedGameObject.GetComponent<UIButton>().StopNormalStateAnimations();  //if a normal state animation was running we stop it
                    GetEventSystem.currentSelectedGameObject.GetComponent<UIButton>().StartHighlightedStateAnimations();  //if the button has a highlighted steate animation, we start it
                }

                currentlySelectedGameObject = GetEventSystem.currentSelectedGameObject;  //we rememebr the current selected GameOnject so that we access this code only on a selection change
            }
        }

        void OnEnable()
        {
            AddListeners();
            InitNavigationHistory();
        }

        void OnDisable()
        {
            RemoveListeners();
        }

        #region Methods for Event Listeners

        void AddListeners()
        {
            Message.AddListener<UIButtonMessage>(OnButtonClick);
            Message.AddListener<GameEventMessage>(OnGameEvent);
        }

        void RemoveListeners()
        {
            Message.RemoveListener<UIButtonMessage>(OnButtonClick);
            Message.RemoveListener<GameEventMessage>(OnGameEvent);
        }

        /// <summary>
        /// This is the main Game Event trigger.
        /// </summary>
        private static void OnGameEvent(GameEventMessage m)
        {
            if (debugEvents)
                Debug.Log("[DoozyUI] [UIManager] Received game event [command: " + m.command + "]");

#if dUI_PlayMaker
            DispatchEventToPlaymakerEventDispatchers(m.command, EventType.GameEvent);
#endif
            TriggerTheTriggers(m.command, EventType.GameEvent);

            if (sceneLoader != null)
                sceneLoader.OnGameEvent(m);

            switch (m.command)
            {
                case "ClearNavigationHistory": //cleares the navigation history (now the back button shows only the quit menu)
                    ClearNavigationHistory();
                    break;
                case "DisableBackButton": //disables the back button functionality (for special cases)
                    DisableBackButton();
                    break;
                case "EnableBackButton": //enables the back button functionality (for special cases) (the back button is enabled by default)
                    EnableBackButton();
                    break;
                case "SoundCheck": //does a sound check and shows the proper sound button state
                    SoundCheck();
                    break;
                case "MusicCheck": //does a music check and shows the proper music button state
                    MusicCheck();
                    break;
            }
        }

        /// <summary>
        /// This is the main Button Click trigger.
        /// </summary>
        private static void OnButtonClick(UIButtonMessage m)
        {
            if (debugButtons)
                Debug.Log("[DoozyUI] [UIManager] Received button click [buttonName: " + m.buttonName + "]");

            if (backButtonDisabled && m.backButton) //if the back button is disabled and the user presses the 'Back' button, then we do not send the event further
                return;

#if dUI_PlayMaker
            DispatchEventToPlaymakerEventDispatchers(m.buttonName, EventType.ButtonClick);
#endif
            TriggerTheTriggers(m.buttonName, EventType.ButtonClick);

            if (isNavigationEnabled == false)
                return;

            if (m.backButton)
            {
                BackButtonEvent();
                return;
            }

            switch (m.buttonName)
            {
                case "GoToMainMenu": //goes to the main menu of the application and cleares the navigation history (now the back button shows only the quit menu)
                    ClearNavigationHistory();
                    if (gamePaused)
                    {
                        TogglePause();
                    }
                    break;

                case "TogglePause": //pauses or unpauses the game
                    TogglePause();
                    break;

                case "ToggleSound": //toggles the sound
                    ToggleSound();
                    break;

                case "ToggleMusic": //toggles the music
                    ToggleMusic();
                    break;

                case "ApplicationQuit": //quits the application or, if in editor, exits play mode
                    ApplicationQuit();
                    break;
            }

            UpdateTheNavigationHistory(m.showElements, m.hideElements, m.addToNavigationHistory);
        }

        #endregion

        #region Methods for DoozyUIData - Init, Reset Database, Save Database, New, Rename, Delete, Sort, Search, Remove Duplicates, Get String Array From Database

        #region Init
        public static void InitDoozyUIData()
        {
            doozyUIData = GetDoozyUIData;

            #region Init or Reset index 0 to default values

            #region ElementNames
            if (GetDoozyUIData.elementNames == null || GetDoozyUIData.elementNames.Count == 0) //the list is null or empty, we reset it to it's default values
            {
                ResetDoozyUIDataElementNames(); //we reset the list to it's default values
            }
            else if (GetIndexForElementName(DEFAULT_ELEMENT_NAME) == -1) //we check that the list still has the default value
            {
                NewElementName(DEFAULT_ELEMENT_NAME); //we add the missing default value
            }
            #endregion

            #region ElementSounds
            if (GetDoozyUIData.elementSounds == null || GetDoozyUIData.elementSounds.Count == 0) //the list is null or empty, we reset it to it's default values
            {
                ResetDoozyUIDataElementSounds(); //we reset the list to it's default values
            }
            else if (GetIndexForElementSound(DEFAULT_SOUND_NAME) == -1) //we check that the list still has the default value
            {
                NewElementSound(DEFAULT_SOUND_NAME); //we add the missing default value
            }
            #endregion

            #region ButtonNames
            if (GetDoozyUIData.buttonNames == null || GetDoozyUIData.buttonNames.Count == 0) //the list is null or empty, we reset it to it's default values
            {
                ResetDoozyUIDataButtonNames(); //we reset the list to it's default values
            }
            else if (GetIndexForButtonName(DEFAULT_BUTTON_NAME) == -1) //we check that the list still has the default value
            {
                NewButtonName(DEFAULT_BUTTON_NAME); //we add the missing default value
            }
            #endregion

            #region ButtonSounds
            if (GetDoozyUIData.buttonSounds == null || GetDoozyUIData.buttonSounds.Count == 0) //the list is null or empty, we reset it to it's default values
            {
                ResetDoozyUIDataButtonSounds(); //we reset the list to it's default values
            }
            else if (GetIndexForButtonSound(DEFAULT_SOUND_NAME) == -1) //we check that the list still has the default value
            {
                NewButtonSound(DEFAULT_SOUND_NAME); //we add the missing default value
            }
            #endregion

            #endregion

            #region Sort the lists
            SortElementNames();
            SortElementSounds();
            SortButtonNames();
            SortButtonSounds();
            #endregion

            #region Remove any duplicates
            RemoveDuplicatesFromTheDatabase();
            #endregion
        }
        #endregion

        #region Reset DoozyUI Database

        #region Reset ElementNames
        public static void ResetDoozyUIDataElementNames()
        {
            GetDoozyUIData.elementNames = new List<UIElement.ElementName>(); //we reset the database
            GetDoozyUIData.elementNames.Add(new UIElement.ElementName { elementName = DEFAULT_ELEMENT_NAME }); //we add the default value
            if (INIT_ELEMENT_NAMES != null && INIT_ELEMENT_NAMES.Length > 0) //we add all the init entries
            {
                for (int i = 0; i < INIT_ELEMENT_NAMES.Length; i++)
                {
                    GetDoozyUIData.elementNames.Add(new UIElement.ElementName { elementName = INIT_ELEMENT_NAMES[i] });
                }
            }
            Debug.Log("[DoozyUI] ElementNames database reset to the default values completed.");
            SortElementNames(); //we sort the database
        }
        #endregion

        #region Reset ElementSounds
        public static void ResetDoozyUIDataElementSounds()
        {
            GetDoozyUIData.elementSounds = new List<UIAnimator.SoundDetails>(); //we reset the database
            GetDoozyUIData.elementSounds.Add(new UIAnimator.SoundDetails { soundName = DEFAULT_SOUND_NAME }); //we add the default value
            Debug.Log("[DoozyUI] ElementSounds database reset to the default values completed.");
            SortElementSounds(); //we sort the database
        }
        #endregion

        #region Reset ButtonNames
        public static void ResetDoozyUIDataButtonNames()
        {
            GetDoozyUIData.buttonNames = new List<UIButton.ButtonName>(); //we reset the database
            GetDoozyUIData.buttonNames.Add(new UIButton.ButtonName { buttonName = DEFAULT_BUTTON_NAME }); //we add the default value
            {
                for (int i = 0; i < INIT_BUTTON_NAMES.Length; i++) //we add all the init entries
                {
                    GetDoozyUIData.buttonNames.Add(new UIButton.ButtonName { buttonName = INIT_BUTTON_NAMES[i] });
                }
            }
            Debug.Log("[DoozyUI] ButtonNames database reset to the default values completed.");
            SortButtonNames(); //we sort the database
        }
        #endregion

        #region Reset ButtonSounds
        public static void ResetDoozyUIDataButtonSounds()
        {
            GetDoozyUIData.buttonSounds = new List<UIButton.ButtonSound>(); //we reset the database
            GetDoozyUIData.buttonSounds.Add(new UIButton.ButtonSound { onClickSound = DEFAULT_SOUND_NAME }); //we add the default value
            Debug.Log("[DoozyUI] ButtonSounds database reset to the default values completed.");
            SortButtonSounds(); //we sort the database
        }
        #endregion

        #region Reset ALL DoozyUIData
        public static void ResetDoozyUIData()
        {
            ResetDoozyUIDataElementNames();
            ResetDoozyUIDataElementSounds();
            ResetDoozyUIDataButtonNames();
            ResetDoozyUIDataButtonSounds();
        }
        #endregion

        #endregion

        #region Save Data
        private static void SaveDoozyUIData()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(GetDoozyUIData);
#endif
        }

        #endregion

        #region New, Rename and Delete Methods

        #region ElementNames

        #region New - ElementName
        public static void NewElementName(string s)
        {
            GetDoozyUIData.elementNames.Add(new UIElement.ElementName { elementName = s });
            SortElementNames();
            SaveDoozyUIData();
        }
        #endregion

        #region Rename - ElementName
        public static void RenameElementName(int index, string newName)
        {
            string previousName = GetDoozyUIData.elementNames[index].elementName;

            UIElement[] tempElementsArray = FindObjectsOfType<UIElement>(); //we get all the UIElements in the scene
            List<UIElement> tempElementsList = new List<UIElement>();

            if (tempElementsArray != null && tempElementsArray.Length > 0)
            {
                for (int i = 0; i < tempElementsArray.Length; i++)
                {
                    if (tempElementsArray[i].elementName.Equals(previousName))
                    {
                        tempElementsList.Add(tempElementsArray[i]);
                    }
                }
            }

            UIButton[] tempButtonsArray = FindObjectsOfType<UIButton>(); //because there might be buttons that show/hide elements, we need to get all the UIButtons in the scene as well
            List<UIButton> tempButtonsList = new List<UIButton>();
            if (tempButtonsArray != null && tempButtonsArray.Length > 0)
            {
                for (int i = 0; i < tempButtonsArray.Length; i++)
                {
                    if (DoesUIButtonInfluenceElementName(tempButtonsArray[i], previousName))
                    {
                        tempButtonsList.Add(tempButtonsArray[i]); //we add the button to the list
                    }
                }
            }

            GetDoozyUIData.elementNames[index].elementName = newName;
            SortElementNames();

            if (tempElementsList.Count > 0) //we found UIElements in the scene, that need to be renamed
            {
                for (int i = 0; i < tempElementsList.Count; i++)
                {
                    tempElementsList[i].elementName = newName;
                    //tempElementsList[i].elementNameReference.elementName = newName;
                }
            }

            if (tempButtonsList.Count > 0) //we found UIButtons in the scene that show/hide ElementNames that need to be renamed
            {
                for (int i = 0; i < tempButtonsList.Count; i++)
                {
                    if (tempButtonsList[i].showElements != null && tempButtonsList[i].showElements.Count > 0)
                    {
                        int tempIndex = -1;
                        tempIndex = tempButtonsList[i].showElements.FindIndex(temp => temp == previousName);
                        if (tempIndex != -1) //we found a match
                        {
                            tempButtonsList[i].showElements[tempIndex] = newName;
                        }
                    }

                    if (tempButtonsList[i].hideElements != null && tempButtonsList[i].hideElements.Count > 0)
                    {
                        int tempIndex = -1;
                        tempIndex = tempButtonsList[i].hideElements.FindIndex(temp => temp == previousName);
                        if (tempIndex != -1) //we found a match
                        {
                            tempButtonsList[i].hideElements[tempIndex] = newName;
                        }
                    }
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #region Delete - ElementName
        public static void DeleteElementName(int index)
        {
            string previousName = GetDoozyUIData.elementNames[index].elementName;

            UIElement[] tempElementsArray = FindObjectsOfType<UIElement>(); //we get all the UIElements in the scene
            List<UIElement> tempElementsList = new List<UIElement>();
            if (tempElementsArray != null && tempElementsArray.Length > 0)
            {
                for (int i = 0; i < tempElementsArray.Length; i++)
                {
                    if (tempElementsArray[i].elementName.Equals(previousName))
                    {
                        tempElementsList.Add(tempElementsArray[i]);
                    }
                }
            }

            UIButton[] tempButtonsArray = FindObjectsOfType<UIButton>(); //because there might be buttons that show/hide elements, we need to get all the UIButtons in the scene as well
            List<UIButton> tempButtonsList = new List<UIButton>();
            if (tempButtonsArray != null && tempButtonsArray.Length > 0)
            {
                for (int i = 0; i < tempButtonsArray.Length; i++)
                {
                    if (DoesUIButtonInfluenceElementName(tempButtonsArray[i], previousName))
                    {
                        tempButtonsList.Add(tempButtonsArray[i]); //we add the button to the list
                    }
                }
            }

            GetDoozyUIData.elementNames.RemoveAt(index);
            //no need for sort since the list is already sorted
            if (tempElementsList.Count > 0) //we found UIElements in the scene, that need to be renamed
            {
                for (int i = 0; i < tempElementsList.Count; i++)
                {
                    tempElementsList[i].elementName = DEFAULT_ELEMENT_NAME;
                    //tempElementsList[i].elementNameReference.elementName = DEFAULT_ELEMENT_NAME;
                }
            }

            if (tempButtonsList.Count > 0) //we found UIButtons in the scene that show/hide ElementNames that need to be removed
            {
                for (int i = 0; i < tempButtonsList.Count; i++)
                {
                    if (tempButtonsList[i].showElements != null && tempButtonsList[i].showElements.Count > 0)
                    {
                        int tempIndex = -1;
                        tempIndex = tempButtonsList[i].showElements.FindIndex(temp => temp == previousName);
                        if (tempIndex != -1) //we found a match
                        {
                            tempButtonsList[i].showElements.RemoveAt(tempIndex); //we remove the entry
                        }
                    }

                    if (tempButtonsList[i].hideElements != null && tempButtonsList[i].hideElements.Count > 0)
                    {
                        int tempIndex = -1;
                        tempIndex = tempButtonsList[i].hideElements.FindIndex(temp => temp == previousName);
                        if (tempIndex != -1) //we found a match
                        {
                            tempButtonsList[i].hideElements.RemoveAt(tempIndex); //we remove the entry
                        }
                    }
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #region Helper methods - DoesUIButtonInfluenceElementName
        /// <summary>
        /// Checks if the UIButton has the eName in either showElements list or hideElements list.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="eName"></param>
        /// <returns></returns>
        private static bool DoesUIButtonInfluenceElementName(UIButton b, string eName)
        {
            if (b.showElements != null && b.showElements.Count > 0) //we check if there are any elements listed in the showElements list
            {
                int tempIndex = -1;
                tempIndex = b.showElements.FindIndex(tmp => tmp == eName);
                if (tempIndex != -1) //we found a match
                {
                    return true;
                }
            }

            if (b.hideElements != null && b.hideElements.Count > 0) //we check if there are any elemets listed in the hideElements list
            {
                int tempIndex = -1;
                tempIndex = b.hideElements.FindIndex(tmp => tmp == eName);
                if (tempIndex != -1) //we found a match
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #endregion

        #region ElementSounds

        #region New - ElementSound
        public static void NewElementSound(string s)
        {
            GetDoozyUIData.elementSounds.Add(new UIAnimator.SoundDetails { soundName = s });
            SortElementSounds();
            SaveDoozyUIData();
        }
        #endregion

        #region Rename - ElementSound
        public static void RenameElementSound(int index, string newName)
        {
            UIElement[] tempArray = FindObjectsOfType<UIElement>(); //we get all the UIElements in the scene
            List<UIElement> tempList = new List<UIElement>();
            string previousName = GetDoozyUIData.elementSounds[index].soundName;

            if (tempArray != null && tempArray.Length > 0)
            {
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (
                    #region IN
                            tempArray[i].moveIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                    #region LOOP
                        || tempArray[i].moveLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                    #region OUT
                        || tempArray[i].moveOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                        )
                    {
                        tempList.Add(tempArray[i]);
                    }
                }
            }

            GetDoozyUIData.elementSounds[index].soundName = newName;
            SortElementSounds();

            if (tempList.Count > 0) //we found UIElements in the scene, that need to be renamed
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    #region IN
                    if (tempList[i].moveIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveIn.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].moveIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveIn.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].rotationIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationIn.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].rotationIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationIn.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].scaleIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleIn.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].scaleIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleIn.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].fadeIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeIn.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].fadeIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeIn.soundAtFinishReference.soundName = newName;
                    }
                    #endregion
                    #region LOOP
                    if (tempList[i].moveLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveLoop.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].moveLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveLoop.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].rotationLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationLoop.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].rotationLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationLoop.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].scaleLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleLoop.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].scaleLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleLoop.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].fadeLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeLoop.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].fadeLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeLoop.soundAtFinishReference.soundName = newName;
                    }
                    #endregion
                    #region OUT
                    if (tempList[i].moveOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveOut.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].moveOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveOut.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].rotationOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationOut.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].rotationOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationOut.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].scaleOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleOut.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].scaleOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleOut.soundAtFinishReference.soundName = newName;
                    }

                    if (tempList[i].fadeOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeOut.soundAtStartReference.soundName = newName;
                    }
                    if (tempList[i].fadeOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeOut.soundAtFinishReference.soundName = newName;
                    }
                    #endregion
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #region Delete - ElementSound
        public static void DeleteElementSound(int index)
        {
            UIElement[] tempArray = FindObjectsOfType<UIElement>(); //we get all the UIElements in the scene
            List<UIElement> tempList = new List<UIElement>();
            string previousName = GetDoozyUIData.elementSounds[index].soundName;

            if (tempArray != null && tempArray.Length > 0)
            {
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (
                    #region IN
                            tempArray[i].moveIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeIn.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeIn.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                    #region LOOP
                        || tempArray[i].moveLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeLoop.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeLoop.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                    #region OUT
                        || tempArray[i].moveOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].moveOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].rotationOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].scaleOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeOut.soundAtStartReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                        || tempArray[i].fadeOut.soundAtFinishReference.soundName.Equals(GetDoozyUIData.elementSounds[index].soundName)
                    #endregion
                        )
                    {
                        tempList.Add(tempArray[i]);
                    }
                }
            }

            GetDoozyUIData.elementSounds.RemoveAt(index);
            //no need for sort since the list is already sorted

            if (tempList.Count > 0) //we found UIElements in the scene, that need to be renamed
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    #region IN
                    if (tempList[i].moveIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveIn.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].moveIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveIn.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].rotationIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationIn.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].rotationIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationIn.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].scaleIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleIn.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].scaleIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleIn.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].fadeIn.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeIn.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].fadeIn.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeIn.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    #endregion
                    #region LOOP
                    if (tempList[i].moveLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveLoop.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].moveLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveLoop.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].rotationLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationLoop.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].rotationLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationLoop.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].scaleLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleLoop.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].scaleLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleLoop.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].fadeLoop.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeLoop.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].fadeLoop.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeLoop.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    #endregion
                    #region OUT
                    if (tempList[i].moveOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveOut.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].moveOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].moveOut.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].rotationOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationOut.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].rotationOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].rotationOut.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].scaleOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleOut.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].scaleOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].scaleOut.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }

                    if (tempList[i].fadeOut.soundAtStartReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeOut.soundAtStartReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    if (tempList[i].fadeOut.soundAtFinishReference.soundName.Equals(previousName))
                    {
                        tempList[i].fadeOut.soundAtFinishReference.soundName = DEFAULT_SOUND_NAME;
                    }
                    #endregion
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #endregion

        #region ButtonNames

        #region New - ButtonName
        public static void NewButtonName(string s)
        {
            GetDoozyUIData.buttonNames.Add(new UIButton.ButtonName { buttonName = s });
            SortButtonNames();
            SaveDoozyUIData();
        }
        #endregion

        #region Rename - ButtonName
        public static void RenameButtonName(int index, string newName)
        {
            string previousName = GetDoozyUIData.buttonNames[index].buttonName;

            UIButton[] tempButtonsArray = FindObjectsOfType<UIButton>(); //we get all the UIButtons in the scene
            List<UIButton> tempButtonsList = new List<UIButton>();

            if (tempButtonsArray != null && tempButtonsArray.Length > 0)
            {
                for (int i = 0; i < tempButtonsArray.Length; i++)
                {
                    if (tempButtonsArray[i].buttonName.Equals(previousName))
                    {
                        tempButtonsList.Add(tempButtonsArray[i]);
                    }
                }
            }

            UITrigger[] tempTriggerArray = FindObjectsOfType<UITrigger>(); //because UITriggers may listen for buttonNames, we need to get all of them from the scene as well
            List<UITrigger> tempTriggerList = new List<UITrigger>();

            if (tempTriggerArray != null && tempTriggerArray.Length > 0)
            {
                for (int i = 0; i < tempTriggerArray.Length; i++)
                {
                    if (tempTriggerArray[i].buttonName.Equals(previousName))
                    {
                        tempTriggerList.Add(tempTriggerArray[i]);
                    }
                }
            }

            GetDoozyUIData.buttonNames[index].buttonName = newName;
            SortButtonNames();

            if (tempButtonsList.Count > 0) //we found UIButtons in the scene, that need to be renamed
            {
                for (int i = 0; i < tempButtonsList.Count; i++)
                {
                    tempButtonsList[i].buttonName = newName;
                }
            }

            if (tempTriggerList.Count > 0) //we found UITriggers in the scene, that listen for the button with the previousName; now we need to rename it to the new name
            {
                for (int i = 0; i < tempTriggerList.Count; i++)
                {
                    tempTriggerList[i].buttonName = newName;
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #region Delete - ButtonName
        public static void DeleteButtonName(int index)
        {
            string previousName = GetDoozyUIData.buttonNames[index].buttonName;

            UIButton[] tempButtonsArray = FindObjectsOfType<UIButton>(); //we get all the UIButtons in the scene
            List<UIButton> tempButtonsList = new List<UIButton>();

            if (tempButtonsArray != null && tempButtonsArray.Length > 0)
            {
                for (int i = 0; i < tempButtonsArray.Length; i++)
                {
                    if (tempButtonsArray[i].buttonName.Equals(previousName))
                    {
                        tempButtonsList.Add(tempButtonsArray[i]);
                    }
                }
            }

            UITrigger[] tempTriggerArray = FindObjectsOfType<UITrigger>(); //because UITriggers may listen for buttonNames, we need to get all of them from the scene as well
            List<UITrigger> tempTriggerList = new List<UITrigger>();

            if (tempTriggerArray != null && tempTriggerArray.Length > 0)
            {
                for (int i = 0; i < tempTriggerArray.Length; i++)
                {
                    if (tempTriggerArray[i].buttonName.Equals(previousName))
                    {
                        tempTriggerList.Add(tempTriggerArray[i]);
                    }
                }
            }

            GetDoozyUIData.buttonNames.RemoveAt(index);
            //no need for sort since the list is already sorted
            if (tempButtonsList.Count > 0) //we found UIButtons in the scene, that need to be renamed
            {
                for (int i = 0; i < tempButtonsList.Count; i++)
                {
                    tempButtonsList[i].buttonName = DEFAULT_BUTTON_NAME;
                }
            }

            if (tempTriggerList.Count > 0) //we found UITriggers in the scene, that listen for the button with the previousName; now we need to rename it to the defaultValue and disable them
            {
                for (int i = 0; i < tempTriggerList.Count; i++)
                {
                    tempTriggerList[i].buttonName = DEFAULT_BUTTON_NAME;
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #endregion

        #region ButtonSounds

        #region New - ButtonSound
        public static void NewButtonSound(string s)
        {
            GetDoozyUIData.buttonSounds.Add(new UIButton.ButtonSound { onClickSound = s });
            SortButtonSounds();
            SaveDoozyUIData();
        }
        #endregion

        #region Rename - ButtonSound
        public static void RenameButtonSound(int index, string newName)
        {
            UIButton[] tempArray = FindObjectsOfType<UIButton>(); //we get all the UIButtons in the scene
            List<UIButton> tempList = new List<UIButton>();

            if (tempArray != null && tempArray.Length > 0)
            {
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (tempArray[i].onClickSound.Equals(GetDoozyUIData.buttonSounds[index].onClickSound))
                    {
                        tempList.Add(tempArray[i]);
                    }
                }
            }

            GetDoozyUIData.buttonSounds[index].onClickSound = newName;
            SortButtonSounds();

            if (tempList.Count > 0) //we found UIButtons in the scene, that need to be renamed
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    tempList[i].onClickSound = newName;
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #region Delete - ButtonSound
        public static void DeleteButtonSound(int index)
        {
            UIButton[] tempArray = FindObjectsOfType<UIButton>(); //we get all the UIButtons in the scene
            List<UIButton> tempList = new List<UIButton>();

            if (tempArray != null && tempArray.Length > 0)
            {
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (tempArray[i].onClickSound.Equals(GetDoozyUIData.buttonSounds[index].onClickSound))
                    {
                        tempList.Add(tempArray[i]);
                    }
                }
            }

            GetDoozyUIData.buttonSounds.RemoveAt(index);
            //no need for sort since the list is already sorted
            if (tempList.Count > 0) //we found UIButtons in the scene, that need to be renamed
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    tempList[i].onClickSound = DEFAULT_SOUND_NAME;
                }
            }
            SaveDoozyUIData();
        }
        #endregion

        #endregion

        #endregion

        #region Sorting Methods
        /// <summary>
        /// Sorts the list alphabetically
        /// </summary>
        public static void SortElementNames()
        {
            UIManager.GetDoozyUIData.elementNames.Sort(
                delegate (UIElement.ElementName element_1, UIElement.ElementName element_2)
                {
                    return element_1.elementName.CompareTo(element_2.elementName);
                });
        }

        /// <summary>
        /// Sorts the list alphabetically
        /// </summary>
        public static void SortElementSounds()
        {
            UIManager.GetDoozyUIData.elementSounds.Sort(
              delegate (UIAnimator.SoundDetails element_1, UIAnimator.SoundDetails element_2)
              {
                  return element_1.soundName.CompareTo(element_2.soundName);
              });
        }

        /// <summary>
        /// Sorts the list alphabetically
        /// </summary>
        public static void SortButtonNames()
        {
            UIManager.GetDoozyUIData.buttonNames.Sort(
                delegate (UIButton.ButtonName element_1, UIButton.ButtonName element_2)
                {
                    return element_1.buttonName.CompareTo(element_2.buttonName);
                });
        }

        /// <summary>
        /// Sorts the list alphabetically
        /// </summary>
        public static void SortButtonSounds()
        {
            UIManager.GetDoozyUIData.buttonSounds.Sort(
                delegate (UIButton.ButtonSound element_1, UIButton.ButtonSound element_2)
                {
                    return element_1.onClickSound.CompareTo(element_2.onClickSound);
                });
        }
        #endregion

        #region Search Methods
        /// <summary>
        /// Searches for s in the list and retruns the index. If not found it returns -1.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetIndexForElementName(string s)
        {
            for (int i = 0; i < UIManager.GetDoozyUIData.elementNames.Count; i++)
            {
                if (UIManager.GetDoozyUIData.elementNames[i].elementName.Equals(s))   //we found a duplicate
                {
                    return i;   //we return the index while stopping this iteration
                }
            }
            return -1;  //we didn't find it so we return -1
        }

        /// <summary>
        /// Searches for s in the list and retruns the index. If not found it returns -1.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetIndexForElementSound(string s)
        {
            for (int i = 0; i < UIManager.GetDoozyUIData.elementSounds.Count; i++)
            {
                if (UIManager.GetDoozyUIData.elementSounds[i].soundName.Equals(s))   //we found a duplicate
                {
                    return i;   //we return the index while stopping this iteration
                }
            }
            return -1;  //we didn't find it so we return -1
        }

        /// <summary>
        /// Searches for s in the list and retruns the index. If not found it returns -1.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetIndexForButtonName(string s)
        {
            for (int i = 0; i < UIManager.GetDoozyUIData.buttonNames.Count; i++)
            {
                if (UIManager.GetDoozyUIData.buttonNames[i].buttonName.Equals(s))   //we found a duplicate
                {
                    return i;   //we return the index while stopping this iteration
                }
            }
            return -1;  //we didn't find it so we return -1
        }

        /// <summary>
        /// Searches for s in the list and retruns the index. If not found it returns -1.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetIndexForButtonSound(string s)
        {
            for (int i = 0; i < UIManager.GetDoozyUIData.buttonSounds.Count; i++)
            {
                if (UIManager.GetDoozyUIData.buttonSounds[i].onClickSound.Equals(s))   //we found a duplicate
                {
                    return i;   //we return the index while stopping this iteration
                }
            }
            return -1;  //we didn't find it so we return -1
        }
        #endregion

        #region Remove Duplicates
        public static void RemoveDuplicatesFromTheDatabase()
        {
            UIManager.GetDoozyUIData.elementNames = UIManager.GetDoozyUIData.elementNames.Distinct().ToList();
            UIManager.GetDoozyUIData.elementSounds = UIManager.GetDoozyUIData.elementSounds.Distinct().ToList();
            UIManager.GetDoozyUIData.buttonNames = UIManager.GetDoozyUIData.buttonNames.Distinct().ToList();
            UIManager.GetDoozyUIData.buttonSounds = UIManager.GetDoozyUIData.buttonSounds.Distinct().ToList();
        }
        #endregion

        #region Get String Array From Lists
        /// <summary>
        /// Retruns all the element names as a string array
        /// </summary>
        /// <returns></returns>
        public static string[] GetElementNames()
        {
            string[] eNames = new string[GetDoozyUIData.elementNames.Count];
            for (int i = 0; i < GetDoozyUIData.elementNames.Count; i++)
            {
                eNames[i] = GetDoozyUIData.elementNames[i].elementName;
            }

            return eNames;
        }

        /// <summary>
        /// Retruns all the element sounds as a string array
        /// </summary>
        /// <returns></returns>
        public static string[] GetElementSounds()
        {
            string[] eSounds = new string[GetDoozyUIData.elementSounds.Count];
            for (int i = 0; i < GetDoozyUIData.elementSounds.Count; i++)
            {
                eSounds[i] = GetDoozyUIData.elementSounds[i].soundName;
            }

            return eSounds;
        }

        /// <summary>
        /// Retruns all the button names as a string array
        /// </summary>
        /// <returns></returns>
        public static string[] GetButtonNames()
        {
            string[] bNames = new string[GetDoozyUIData.buttonNames.Count];
            for (int i = 0; i < GetDoozyUIData.buttonNames.Count; i++)
            {
                bNames[i] = GetDoozyUIData.buttonNames[i].buttonName;
            }

            return bNames;
        }

        /// <summary>
        /// Retruns all the button sounds as a string array
        /// </summary>
        /// <returns></returns>
        public static string[] GetButtonSounds()
        {
            string[] bSounds = new string[GetDoozyUIData.buttonSounds.Count];
            for (int i = 0; i < GetDoozyUIData.buttonSounds.Count; i++)
            {
                bSounds[i] = GetDoozyUIData.buttonSounds[i].onClickSound;
            }

            return bSounds;
        }
        #endregion

        #endregion

        #region Methods for the Orientation Manager - OnRectTransformDimensionsChange, CheckDeviceOrientation, ChangeOrientation

        void OnRectTransformDimensionsChange()
        {
            if (useOrientationManager == false)
                return;

            CheckDeviceOrientation();
        }

        public static void CheckDeviceOrientation()
        {
#if UNITY_EDITOR
            //PORTRAIT
            if (Screen.width < Screen.height)
            {
                if (currentOrientation != Orientation.Portrait) //Orientation changed to PORTRAIT
                {
                    ChangeOrientation(Orientation.Portrait);
                    //Debug.Log("[DoozyUI] Orientation changed to PORTRAIT");
                }
            }
            //LANDSCAPE
            else
            {
                if (currentOrientation != Orientation.Landscape) //Orientation changed to LANDSCAPE
                {
                    ChangeOrientation(Orientation.Landscape);
                    //Debug.Log("[DoozyUI] Orientation changed to LANDSCAPE");
                }
            }
#else
            if (Screen.orientation == ScreenOrientation.Landscape ||
               Screen.orientation == ScreenOrientation.LandscapeLeft ||
               Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                if (currentOrientation != Orientation.Landscape) //Orientation changed to LANDSCAPE
                {
                    ChangeOrientation(Orientation.Landscape);
                }
            }
            else if (Screen.orientation == ScreenOrientation.Portrait ||
                     Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                if (currentOrientation != Orientation.Portrait) //Orientation changed to PORTRAIT
                {
                    ChangeOrientation(Orientation.Portrait);
                }
            }
            else //FALLBACK option if we are in AutoRotate or if we are in Unknown
            {
                ChangeOrientation(Orientation.Landscape);
            }
#endif
        }

        public static void ChangeOrientation(Orientation newOrientation)
        {
            currentOrientation = newOrientation; //we update the current orientation to the new one

            SendGameEvent("DeviceOrientation_" + currentOrientation);

            List<string> visibleUIElementsNames = GetVisibleUIElementElementNames(); //we get the list of all the visible UIElement ElementNames
            if (visibleUIElementsNames != null && visibleUIElementsNames.Count > 0)
            {
                for (int i = 0; i < visibleUIElementsNames.Count; i++)
                {
                    ShowUiElement(visibleUIElementsNames[i], false); //we show instantly all the UIElements with this element name (under the new orientation)
                }
            }
        }

        #endregion

        #region Methods for UIElements - Register, Unregister, GetUiElements, GetVisibleUIElementElementNames, Show, Hide

        /// <summary>
        /// Every UIElement will register itself here on OnEnable.
        /// </summary>
        /// <param name="element"></param>
        public static void RegisterUiElement(UIElement element)
        {
            if (element == null)  //we check that the element is not null (should not happen)
                return;

            if (uiElementRegistry == null) //we check taht the registry is not null (it is null for the first entry)
                uiElementRegistry = new Dictionary<string, List<UIElement>>();

            if (uiElementRegistry.ContainsKey(element.elementName)) //we check if the dictionary has the key (the element's elementName)
            {
                if (uiElementRegistry[element.elementName].Contains(element) == false) //because the registry has the key (the element's elementName), we check that the element is not already registered to the registry with that element (the reference)
                {
                    uiElementRegistry[element.elementName].Add(element); //we add the element reference with the elementName key
                }
            }
            else
            {
                uiElementRegistry.Add(element.elementName, new List<UIElement>() { element }); //because the registry does not contain the elementName key, we add it and we also add the value (the element reference)
            }
        }

        /// <summary>
        /// Every UIElement will unregister itself from here OnDisable and/or OnDestroy.
        /// </summary>
        /// <param name="trigger"></param>
        public static void UnregisterUiElement(UIElement element)
        {
            if (element == null)
                return;

            if (uiElementRegistry == null)
                return;

            if (uiElementRegistry.ContainsKey(element.elementName))
            {
                uiElementRegistry[element.elementName].Remove(element);
            }
        }

        /// <summary>
        /// Returns a List of all UIElements that have a given elementName. If no UIElement with the given elementName is found, it will return null.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static List<UIElement> GetUiElements(string elementName)
        {
            if (uiElementRegistry == null || uiElementRegistry.Count == 0)
            {
                //Debug.Log("[DoozyUI] the uiElementRegisty is null or empty");
                return null;
            }
            else

                if (uiElementRegistry.ContainsKey(elementName))
            {
                return uiElementRegistry[elementName];
            }
            else
            {
                //Debug.Log("[DoozyUI] No UIElement with the the elementName [" + elementName + "] was found. GetUiElements returned null.");
                return new List<UIElement>();
            }
        }

        /// <summary>
        /// Returns a List of all the UIElement ElementNames that are visible on the screen (they have isVisible = true)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetVisibleUIElementElementNames()
        {
            List<UIElement> visibleUIElements = new List<UIElement>();

            if (uiElementRegistry == null || uiElementRegistry.Count == 0)
            {
                //Debug.Log("[DoozyUI] the uiElementRegisty is null or empty");
                return null;
            }


            foreach (KeyValuePair<string, List<UIElement>> kvp in uiElementRegistry)
            {
                if (kvp.Value != null && kvp.Value.Count > 0)
                {
                    for (int i = 0; i < kvp.Value.Count; i++)
                    {
                        if (kvp.Value[i].isVisible)
                        {
                            visibleUIElements.Add(kvp.Value[i]);
                        }
                    }
                }
            }

            if (visibleUIElements == null || visibleUIElements.Count == 0)
            {
                //Debug.Log("[DoozyUI] No visible UIElement was found. GetVisibleUIElements returned null.");
                return null;
            }

            List<string> visibleUIElementsNames = new List<string>(); //we create a list to store all the visible UIElement's ElementNames
            for (int i = 0; i < visibleUIElements.Count; i++)
            {
                if (visibleUIElementsNames.Contains(visibleUIElements[i].elementName) == false)
                {
                    visibleUIElementsNames.Add(visibleUIElements[i].elementName); //we store this value in the visible ElementNames list
                }
            }

            return visibleUIElementsNames;
        }

        /// <summary>
        /// Shows an UIElement by playing the active IN Animations
        /// </summary>
        /// <param name="elementName">The ElementName</param>
        public static void ShowUiElement(string elementName)
        {
            ShowUiElement(elementName, false);
        }

        /// <summary>
        /// Shows an UIElement by playing the active IN Animations
        /// </summary>
        /// <param name="elementName">The ElementName</param>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void ShowUiElement(string elementName, bool instantAction)
        {
            if(elementName.Equals(DEFAULT_ELEMENT_NAME)) { return; }
            showElementsList = GetUiElements(elementName);
            if (showElementsList != null && showElementsList.Count > 0)
            {
                for (int i = 0; i < showElementsList.Count; i++)
                {
                    if (showElementsList[i] != null) //this null check has been added to fix the slim chance that we registered a UIElement to the registry and it has been destroyed/deleted (thus now it's null)
                    {
                        showElementsList[i].gameObject.SetActive(true);

                        if (showElementsList[i].gameObject.activeInHierarchy)
                        {
                            if (useOrientationManager == false)
                            {
                                showElementsList[i].Show(instantAction);
                            }
                            else
                            {
                                if (currentOrientation == Orientation.Landscape)
                                {
                                    if (showElementsList[i].LANDSCAPE)
                                    {
                                        showElementsList[i].Show(instantAction);
                                    }
                                    else if (showElementsList[i].PORTRAIT)
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                    else
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                }
                                else if (currentOrientation == Orientation.Portrait)
                                {
                                    if (showElementsList[i].PORTRAIT)
                                    {
                                        showElementsList[i].Show(instantAction);
                                    }
                                    else if (showElementsList[i].LANDSCAPE)
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                    else
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            showEffectsList = GetUiEffects(elementName);
            if (showEffectsList != null && showEffectsList.Count > 0)
            {
                for (int i = 0; i < showEffectsList.Count; i++)
                {
                    showEffectsList[i].gameObject.SetActive(true);
                    if (showEffectsList[i].gameObject.activeInHierarchy)
                    {
                        if (useOrientationManager == false)
                        {
                            showEffectsList[i].Show();
                        }
                        else
                        {
                            if (currentOrientation == Orientation.Landscape)
                            {
                                if (showEffectsList[i].targetUIElement.LANDSCAPE)
                                {
                                    showEffectsList[i].Show();
                                }
                                else if (showEffectsList[i].targetUIElement.PORTRAIT)
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                                else
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                            }
                            else if (currentOrientation == Orientation.Portrait)
                            {
                                if (showEffectsList[i].targetUIElement.PORTRAIT)
                                {
                                    showEffectsList[i].Show();
                                }
                                else if (showEffectsList[i].targetUIElement.LANDSCAPE)
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                                else
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hides an UIElement by playing the active OUT Animations.
        /// </summary>
        /// <param name="elementName"></param>
        public static void HideUiElement(string elementName)
        {
            HideUiElement(elementName, false, true);
        }

        /// <summary>
        /// Hides an UIElement by playing the active OUT Animations. If instantAction is true, it will hide the element instantly without playing the active OUT Animations.
        /// </summary>
        public static void HideUiElement(string elementName, bool instantAction, bool shouldDisable = true)
        {
            if (elementName.Equals(DEFAULT_ELEMENT_NAME)) { return; }
            hideElementsList = GetUiElements(elementName);
            if (hideElementsList != null && hideElementsList != null && hideElementsList.Count > 0)
            {
                for (int i = 0; i < hideElementsList.Count; i++)
                {
                    if (hideElementsList[i] != null) //this null check has been added to fix the slim chance that we registered a UIElement to the registry and it has been destroyed/deleted (thus now it's null)
                    {
                        if (hideElementsList[i].gameObject.activeInHierarchy)
                        {
                            hideElementsList[i].Hide(instantAction, shouldDisable);
                        }
                    }
                }
            }

            hideEffectsList = GetUiEffects(elementName);
            if (hideEffectsList != null && hideEffectsList.Count > 0)
            {
                for (int i = 0; i < hideEffectsList.Count; i++)
                {
                    if (hideEffectsList[i].gameObject.activeInHierarchy)
                    {
                        hideEffectsList[i].Hide();
                    }
                }
            }
        }

        #endregion

        #region Methods for UITriggers - Register, Unregister, Get, Trigger

        /// <summary>
        /// Every UITrigger will register itself here on OnEnable.
        /// </summary>
        /// <param name="trigger"></param>
        public static void RegisterUiTrigger(UITrigger trigger, EventType triggerType)
        {
            if (trigger == null)
                return;

            switch (triggerType)
            {
                case EventType.GameEvent:
                    if (gameEventsTriggerRegistry == null)
                        gameEventsTriggerRegistry = new Dictionary<string, List<UITrigger>>();

                    if (gameEventsTriggerRegistry.ContainsKey(trigger.gameEvent))
                    {
                        gameEventsTriggerRegistry[trigger.gameEvent].Add(trigger);
                    }
                    else
                    {
                        gameEventsTriggerRegistry.Add(trigger.gameEvent, new List<UITrigger>() { trigger });
                    }
                    break;

                case EventType.ButtonClick:
                    if (buttonClicksTriggerRegistry == null)
                        buttonClicksTriggerRegistry = new Dictionary<string, List<UITrigger>>();

                    if (buttonClicksTriggerRegistry.ContainsKey(trigger.buttonName))
                    {
                        buttonClicksTriggerRegistry[trigger.buttonName].Add(trigger);
                    }
                    else
                    {
                        buttonClicksTriggerRegistry.Add(trigger.buttonName, new List<UITrigger>() { trigger });
                    }
                    break;
            }
        }

        /// <summary>
        /// Every UITrigger will unregister itself from here OnDisable and/or OnDestroy.
        /// </summary>
        /// <param name="trigger"></param>
        public static void UnregisterUiTrigger(UITrigger trigger, EventType triggerType)
        {
            if (trigger == null)
                return;

            switch (triggerType)
            {
                case EventType.GameEvent:
                    if (gameEventsTriggerRegistry == null)
                        return;

                    if (gameEventsTriggerRegistry.ContainsKey(trigger.gameEvent))
                    {
                        gameEventsTriggerRegistry[trigger.gameEvent].Remove(trigger);   //we remove the trigger from the list
                    }
                    break;

                case EventType.ButtonClick:
                    if (buttonClicksTriggerRegistry == null)
                        return;

                    if (buttonClicksTriggerRegistry.ContainsKey(trigger.buttonName))
                    {
                        buttonClicksTriggerRegistry[trigger.buttonName].Remove(trigger);
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns a List of all UITriggers that have a given triggerValue (gameEvent or buttonName). If no UITrigger with the given triggerValue is found, it will return null.
        /// </summary>
        /// <param name="triggerValue"></param>
        /// <returns></returns>
        public static List<UITrigger> GetUiTriggers(string triggerValue, EventType triggerType)
        {
            var tempList = new List<UITrigger>();

            switch (triggerType)
            {
                case EventType.GameEvent:
                    if (gameEventsTriggerRegistry == null || gameEventsTriggerRegistry.Count == 0)
                    {
                        //Debug.Log("[DoozyUI] the gameEventsTriggerRegistry is null or empty");
                        return null;
                    }
                    else
                    {
                        if (gameEventsTriggerRegistry.ContainsKey(triggerValue))
                        {
                            tempList.AddRange(gameEventsTriggerRegistry[triggerValue]);
                        }
                        if (gameEventsTriggerRegistry.ContainsKey(DISPATCH_ALL))
                        {
                            tempList.AddRange(gameEventsTriggerRegistry[DISPATCH_ALL]);
                        }
                        return tempList;
                    }

                case EventType.ButtonClick:
                    if (buttonClicksTriggerRegistry == null || buttonClicksTriggerRegistry.Count == 0)
                    {
                        //Debug.Log("[DoozyUI] the buttonClicksTriggerRegistry is null or empty");
                        return null;
                    }
                    else
                    {
                        if (buttonClicksTriggerRegistry.ContainsKey(triggerValue))
                        {
                            tempList.AddRange(buttonClicksTriggerRegistry[triggerValue]);
                        }
                        if (buttonClicksTriggerRegistry.ContainsKey(DISPATCH_ALL))
                        {
                            tempList.AddRange(buttonClicksTriggerRegistry[DISPATCH_ALL]);
                        }
                        return tempList;
                    }

                default:
                    //Debug.Log("[DoozyUI] GetUiTriggers encountered an unexpected error and returned null. The default state of the switch was triggered. This sould not happen");
                    return null;
            }
        }

        /// <summary>
        /// Triggers all the UITriggers that are registered with the triggerValue (gameEvent or buttonName) of the given triggerType
        /// </summary>
        /// <param name="triggerValue"></param>
        /// <param name="triggerType"></param>
        public static void TriggerTheTriggers(string triggerValue, EventType triggerType)
        {
            triggerList = GetUiTriggers(triggerValue, triggerType);
            if (triggerList != null && triggerList.Count > 0)
            {
                for (int i = 0; i < triggerList.Count; i++)
                {
                    triggerList[i].TriggerTheTrigger(triggerValue);
                }
            }
        }

        #endregion

        #region Methods for UIEffects - Register, Unregister, Get

        /// <summary>
        /// Every UIEffect will register itself here on Awake.
        /// </summary>
        /// <param name="effect"></param>
        public static void RegisterUiEffect(UIEffect effect)
        {
            if (effect == null || effect.targetUIElement == null || string.IsNullOrEmpty(effect.targetUIElement.elementName))
                return;

            if (uiEffectRegistry == null)
                uiEffectRegistry = new Dictionary<string, List<UIEffect>>();

            if (uiEffectRegistry.ContainsKey(effect.targetUIElement.elementName))
            {
                uiEffectRegistry[effect.targetUIElement.elementName].Add(effect);
            }
            else
            {
                uiEffectRegistry.Add(effect.targetUIElement.elementName, new List<UIEffect>() { effect });
            }
        }

        /// <summary>
        /// Every UIEffect will unregister itself from here OnDisable and/or OnDestroy.
        /// </summary>
        /// <param name="effect"></param>
        public static void UnregisterUiEffect(UIEffect effect)
        {
            if (effect == null || effect.targetUIElement == null)
                return;

            if (uiEffectRegistry == null)
                return;

            if (uiEffectRegistry.ContainsKey(effect.targetUIElement.elementName))
            {
                uiEffectRegistry[effect.targetUIElement.elementName].Remove(effect);
            }
        }

        /// <summary>
        /// Returns a List of all UIEffects that have a given elementName. If no UIEffect with the given elementName is found, it will return null.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static List<UIEffect> GetUiEffects(string elementName)
        {
            if (uiEffectRegistry == null || uiEffectRegistry.Count == 0)
            {
                //Debug.Log("[DoozyUI] the uiEffectRegistry is null or empty");
                return null;
            }
            else

                if (uiEffectRegistry.ContainsKey(elementName))
            {
                return uiEffectRegistry[elementName];
            }
            else
            {
                //Debug.Log("[DoozyUI] No UIEffect with the the elementName [" + elementName + "] was found. GetUiEffects returned null.");
                return null;
            }
        }

        #endregion

        #region Methods for UINotifications - Register, Unregister, Show

        /// <summary>
        /// Every notification that needs to enter the Notification Queue will be added to the notificatioQueue list as the last item.
        /// </summary>
        /// <param name="notification"></param>
        private static void RegisterToNotificationQueue(UINotification.NotificationData nData)
        {
            if (nData == null)
                return;

            if (notificationQueue == null)
                notificationQueue = new List<UINotification.NotificationData>();

            notificationQueue.Add(nData); //we add the notification data to the queue

            if (notificationQueue.Count == 1) //because this is the last (and only) notification data in the queue we show the notification now
            {
                ShowNextNotificationInQueue();
            }
        }

        /// <summary>
        /// Unregisteres a notification, by removing the notification data that started it.
        /// </summary>
        /// <param name="nData"></param>
        public static void UnregisterFromNotificationQueue(UINotification.NotificationData nData)
        {
            notificationQueue.Remove(nData);

            if (notificationQueue != null && notificationQueue.Count > 0)
            {
                LoadNotification(notificationQueue[0]);  //We always show the first item in the list because it is the oldest
            }
        }

        /// <summary>
        /// Shows the next notification in the Notification Queue, if there is one.
        /// </summary>
        private static void ShowNextNotificationInQueue()
        {
            if (notificationQueue != null && notificationQueue.Count > 0)  //if the Notification Queue is not null and it has at least 1 notification data in it, we show it
            {
                LoadNotification(notificationQueue[0]); //We always show the first item in the list because it is the oldest
            }
        }

        /// <summary>
        /// Sets up a Notification.
        /// </summary>
        private static UINotification SetupNotification(UINotification.NotificationData nData)
        {
            if (string.IsNullOrEmpty(nData.prefabName) && nData.prefab == null)
            {
                Debug.Log("[DoozyUI] [SetupNotification]: The nPrefabName is null or empty and the nPrefab is null as well. Something went wrong.");
                return null;
            }

            if (nData.addToNotificationQueue)
            {
                RegisterToNotificationQueue(nData);   //We register the notification to the Notification Queue and let it handle it.
                return null;
            }

            return LoadNotification(nData);  //Because we didn't add this notification to the Notification Queue, we show it without adding it to the queue
        }

        /// <summary>
        /// Loads the notification by instatiating the prefab and doing the initial setup to it
        /// </summary>
        /// <param name="nData"></param>
        private static UINotification LoadNotification(UINotification.NotificationData nData)
        {
            GameObject notification = null;

            if (nData.prefab != null) //we have a prefab reference
            {
                notification = (GameObject)Instantiate(nData.prefab);
            }
            else if (string.IsNullOrEmpty(nData.prefabName) == false)//we don't have a prefab reference and we check if we have a prefabName we should be looking for in Resources
            {
                Object notificationPrefab = null;
                try
                {
                    notificationPrefab = Resources.Load(nData.prefabName); //we look for the notification prefab; we do this in a 'try catch' just in case the name was mispelled or the prefab does not exist
                }
                catch (UnityException e)
                {
                    Debug.Log("[DoozyUI] [SetupNotification] [Error]: " + e);
                }

                if (notificationPrefab == null)
                {
                    Debug.Log("[DoozyUI] [SetupNotification]: The notification named [" + nData.prefabName + "] prefab does not exist or is not located under a Resources folder");
                    return null;
                }

                notification = (GameObject)Instantiate(Resources.Load(nData.prefabName, typeof(GameObject)), GetUiContainer.transform.position, Quaternion.identity);
            }
            else //the developer didn't link a prefab, nor did he set a prefabName; this is a fail safe option
            {
                Debug.Log("[DoozyUI] [SetupNotification] [Error]: You are trying to show a notification, but you didn't set neither a prefab reference, nor a prefabName. This is a fail safe debug log. Check your ShowNotification method call and fix this.");
                return null;
            }

            if (notification.GetComponent<UINotification>() == null) //we make sure the notification gameobject has an UINotification component attached (this is a fail safe in case the developer links the wrong prefab)
            {
                Debug.Log("[DoozyUI] [SetupNotification] [Error]: The notification prefab named " + notification.name + " does not have an UINotification component attached. Check if this prefab is really a notification or not.");
                return null;
            }

            notification.transform.SetParent(GetUiContainer, false);
            notification.gameObject.layer = notification.transform.parent.gameObject.layer; //we set the physics layer (just in case)
            UpdateCanvases(notification.gameObject, GetUiContainer.GetComponent<Canvas>().sortingLayerName); //we update the sorting layers for all the canvases (just in case)
            UpdateRenderers(notification.gameObject, GetUiContainer.GetComponent<Canvas>().sortingLayerName); //we update the sorting layers for all the rendereres (just in case)
            RectTransform rt = notification.GetComponent<RectTransform>();
            rt.anchoredPosition = GetUiContainer.GetComponent<RectTransform>().anchoredPosition;
            if (GetUiContainer.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
            notification.GetComponent<UINotification>().ShowNotification(nData);
            return notification.GetComponent<UINotification>();
        }

        #region ShowNotification methods

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            if (debugNotifications)
            {
                if (_lifetime == -1)
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefabName);
                }
                else
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefabName + " for " + _lifetime + " seconds");
                }
            }

            UINotification.NotificationData nData =
                new UINotification.NotificationData()
                {
                    prefabName = _prefabName,
                    lifetime = _lifetime,
                    addToNotificationQueue = _addToNotificationQueue,
                    title = _title,
                    message = _message,
                    icon = _icon,
                    buttonNames = _buttonNames,
                    buttonTexts = _buttonTexts,
                    buttonCallback = _buttonCallback,
                    hideCallback = _hideCallback
                };

            return SetupNotification(nData);
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            if (debugNotifications)
            {
                if (_lifetime == -1)
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefab.name);
                }
                else
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefab.name + " for " + _lifetime + " seconds");
                }
            }

            UINotification.NotificationData nData =
                new UINotification.NotificationData()
                {
                    prefab = _prefab,
                    lifetime = _lifetime,
                    addToNotificationQueue = _addToNotificationQueue,
                    title = _title,
                    message = _message,
                    icon = _icon,
                    buttonNames = _buttonNames,
                    buttonTexts = _buttonTexts,
                    buttonCallback = _buttonCallback,
                    hideCallback = _hideCallback
                };

            return SetupNotification(nData);
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
                ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, UnityAction _hideCallback = null)
        {
            return
                ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                UINotification.defaultIcon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                _icon,
                UINotification.defaultButtonNames,
                UINotification.defaultButtonTexts,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.defaultTitle,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                UINotification.defaultButtonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.defaultMessage,
                UINotification.defaultIcon,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        #endregion

        #endregion

        #region Methods for Playmaker Event Dispatchers - Register, Unregister, DispatchEventToPlaymakerEventDispatchers
#if dUI_PlayMaker

        /// <summary>
        /// Every PlaymakerEventDispatcher will register itself here
        /// </summary>
        public static void RegisterPlaymakerEventDispatcher(PlaymakerEventDispatcher ped)
        {
            if (ped == null)
                return;

            if (playmakerEventDispatcherRegistry == null)
                playmakerEventDispatcherRegistry = new List<PlaymakerEventDispatcher>();

            if (playmakerEventDispatcherRegistry.Contains(ped) == false)
            {
                playmakerEventDispatcherRegistry.Add(ped);
            }
        }

        /// <summary>
        /// Every PlaymakerEventDispatcher will unregister itself from here OnDisable and/or OnDestroy.
        /// </summary>
        public static void UnregisterPlaymakerEventDispatcher(PlaymakerEventDispatcher ped)
        {
            if (ped == null)
                return;

            if (playmakerEventDispatcherRegistry == null)
                return;

            playmakerEventDispatcherRegistry.Remove(ped);
        }

        /// <summary>
        /// Dispatches the eventValue to all the registered dispatchers
        /// </summary>
        public static void DispatchEventToPlaymakerEventDispatchers(string eventValue, EventType eventType)
        {
            if (playmakerEventDispatcherRegistry == null || playmakerEventDispatcherRegistry.Count == 0)
                return;

            for (int i = 0; i < playmakerEventDispatcherRegistry.Count; i++)
            {
                playmakerEventDispatcherRegistry[i].DispatchEvent(eventValue, eventType);
            }
        }

#endif
        #endregion

        #region Methods for Sound and Music - SoundCheck, MusicCheck, ToggleSound, ToggleMusic

        /// <summary>
        /// Checks the soundState when the game starts in the PlayerPrefs
        /// </summary>
        public static void SoundCheck()
        {
            int soundState = PlayerPrefs.GetInt("soundState", 1); //We check if the sound is ON (1) or OFF (0). By default we assume it's 1.

            if (soundState == 1)
            {
                //Sound ON
                isSoundOn = true;             //We set the static variable soundON = true
                ShowUiElement("SoundON");   //We show the SoundON UIElement
                HideUiElement("SoundOFF", true);  //We hide the SoundOFF UIElement
            }
            else
            {
                //Sound OFF
                isSoundOn = false;             //We set the static variable soundON = false
                ShowUiElement("SoundOFF");   //We show the SoundOFF UIElement
                HideUiElement("SoundON", true);    //We hide the SoundON UIElement
            }
            SendGameEvent("UpdateSoundSettings");
        }

        /// <summary>
        /// Checks the musicState when the game starts in the PlayerPrefs
        /// </summary>
        public static void MusicCheck()
        {
            int musicState = PlayerPrefs.GetInt("musicState", 1); //We check if the music is ON (1) or OFF (0). By default we assume it's 1.

            if (musicState == 1)
            {
                //Music ON
                isMusicOn = true;             //We set the static variable isMusicOn = true
                ShowUiElement("MusicON");   //We show the MusicON UIElement
                HideUiElement("MusicOFF", true);  //We hide the MusicOFF UIElement
            }
            else
            {
                //Music OFF
                isMusicOn = false;             //We set the static variable isMusicOn = false
                ShowUiElement("MusicOFF");   //We show the SoundOFF UIElement
                HideUiElement("MusicON", true);    //We hide the MusicOFF UIElement
            }
            SendGameEvent("UpdateSoundSettings");
        }

        /// <summary>
        /// Toggles the soundState and saves it to the PlayerPrefs
        /// </summary>
        public static void ToggleSound()
        {
            isSoundOn = !isSoundOn;

            int soundState = -1;

            if (isSoundOn == true)
            {
                //Sound ON
                soundState = 1;             //Value if the sound is on
                ShowUiElement("SoundON");   //We show the SoundON UIElement
                HideUiElement("SoundOFF");  //We hide the SoundOFF UIElement
            }
            else
            {
                soundState = 0;              //Value if the sound is off
                ShowUiElement("SoundOFF");   //We show the SoundOFF UIElement
                HideUiElement("SoundON");    //We hide the SoundON UIElement
            }

            PlayerPrefs.SetInt("soundState", soundState); //We set the new value in the PlayerPrefs
            PlayerPrefs.Save(); //We save the value

            SendGameEvent("UpdateSoundSettings");
        }

        /// <summary>
        /// Toggles the musicState and saves it to the PlayerPrefs
        /// </summary>
        public static void ToggleMusic()
        {
            isMusicOn = !isMusicOn;

            int musicState = -1;

            if (isMusicOn == true)
            {
                //MUSIC ON
                musicState = 1;             //Value if the music is on
                ShowUiElement("MusicON");   //We show the SoundON UIElement
                HideUiElement("MusicOFF");  //We hide the MusicON UIElement
            }
            else
            {
                //MUSIC OFF
                musicState = 0;              //Value if the music is off
                ShowUiElement("MusicOFF");   //We show the MusicOFF UIElement
                HideUiElement("MusicON");    //We hide the MusicON UIElement
            }

            PlayerPrefs.SetInt("musicState", musicState); //We set the new value in the PlayerPrefs
            PlayerPrefs.Save(); //We save the value

            SendGameEvent("UpdateSoundSettings");
        }

        #endregion

        #region Methods for the Navigation History - InitNavigationHistory, UpdateTheNavigationHistory, AddItemToNavigationHistory, RemoveLastItemFromNavigationHistory, GetLastItemFromNavigationHistory, ClearNavigationHistory

        /// <summary>
        /// Initiates the Navigation History stack.
        /// </summary>
        private static void InitNavigationHistory()
        {
            if (isNavigationEnabled)
                navStack = new Stack<Navigation>();
        }

        /// <summary>
        /// Updates the Navigation History while showing and hiding the relevant UIElements. 
        /// </summary>
        /// <param name="showElements"></param>
        /// <param name="hideElements"></param>
        /// <param name="addToNavigationHistory"></param>
        private static void UpdateTheNavigationHistory(List<string> showElements, List<string> hideElements, bool addToNavigationHistory)
        {
            visibleHideElementsList = new List<string>();
            hideElements = hideElements.Where(s => s.Equals(DEFAULT_ELEMENT_NAME) == false).ToList(); //we remove any default element name values (just in case) | FIXED
            visibleHideElementsList = hideElements.Where(s => GetUiElements(s).Any(element => element.isVisible)).ToList(); //v2.6 fix generously provided by [bomberest] Andrew AnF Shut

            if (showElements != null && showElements.Count > 0)
            {
                for (int i = 0; i < showElements.Count; i++)
                {
                    ShowUiElement(showElements[i]);
                }
            }

            if (hideElements != null && hideElements.Count > 0)
            {
                for (int i = 0; i < hideElements.Count; i++)
                {
                    HideUiElement(hideElements[i]);
                }
            }

            if (addToNavigationHistory)
            {
                Navigation navItem = new Navigation();
                navItem.showElements = new List<string>();
                if (visibleHideElementsList != null && visibleHideElementsList.Count > 0)
                    navItem.showElements = visibleHideElementsList;
                navItem.hideElements = new List<string>();
                navItem.hideElements = showElements;
                navStack.Push(navItem);
            }
        }

        /// <summary>
        /// Adds a navigation item to the Navigation History stack.
        /// </summary>
        /// <param name="navItem"></param>
        public static void AddItemToNavigationHistory(Navigation navItem)
        {
            if (isNavigationEnabled == false)
            {
                Debug.Log("[DoozyUI] [UIManager] [AddItemToNavigationHistory] You are trying to add a navigation item to the Navigation History stack, but the system is disabled. Nothing happened.");
                return;
            }

            if (navStack == null)
                InitNavigationHistory();

            navStack.Push(navItem);
        }

        /// <summary>
        /// Removes the last item from the Navigation History stack.
        /// </summary>
        public static void RemoveLastItemFromNavigationHistory()
        {
            if (isNavigationEnabled == false)
            {
                Debug.Log("[DoozyUI] [UIManager] [RemoveLastItemFromNavigationHistory] You are trying to remove a navigation item from the Navigation History stack, but the system is disabled. Nothing happened.");
                return;
            }

            if (navStack == null)
                InitNavigationHistory();
            else if (navStack.Count == 0)
                return;

            navStack.Pop();
        }

        /// <summary>
        /// Returns the last item in the Navigation History stack. It removes the item from the stack by default.
        /// </summary>
        /// <param name="removeFromStack"></param>
        /// <returns></returns>
        public static Navigation GetLastItemFromNavigationHistory(bool removeFromStack = true)
        {
            if (isNavigationEnabled == false)
            {
                Debug.Log("[DoozyUI] [UIManager] [GetLastItemFromNavigationHistory] You are trying to get the last navigation item from the Navigation History stack, but the system is disabled. Nothing happened.");
                return null;
            }

            if (navStack == null)
                InitNavigationHistory();
            else if (navStack.Count == 0)
                return null;

            if (removeFromStack)
                return navStack.Pop();
            else
                return navStack.Peek();
        }

        /// <summary>
        /// Cleares the Navigation History stack.
        /// </summary>
        public static void ClearNavigationHistory()
        {
            if (isNavigationEnabled == false)
            {
                Debug.Log("[DoozyUI] [UIManager] [ClearNavigationHistory] You are trying to clear the Navigation History stack, but the system is disabled. Nothing happened.");
                return;
            }

            if (navStack == null)
                InitNavigationHistory();

            navStack.Clear();
        }

        #endregion

        #region Methods Game Management - TogglePause, ApplicationQuit, BackButtonEvent, DisableBackButton, EnableBackButton, EnableBackButtonByForce, DisableButtonClicks, EnableButtonClicks, EnableButtonClicksByForce

        /// <summary>
        /// Pauses or Unpauses the application
        /// </summary>
        public static void TogglePause()
        {
            if (gamePaused)
            {
                //DOTween.To(x => Time.timeScale = x, 0f, currentGameTimeScale, transitionTimeForTimeScaleChange).Play(); //DISABLED in 2.4.1
                Time.timeScale = currentGameTimeScale;
                gamePaused = false;
            }
            else
            {
                currentGameTimeScale = Time.timeScale;
                //DOTween.To(x => Time.timeScale = x, currentGameTimeScale, 0f, transitionTimeForTimeScaleChange).Play(); //DISABLED in 2.4.1
                Time.timeScale = 0f;
                gamePaused = true;
            }
        }

        /// <summary>
        /// Exits play mode (if in editor) or quits the application if in build mode
        /// </summary>
        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// The 'back' button was pressed (or escape key)
        /// </summary>
        public static void BackButtonEvent()
        {
            if (backButtonDisabled) //if the back button is disabled we do not continue
                return;

            if (gamePaused) //if the game is paused, we unpause it
                TogglePause();

            Navigation navItem = new Navigation();
            navItem.showElements = new List<string>();
            navItem.hideElements = new List<string>();

            if (navStack.Count == 0) //if the navigation stack is empty then we must be in the main menu; since the back button was pressed we show the quit panel
            {
                ShowUiElement("QuitMenu");
                HideUiElement("MainMenu");
                navItem.showElements.Add("MainMenu");
                navItem.hideElements.Add("QuitMenu");
                navStack.Push(navItem);
                return;
            }

            navItem = GetLastItemFromNavigationHistory();

            if (navItem.showElements != null && navItem.showElements.Count > 0)
            {
                for (int i = 0; i < navItem.showElements.Count; i++)
                {
                    ShowUiElement(navItem.showElements[i]);
                }
            }

            if (navItem.hideElements != null && navItem.hideElements.Count > 0)
            {
                for (int i = 0; i < navItem.hideElements.Count; i++)
                {
                    HideUiElement(navItem.hideElements[i]);
                }
            }
        }

        /// <summary>
        /// Disables the 'Back' button functionality
        /// </summary>
        public static void DisableBackButton()
        {
            backButtonDisableLevel++; //if == 0 --> false (back button is not disabled) if > 0 --> true (back button is disabled)
        }

        /// <summary>
        /// Enables the 'Back' button functionality
        /// </summary>
        public static void EnableBackButton()
        {
            backButtonDisableLevel--; //if == 0 --> false (back button is not disabled) if > 0 --> true (back button is disabled)
            if (backButtonDisableLevel < 0) { backButtonDisableLevel = 0; } //Check so that the backButtonDisableLevel does not go below zero
        }

        /// <summary>
        /// Enables the 'Back' button functionality by resetting the additive bool to zero. backButtonDisableLevel = 0. Use this ONLY for special cases when something wrong happens and the back button is stuck in disabled mode.
        /// </summary>
        public static void EnableBackButtonByForce()
        {
            backButtonDisableLevel = 0;
        }

        /// <summary>
        /// Disables all the button clicks. This is triggered by the system when an UIElement started a transition (IN/OUT animations).
        /// </summary>
        public static void DisableButtonClicks()
        {
            buttonClicksDisableLevel++; //if == 0 --> false (button clicks are not disabled) if > 0 --> true (button clicks are disabled)
            //Debug.Log("DisableButtonClicks | buttonClicksDisableLevel: " + buttonClicksDisableLevel);
        }

        /// <summary>
        /// Enables all the button clicks. This is triggered by the system when an UIElement finished a transition (IN/OUT animations).
        /// </summary>
        public static void EnableButtonClicks()
        {
            buttonClicksDisableLevel--; //if == 0 --> false (button clicks are not disabled) if > 0 --> true (button clicks are disabled)
            if (buttonClicksDisableLevel < 0) { buttonClicksDisableLevel = 0; } //Check so that the buttonClicksDisableLevel does not go below zero
            //Debug.Log("EnableButtonClicks | buttonClicksDisableLevel: " + buttonClicksDisableLevel);
        }

        /// <summary>
        /// Enables the button clicks by resetting the additive bool to zero. buttonClicksDisableLevel = 0. Use this ONLY for special cases when something unexpected happens and the button clicks are stuck in disabled mode.
        /// </summary>
        public static void EnableButtonClicksByForce()
        {
            buttonClicksDisableLevel = 0;
            //Debug.Log("EnableButtonClicksByForce | buttonClicksDisableLevel: " + buttonClicksDisableLevel);
        }

        #endregion

        #region Methods for GameEvents and ButtonClicks - SendGameEvent, SendGameEvents, SendButtonClick

        /// <summary>
        /// Sends a Game Event
        /// </summary>
        /// <param name="command">This is the game event command (a string) that you want to trigger.</param>
        public static void SendGameEvent(string _command)
        {
            GameEventMessage m = new GameEventMessage()
            {
                command = _command
            };
            //Message.Send<GameEventMessage>(gem);
            OnGameEvent(m);
        }

        /// <summary>
        /// Sends several Game Events by triggering a Game Event for every command (string) in the list.
        /// </summary>
        /// <param name="gameEvents">This is a list of game events commands (strings) that you want to trigger.</param>
        public static void SendGameEvents(List<string> gameEvents)
        {
            if (gameEvents != null)
            {
                for (int i = 0; i < gameEvents.Count; i++)
                {
                    OnGameEvent(new GameEventMessage() { command = gameEvents[i] });
                }
            }
        }

        /// <summary>
        /// Simulates a Button Click
        /// </summary>
        /// <param name="_buttonName">The name of the button (this is mostly what we are looking for)</param>
        /// <param name="_addToNavigationHistory">Should this button be added to the navigation history if the Navigation System is enabled? (default: false)</param>
        /// <param name="_backButton">Is this a 'Back' button? (it will simulate a 'Back' button event) (default: false)</param>
        /// <param name="_gameObject">Every button also sends it's gameObject reference (in case you want to do something to it). If you want to send only the button name, you can set it to null. (default: null)</param>
        /// <param name="_showElements">The names of all the elementNames that you want to show if the Navigation System is enabled. (default: null)</param>
        /// <param name="_hideElements">The names of all the elementNames that you want to hide if the Navigation System is denabled. (default: null)</param>
        /// <param name="_gameEvents">The game event commands that you want to trigger. (default: null)</param>
        public static void SendButtonClick(string _buttonName, bool _addToNavigationHistory = false, bool _backButton = false, GameObject _gameObject = null, List<string> _showElements = null, List<string> _hideElements = null, List<string> _gameEvents = null)
        {
            UIButtonMessage m = new UIButtonMessage()
            {
                buttonName = _buttonName,
                addToNavigationHistory = _addToNavigationHistory,
                backButton = _backButton,
                gameObject = _gameObject,
                showElements = _showElements,
                hideElements = _hideElements,
                gameEvents = _gameEvents
            };
            //Message.Send<UIButtonMessage>(m);
            UIManager.OnButtonClick(m);
        }
        #endregion

        #region Methods for UI - UpdateUiContainer, UpdateSettings, InitDoTween, UpdateUIScreenRect, CreateBlackScreen
        /// <summary>
        /// All the UI should be under a gameObject named "UI Container".
        /// Since this method is called only once in awake, it should not generate any overhead.
        /// On the plus side, there is no need for a reference to the "UI Panels" gameObject
        /// </summary>
        private static void UpdateUiContainer()
        {
            if (uiContainer == null)
                uiContainer = FindObjectOfType<UIManager>().transform.parent;
        }

        /// <summary>
        /// This just updates the static variables. It is useful since now we don't need an instance to reference the variables
        /// </summary>
        public void UpdateSettings()
        {
            autoDisableButtonClicks = _autoDisableButtonClicks;
            usesMA_FireCustomEvent = useMasterAudio_FireCustomEvent;
            usesMA_PlaySoundAndForget = useMasterAudio_PlaySoundAndForget;
            usesTMPro = useTextMeshPro;

            debugEvents = _debugEvents;
            debugButtons = _debugButtons;
            debugNotifications = _debugNotifications;
        }

        /// <summary>
        /// Initializes DOTween (not really needed, but gives you the option to create a custom configuration here)
        /// </summary>
        private void InitDoTween()
        {
            DOTween.Init();
        }

        /// <summary>
        /// This gets the actual screen size the application runs on. It is used for the animations calucalations.
        /// </summary>
        private static void UpdateUIScreenRect()
        {
            uiScreenRect = new UIScreenRect();
            UpdateUiContainer();
            uiScreenRect.size = GetUiContainer.GetComponent<RectTransform>().rect.size;
            uiScreenRect.position = GetUiContainer.GetComponent<RectTransform>().rect.position;
        }
        #endregion

        #region Methods for General Use - CreateDoozyUI, TrimStartAndEndSpaces
        /// <summary>
        /// Creates the DoozyUI prefab in the current Scene.
        /// </summary>
        public static GameObject CreateDoozyUI()
        {
#if UNITY_EDITOR
            var doozyUIprefab = AssetDatabase.LoadAssetAtPath(FileHelper.GetRelativeFolderPath("DoozyUI") + "/Prefabs/DoozyUI.prefab", typeof(GameObject));
#else
			var doozyUIprefab = Resources.Load(FileHelper.GetFolderPath("DoozyUI") + "/Prefabs/DoozyUI.prefab", typeof(GameObject));
#endif
            if (doozyUIprefab == null)
            {
                Debug.LogError("[DoozyUI] Could not find DoozyUI prefab. It should be at " + FileHelper.GetRelativeFolderPath("DoozyUI") + "/Prefabs/DoozyUI.prefab");
                return null;
            }

            var go = GameObject.Instantiate(doozyUIprefab) as GameObject;
            go.name = "DoozyUI";
            return go;
        }

        /// <summary>
        /// Trims the empty spaces from the start and the end of the string. It returns the cleaned string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimStartAndEndSpaces(string s)
        {
            s.TrimStart(' ');
            s.TrimEnd(' ');
            return s;
        }

        /// <summary>
        /// Updates the sorting layer for all the canvases on and under the target gameObject
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="sortingLayerName"></param>
        public static void UpdateCanvases(GameObject targetObject, string sortingLayerName)
        {
            Canvas[] canvas = targetObject.GetComponentsInChildren<Canvas>();
            foreach (Canvas c in canvas)
            {
                c.sortingLayerName = sortingLayerName;
            }
        }

        /// <summary>
        /// Updates all the sorting layer for all the renderers on and under the target gameObject
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="sortingLayerName"></param>
        public static void UpdateRenderers(GameObject targetObject, string sortingLayerName)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.sortingLayerName = sortingLayerName;
            }
        }

        /// <summary>
        /// Iterates through the given string array for the target string and returns the index. Retunrs -1 in case of error.
        /// </summary>
        /// <param name="stringArray">the string array we iterate through</param>
        /// <param name="targetString">the string we are looking for to get the index to</param>
        /// <returns></returns>
        public static int GetIndexForStringInArray(string[] stringArray, string targetString)
        {
            if (stringArray != null && stringArray.Length > 0 && string.IsNullOrEmpty(targetString) == false)
            {
                for (int i = 0; i < stringArray.Length; i++)
                {
                    if (stringArray[i].Equals(targetString)) //we found the value, we return the index
                    {
                        return i;
                    }
                }
            }

            if (stringArray == null) Debug.LogWarning("[DoozyUI] The stringArray is null");
            else if (stringArray.Length == 0) Debug.LogWarning("[DoozyUI] The stringArray is not null, but is empty");
            else if (string.IsNullOrEmpty(targetString) == false) Debug.LogWarning("[DoozyUI] The targeString is either null or empty");

            return -1; //we return an error
        }

        /// <summary>
        /// Iterates through the given string array for the target string. Returns TRUE if the string has been found and FALE is not.
        /// </summary>
        /// <param name="stringArray">the string array we iterate through</param>
        /// <param name="targetString">the string we are looking for</param>
        /// <returns></returns>
        public static bool IsStringInArray(string[] stringArray, string targetString)
        {
            if (stringArray != null && stringArray.Length > 0 && string.IsNullOrEmpty(targetString) == false)
            {
                for (int i = 0; i < stringArray.Length; i++)
                {
                    if (stringArray[i].Equals(targetString)) //we found the value, we return TRUE
                    {
                        return true;
                    }
                }
            }

            return false; //we didn find the targetString in the string array
        }
        #endregion

        #region IEnumerators - GetScreenSize, GetOrientation
        IEnumerator GetScreenSize()
        {
            int infiniteLoopBreak = 0;

            while (firstPass)
            {
                yield return new WaitForEndOfFrame();
                UpdateUIScreenRect();

                if (firstPass)  //this check is needed since in the first frame of the application the uiScreenRect is (0,0); only from the second frame can we get the screen size values
                {
                    firstPass = false;
                }

                infiniteLoopBreak++;
                if (infiniteLoopBreak > 1000)
                    break;
            }

            GetUICamera.enabled = true;
            //DestroyBlackScreen();
        }

        IEnumerator GetOrientation()
        {
            if (currentOrientation != Orientation.Unknown)
                SendGameEvent("DeviceOrientation_" + currentOrientation);

            int infiniteLoopBreak = 0;

            while (currentOrientation == Orientation.Unknown)
            {
                CheckDeviceOrientation();

                if (currentOrientation != Orientation.Unknown)
                    break;

                yield return null;

                infiniteLoopBreak++;
                if (infiniteLoopBreak > 1000)
                    break;
            }
        }
        #endregion
    }
}
