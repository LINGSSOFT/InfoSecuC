// Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using DG.Tweening;

#if dUI_MasterAudio
using DarkTonic.MasterAudio;
#endif

namespace DoozyUI
{
    public class UIAnimator : MonoBehaviour
    {
        #region Enums - MoveDetails, SoundOutput, ButtonAnimationType, ResetType
        public enum MoveDetails
        {
            ParentPosition,
            LocalPosition,
            TopScreenEdge,
            RightScreenEdge,
            BottomScreenEdge,
            LeftScreenEdge,
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        public enum SoundOutput
        {
            AudioSource,
            MasterAudioPlaySoundAndForget,
            MasterAudioFireCustomEvent
        }

        public enum ButtonAnimationType
        {
            None,
            PunchPosition,
            PunchRotation,
            PunchScale
        }

        public enum AnimationTarget
        {
            None,
            UIElement,
            UIButton
        }

        public enum ResetType
        {
            All,
            Position,
            Rotation,
            Scale,
            Fade
        }
        #endregion

        #region Internal Classes - initialData, SoundDetails
        [System.Serializable]
        public class InitialData
        {
            public Vector3 startAnchoredPosition3D = Vector3.zero;
            public Vector3 startRotation = Vector3.zero;
            public Vector3 startScale = Vector3.one;
            public float startFadeAlpha = 1f;
            public bool soundOn = true;
        }

        [System.Serializable]
        public class SoundDetails
        {
            public string soundName = UIManager.DEFAULT_SOUND_NAME;
        }
        #endregion

        #region IN ANIMATION CLASSES

        [System.Serializable]
        public class MoveIn
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Where does the animation begin from?
            /// </summary>
            public MoveDetails moveFrom = MoveDetails.BottomCenter;
            /// <summary>
            /// Use this if you need to adjust the target position. You add or subtract (if the number is negative) values to the position of the target location
            /// </summary>
            public Vector3 positionAdjustment = Vector3.zero;
            /// <summary>
            /// This is used when the Move From LocalPosition is selected
            /// </summary>
            public Vector3 positionFrom = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.OutBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class RotationIn
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Where does the animation begin from?
            /// </summary>
            public Vector3 rotateFrom = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.OutBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class ScaleIn
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// From what scale factor does the animation begin? (default: 0)
            /// </summary>
            public Vector3 scaleBegin = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.OutBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class FadeIn
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }
        #endregion

        #region LOOP ANIMATION CLASSES

        [System.Serializable]
        public class MoveLoop
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// If you want this animation to ignore IN and OUT animations and auto start then select this as true
            /// </summary>
            public bool autoStart = false;
            /// <summary>
            /// This movement is calculated startAnchoredPosition-movement for min and startAnchoredPosition+movment for max
            /// </summary>
            public Vector3 movement = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.InOutSine;
            /// <summary>
            /// Number of loops (-1 = infinite loops)
            /// </summary>
            public int loops = -1;
            /// <summary>
            /// Types of loop
            /// </summary>
            public LoopType loopType = LoopType.Yoyo;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class RotationLoop
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// If you want this animation to ignore IN and OUT animations and auto start then select this as true
            /// </summary>
            public bool autoStart = false;
            /// <summary>
            /// This rotation is calculated startRotation-rotation for min and startRotation+rotation for max
            /// </summary>
            public Vector3 rotation = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.InOutSine;
            /// <summary>
            /// Number of loops (-1 = infinite loops)
            /// </summary>
            public int loops = -1;
            /// <summary>
            /// Types of loop
            /// </summary>
            public LoopType loopType = LoopType.Yoyo;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class ScaleLoop
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// If you want this animation to ignore IN and OUT animations and auto start then select this as true
            /// </summary>
            public bool autoStart = false;
            /// <summary>
            /// The minimum values for the scale factor of the scale loop animation (default: 1)
            /// </summary>
            public Vector3 min = new Vector3(1, 1, 1);
            /// <summary>
            /// The maximum values for the scale factor of the scale loop animation (default: 1.05)
            /// </summary>
            public Vector3 max = new Vector3(1.05f, 1.05f, 1.05f);
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;
            /// <summary>
            /// Number of loops (-1 = infinite loops)
            /// </summary>
            public int loops = -1;
            /// <summary>
            /// Types of loop
            /// </summary>
            public LoopType loopType = LoopType.Yoyo;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class FadeLoop
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// If you want this animation to ignore IN and OUT animations and auto start then select this as true
            /// </summary>
            public bool autoStart = false;
            /// <summary>
            /// The minimum alpha value for the fade animation loop
            /// </summary>
            public float min = 0;
            /// <summary>
            /// The maximum alpha value for the fade animation loop
            /// </summary>
            public float max = 1;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;
            /// <summary>
            /// Number of loops (-1 = infinite loops)
            /// </summary>
            public int loops = -1;
            /// <summary>
            /// Types of loop
            /// </summary>
            public LoopType loopType = LoopType.Yoyo;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }
        #endregion

