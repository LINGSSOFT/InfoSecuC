// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [AddComponentMenu("DoozyUI/UI Element", 1)]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(UIAnimationManager))]
    [DisallowMultipleComponent]
    public class UIElement : MonoBehaviour
    {
        #region Context Menu Methods

#if UNITY_EDITOR
        [MenuItem("DoozyUI/Components/UI Element", false, 1)]
        [MenuItem("GameObject/DoozyUI/UI Element", false, 1)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            if (GameObject.Find("UIManager") == null)
            {
                Debug.LogError("[DoozyUI] The DoozyUI system was not found in the scene. Please add it before trying to create a UI Element.");
                return;
            }
            GameObject go = new GameObject("New UIElement");
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
            UIElement uiElement = go.AddComponent<UIElement>();
            uiElement.elementNameReference = new ElementName { elementName = UIManager.DEFAULT_ELEMENT_NAME };
            uiElement.elementName = UIManager.DEFAULT_ELEMENT_NAME;
            Selection.activeObject = go;
        }
#endif
        #endregion

        #region Internal Classes --> ElementName, TriggerEvent
        [Serializable]
        public class ElementName
        {
            public string elementName = UIManager.DEFAULT_ELEMENT_NAME;
        }

        [Serializable]
        public class TriggerEvent : UnityEvent { }
        #endregion

        #region BACKUP VARIABLES
        public string elementName = UIManager.DEFAULT_ELEMENT_NAME;

        public string moveInSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string moveInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string rotationInSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string rotationInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string scaleInSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string scaleInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string fadeInSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string fadeInSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;

        public string moveLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string moveLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string rotationLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string rotationLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string scaleLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string scaleLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string fadeLoopSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string fadeLoopSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;

        public string moveOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string moveOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string rotationOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string rotationOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string scaleOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string scaleOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        public string fadeOutSoundAtStart = UIManager.DEFAULT_SOUND_NAME;
        public string fadeOutSoundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        #endregion

        #region Public Variables
        public bool showHelp = false;

        public bool LANDSCAPE = true;
        public bool PORTRAIT = true;

        public bool linkedToNotification = false; //if we link a notification to this UIElement, then we will let the UINotification auto-generate the elementName; thus we need to disable user's ability to tamper with it

        public bool useCustomStartAnchoredPosition = false;
        public Vector3 customStartAnchoredPosition = Vector3.zero;

        public ElementName elementNameReference = new ElementName();
        public bool startHidden = false;
        public bool animateAtStart = false;
        public bool disableWhenHidden = false;
        public GameObject selectedButton = null;  //this is the button that gets selected when this UIElement gets shown; if null then no button will get auto selected

        public bool autoRegister = true;    //if this element is handled by a notification, the we let the notification handle the registration process with an auto generated name
        public bool isVisible = true;

        public bool containsChildUIElements = false;
        //
        public bool useInAnimations = false;
        public bool useLoopAnimations = false;
        public bool useOutAnimations = false;

        //
        public bool useInAnimationsStartEvents = false;
        public bool useInAnimationsFinishEvents = false;
        public bool useOutAnimationsStartEvents = false;
        public bool useOutAnimationsFinishEvents = false;

        //In Animations
        public int activeInAnimationsPresetIndex = 0;
        public string[] inAnimationsPresetNames;
        public string inAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimator.MoveIn moveIn = new UIAnimator.MoveIn();
        public UIAnimator.RotationIn rotationIn = new UIAnimator.RotationIn();
        public UIAnimator.ScaleIn scaleIn = new UIAnimator.ScaleIn();
        public UIAnimator.FadeIn fadeIn = new UIAnimator.FadeIn();

        //Loop Animations
        public int activeLoopAnimationsPresetIndex = 0;
        public string[] loopAnimationsPresetNames;
        public string loopAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimator.MoveLoop moveLoop = new UIAnimator.MoveLoop();
        public UIAnimator.RotationLoop rotationLoop = new UIAnimator.RotationLoop();
        public UIAnimator.ScaleLoop scaleLoop = new UIAnimator.ScaleLoop();
        public UIAnimator.FadeLoop fadeLoop = new UIAnimator.FadeLoop();

        //Out Animations
        public int activeOutAnimationsPresetIndex = 0;
        public string[] outAnimationsPresetNames;
        public string outAnimationsPresetName = UIAnimationManager.DEFAULT_PRESET_NAME;
        public UIAnimator.MoveOut moveOut = new UIAnimator.MoveOut();
        public UIAnimator.RotationOut rotationOut = new UIAnimator.RotationOut();
        public UIAnimator.ScaleOut scaleOut = new UIAnimator.ScaleOut();
        public UIAnimator.FadeOut fadeOut = new UIAnimator.FadeOut();

        public TriggerEvent onInAnimationsStart = new TriggerEvent();
        public TriggerEvent onInAnimationsFinish = new TriggerEvent();
        public TriggerEvent onOutAnimationsStart = new TriggerEvent();
        public TriggerEvent onOutAnimationsFinish = new TriggerEvent();
        #endregion

        #region Private Variables
        [SerializeField]
        private UIAnimationManager animationManager;
        private Canvas Canvas;
        private GraphicRaycaster GraphicRaycaster;

        private RectTransform rectTransform;

        private Vector3 startAnchoredPosition3D;
        private Vector3 startRotation;
        private Vector3 startScale;


        private float disableTimeBuffer = 0.5f;
        private WaitForSeconds outAnimationsDDisableDelay; //this will be the max time+delay for animations delay before the disable
        private Coroutine inAnimationsCoroutine;
        private Coroutine outAnimationsCoroutine;

        private bool inTransition = false; //if this UIElement is running DisableButtonClicksForTime and we disable it before it needs to finish, we trigger EnableButtonClicks OnDisable
        private Coroutine disableButtonClicksForTime;

        private float inAnimationsFinishTime = 0;
        private float outAnimationsFinishTime = 0;
        private Canvas[] childCanvas;
        private UIButton[] childButtons;
        #endregion

        #region Properties
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

        public string[] GetInAnimationsPresetNames
        {
            get
            {
                GetAnimationManager.LoadPresetList(UIAnimationManager.AnimationType.IN);
                return inAnimationsPresetNames;
            }
        }

        public string[] GetLoopAnimationsPresetNames
        {
            get
            {
                GetAnimationManager.LoadPresetList(UIAnimationManager.AnimationType.LOOP);
                return loopAnimationsPresetNames;
            }
        }

        public string[] GetOutAnimationsPresetNames
        {
            get
            {
                GetAnimationManager.LoadPresetList(UIAnimationManager.AnimationType.OUT);
                return outAnimationsPresetNames;
            }
        }

        public UIAnimationManager.InAnimations GetInAnimations
        {
            get
            {
                UIAnimationManager.InAnimations inAnimations = new UIAnimationManager.InAnimations();
                inAnimations.inAnimationsPresetName = inAnimationsPresetName;
                inAnimations.moveIn = moveIn;
                inAnimations.rotationIn = rotationIn;
                inAnimations.scaleIn = scaleIn;
                inAnimations.fadeIn = fadeIn;
                return inAnimations;
            }
        }

        public UIAnimationManager.InAnimations SetInAnimations
        {
            set
            {
                inAnimationsPresetName = value.inAnimationsPresetName;
                moveIn = value.moveIn;
                rotationIn = value.rotationIn;
                scaleIn = value.scaleIn;
                fadeIn = value.fadeIn;
            }
        }

        public UIAnimationManager.LoopAnimations GetLoopAnimations
        {
            get
            {
                UIAnimationManager.LoopAnimations loopAnimations = new UIAnimationManager.LoopAnimations();
                loopAnimations.loopAnimationsPresetName = loopAnimationsPresetName;
                loopAnimations.moveLoop = moveLoop;
                loopAnimations.rotationLoop = rotationLoop;
                loopAnimations.scaleLoop = scaleLoop;
                loopAnimations.fadeLoop = fadeLoop;
                return loopAnimations;
            }
        }

        public UIAnimationManager.LoopAnimations SetLoopAnimations
        {
            set
            {
                loopAnimationsPresetName = value.loopAnimationsPresetName;
                moveLoop = value.moveLoop;
                rotationLoop = value.rotationLoop;
                scaleLoop = value.scaleLoop;
                fadeLoop = value.fadeLoop;
            }
        }

        public UIAnimationManager.OutAnimations GetOutAnimations
        {
            get
            {
                UIAnimationManager.OutAnimations outAnimations = new UIAnimationManager.OutAnimations();
                outAnimations.outAnimationsPresetName = outAnimationsPresetName;
                outAnimations.moveOut = moveOut;
                outAnimations.rotationOut = rotationOut;
                outAnimations.scaleOut = scaleOut;
                outAnimations.fadeOut = fadeOut;
                return outAnimations;
            }
        }

        public UIAnimationManager.OutAnimations SetOutAnimations
        {
            set
            {
                outAnimationsPresetName = value.outAnimationsPresetName;
                moveOut = value.moveOut;
                rotationOut = value.rotationOut;
                scaleOut = value.scaleOut;
                fadeOut = value.fadeOut;
            }
        }

        public UIAnimator.InitialData GetInitialData
        {
            get
            {
                UIAnimator.InitialData initialData = new UIAnimator.InitialData();
                initialData.startAnchoredPosition3D = useCustomStartAnchoredPosition ? customStartAnchoredPosition : startAnchoredPosition3D;
                initialData.startRotation = startRotation;
                initialData.startScale = startScale;
                initialData.startFadeAlpha = 1f;
                initialData.soundOn = UIManager.isSoundOn;

                return initialData;
            }
        }

        public RectTransform GetRectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one IN Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreInAnimationsEnabled
        {
            get
            {
                if (moveIn.enabled) return true;
                else if (rotationIn.enabled) return true;
                else if (scaleIn.enabled) return true;
                else if (fadeIn.enabled) return true;
                else return false;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one LOOP Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreLoopAnimationsEnabled
        {
            get
            {
                if (moveLoop.enabled) return true;
                else if (rotationLoop.enabled) return true;
                else if (scaleLoop.enabled) return true;
                else if (fadeLoop.enabled) return true;
                else return false;
            }
        }

        /// <summary>
        /// Retruns TRUE if at least one OUT Animation is enabled. Otherwise it returns FALSE
        /// </summary>
        public bool AreOutAnimationsEnabled
        {
            get
            {
                if (moveOut.enabled) return true;
                else if (rotationOut.enabled) return true;
                else if (scaleOut.enabled) return true;
                else if (fadeOut.enabled) return true;
                else return false;
            }
        }
        #endregion

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
            if (Canvas == null) { Canvas = gameObject.AddComponent<Canvas>(); }
            GraphicRaycaster = GetComponent<GraphicRaycaster>();
            if (GraphicRaycaster == null) { GraphicRaycaster = gameObject.AddComponent<GraphicRaycaster>(); }
            if (useCustomStartAnchoredPosition) { GetRectTransform.anchoredPosition3D = customStartAnchoredPosition; }
            startAnchoredPosition3D = GetRectTransform.anchoredPosition3D;
            startRotation = GetRectTransform.localRotation.eulerAngles;
            startScale = GetRectTransform.localScale;
            childCanvas = GetComponentsInChildren<Canvas>();
            childButtons = GetComponentsInChildren<UIButton>();

            moveIn.soundAtStart = moveInSoundAtStart;
            moveIn.soundAtFinish = moveInSoundAtFinish;
            rotationIn.soundAtStart = rotationInSoundAtStart;
            rotationIn.soundAtFinish = rotationInSoundAtFinish;
            scaleIn.soundAtStart = scaleInSoundAtStart;
            scaleIn.soundAtFinish = scaleInSoundAtFinish;
            fadeIn.soundAtStart = fadeInSoundAtStart;
            fadeIn.soundAtFinish = fadeInSoundAtFinish;

            moveLoop.soundAtStart = moveLoopSoundAtStart;
            moveLoop.soundAtFinish = moveLoopSoundAtFinish;
            rotationLoop.soundAtStart = rotationLoopSoundAtStart;
            rotationLoop.soundAtFinish = rotationLoopSoundAtFinish;
            scaleLoop.soundAtStart = scaleLoopSoundAtStart;
            scaleLoop.soundAtFinish = scaleLoopSoundAtFinish;
            fadeLoop.soundAtStart = fadeLoopSoundAtStart;
            fadeLoop.soundAtFinish = fadeLoopSoundAtFinish;

            moveOut.soundAtStart = moveOutSoundAtStart;
            moveOut.soundAtFinish = moveOutSoundAtFinish;
            rotationOut.soundAtStart = rotationOutSoundAtStart;
            rotationOut.soundAtFinish = rotationOutSoundAtFinish;
            scaleOut.soundAtStart = scaleOutSoundAtStart;
            scaleOut.soundAtFinish = scaleOutSoundAtFinish;
            fadeOut.soundAtStart = fadeOutSoundAtStart;
            fadeOut.soundAtFinish = fadeOutSoundAtFinish;
        }

        void Start()
        {
            if (autoRegister)
                UIManager.RegisterUiElement(this);
            SetupElement();
            InitLoopAnimations();
        }

        void OnEnable()
        {
            ExecuteFixForLayouts();
        }

        void OnDisable()
        {
            if (UIManager.autoDisableButtonClicks) { if (inTransition) { EnableButtonClicks(); } }
        }

        void OnDestroy()
        {
            UIManager.UnregisterUiElement(this);
        }

        #region Setup Element Methods
        /// <summary>
        /// Setups the element.
        /// </summary>
        void SetupElement()
        {
            if (GetComponentsInChildren<UIElement>().Length > 1)
            {
                containsChildUIElements = true;
            }

            if (animateAtStart)
            {
                if (linkedToNotification)
                {
                    Hide(true, false);
                    Show(false);
                }
                else
                {
                    if (UIManager.useOrientationManager)
                    {
                        if (UIManager.currentOrientation == UIManager.Orientation.Unknown)
                        {
                            StartCoroutine(GetOrientation());
                        }
                        else
                        {
                            if (LANDSCAPE && UIManager.currentOrientation == UIManager.Orientation.Landscape)
                            {
                                UIManager.HideUiElement(elementName, true, disableWhenHidden);
                                UIManager.ShowUiElement(elementName, false);
                                if (disableWhenHidden && containsChildUIElements) StartCoroutine(TriggerShowInTheNextFrame(false));
                            }
                            else if (PORTRAIT && UIManager.currentOrientation == UIManager.Orientation.Portrait)
                            {
                                UIManager.HideUiElement(elementName, true, disableWhenHidden);
                                UIManager.ShowUiElement(elementName, false);
                                if (disableWhenHidden && containsChildUIElements) StartCoroutine(TriggerShowInTheNextFrame(false));
                            }
                            else
                            {
                                Hide(true, disableWhenHidden);
                            }
                        }
                    }
                    else
                    {
                        UIManager.HideUiElement(elementName, true, disableWhenHidden);
                        UIManager.ShowUiElement(elementName, false);
                        if (disableWhenHidden && containsChildUIElements) StartCoroutine(TriggerShowInTheNextFrame(false));
                    }
                }
            }
            else if (startHidden)
            {
                UIManager.HideUiElement(elementName, true, disableWhenHidden);
            }
        }
        #endregion

        #region Show Methods (IN Animations)

        /// <summary>
        /// Shows the element.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will execute the animations in 0 seconds and with 0 delay</param>
        public void Show(bool instantAction)
        {
            if (outAnimationsCoroutine != null)
            {
                isVisible = false;
                StopCoroutine(outAnimationsCoroutine);
                outAnimationsCoroutine = null;
            }

            if (AreInAnimationsEnabled)
            {
                if (isVisible == false)
                {
                    inAnimationsFinishTime = GetInAnimationsFinishTime();
                    TriggerInAnimationsEvents();
                    UIAnimator.StopOutAnimations(GetRectTransform, GetInitialData);
                    inAnimationsCoroutine = StartCoroutine(InAnimationsEnumerator(instantAction));
                    isVisible = true;
                    if (instantAction == false) { DisableButtonClicks(inAnimationsFinishTime); }
                    if (disableWhenHidden && containsChildUIElements) StartCoroutine(TriggerShowInTheNextFrame(instantAction));
                    ExecuteFixForLayouts();
                }
            }
            else if (AreInAnimationsEnabled == false)
            {
                Debug.LogWarning("[DoozyUI] [" + name + "] You are trying to SHOW the " + elementName + " UIElement, but you didn't enable any IN animations. To fix this warning you should enable at least one IN animation.");
            }
        }

        IEnumerator SetSelectedGameObject()
        {
            int infiniteLoopCount = 0;
            while (UIManager.GetEventSystem == null)
            {
                yield return null;
                infiniteLoopCount++;
                if (infiniteLoopCount > 1000) { break; }
            }
            UIManager.GetEventSystem.SetSelectedGameObject(selectedButton);
        }

        IEnumerator InAnimationsEnumerator(bool instantAction)
        {
            yield return null;
            UIAnimator.StopLoopAnimations(GetRectTransform, GetInitialData);
            ToggleCanvasAndGraphicRaycaster(true);
            UIAnimator.DoMoveIn(moveIn, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoRotationIn(rotationIn, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoScaleIn(scaleIn, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoFadeIn(fadeIn, GetRectTransform, GetInitialData, instantAction);
            StartCoroutine("SetSelectedGameObject");
            inAnimationsCoroutine = null;
            //yield return null;
            yield return new WaitForSecondsRealtime(GetInAnimationsFinishTime());
            if(childButtons != null && childButtons.Length > 0)
            {
                for(int i = 0; i < childButtons.Length; i++)
                {
                    childButtons[i].ResetAnimations();
                }
            }
        }
        #endregion

        #region Loop Methods (LOOP Animations)

        /// <summary>
        /// Initiates (if enabled) and plays (if set to autoStart) the idle animations.
        /// </summary>
        public void InitLoopAnimations()
        {
            if (AreLoopAnimationsEnabled)
                StartCoroutine(LoopAnimationsEnumerator());
        }

        IEnumerator LoopAnimationsEnumerator()
        {
            yield return null;
            UIAnimator.DoMoveLoop(moveLoop, GetRectTransform, GetInitialData);
            UIAnimator.DoRotationLoop(rotationLoop, GetRectTransform, GetInitialData);
            UIAnimator.DoScaleLoop(scaleLoop, GetRectTransform, GetInitialData);
            UIAnimator.DoFadeLoop(fadeLoop, GetRectTransform, GetInitialData);
        }

        #endregion

        #region Hide Methods (OUT Animations)

        public void Hide(bool instantAction)
        {
            Hide(instantAction, true);
        }

        /// <summary>
        /// Hides the element.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will execute the animations in 0 seconds and with 0 delay</param>
        public void Hide(bool instantAction, bool shouldDisable)
        {
            if (inAnimationsCoroutine != null)
            {
                isVisible = true;
                StopCoroutine(inAnimationsCoroutine);
                inAnimationsCoroutine = null;
            }
            if (AreOutAnimationsEnabled)
            {
                if (isVisible)
                {
                    outAnimationsFinishTime = GetOutAnimationsFinishTime();
                    if (instantAction == false) { TriggerOutAnimationsEvents(); } //we do this check so that the events are not triggered onEnable when we have startHidden set as true
                    UIAnimator.StopInAnimations(GetRectTransform, GetInitialData);
                    outAnimationsCoroutine = StartCoroutine(OutAnimationsEnumerator(instantAction, shouldDisable));
                    isVisible = false;
                    if (instantAction == false) { DisableButtonClicks(outAnimationsFinishTime); }
                }
            }
            else if (AreOutAnimationsEnabled == false)
            {
                Debug.LogWarning("[DoozyUI] [" + name + "] You are trying to HIDE the " + elementName + " UIElement, but you didn't enable any OUT animations. To fix this warning you should enable at least one OUT animation.");
            }
        }

        IEnumerator OutAnimationsEnumerator(bool instantAction, bool shouldDisable = true)
        {
            float start = Time.realtimeSinceStartup;
            UIAnimator.StopLoopAnimations(GetRectTransform, GetInitialData);
            UIAnimator.DoMoveOut(moveOut, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoRotationOut(rotationOut, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoScaleOut(scaleOut, GetRectTransform, GetInitialData, instantAction);
            UIAnimator.DoFadeOut(fadeOut, GetRectTransform, GetInitialData, instantAction);
            if (disableWhenHidden)
            {
                if (shouldDisable)
                {
                    while (Time.realtimeSinceStartup < start + disableTimeBuffer) { yield return null; }
                    if (instantAction == false) { while (Time.realtimeSinceStartup < start + outAnimationsFinishTime + disableTimeBuffer) { yield return null; } }
                    ToggleCanvasAndGraphicRaycaster(false);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (instantAction == false) { while (Time.realtimeSinceStartup < start + outAnimationsFinishTime + disableTimeBuffer) { yield return null; } }
                ToggleCanvasAndGraphicRaycaster(false);
            }
            outAnimationsCoroutine = null;
            yield return null;
        }
        #endregion

        #region Animation Start and Finish Times
        /// <summary>
        /// This returns the start time of the IN Animations, taking into account all the delays. It retruns the minimum animationStartDelay.
        /// How long does the animation take to start?
        /// It will return -1 if no IN Animations are enabled
        /// </summary>
        /// <returns></returns>
        private float GetInAnimationsStartTime()
        {
            if (moveIn.enabled || rotationIn.enabled || scaleIn.enabled || fadeIn.enabled)
            {
                return Mathf.Min(moveIn.enabled ? moveIn.delay : 10000,
                                 rotationIn.enabled ? rotationIn.delay : 10000,
                                 scaleIn.enabled ? scaleIn.delay : 10000,
                                 fadeIn.enabled ? fadeIn.delay : 10000);
            }
            return -1f;
        }

        /// <summary>
        /// This returns the finish time of the IN Animations, taking into account all the delays. It retruns the maximum animationStatDelay + the maximum animationTime.
        /// How long does the animation take to finish?
        /// It will return -1 if no IN Animations are enabled
        /// </summary>
        /// <returns></returns>
        private float GetInAnimationsFinishTime()
        {
            if (moveIn.enabled || rotationIn.enabled || scaleIn.enabled || fadeIn.enabled)
            {
                return Mathf.Max(moveIn.enabled ? moveIn.time + moveIn.delay : 0,
                                 rotationIn.enabled ? rotationIn.time + rotationIn.delay : 0,
                                 scaleIn.enabled ? scaleIn.time + scaleIn.delay : 0,
                                 fadeIn.enabled ? fadeIn.time + fadeIn.delay : 0);
            }
            return -1f;
        }

        /// <summary>
        /// This returns the start time of the OUT Animations, taking into account all the delays. It retruns the minimum animationStartDelay.
        /// How long does the animation take to start?
        /// It will return -1 if no OUT Animations are enabled
        /// </summary>
        /// <returns></returns>
        private float GetOutAnimationsStartTime()
        {
            if (moveOut.enabled || rotationOut.enabled || scaleOut.enabled || fadeOut.enabled)
            {
                return Mathf.Min(moveOut.enabled ? moveOut.delay : 10000,
                                 rotationOut.enabled ? rotationOut.delay : 10000,
                                 scaleOut.enabled ? scaleOut.delay : 10000,
                                 fadeOut.enabled ? fadeOut.delay : 10000);
            }
            return -1f;
        }

        /// <summary>
        /// This returns the finish time of the OUT Animations, taking into account all the delays. It retruns the maximum animationStatDelay + the maximum animationTime.
        /// How long does the animation take to finish?
        /// It will return -1 if no OUT Animations are enabled
        /// </summary>
        /// <returns></returns>
        private float GetOutAnimationsFinishTime()
        {
            if (moveOut.enabled || rotationOut.enabled || scaleOut.enabled || fadeOut.enabled)
            {
                return Mathf.Max(moveOut.enabled ? moveOut.time + moveOut.delay : 0,
                                 rotationOut.enabled ? rotationOut.time + rotationOut.delay : 0,
                                 scaleOut.enabled ? scaleOut.time + scaleOut.delay : 0,
                                 fadeOut.enabled ? fadeOut.time + fadeOut.delay : 0);
            }
            return -1f;
        }
        #endregion

        #region Events

        #region IN Animations
        /// <summary>
        /// Triggers the IN Animations Events, if enabled.
        /// </summary>
        private void TriggerInAnimationsEvents()
        {
            if (useInAnimationsStartEvents)
            {
                if (GetInAnimationsStartTime() == -1)
                {
                    Debug.Log("[DoozyUI] You have activated IN Animations Start Events for the " + elementName + " UIElement on " + gameObject.name + " gameObject, but you did not enable any IN animations. Nothing happened!");
                }
                else
                {
                    StartCoroutine(TriggerInAnimaionsStartEvents(GetInAnimationsStartTime()));
                }
            }

            if (useInAnimationsFinishEvents)
            {
                if (GetInAnimationsFinishTime() == -1)
                {
                    Debug.Log("[DoozyUI] You have activated IN Animations Finish Events for the " + elementName + " UIElement on " + gameObject.name + " gameObject, but you did not enable any IN animations. Nothing happened!");
                }
                else
                {
                    StartCoroutine(TriggerInAnimaionsFinishEvents(inAnimationsFinishTime));
                }
            }
        }

        IEnumerator TriggerInAnimaionsStartEvents(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay) { yield return null; }
            onInAnimationsStart.Invoke();
        }

        IEnumerator TriggerInAnimaionsFinishEvents(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay) { yield return null; }
            onInAnimationsFinish.Invoke();
        }
        #endregion

        #region OUT Animations
        /// <summary>
        /// Triggers the OUT Animations Events, if enabled.
        /// </summary>
        private void TriggerOutAnimationsEvents()
        {
            if (useOutAnimationsStartEvents)
            {
                if (GetOutAnimationsStartTime() == -1)
                {
                    Debug.Log("[DoozyUI] You have activated OUT Animations Start Events for the " + elementName + " UIElement on " + gameObject.name + " gameObject, but you did not enable any OUT animations. Nothing happened!");
                }
                else
                {
                    StartCoroutine(TriggerOutAnimaionsStartEvents(GetOutAnimationsStartTime()));
                }
            }

            if (useOutAnimationsFinishEvents)
            {
                if (GetOutAnimationsFinishTime() == -1)
                {
                    Debug.Log("[DoozyUI] You have activated OUT Animations Finish Events for the " + elementName + " UIElement on " + gameObject.name + " gameObject, but you did not enable any OUT animations. Nothing happened!");
                }
                else
                {
                    StartCoroutine(TriggerOutAnimaionsFinishEvents(GetOutAnimationsFinishTime()));
                }
            }
        }

        IEnumerator TriggerOutAnimaionsStartEvents(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay) { yield return null; }
            onOutAnimationsStart.Invoke();
        }

        IEnumerator TriggerOutAnimaionsFinishEvents(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay) { yield return null; }
            onOutAnimationsFinish.Invoke();
        }
        #endregion

        #endregion

        void EnableButtonClicks()
        {
            if (!UIManager.autoDisableButtonClicks)
                return;

            if (inTransition)
            {
                inTransition = false;
                UIManager.EnableButtonClicks();
            }

            if (disableButtonClicksForTime != null)
            {
                StopCoroutine(disableButtonClicksForTime);
                disableButtonClicksForTime = null;
            }
        }

        void DisableButtonClicks(float time)
        {
            if (!UIManager.autoDisableButtonClicks)
                return;

            EnableButtonClicks();
            disableButtonClicksForTime = StartCoroutine(DisableButtonClicksForTime(time));
        }

        /// <summary>
        /// While an IN or an OUT animations is in transition, we disable the button clicks
        /// </summary>
        IEnumerator DisableButtonClicksForTime(float delay)
        {
            UIManager.DisableButtonClicks();
            inTransition = true;
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay) { yield return null; }
            inTransition = false;
            UIManager.EnableButtonClicks();
            disableButtonClicksForTime = null;
        }

        void ToggleCanvasAndGraphicRaycaster(bool isEnabled)
        {
            Canvas.enabled = isEnabled;
            GraphicRaycaster.enabled = isEnabled;
        }

        IEnumerator GetOrientation()
        {
            while (UIManager.currentOrientation == UIManager.Orientation.Unknown)
            {
                UIManager.CheckDeviceOrientation();
                if (UIManager.currentOrientation != UIManager.Orientation.Unknown)
                    break;

                yield return null;
            }

            if (LANDSCAPE && UIManager.currentOrientation == UIManager.Orientation.Landscape)
            {
                UIManager.HideUiElement(elementName, true, disableWhenHidden);
                UIManager.ShowUiElement(elementName, false);
            }
            else if (PORTRAIT && UIManager.currentOrientation == UIManager.Orientation.Portrait)
            {
                UIManager.HideUiElement(elementName, true, disableWhenHidden);
                UIManager.ShowUiElement(elementName, false);
            }
        }

        /// <summary>
        /// This fixes a very strange issue inside Unity. When setting a VerticalLayoutGroup or a HorizontalLayoutGroup, the Image bounds get moved (the image appeares in a different place).
        /// We could not find a better solution, but this should work for now.
        /// </summary>
        void ExecuteFixForLayouts()
        {
            childCanvas = GetComponentsInChildren<Canvas>();
            if (childCanvas != null && childCanvas.Length > 0)
            {
                for (int i = 0; i < childCanvas.Length; i++)
                {
                    childCanvas[i].enabled = false;
                    childCanvas[i].enabled = true;
                }
            }
        }

        IEnumerator TriggerShowInTheNextFrame(bool instantAction)
        {
            yield return null;
            UIManager.ShowUiElement(elementName, instantAction);
        }
    }
}