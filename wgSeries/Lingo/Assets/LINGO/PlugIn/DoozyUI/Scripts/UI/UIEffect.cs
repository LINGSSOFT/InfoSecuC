// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [AddComponentMenu("DoozyUI/UI Effect", 4)]
    [RequireComponent(typeof(ParticleSystem))]
    public class UIEffect : MonoBehaviour
    {
        #region Context Menu Methods
#if UNITY_EDITOR

        [MenuItem("DoozyUI/Components/UI Effect", false, 4)]
        [MenuItem("GameObject/DoozyUI/UI Effect", false, 4)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            if (GameObject.Find("UIManager") == null)
            {
                Debug.LogError("[DoozyUI] The DoozyUI system was not found in the scene. Please add it before trying to create a UI Effect.");
                return;
            }
            GameObject go = new GameObject("New UIEffect");
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
            go.AddComponent<UIEffect>();
            Selection.activeObject = go;
        }
#endif
        #endregion

        #region Enums
        public enum EffectPosition { InFrontOfTarget, BehindTarget };
        #endregion

        #region Public Variables
        [HideInInspector]
        public bool showHelp = false;

        public UIElement targetUIElement = null;

        [Tooltip("After the show element command has been issues, start this effect after the startDelay")]
        public float startDelay = 0f;

        [Tooltip("If you want this to particle effect to play on awake, set it true. (Deafult: false)")]
        public bool playOnAwake = false;

        [Tooltip("If you want the particle system to wait for all the particles to dissapear or clear the screen by hiding them all at once. (Default: false)")]
        public bool stopInstantly = false;

        public EffectPosition effectPosition = EffectPosition.InFrontOfTarget;
        public int sortingOrderStep = 1;   //Taking into account the target's [Canvas][Order in Layer][value] - we adjust the [ParticleSystem][Renderer][Order in Layer][value] with this sorting step (by adding, if set to InFrontOfTarget or subtrcting, id set BehindTarget)

        [HideInInspector]
        public bool autoRegister = true;    //if this effect is handled by a notification, the we let the notification handle the registration process with an auto generated name
        [HideInInspector]
        public bool isVisible = true;
        #endregion

        #region Private Variables
        private ParticleSystem masterPS;
#if UNITY_5_5_OR_NEWER
        private ParticleSystem.MainModule masterPS_MainModule;
#endif
        private float lifetime;
        private Coroutine resetCoroutine;
        private Coroutine startCoroutine;

        private ParticleSystem[] allThePS;
        private Canvas targetCanvas;
        private int targetPhysicsLayerID;
        //private int targetSortingLayerID;
        private string targetSortingLayerName;
        private int targetSortingOrder;
#endregion

        void Awake()
        {
            masterPS = GetComponent<ParticleSystem>();
#if UNITY_5_5_OR_NEWER
            masterPS_MainModule = masterPS.main;
            masterPS_MainModule.playOnAwake = playOnAwake;
            lifetime = masterPS_MainModule.startLifetimeMultiplier;
#else
            masterPS.playOnAwake = playOnAwake;
            lifetime = masterPS.startLifetime;
#endif
            resetCoroutine = null;
            startCoroutine = null;
        }

        void OnEnable()
        {
            if (targetUIElement == null)
            {
                Debug.Log("[DoozyUI] The UIEffect on [" + gameObject.name + "] gameObject is disabled. It will not work because it has no target UIElement selected.");
                playOnAwake = false;
                stopInstantly = true;
                ResetParticleSystem();
                return;
            }

            if (autoRegister)
            {
                UIManager.RegisterUiEffect(this);
            }

            masterPS.Stop(true);
            masterPS.Clear(true);
            isVisible = false;

            if (playOnAwake)
            {
                masterPS.Play(true);
                isVisible = true;
            }
        }

        void OnDisable()
        {
            if (targetUIElement == null)
                return;

            UIManager.UnregisterUiEffect(this);
        }

        void Start()
        {
            if (targetUIElement == null)
                return;

            UpdateEffectSortingOrder();
        }

#region Update UIEffect SortingOrder
        public void UpdateEffectSortingOrder()
        {
            allThePS = GetComponentsInChildren<ParticleSystem>(true);

            targetPhysicsLayerID = targetUIElement.transform.gameObject.layer;

            targetCanvas = targetUIElement.transform.GetComponent<Canvas>();

            if (targetCanvas == null)
                targetCanvas = targetUIElement.gameObject.AddComponent<Canvas>();

            //targetSortingLayerID = targetCanvas.sortingLayerID;
            targetSortingLayerName = targetCanvas.sortingLayerName;
            targetSortingOrder = targetCanvas.sortingOrder;

            //Update the sorting layer (for the camera)
            gameObject.layer = targetPhysicsLayerID;

            foreach (Transform child in transform)
            {
                //Update the sorting layer in children (for the camera)
                child.gameObject.layer = targetPhysicsLayerID;
            }

            for (int i = 0; i < allThePS.Length; i++)
            {

                //allThePS[i].GetComponent<Renderer>().sortingLayerID = targetSortingLayerID;
                allThePS[i].GetComponent<Renderer>().sortingLayerName = targetSortingLayerName;

                if (effectPosition == EffectPosition.InFrontOfTarget)
                {
                    allThePS[i].GetComponent<Renderer>().sortingOrder = targetSortingOrder + sortingOrderStep;
                }
                else
                {
                    allThePS[i].GetComponent<Renderer>().sortingOrder = targetSortingOrder - sortingOrderStep;
                }
            }
        }
#endregion

#region Show
        /// <summary>
        /// Shows the effect.
        /// </summary>
        public void Show()
        {
            if (!isVisible)
            {
                if (resetCoroutine != null)
                {
                    StopCoroutine(resetCoroutine);
                }

                if (startCoroutine != null)
                {
                    StopCoroutine(startCoroutine);
                }

                startCoroutine = StartCoroutine("StartEffectAfterDelay");
            }
        }
#endregion

#region Hide
        /// <summary>
        /// Hides the effect.
        /// </summary>
        public void Hide()
        {
            if (isVisible)
            {
                ResetParticleSystem();
            }
        }
#endregion

#region Reset ParticleSystem
        /// <summary>
        /// Resets the particle system instantly
        /// </summary>
        void ResetParticleSystem()
        {
            isVisible = false;

            if (stopInstantly)
            {
                masterPS.Stop(true);
                masterPS.Clear(true);
                resetCoroutine = null;
            }
            else
            {
                resetCoroutine = StartCoroutine("ResetAndDisableParticleSystemAfterLifetime");
            }
        }
#endregion

#region IEnumerators - ResetAndDisableParticleSystemAfterLifetime, StartEffectAfterDelay
        /// <summary>
        /// Resets the particle system after all the particles have dissapeared naturally (after their lifetime)
        /// </summary>
        /// <returns></returns>
        IEnumerator ResetAndDisableParticleSystemAfterLifetime()
        {
            masterPS.Stop(true);
            yield return new WaitForSeconds(lifetime);
            masterPS.Clear(true);

            resetCoroutine = null;
        }

        /// <summary>
        /// Starts the particle system after the initial delay (default is 0 seconds)
        /// </summary>
        /// <returns></returns>
        IEnumerator StartEffectAfterDelay()
        {
            yield return new WaitForSeconds(startDelay);
            masterPS.Play(true);
            isVisible = true;

            startCoroutine = null;
        }
#endregion
    }
}