        #region OUT ANIMATION CLASSES
        [System.Serializable]
        public class MoveOut
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Where does the animation end?
            /// </summary>
            public MoveDetails moveTo = MoveDetails.BottomCenter;
            /// <summary>
            /// Use this if you need to adjust the target position. You add or substract (if the number is negative) values to the position of the target location
            /// </summary>
            public Vector3 positionAdjustment = Vector3.zero;
            /// <summary>
            /// This is used when the Move From LocalPosition is selected
            /// </summary>
            public Vector3 positionTo = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.InBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class RotationOut
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Where does the animation end?
            /// </summary>
            public Vector3 rotateTo = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.InBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }

        [System.Serializable]
        public class ScaleOut
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// This is the scale factor at which the animation ends at
            /// </summary>
            public Vector3 scaleEnd = Vector3.zero;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.InBack;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;

        }

        [System.Serializable]
        public class FadeOut
        {
            /// <summary>
            /// Is the animation enabled?
            /// </summary>
            public bool enabled = false;
            /// <summary>
            /// Easing is the rate of change of animation over time
            /// </summary>
            public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;
            /// <summary>
            /// Time is amount (seconds) that the animation will take to complete
            /// </summary>
            public float time = 0.5f;
            /// <summary>
            /// Delay is amount (seconds) that the animation will wait before beginning
            /// </summary>
            public float delay = 0;
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtStartReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// (deprecated) Sends trigger sounds
            /// </summary>
            public SoundDetails soundAtFinishReference = new SoundDetails() { soundName = UIManager.DEFAULT_SOUND_NAME };
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtStart = UIManager.DEFAULT_SOUND_NAME;
            /// <summary>
            /// Sends trigger sounds
            /// </summary>
            public string soundAtFinish = UIManager.DEFAULT_SOUND_NAME;
        }
        #endregion

        #region Sound Methods - PlayClipAt, PlaySound
        /// <summary>
        /// Plays a clip at the specified location
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
        {
            var tempGO = new GameObject("TempAudio - " + clip.name);
            tempGO.transform.position = pos;
            var aSource = tempGO.AddComponent<AudioSource>(); ;
            aSource.clip = clip; 
            aSource.Play();
            Destroy(tempGO, clip.length);
            return aSource;
        }

        /// <summary>
        /// Plays a sound with an AudioSource or with Master Audio. If the string is empty or null this function does nothing.
        /// This function will play a sound regardless if the sound is off in UIManager.
        /// To play a sound and check if the sound is on in UIManager call UIAnimator.PlaySound(soundName, UIManager.Instance.soundOn);
        /// </summary>
        /// <param name="soundName"></param>
        public static void PlaySound(string soundName)
        {
            PlaySound(soundName, true);
        }

        /// <summary>
        /// Plays a sound with an AudioSource or with Master Audio. If the string is empty or null this function does nothing.
        /// Also if the sound is off (soundOn == false) then this function will do nothing.
        /// </summary>
        /// <param name="soundName">Sound name.</param>
        public static void PlaySound(string soundName, bool soundOn)
        {
            if (string.IsNullOrEmpty(soundName) || soundOn == false || soundName.Equals(UIManager.DEFAULT_SOUND_NAME)) { return; }
            SoundOutput soundOutput = SoundOutput.AudioSource;
            if (UIManager.usesMA_FireCustomEvent) { soundOutput = SoundOutput.MasterAudioFireCustomEvent; }
            else if (UIManager.usesMA_PlaySoundAndForget) { soundOutput = SoundOutput.MasterAudioPlaySoundAndForget; }
            switch (soundOutput)
            {
                case SoundOutput.AudioSource:
                    AudioClip clip = Resources.Load(soundName) as AudioClip;
                    if (clip == null) { Debug.Log("[Doozy] There is no sound file with the name [" + soundName + "] in any of the Resources folders.\n Check that the spelling of the fileName (without the extension) is correct or if the file exists in under a Resources folder"); return; }
                    //play without an AudioSource component
                    PlayClipAt(clip, Vector3.zero);
                    break;

                case SoundOutput.MasterAudioPlaySoundAndForget:
#if dUI_MasterAudio
                    MasterAudio.PlaySoundAndForget(soundName);
#else
                    Debug.Log("[Doozy] You are trying to use MasterAudio by calling the PlaySoundAndForget method, but you don't have it enabled. Please check if the 'dUI_MasterAudio' symbol is defined in 'Scripting Define Symbol'. You can find it by going to [Edit] -> [Project Settings] -> [Player] -> look at the inspector -> [Other Settings] -> look for [Scripting Define Symbols] => if you do not see 'dUI_MasterAudio' there, please add it.");
#endif
                    break;

                case SoundOutput.MasterAudioFireCustomEvent:
#if dUI_MasterAudio
                    MasterAudio.FireCustomEvent(soundName, UIManager.GetUiContainer);
#else
                    Debug.Log("[Doozy] You are trying to use MasterAudio by calling the FireCustomEvent method, but you don't have it enabled. Please check if the 'dUI_MasterAudio' symbol is defined in 'Scripting Define Symbol'. You can find it by going to [Edit] -> [Project Settings] -> [Player] -> look at the inspector -> [Other Settings] -> look for [Scripting Define Symbols] => if you do not see 'dUI_MasterAudio' there, please add it.");
#endif
                    break;
            }
        }
        #endregion

        #region Reset Rect Transfrom
        /// <summary>
        /// Resets the rect transform to it's initial states.
        /// </summary>
        public static void ResetRectTransform(RectTransform rectTransform, InitialData initialData, ResetType resetType = ResetType.All)
        {
            if (rectTransform == null || initialData == null) { return; }
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            switch (resetType)
            {
                case ResetType.All:
                    rectTransform.anchoredPosition = initialData.startAnchoredPosition3D;
                    rectTransform.localRotation = Quaternion.Euler(initialData.startRotation);
                    rectTransform.localScale = initialData.startScale;
                    if (canvasGroup != null)
                    {
                        canvasGroup.interactable = true;
                        canvasGroup.blocksRaycasts = true;
                        canvasGroup.alpha = 1f;
                    }
                    break;

                case ResetType.Position:
                    rectTransform.anchoredPosition = initialData.startAnchoredPosition3D;
                    break;

                case ResetType.Rotation:
                    rectTransform.localRotation = Quaternion.Euler(initialData.startRotation);
                    break;

                case ResetType.Scale:
                    rectTransform.localScale = initialData.startScale;
                    break;

                case ResetType.Fade:
                    if (canvasGroup != null)
                    {
                        canvasGroup.interactable = true;
                        canvasGroup.blocksRaycasts = true;
                        canvasGroup.alpha = 1f;
                    }
                    break;
            }
        }
        #endregion

        #region IN ANIMATIONS

        #region MoveIN
        /// <summary>
        /// Plays the Move In view animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoMoveIn(MoveIn moveIn, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!moveIn.enabled) { return; }
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();  //We need to do this check because when we Instantiate a notification we need to use the uiContainer if the parent is null.
            if (parent == null) { parent = UIManager.GetUiContainer.GetComponent<RectTransform>(); }
            Vector3 targetPosition = initialData.startAnchoredPosition3D;
