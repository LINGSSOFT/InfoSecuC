// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [AddComponentMenu("DoozyUI/UI Button", 2)]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(UIAnimationManager))]
    [DisallowMultipleComponent]
    public class UIButton : MonoBehaviour
    {

        #region Context Menu Methods

#if UNITY_EDITOR
        [MenuItem("DoozyUI/Components/UI Button", false, 2)]
        [MenuItem("GameObject/DoozyUI/UI Button", false, 2)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            if (GameObject.Find("UIManager") == null)
            {
                Debug.LogError("[DoozyUI] The DoozyUI system was not found in the scene. Please add it before trying to create a UI Button.");
                return;
            }
            GameObject go = new GameObject("New UIButton");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            if (go.GetComponent<Transform>() != null)
            {
                go.AddComponent<RectTransform>();
            }
            if (go.transform.parent == null)
            {
                go.transform.SetParent(UIManager.GetUiContainer);
            }
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.AddComponent<Image>();
            UIButton uiButton = go.AddComponent<UIButton>();
            uiButton.buttonNameReference = new ButtonName { buttonName = UIManager.DEFAULT_BUTTON_NAME };
            uiButton.buttonName = UIManager.DEFAULT_BUTTON_NAME;
            uiButton.onClickSoundReference = new ButtonSound { onClickSound = UIManager.DEFAULT_SOUND_NAME };
            uiButton.onClickSound = UIManager.DEFAULT_SOUND_NAME;
            Selection.activeObject = go;
        }
#endif

        #endregion

        #region Internal Classes --> ButtonName, ButtonSound
        [System.Serializable]
        public class ButtonName
        {
            public string buttonName = string.Empty;
        }

        [System.Serializable]
        public class ButtonSound
        {
            public string onClickSound = string.Empty;
        }
        #endregion

        #region BACKUP VARIABLES
        public string buttonName = UIManager.DEFAULT_BUTTON_NAME;
        public string onClickSound = UIManager.DEFAULT_SOUND_NAME;
        #endregion

        #region Public Variables
        [HideInInspector]
        public bool showHelp = false;

        public bool allowMultipleClicks = true; //by default we allow the user to press the button multiple times
        public float disableButtonInterval = 0.5f; //if allowMultipleClicks is false, then this is the interval that this button will be disabled for between each click

        public bool useOnClickAnimations = false;
        public bool useNormalStateAnimations = false;
        public bool useHighlightedStateAnimations = false;

        public ButtonName buttonNameReference;
        public bool addToNavigationHistory = false;
        public bool backButton = false;

        public bool waitForOnClickAnimation = true;

        public ButtonSound onClickSoundReference;

        public string[] onClickAnimationsPresetNames;

        public int activeOnclickAnimationsPresetIndex = 0;
        public string onClickAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimationManager.OnClickAnimations onClickAnimationSettings = new UIAnimationManager.OnClickAnimations();

        public string[] buttonLoopsAnimationsPresetNames;

        public int activeNormalAnimationsPresetIndex = 0;
        public string normalAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimationManager.ButtonLoopsAnimations normalAnimationSettings = new UIAnimationManager.ButtonLoopsAnimations();

        public int activeHighlightedAnimationsPresetIndex = 0;
        public string highlightedAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimationManager.ButtonLoopsAnimations highlightedAnimationSettings = new UIAnimationManager.ButtonLoopsAnimations();

        public List<string> showElements;
        public List<string> hideElements;
        public List<string> gameEvents;
        #endregion

        #region Private Variables
        [SerializeField]
        private UIAnimationManager animationManager; //reference to the animationMManager
        private Vector3 startAnchoredPosition; //initial anchored position
        private Vector3 startRotation; //initial start position
        private Vector3 startScale; //initial scale
        private Button button; //the Button component on this gameObject
        private RectTransform rectTransform; //the rectTransform of this gameObject
        private WaitForSeconds disableInterval; //the time the button is disabled between clicks
        private bool isThisButtonAnUIElement = false; //doe this gameObject also have an UIElement component attached?
        private Coroutine disableButtonCoroutine = null; //reference to the disable button coroutine; we use it to re-enable the button in case we disable it ahead of time (while it is disabled)
        #endregion

        #region Properties
        /// <summary>
        /// Returns a reference to the UIAnimator component that is attached to this gameObject
        /// </summary>
        public UIAnimationManager GetAnimationManager
        {
            get
            {
                if (animationManager == null)
                {
                    animationManager = GetComponent<UIAnimationManager>();
                    if (animationManager == null)
                        animationManager = gameObject.AddComponent<UIAnimationManager>();
                }
                return animationManager;
            }
        }

        private UIAnimator.InitialData m_initialData;
        /// <summary>
        /// Returns the initial state of this RectTransfrom (we use tis for reset purposes and to recalibrate animations)
        /// </summary>
        public UIAnimator.InitialData GetInitialData
        {
            get
            {
                if (m_initialData == null)
                {
                    m_initialData = new UIAnimator.InitialData();
                    m_initialData.startAnchoredPosition3D = startAnchoredPosition;
                    m_initialData.startRotation = startRotation;
                    m_initialData.startScale = startScale;
                    m_initialData.startFadeAlpha = 1f;
                    m_initialData.soundOn = UIManager.isSoundOn;
                }
                return m_initialData;
            }
        }

        /// <summary>
        /// Returns a string array of all the OnClick preset names (all the preset filenames from the OnClick folder)
        /// </summary>
        public string[] GetOnClickAnimationsPresetNames
        {
            get
            {
                GetAnimationManager.LoadPresetList(UIAnimationManager.AnimationType.OnClick);
                return onClickAnimationsPresetNames;
            }
        }

        /// <summary>
        /// Returns a string array of all the Button Loops preset names (all the preset filenames from the ButtonLoops folder)
        /// </summary>
        public string[] GetButtonLoopsAnimationsPresetNames
        {
            get
            {
                GetAnimationManager.LoadPresetList(UIAnimationManager.AnimationType.ButtonLoops);
                return buttonLoopsAnimationsPresetNames;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one Normal Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreNormalAnimationsEnabled
        {
            get
            {
                if (normalAnimationSettings.moveLoop.enabled) return true;
                else if (normalAnimationSettings.rotationLoop.enabled) return true;
                else if (normalAnimationSettings.scaleLoop.enabled) return true;
                else if (normalAnimationSettings.fadeLoop.enabled) return true;
                else return false;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one Highlighted Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreHighlightedAnimationsEnabled
        {
            get
            {
                if (highlightedAnimationSettings.moveLoop.enabled) return true;
                else if (highlightedAnimationSettings.rotationLoop.enabled) return true;
                else if (highlightedAnimationSettings.scaleLoop.enabled) return true;
                else if (highlightedAnimationSettings.fadeLoop.enabled) return true;
                else return false;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one OnClick Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreOnClickAnimationsEnabled
        {
            get
            {
                if (onClickAnimationSettings.punchPositionEnabled) return true;
                else if (onClickAnimationSettings.punchRotationEnabled) return true;
                else if (onClickAnimationSettings.punchScaleEnabled) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns the Mathf.Max time value for OnClick animations. If the OnClick animations are disabled, it returns 0; time = duration + delay for each animation type (MovePunch, RotatePunch, ScalePunch)
        /// </summary>
        public float GetOnClickAnimationsDuration
        {
            get
            {
                if (AreOnClickAnimationsEnabled == false)
                    return 0;

                if (waitForOnClickAnimation == false)
                    return 0;

                return Mathf.Max(
                    onClickAnimationSettings.punchPositionDuration + onClickAnimationSettings.punchPositionDelay,
                    onClickAnimationSettings.punchRotationDuration + onClickAnimationSettings.punchRotationDelay,
                    onClickAnimationSettings.punchScaleDuration + onClickAnimationSettings.punchScaleDelay);
            }
        }
        #endregion

        void Awake()
        {
            animationManager = GetComponent<UIAnimationManager>();
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                ExecuteButtonClick();
            });
            rectTransform = GetComponent<RectTransform>();
            disableInterval = new WaitForSeconds(disableButtonInterval);
        }

        void Start()
        {
            CheckIfThisIsAlsoAnUIElementButton();
            SetupButton();
        }

        void OnDisable()
        {
            if (disableButtonCoroutine != null)
            {
                StopCoroutine(disableButtonCoroutine);
                disableButtonCoroutine = null;
                EnableButtonClicks();
            }
        }

        void SetupButton()
        {
            startAnchoredPosition = rectTransform.anchoredPosition;
            startRotation = rectTransform.localRotation.eulerAngles;
            startScale = rectTransform.localScale;
            StartNormalStateAnimations();
        }

        #region DisableButtonClicks, EnableButtonClicks, DisableButtonForTime | IEnumerator - DiableButtonWithDelay

        /// <summary>
        /// Disables this button by making it non-interactable.
        /// </summary>
        public void DisableButtonClicks()
        {
            button.interactable = false;
        }

        /// <summary>
        /// Enables this button by making it interactable.
        /// </summary>
        public void EnableButtonClicks()
        {
            button.interactable = true;
        }

        private void DisableButtonForTime()
        {
            if (allowMultipleClicks == false)
            {
                disableButtonCoroutine = StartCoroutine("DiableButtonWithDelay");
            }
        }

        IEnumerator DiableButtonWithDelay()
        {
            DisableButtonClicks();
            yield return disableInterval;
            EnableButtonClicks();
            disableButtonCoroutine = null;
        }
        #endregion

        #region Play Sound
        void PlaySound()
        {
            UIAnimator.PlaySound(onClickSound, UIManager.isSoundOn);
        }
        #endregion

        #region StartOnClickAnimations, StartNormalStateAnimations, StopNormalStateAnimations, StartHighlightedStateAnimations, StopHighlightedSteateAnimations
        public void StartOnClickAnimations()
        {
            if (isThisButtonAnUIElement) //because this is an UIElement button we set the initial position, as the actual position on click, and disallow multiple clicks
            {
                allowMultipleClicks = false;
                disableButtonInterval = GetOnClickAnimationsDuration;
                startAnchoredPosition = rectTransform.anchoredPosition3D; //we get the current position (the one on Start is no longer valid for animations
                startRotation = rectTransform.localRotation.eulerAngles; //we get the current rotation
                startScale = rectTransform.localScale; //we get the current scale
            }
            UIAnimator.StartOnClickAnimations(rectTransform, GetInitialData, onClickAnimationSettings);
            //Debug.Log("START OnClickAnimation for " + name);
        }

        public void StartNormalStateAnimations()
        {
            if (AreNormalAnimationsEnabled == false)
                return;
            UIAnimator.StartButtonLoopsAnimations(rectTransform, GetInitialData, normalAnimationSettings);
            //Debug.Log("START NormalStateAnimation for " + name);
        }

        public void StopNormalStateAnimations()
        {
            if (AreNormalAnimationsEnabled == false)
                return;
            UIAnimator.StopButtonLoopsAnimations(rectTransform, GetInitialData);
            //Debug.Log("STOP NormalStateAnimation for " + name);
        }

        public void StartHighlightedStateAnimations()
        {
            if (AreHighlightedAnimationsEnabled == false) { StartNormalStateAnimations(); return; }
            UIAnimator.StartButtonLoopsAnimations(rectTransform, GetInitialData, highlightedAnimationSettings);
            //Debug.Log("START HighlightedStateAnimation for " + name);
        }

        public void StopHighlightedSteateAnimations()
        {
            if (AreHighlightedAnimationsEnabled == false)
                return;
            UIAnimator.StopButtonLoopsAnimations(rectTransform, GetInitialData);
            //Debug.Log("STOP HighlightedStateAnimation for " + name);
        }
        #endregion

        #region Send - ButtonClick
        void SendButtonClick()
        {
            UIButtonMessage m = new UIButtonMessage()
            {
                buttonName = buttonName,
                addToNavigationHistory = addToNavigationHistory,
                backButton = backButton,
                gameObject = gameObject,
                showElements = showElements,
                hideElements = hideElements,
                gameEvents = gameEvents
            };
            //Message.Send<UIButtonMessage>(m);
            UIManager.SendButtonClick(m.buttonName, m.addToNavigationHistory, m.backButton, m.gameObject, m.showElements, m.hideElements, m.gameEvents);
        }
        #endregion

        #region Send - Game Events
        void SendGameEvents()
        {
            StartCoroutine("SendGameEventsBetweenFrames");
        }

        IEnumerator SendGameEventsBetweenFrames()
        {
            if (gameEvents != null)
            {
                for (int i = 0; i < gameEvents.Count; i++)
                {
                    yield return null;
                    UIManager.SendGameEvent(gameEvents[i]);
                }
            }
        }
        #endregion

        #region Add & Remove GameEvents
        /// <summary>
        /// Add a game event to this UIButton's gameEvents list.
        /// </summary>
        /// <param name="eventName"></param>
        public void AddGameEvent(string eventName)
        {
            if (gameEvents == null)
                gameEvents = new List<string>();

            if (gameEvents.Contains(eventName) == false)
                gameEvents.Add(eventName);
        }

        /// <summary>
        /// Remove a game event from this UIButton's gameEvents list.
        /// </summary>
        /// <param name="eventName"></param>
        public void RemoveGameEvent(string eventName)
        {
            if (gameEvents == null)
                return;

            if (gameEvents.Contains(eventName))
                gameEvents.Remove(eventName);
        }
        #endregion

        /// <summary>
        /// We check that this is an UiElement as well. We do this so that the OnClick animations work as intended.
        /// </summary>
        bool CheckIfThisIsAlsoAnUIElementButton()
        {
            isThisButtonAnUIElement = GetComponent<UIElement>() != null;  //we check that this is also an UIElement
            return isThisButtonAnUIElement;
        }

        /// <summary>
        /// Executes the button click by playing the button sound (if set), starting the OnClick animation (if enabled) and sending the ButtonClick and GameEvents to the UIManager
        /// </summary>
        public void ExecuteButtonClick()
        {
            if (UIManager.buttonClicksDisabled)
                return;

            PlaySound();
            StartOnClickAnimations();

            if (Time.timeScale != 1) //This is a very special case when the timescale is zero we we want the button to work
            {
                SendButtonClickAndGameEvents();
            }
            else
            {
                Invoke("SendButtonClickAndGameEvents", GetOnClickAnimationsDuration);
                DisableButtonForTime();
            }
        }

        /// <summary>
        /// Sends the ButtonClick and the GameEvents to the UIManager without starting the OnClick animation (if enabled) and playing the button sound (if set)
        /// </summary>
        public void SendButtonClickAndGameEvents()
        {
            SendButtonClick();
            SendGameEvents();
        }

        public void ResetAnimations()
        {
            StopNormalStateAnimations();
            StopHighlightedSteateAnimations();
            if (UIManager.currentlySelectedGameObject == this)
            {
                StartHighlightedStateAnimations();
            }
            else
            {
                StartNormalStateAnimations();
            }
        }
    }
}