#if UNITY_5_3 == false
            Canvas tempCanvas = rectTransform.GetComponent<Canvas>();
#endif
            Canvas rootCanvas = null;
#if UNITY_5_3
            rootCanvas = rectTransform.root.GetComponentInChildren<Canvas>();
#else
            if (tempCanvas == null) //this might be a button or an UIElement that does not have a Canvas component (this should not happen)
            {
                rootCanvas = rectTransform.root.GetComponentInChildren<Canvas>();
            }
            else
            {
                rootCanvas = tempCanvas.rootCanvas;
            }
#endif
            Rect rootCanvasRect = rootCanvas.GetComponent<RectTransform>().rect;
            float xOffset = rootCanvasRect.width / 2 + rectTransform.rect.width * rectTransform.pivot.x;
            float yOffset = rootCanvasRect.height / 2 + rectTransform.rect.height * rectTransform.pivot.y;

            switch (moveIn.moveFrom)
            {
                case MoveDetails.ParentPosition:
                    if (parent == null)
                        return;

                    targetPosition = new Vector2(parent.anchoredPosition.x + moveIn.positionAdjustment.x,
                                                 parent.anchoredPosition.y + moveIn.positionAdjustment.y);
                    break;

                case MoveDetails.LocalPosition:
                    if (parent == null)
                        return;

                    targetPosition = new Vector2(moveIn.positionFrom.x + moveIn.positionAdjustment.x,
                                                 moveIn.positionFrom.y + moveIn.positionAdjustment.y);
                    break;

                case MoveDetails.TopScreenEdge:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + initialData.startAnchoredPosition3D.x,
                                                 moveIn.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.RightScreenEdge:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + xOffset,
                                                 moveIn.positionAdjustment.y + initialData.startAnchoredPosition3D.y);
                    break;

                case MoveDetails.BottomScreenEdge:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + initialData.startAnchoredPosition3D.x,
                                                 moveIn.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.LeftScreenEdge:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x - xOffset,
                                                 moveIn.positionAdjustment.y + initialData.startAnchoredPosition3D.y);
                    break;

                case MoveDetails.TopLeft:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x - xOffset,
                                                 moveIn.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.TopCenter:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x,
                                                 moveIn.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.TopRight:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + xOffset,
                                                 moveIn.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.MiddleLeft:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x - xOffset,
                                                 moveIn.positionAdjustment.y);
                    break;

                case MoveDetails.MiddleCenter:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x,
                                                 moveIn.positionAdjustment.y);
                    break;

                case MoveDetails.MiddleRight:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + xOffset,
                                                 moveIn.positionAdjustment.y);
                    break;

                case MoveDetails.BottomLeft:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x - xOffset,
                                                 moveIn.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.BottomCenter:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x,
                                                 moveIn.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.BottomRight:
                    targetPosition = new Vector2(moveIn.positionAdjustment.x + xOffset,
                                                 moveIn.positionAdjustment.y - yOffset);
                    break;

                default:
                    Debug.LogWarning("[Doozy] This should not happen! DoMoveIn in UIAnimator went to the default setting!");
                    return;
            }

            DoMoveInAnimation(targetPosition, moveIn, rectTransform, initialData, instantAction);
        }

        /// <summary>
        /// This is a helper method for the DoMoveIn. It simplifies a lot the switch case for each MoveDetails
        /// </summary>
        public static void DoMoveInAnimation(Vector3 position, MoveIn moveIn, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (instantAction) { rectTransform.anchoredPosition = initialData.startAnchoredPosition3D; StartLoopAnimations(rectTransform); return; }
            rectTransform.anchoredPosition = position;
            rectTransform
                .DOAnchorPos3D(initialData.startAnchoredPosition3D, moveIn.time, false)
                    .SetDelay(moveIn.delay)
                    .SetEase(moveIn.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoMoveIn")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(moveIn.soundAtStart, initialData.soundOn); })
                    .OnComplete(() =>
                    {
                        PlaySound(moveIn.soundAtFinish, initialData.soundOn);
                        StartLoopAnimations(rectTransform);
                    })
                    .Play();
        }
        #endregion

        #region RotateIN
        /// <summary>
        /// Plays the Rotate In animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoRotationIn(RotationIn rotationIn, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!rotationIn.enabled) { return; }
            rectTransform.localRotation = Quaternion.Euler(initialData.startRotation);
            if (instantAction) { StartLoopAnimations(rectTransform); return; }
            rectTransform
                .DOLocalRotate(rotationIn.rotateFrom, rotationIn.time, RotateMode.FastBeyond360)
                    .SetDelay(rotationIn.delay)
                    .SetEase(rotationIn.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoRotationIn")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(rotationIn.soundAtStart, initialData.soundOn); })
                    .OnComplete(() =>
                    {
                        PlaySound(rotationIn.soundAtFinish, initialData.soundOn);
                        StartLoopAnimations(rectTransform);
                    })
                    .From()
                    .Play();
        }
        #endregion

        #region ScaleIN
        /// <summary>
        /// Plays the Scale In animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoScaleIn(ScaleIn scaleIn, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!scaleIn.enabled) { return; }
            if (instantAction) { rectTransform.localScale = scaleIn.scaleBegin; StartLoopAnimations(rectTransform); return; }
            rectTransform
            .DOScale(scaleIn.scaleBegin, scaleIn.time)
                .SetDelay(scaleIn.delay)
                .SetEase(scaleIn.easeType)
                .SetId(rectTransform.GetInstanceID() + "_DoScaleIn")
                .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                .OnStart(() => { PlaySound(scaleIn.soundAtStart, initialData.soundOn); })
                .OnComplete(() =>
                {
                    PlaySound(scaleIn.soundAtFinish, initialData.soundOn);
                    StartLoopAnimations(rectTransform);
                })
                .From()
                .Play();
        }
        #endregion

        #region FadeIN
        /// <summary>
        /// Plays the Fade In animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoFadeIn(FadeIn fadeIn, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!fadeIn.enabled) { return; }
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            if (canvasGroup == null) { canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>(); }
            canvasGroup.alpha = 0f;
            if (instantAction)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                StartLoopAnimations(rectTransform);
                return;
            }
            canvasGroup
            .DOFade(1f, fadeIn.time)
                .SetDelay(fadeIn.delay)
                .SetEase(fadeIn.easeType)
                .SetId(rectTransform.GetInstanceID() + "_DoFadeIn")
                .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                .OnStart(() => { PlaySound(fadeIn.soundAtStart, initialData.soundOn); })
                .OnComplete(() =>
                {
                    PlaySound(fadeIn.soundAtFinish, initialData.soundOn);
                    StartLoopAnimations(rectTransform);
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.alpha = 1f;
                })
                .Play();
        }
        #endregion

        #region IN ANIMATIONS - Stop method
        /// <summary>
        /// Stops the IN animations on a rectTransform. This is used before any OUT animations to avoid calling a hide method while a show method is still playing (without this, unexpected behaviour can occur)
        /// </summary>
        public static void StopInAnimations(RectTransform rectTransform, InitialData initialData)
        {
            if (rectTransform == null || rectTransform.GetComponent<UIElement>() == null) { return; }
            int id = rectTransform.GetInstanceID();
            DOTween.Kill(id + "_DoMoveIn");
            DOTween.Kill(id + "_DoRotationIn");
            DOTween.Kill(id + "_DoScaleIn");
            DOTween.Kill(id + "_DoFadeIn");
        }
        #endregion

        #endregion

        #region LOOP ANIMATIONS

        #region MoveLOOP
        /// <summary>
        /// This initialises and plays (if set to autoStart) the idle animation Move Loop for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoMoveLoop(MoveLoop moveLoop, RectTransform rectTransform, InitialData initialData)
        {
            if (!moveLoop.enabled) { return; }
            Vector3 animBeginPosition = new Vector3(initialData.startAnchoredPosition3D.x - moveLoop.movement.x,
                                                    initialData.startAnchoredPosition3D.y - moveLoop.movement.y,
                                                    initialData.startAnchoredPosition3D.z - moveLoop.movement.z);

            Vector3 animEndPosition = new Vector3(initialData.startAnchoredPosition3D.x + moveLoop.movement.x,
                                                   initialData.startAnchoredPosition3D.y + moveLoop.movement.y,
                                                   initialData.startAnchoredPosition3D.z + moveLoop.movement.z);
            if (moveLoop.loopType == LoopType.Yoyo)
            {
                rectTransform
                .DOAnchorPos(animBeginPosition, moveLoop.time / 2f, false)
                    .SetDelay(moveLoop.delay)
                    .SetEase(moveLoop.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoMoveLoop")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(moveLoop.soundAtStart, initialData.soundOn); })
                    .OnComplete(() =>
                    {
                        rectTransform
                        .DOAnchorPos(animEndPosition, moveLoop.time, false)
                            .SetEase(moveLoop.easeType)
                            .SetLoops(moveLoop.loops, moveLoop.loopType)
                            .SetId(rectTransform.GetInstanceID() + "_DoMoveLoop")
                            .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                            .OnComplete(() => { PlaySound(moveLoop.soundAtFinish, initialData.soundOn); })
                            .Play();
                    })
                    .Pause();
            }
            else
            {
                rectTransform
                .DOAnchorPos(moveLoop.movement, moveLoop.time, false)
                    .SetRelative(true)
                    .SetDelay(moveLoop.delay)
                    .SetLoops(moveLoop.loops, moveLoop.loopType)
                    .SetEase(moveLoop.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoMoveLoop")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(moveLoop.soundAtStart, initialData.soundOn); })
                    .OnComplete(() => { PlaySound(moveLoop.soundAtFinish, initialData.soundOn); })
                    .Pause();
            }
            if (moveLoop.autoStart) { DOTween.Play(rectTransform.GetInstanceID() + "_DoMoveLoop"); }
        }
        #endregion

        #region RotateLOOP
        /// <summary>
        /// This initialises and plays (if set to autoStart) the idle animation Rotation Loop for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoRotationLoop(RotationLoop rotationLoop, RectTransform rectTransform, InitialData initialData)
        {
            if (!rotationLoop.enabled) { return; }
            Vector3 animBeginRotation = new Vector3(initialData.startRotation.x - rotationLoop.rotation.x,
                                                     initialData.startRotation.y - rotationLoop.rotation.y,
                                                     initialData.startRotation.z - rotationLoop.rotation.z - rotationLoop.rotation.z / 4f);
            Vector3 animEndRotation = new Vector3(initialData.startRotation.x + rotationLoop.rotation.x,
                                                   initialData.startRotation.y + rotationLoop.rotation.y,
                                                   initialData.startRotation.z + rotationLoop.rotation.z);
            if (rotationLoop.loopType == LoopType.Yoyo)
            {
                rectTransform.DOLocalRotate(animBeginRotation, rotationLoop.time / 2f, RotateMode.Fast)
                   .SetDelay(rotationLoop.delay)
                   .SetEase(rotationLoop.easeType)
                   .SetId(rectTransform.GetInstanceID() + "_DoRotationLoop")
                   .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                   .OnStart(() => { PlaySound(rotationLoop.soundAtStart, initialData.soundOn); })
                   .OnComplete(() =>
                   {
                       rectTransform.DOLocalRotate(animEndRotation, rotationLoop.time, RotateMode.Fast)
                               .SetEase(rotationLoop.easeType)
                               .SetLoops(rotationLoop.loops, rotationLoop.loopType)
                               .SetId(rectTransform.GetInstanceID() + "_DoRotationLoop")
                               .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                               .OnComplete(() => { PlaySound(rotationLoop.soundAtFinish, initialData.soundOn); })
                               .Play();
                   })
                   .Pause();
            }
            else
            {
                rectTransform.DOLocalRotate(rotationLoop.rotation, rotationLoop.time, RotateMode.FastBeyond360)
                  .SetRelative(true)
                  .SetDelay(rotationLoop.delay)
                  .SetLoops(rotationLoop.loops, rotationLoop.loopType)
                  .SetEase(rotationLoop.easeType)
                  .SetId(rectTransform.GetInstanceID() + "_DoRotationLoop")
                  .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                  .OnStart(() => { PlaySound(rotationLoop.soundAtStart, initialData.soundOn); })
                  .OnComplete(() => { PlaySound(rotationLoop.soundAtFinish, initialData.soundOn); })
                  .Pause();
            }
            if (rotationLoop.autoStart) { DOTween.Play(rectTransform.GetInstanceID() + "_DoRotationLoop"); }
        }
        #endregion

        #region ScaleLOOP
        /// <summary>
        /// This initialises and plays (if set to autoStart) the idle animation Scale Loop for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoScaleLoop(ScaleLoop scaleLoop, RectTransform rectTransform, InitialData initialData)
        {
            if (!scaleLoop.enabled) { return; }
            rectTransform.localScale = scaleLoop.min;
            rectTransform
                .DOScale(scaleLoop.max, scaleLoop.time)
                .SetDelay(scaleLoop.delay)
                .SetEase(scaleLoop.easeType)
                .SetLoops(scaleLoop.loops, scaleLoop.loopType)
                .SetId(rectTransform.GetInstanceID() + "_DoScaleLoop")
                .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
               .OnStart(() => { PlaySound(scaleLoop.soundAtStart, initialData.soundOn); })
               .OnComplete(() => { PlaySound(scaleLoop.soundAtFinish, initialData.soundOn); })
               .Pause();
            if (scaleLoop.autoStart) { DOTween.Play(rectTransform.GetInstanceID() + "_DoScaleLoop"); }
        }
        #endregion

        #region FadeLOOP
        /// <summary>
        /// This initialises and plays (if set to autoStart) the idle animation Fade Loop for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoFadeLoop(FadeLoop fadeLoop, RectTransform rectTransform, InitialData initialData, AnimationTarget animationTarget = AnimationTarget.UIElement)
        {
            if (!fadeLoop.enabled) { return; }
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            if (canvasGroup == null) { canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>(); }
            canvasGroup.alpha = fadeLoop.max;
            switch (animationTarget)
            {
                case AnimationTarget.UIElement: canvasGroup.blocksRaycasts = false; break; //we do this so that the UIElement ignores the clicks
                case AnimationTarget.UIButton: canvasGroup.blocksRaycasts = true; break; //we do this so that we can click on the button
            }
            canvasGroup
                .DOFade(fadeLoop.min, fadeLoop.time)
                    .SetDelay(fadeLoop.delay)
                    .SetEase(fadeLoop.easeType)
                    .SetLoops(fadeLoop.loops, fadeLoop.loopType)
                    .SetId(rectTransform.GetInstanceID() + "_DoFadeLoop")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(fadeLoop.soundAtStart, initialData.soundOn); })
                    .OnComplete(() => { PlaySound(fadeLoop.soundAtFinish, initialData.soundOn); })
                    .Pause();
            if (fadeLoop.autoStart) { DOTween.Play(rectTransform.GetInstanceID() + "_DoFadeLoop"); }
        }
        #endregion

        #region LOOP ANIMATIONS - Start and Stop methods
        /// <summary>
        /// Starts the idle animations set up on a rectTransform.
        /// </summary>
        public static void StartLoopAnimations(RectTransform rectTransform)
        {
            UIElement uiElement = rectTransform.GetComponent<UIElement>();
            if (uiElement == null) { Debug.LogWarning("[Doozy] " + rectTransform.name + " does not have a UIElement Component attached and you are trying to start an idle animation"); return; }
            int id = rectTransform.GetInstanceID();
            if (uiElement.moveLoop.enabled) { DoMoveLoop(uiElement.moveLoop, rectTransform, uiElement.GetInitialData); DOTween.Play(id + "_DoMoveLoop"); }
            if (uiElement.rotationLoop.enabled) { DoRotationLoop(uiElement.rotationLoop, rectTransform, uiElement.GetInitialData); DOTween.Play(id + "_DoRotationLoop"); }
            if (uiElement.scaleLoop.enabled) { DoScaleLoop(uiElement.scaleLoop, rectTransform, uiElement.GetInitialData); DOTween.Play(id + "_DoScaleLoop"); }
            if (uiElement.fadeLoop.enabled) { DoFadeLoop(uiElement.fadeLoop, rectTransform, uiElement.GetInitialData); DOTween.Play(id + "_DoFadeLoop"); }
        }

        /// <summary>
        /// Stops the idle animations on a rectTransform.
        /// </summary>
        public static void StopLoopAnimations(RectTransform rectTransform, InitialData initialData)
        {
            if (rectTransform == null || initialData == null || rectTransform.GetComponent<UIElement>() == null) { return; }
            ResetRectTransform(rectTransform, initialData);
            int id = rectTransform.GetInstanceID();
            DOTween.Kill(id + "_DoMoveLoop");
            DOTween.Kill(id + "_DoRotationLoop");
            DOTween.Kill(id + "_DoScaleLoop");
            DOTween.Kill(id + "_DoFadeLoop");

            // Unity has a bug where the layouts don't update correctly (2017-04-07 / Unity 5.5.0p4)
            // This bug has been around for awhile and may be around for the future
            // This forces an update of at least the parent, this should be enough since we are only animating our local positions
            if (rectTransform.parent)
            {
                var layout = rectTransform.parent.GetComponent<UnityEngine.UI.LayoutGroup>();
                if (layout && layout.enabled)
                {
                    layout.enabled = !layout.enabled; layout.enabled = !layout.enabled;
                }
            }
        }
        #endregion

        #endregion

        #region OUT ANIMATIONS

        #region MoveOUT
        /// <summary>
        /// Plays the Move Out of view animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoMoveOut(MoveOut moveOut, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!moveOut.enabled) { return; }
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();  //We need to do this check because when we Instantiate a notification we need to use the uiContainer if the parent is null.
            if (parent == null) { parent = UIManager.GetUiContainer.GetComponent<RectTransform>(); }
            Vector2 targetPosition = Vector2.zero;
#if UNITY_5_3 == false
            Canvas tempCanvas = rectTransform.GetComponent<Canvas>();
#endif
            Canvas rootCanvas = null;
#if UNITY_5_3
            rootCanvas = rectTransform.root.GetComponentInChildren<Canvas>();
#else
            if (tempCanvas == null) //this might be a button or an UIElement that does not have a Canvas component (this should not happen)
            {
                rootCanvas = rectTransform.root.GetComponentInChildren<Canvas>();
            }
            else
            {
                rootCanvas = tempCanvas.rootCanvas;
            }
#endif
            Rect rootCanvasRect = rootCanvas.GetComponent<RectTransform>().rect;
            float xOffset = rootCanvasRect.width / 2 + rectTransform.rect.width * rectTransform.pivot.x;
            float yOffset = rootCanvasRect.height / 2 + rectTransform.rect.height * rectTransform.pivot.y;

            switch (moveOut.moveTo)
            {
                case MoveDetails.ParentPosition:
                    if (parent == null)
                        return;

                    targetPosition = new Vector2(moveOut.positionAdjustment.x + parent.anchoredPosition.x,
                                                 moveOut.positionAdjustment.y + parent.anchoredPosition.y);
                    break;

                case MoveDetails.LocalPosition:
                    if (parent == null)
                        return;

                    targetPosition = new Vector2(moveOut.positionAdjustment.x + moveOut.positionTo.x,
                                                 moveOut.positionAdjustment.y + moveOut.positionTo.y);
                    break;

                case MoveDetails.TopScreenEdge:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + initialData.startAnchoredPosition3D.x,
                                                 moveOut.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.RightScreenEdge:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + xOffset,
                                                 moveOut.positionAdjustment.y + initialData.startAnchoredPosition3D.y);
                    break;

                case MoveDetails.BottomScreenEdge:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + initialData.startAnchoredPosition3D.x,
                                                 moveOut.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.LeftScreenEdge:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x - xOffset,
                                                 moveOut.positionAdjustment.y + initialData.startAnchoredPosition3D.y);
                    break;

                case MoveDetails.TopLeft:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x - xOffset,
                                                 moveOut.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.TopCenter:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x,
                                                 moveOut.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.TopRight:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + xOffset,
                                                 moveOut.positionAdjustment.y + yOffset);
                    break;

                case MoveDetails.MiddleLeft:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x - xOffset,
                                                 moveOut.positionAdjustment.y);
                    break;

                case MoveDetails.MiddleCenter:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x,
                                                 moveOut.positionAdjustment.y);
                    break;

                case MoveDetails.MiddleRight:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + xOffset,
                                                 moveOut.positionAdjustment.y);
                    break;

                case MoveDetails.BottomLeft:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x - xOffset,
                                                 moveOut.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.BottomCenter:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x,
                                                 moveOut.positionAdjustment.y - yOffset);
                    break;

                case MoveDetails.BottomRight:
                    targetPosition = new Vector2(moveOut.positionAdjustment.x + xOffset,
                                                 moveOut.positionAdjustment.y - yOffset);
                    break;

                default:
                    Debug.LogWarning("[Doozy] This should not happen! DoMoveOut in UIAnimator went to the default setting!");
                    return;
            }

            DoMoveOutAnimation(targetPosition, moveOut, rectTransform, initialData, instantAction);
        }

        /// <summary>
        /// This is a helper method for the DoMoveOut. It simplifies a lot the switch case for each MoveDetails
        /// </summary>
        public static void DoMoveOutAnimation(Vector2 position, MoveOut moveOut, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (instantAction) { rectTransform.anchoredPosition3D = position; return; }
            rectTransform
                .DOAnchorPos3D(position, moveOut.time, false)
                    .SetDelay(moveOut.delay)
                    .SetEase(moveOut.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoMoveOut")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(moveOut.soundAtStart, initialData.soundOn); })
                    .OnComplete(() => { PlaySound(moveOut.soundAtFinish, initialData.soundOn); })
                    .Play();
        }
        #endregion

        #region RotateOUT
        /// <summary>
        /// Plays the Rotate Out animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoRotationOut(RotationOut rotationOut, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!rotationOut.enabled) { return; }
            if (instantAction) { rectTransform.localRotation = Quaternion.Euler(rotationOut.rotateTo); return; }
            rectTransform
                .DOLocalRotate(rotationOut.rotateTo, rotationOut.time, RotateMode.FastBeyond360)
                    .SetDelay(rotationOut.delay)
                    .SetEase(rotationOut.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoRotationOut")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(rotationOut.soundAtStart, initialData.soundOn); })
                    .OnComplete(() => { PlaySound(rotationOut.soundAtFinish, initialData.soundOn); })
                    .Play();
        }
        #endregion

        #region ScaleOUT
        /// <summary>
        /// Plays the Scale Out animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoScaleOut(ScaleOut scaleOut, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!scaleOut.enabled) { return; }
            if (instantAction) { rectTransform.localScale = scaleOut.scaleEnd; return; }
            rectTransform
                .DOScale(scaleOut.scaleEnd, scaleOut.time)
                    .SetDelay(scaleOut.delay)
                    .SetEase(scaleOut.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoScaleOut")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(scaleOut.soundAtStart, initialData.soundOn); })
                    .OnComplete(() =>
                    {
                        PlaySound(scaleOut.soundAtFinish, initialData.soundOn);
                        if (rectTransform.localScale != scaleOut.scaleEnd) { rectTransform.DOScale(scaleOut.scaleEnd, 0).Play(); }
                    })
                    .Play();
        }
        #endregion

        #region FadeOUT
        /// <summary>
        /// Plays the Fade Out animation for a RectTransform with a PanelController component added to it.
        /// </summary>
        public static void DoFadeOut(FadeOut fadeOut, RectTransform rectTransform, InitialData initialData, bool instantAction)
        {
            if (!fadeOut.enabled) { return; }
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            if (canvasGroup == null) { canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>(); }
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            if (instantAction) { canvasGroup.alpha = 0f; return; }
            canvasGroup
                .DOFade(0f, fadeOut.time)
                    .SetDelay(fadeOut.delay)
                    .SetEase(fadeOut.easeType)
                    .SetId(rectTransform.GetInstanceID() + "_DoFadeOut")
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnStart(() => { PlaySound(fadeOut.soundAtStart, initialData.soundOn); })
                    .OnComplete(() => { PlaySound(fadeOut.soundAtFinish, initialData.soundOn); })
                    .Play();
        }
        #endregion

        #region OUT ANIMATIONS - Stop method
        /// <summary>
        /// Stops the OUT animations on a rectTransform. This is used before any IN animations to avoid calling a show method while a hide method is still playing (without this, unexpected behaviour can occur)
        /// </summary>
        public static void StopOutAnimations(RectTransform rectTransform, InitialData initialData)
        {
            if (rectTransform == null || rectTransform.GetComponent<UIElement>() == null) { return; }
            int id = rectTransform.GetInstanceID();
            DOTween.Kill(id + "_DoMoveOut");
            DOTween.Kill(id + "_DoRotationOut");
            DOTween.Kill(id + "_DoScaleOut");
            DOTween.Kill(id + "_DoFadeOut");
        }
        #endregion

        #endregion

        #region BUTTON ANIMATIONS

        #region OnClick Animations
        public static void StartOnClickAnimations(RectTransform rectTransform, InitialData initialData, UIAnimationManager.OnClickAnimations onClickAnimSettings)
        {
            if (onClickAnimSettings.punchPositionEnabled)
            {
                rectTransform.DOPunchAnchorPos(onClickAnimSettings.punchPositionPunch, onClickAnimSettings.punchPositionDuration, onClickAnimSettings.punchPositionVibrato, onClickAnimSettings.punchPositionElasticity, onClickAnimSettings.punchPositionSnapping)
                    .SetDelay(onClickAnimSettings.punchPositionDelay)
                    .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                    .OnComplete(() =>
                    {
                        rectTransform.DOAnchorPos3D(initialData.startAnchoredPosition3D, 0.1f)
                            .SetUpdate(UpdateType.Normal, true)
                            .Play();
                    })
                    .Play();
            }

            if (onClickAnimSettings.punchRotationEnabled)
            {
                rectTransform.DOPunchRotation(onClickAnimSettings.punchRotationPunch, onClickAnimSettings.punchRotationDuration, onClickAnimSettings.punchRotationVibrato, onClickAnimSettings.punchPositionElasticity)
                        .SetDelay(onClickAnimSettings.punchRotationDelay)
                        .SetUpdate(UpdateType.Normal, true)
                        .OnComplete(() =>
                        {
                            rectTransform.DORotate(initialData.startRotation, 0.1f)
                                .SetUpdate(UpdateType.Normal, true)
                                .Play();
                        })
                        .Play();
            }

            if (onClickAnimSettings.punchScaleEnabled)
            {
                rectTransform.DOPunchScale(onClickAnimSettings.punchScalePunch, onClickAnimSettings.punchScaleDuration, onClickAnimSettings.punchScaleVibrato, onClickAnimSettings.punchScaleElasticity)
                        .SetDelay(onClickAnimSettings.punchScaleDelay)
                        .SetUpdate(UpdateType.Normal, UIManager.isTimeScaleIndependent)
                        .OnComplete(() =>
                        {
                            rectTransform.DOScale(initialData.startScale, 0.1f)
                                .SetUpdate(UpdateType.Normal, true)
                                .Play();
                        })
                        .Play();
            }
        }
        #endregion

        #region Normal and Highlighted Animations
        /// <summary>
        /// Starts button loop animations for an UIButton
        /// </summary>
        public static void StartButtonLoopsAnimations(RectTransform rectTransform, InitialData initialData, UIAnimationManager.ButtonLoopsAnimations animationSettings)
        {
            if (rectTransform.GetComponent<UIButton>() == null) { Debug.LogWarning("[Doozy] " + rectTransform.name + " does not have a UIButton Component attached and you are trying to start a normal state animation"); return; }
            int id = rectTransform.GetInstanceID();
            if (animationSettings.moveLoop.enabled) { DoMoveLoop(animationSettings.moveLoop, rectTransform, initialData); DOTween.Play(id + "_DoMoveLoop"); }
            if (animationSettings.rotationLoop.enabled) { DoRotationLoop(animationSettings.rotationLoop, rectTransform, initialData); DOTween.Play(id + "_DoRotationLoop"); }
            if (animationSettings.scaleLoop.enabled) { DoScaleLoop(animationSettings.scaleLoop, rectTransform, initialData); DOTween.Play(id + "_DoScaleLoop"); }
            if (animationSettings.fadeLoop.enabled) { DoFadeLoop(animationSettings.fadeLoop, rectTransform, initialData, AnimationTarget.UIButton); DOTween.Play(id + "_DoFadeLoop"); }
        }

        /// <summary>
        /// Stops the button loops animations for a UIButton
        /// </summary>
        public static void StopButtonLoopsAnimations(RectTransform rectTransform, InitialData initialData)
        {
            if (rectTransform == null || initialData == null || rectTransform.GetComponent<UIButton>() == null) { return; }
            ResetRectTransform(rectTransform, initialData);
            int id = rectTransform.GetInstanceID();
            DOTween.Kill(id + "_DoMoveLoop");
            DOTween.Kill(id + "_DoRotationLoop");
            DOTween.Kill(id + "_DoScaleLoop");
            DOTween.Kill(id + "_DoFadeLoop");
        }
        #endregion

        #endregion
    }
}
