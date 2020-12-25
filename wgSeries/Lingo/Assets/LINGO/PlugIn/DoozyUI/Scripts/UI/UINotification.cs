// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [AddComponentMenu("DoozyUI/UI Notification", 5)]
    public class UINotification : MonoBehaviour
    {
        #region Context Menu
#if UNITY_EDITOR
        [MenuItem("DoozyUI/Components/UI Notification", false, 5)]
        [MenuItem("GameObject/DoozyUI/UI Notification", false, 5)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            if (GameObject.Find("UIManager") == null)
            {
                Debug.LogError("[DoozyUI] The DoozyUI system was not found in the scene. Please add it before trying to create a UI Notification.");
                return;
            }

            GameObject notification = new GameObject("New UINotification");
            GameObjectUtility.SetParentAndAlign(notification, menuCommand.context as GameObject);
            notification.AddComponent<RectTransform>();
            if (notification.transform.parent == null)
            {
                notification.transform.SetParent(UIManager.GetUiContainer);
            }
            notification.GetComponent<RectTransform>().localScale = Vector3.one;
            notification.AddComponent<UINotification>();

            GameObject notificationContainer = new GameObject("NotificationContainer");
            notificationContainer.transform.parent = notification.transform;
            notificationContainer.AddComponent<RectTransform>();
            notificationContainer.GetComponent<RectTransform>().position = notification.GetComponent<RectTransform>().position;
            notificationContainer.GetComponent<RectTransform>().localScale = Vector3.one;
            notificationContainer.AddComponent<Button>();
            notificationContainer.GetComponent<Button>().transition = Selectable.Transition.None;
            notificationContainer.AddComponent<UIElement>();
            notificationContainer.layer = notification.layer;
            notification.GetComponent<UINotification>().notificationContainer = notificationContainer.GetComponent<UIElement>();
            notification.GetComponent<UINotification>().closeButton = notificationContainer.GetComponent<Button>();
            Undo.RegisterCreatedObjectUndo(notification, "Create " + notification.name);
            Selection.activeObject = notificationContainer;
            Selection.activeObject = notification;
        }
#endif
        #endregion

        #region Internal Classes - NotificationData
        [System.Serializable]
        public class NotificationData
        {
            /// <summary>
            /// The name of the notification prefab in a 'Resources' folder
            /// </summary>
            public string prefabName;

            /// <summary>
            /// The prefab GameObject
            /// </summary>
            public GameObject prefab;

            /// <summary>
            /// The lifetime of the norification. Excluding the IN and OUT animation times, as they are calculated separately.
            /// </summary>
            public float lifetime = defaultLifetime;

            /// <summary>
            /// Should this notification be added to the Norification Queue or should it ignore it? (default: true)
            /// </summary>
            public bool addToNotificationQueue = defaultAddToNotificationQueue;

            /// <summary>
            /// If the notification has a title, this text will appear there.
            /// </summary>
            public string title = defaultTitle;

            /// <summary>
            /// If the notification has a message, this text will appear there.
            /// </summary>
            public string message = defaultMessage;

            /// <summary>
            /// If the notification has a custom icon, this sprite will appear there.
            /// </summary>
            public Sprite icon = defaultIcon;

            /// <summary>
            /// If the notification has buttons, these are the buttonNames that will be sent on Button Click. If there are 3 buttons available and you enter only 2 buttonNames, only those 2 buttons will be visible and active (the 3rd will not appear, nor work).
            /// </summary>
            public string[] buttonNames = defaultButtonNames;

            /// <summary>
            /// If the notification has buttons and those buttons have a Text or a TextMeshProUGUI compoment attached to them or one of their children, then these are the button text that will appear on the buttons.
            /// If there are 3 buttons available and active, and you enter the button text for only 2 of them, only the first 2 buttons well have a text, and the third will have nothing.
            /// You can leave this null if your buttons show pre-set icons instead of text.
            /// </summary>
            public string[] buttonTexts = defaultButtonTexts;

            /// <summary>
            /// Callback action @Hide
            /// </summary>
            public UnityAction hideCallback = null;

            /// <summary>
            /// Callback action for every button
            /// </summary>
            public UnityAction[] buttonCallback = null;
        }
        #endregion

        #region Default Values for NotificationData
        public const float defaultLifetime = 3f;
        public const bool defaultAddToNotificationQueue = true;
        public const string defaultTitle = null;
        public const string defaultMessage = null;
        public const Sprite defaultIcon = null;
        public const string[] defaultButtonNames = null;
        public const string[] defaultButtonTexts = null;
        #endregion

        #region Public Variables
        [HideInInspector]
        public bool showHelp = false;
        public bool listenForBackButton = true;

        public UIElement notificationContainer; //the main UIElement
        public UIElement overlay;               //an overlay to dimm or tint the screen
        public GameObject title = null;         //the title of the notification
        public GameObject message = null;       //the message of the notification
        public Image icon = null;               //the icon of the notification
        public UIButton[] buttons = null;       //the buttons on the notification
        public Button closeButton = null;       //this is a button that covers the entire notification and closes it on click
        public UIElement[] specialElements;     //if there are special elements that need to be animated they need to be linked here (like stars, banners, flags, icons...)
        public UIEffect[] effects;              //if you add any effects on the notification, you can link them here
        #endregion

        #region Private Variables
        private NotificationData notificationData;
        private string notificationName = string.Empty;     //this is used to auto-generate a uiElement name (we use it to hide this notification when a one of it's buttons is clicked)
        private bool closeOnClick = false;
        #endregion

        void Awake()
        {
            if (notificationContainer == null)
            {
                return;
            }

            gameObject.layer = UIManager.GetUiContainer.gameObject.layer;

            notificationName = "Notification" + " [" + gameObject.GetInstanceID() + "]";    //we generate a unique name for this notification UIElement (we need it to be able to hide it when we press one of it's buttons)
            notificationContainer.elementName = notificationName;
            notificationContainer.autoRegister = false; //we want to register this element to the registry with the auto generated name (notifiacationName)
            notificationContainer.animateAtStart = true;

            if (overlay != null)
            {
                overlay.elementName = notificationName;    //we use the same elementName so it will get hidden along with the main uiElement panel
                overlay.autoRegister = false;   //we want this notification to register this element to the registry with the auto generated name (notifiacationName)
                overlay.animateAtStart = true;
            }

            if (specialElements != null && specialElements.Length > 0)
            {
                for (int i = 0; i < specialElements.Length; i++)
                {
                    specialElements[i].elementName = notificationName;   //we use the same elementName so it will get hidden along with the main uiElement panel
                    specialElements[i].autoRegister = false;    //we want this notification to register this element to the registry with the auto generated name (notifiacationName)
                    specialElements[i].animateAtStart = true;
                }
            }

            if (effects != null && effects.Length > 0)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] != null && effects[i].targetUIElement != null)
                    {
                        effects[i].targetUIElement.elementName = notificationName;   //we use the same elementName so it will get hidden along with the main uiElement panel
                        effects[i].autoRegister = false;    //we want this notification to register this effect to the registry with the auto generated name (notifiacationName)
                        effects[i].playOnAwake = true;
                    }
                }
            }

            notificationData = new NotificationData();

            //New back button disable (replaces the method used above)
            UIManager.DisableBackButton(); //we disable the 'Back' button

            RegisterToUIManager();
        }

        void OnEnable()
        {
            if (notificationContainer == null)
            {
                Debug.Log("[DoozyUI] The UINotification on [" + gameObject.name + "] gameObject is disabled. It will not work because you didn't link a notification container. This should be a child of the notification gamObject and it should have a UIElement on it. Also, the notification container should have at least one IN and one OUT animations enabled.");
                return;
            }

            RegisterToUIManager();
        }

        void Update()
        {
            if (listenForBackButton && Input.GetKeyDown(KeyCode.Escape)) //The listener for the 'Back' button event; we do this because we do not want to change the UI state while a notification is on screen (we disabled the back button from the UI Manager and the UI Notifications needs to listen for it now)
            {
                if (closeOnClick) //only if closeOnClick has been enabled (a closeOnClick button has been assigned) we close the notification
                    BackButtonEvent();
            }
        }

        void OnDisable()
        {
            if (notificationContainer == null)
            {
                return;
            }

            /*
             *OLD METHOD TO DISABLE THE BACK BUTTON (before the additive bool method)
            if (didThisNotificationDisableTheBackButton) //if we disabled the 'Back' button on Awake, we enable it back, now that this notification is closing
            {
                UIManager.EnableBackButton();
            }
            */

            //New back button disable (replaces the method used above)
            UIManager.EnableBackButton();

            UnregisterFromNotificationQueue(notificationData);
            UnregisterFromUIManager();
        }

        #region Show Notification
        public void ShowNotification(NotificationData ndata)
        {
            foreach (Transform t in transform) //in case we have a child that is not on the proper layer, we set it here so it shows up in the UI camera
            {
                t.gameObject.layer = gameObject.layer;
            }

            notificationData = ndata;   //we save this data to use it if we need to unregister from the Notification Queue

            if (icon != null && ndata.icon != null) //if this notification has an icon slot and the show notification passed a new icon, we update it
            {
                icon.sprite = ndata.icon;
            }

            if (ndata.buttonTexts == null || ndata.buttonTexts.Length == 0)
                closeOnClick = true;    //if there are no button texts we let the user close this notification just by ckicking it

            if (ndata.buttonNames == null || ndata.buttonNames.Length == 0)
                closeOnClick = true;    //if there are no button names we let the user close this notification just by ckicking it

            if (closeButton != null && closeOnClick) //if we linked the closeOnClickButton, we configure it to close the notification window on click
            {
                UIButton b = closeButton.GetComponent<UIButton>();
                float onClickDelay = 0f;
                if (b != null) //we check if we have an UIButton attached (we do this so that if the button has an onClick animation, we show it and after that we hide the notification)
                {
                    onClickDelay = b.GetOnClickAnimationsDuration; //this creates a more pleasent user experience (UX) by letthing the OnClick animation play before hiding the notification
                }

                closeButton.onClick.AddListener(() =>
                {
                    StartCoroutine(HideNotification(onClickDelay));
                    //Destroy(gameObject, GetOutAnimationsTimeAndDelay() + onClickDelay); //we destroy this notification after the Out animation finished
                    StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay() + onClickDelay));
                });
            }

            if (ndata.lifetime > 0)  //We look for the lifetime (if it's -1 we do not auto hide the notification. We wait for the player to hit a button.
            {
                StartCoroutine(HideNotification(GetInAnimationsTimeAndDelay() + ndata.lifetime));
                //Destroy(gameObject, GetInAnimationsTimeAndDelay() + ndata.lifetime + GetOutAnimationsTimeAndDelay()); //We wait for the in animations + the specified lifetime + the out animations and then we destroy the object
                StartCoroutine(DestroyAfterTime(GetInAnimationsTimeAndDelay() + ndata.lifetime + GetOutAnimationsTimeAndDelay()));
            }

            if (UIManager.usesTMPro) //If we are using the TextMeshPro plugin we will look for TextMeshProUGUI component otherwise we look for the native Text component
            {
#if dUI_TextMeshPro
                if (this.title != null)
                    this.title.GetComponent<TMPro.TextMeshProUGUI>().text = ndata.title;
                if (this.message != null)
                    this.message.GetComponent<TMPro.TextMeshProUGUI>().text = ndata.message;
#endif
            }
            else
            {
                if (this.title != null)
                    this.title.GetComponent<Text>().text = ndata.title;
                if (this.message != null)
                    this.message.GetComponent<Text>().text = ndata.message;
            }

            if (buttons != null && ndata.buttonNames != null) //If this notification prefab has buttons and buttonNames is not null (those are the buttonNames we are listening for) we start adding the buttons
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var index = i;
                    buttons[i].GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (ndata.buttonCallback != null && index < ndata.buttonCallback.Length && ndata.buttonCallback[index] != null)
                        {
                            ndata.buttonCallback[index].Invoke();
                        }
                        StartCoroutine(HideNotification(0.2f));
                        StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay() + 0.2f)); //We destroy this notification after the Out animation finished
                    });

                    if (ndata.buttonNames.Length > i && string.IsNullOrEmpty(ndata.buttonNames[i]) == false) //If we have a buttonName we make the button active and set the buttonName to the UIButton compoenent
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].buttonName = ndata.buttonNames[i];   //We set the buttonName

                        if (ndata.buttonTexts != null) //We might not have a text for the button (it might be an image or an icon) so we check if we wanted a text on it
                        {
                            if (ndata.buttonTexts.Length > i && !string.IsNullOrEmpty(ndata.buttonTexts[i]))
                            {
                                if (UIManager.usesTMPro)
                                {
#if dUI_TextMeshPro
                                    if (buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>() != null)
                                        buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ndata.buttonTexts[i];
#endif
                                }
                                else
                                {
                                    if (buttons[i].GetComponentInChildren<Text>() != null)
                                        buttons[i].GetComponentInChildren<Text>().text = ndata.buttonTexts[i];
                                }
                            }
                        }

                    }
                    else
                    {
                        buttons[i].gameObject.SetActive(false); //if we still have unused buttons on this notification prefab, we hide them
                    }
                }
            }
            UIManager.ShowUiElement(notificationName);
        }
        #endregion

        #region IEnumerator and Method - Hide Notification
        IEnumerator HideNotification(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay)
            {
                yield return null;
            }

            UIManager.HideUiElement(notificationName, false, false);
        }

        public void HideNotification(bool hideAndDestroy = true)
        {
            UIManager.HideUiElement(notificationName, false, false);
            if (hideAndDestroy)
                StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay()));
        }
        #endregion

        #region Register to UIManager
        void RegisterToUIManager()
        {
            if (notificationContainer == null)
            {
                Debug.LogError("[DoozyUI] The notification [" + gameObject.name + "] does not have an UIElement linked to the notification field. This should not happen and you will get unexpected results. To fix this, link a child UIElement as the notification's body.");
            }
            else
            {
                UIManager.RegisterUiElement(notificationContainer);
            }

            if (overlay != null)
            {
                UIManager.RegisterUiElement(overlay);
            }

            if (specialElements != null && specialElements.Length > 0)
            {
                for (int i = 0; i < specialElements.Length; i++)
                {
                    if (specialElements[i] != null)
                    {
                        UIManager.RegisterUiElement(specialElements[i]);
                    }
                    else
                    {
                        Debug.LogWarning("[DoozyUI] The notification [" + gameObject.name + "] has unassigned array slots for the Special Elements. To fix this, just remove the unused slots in the array or assign UIElements to them.");
                    }
                }
            }

            if (effects != null && effects.Length > 0)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] != null)
                    {
                        if (effects[i].targetUIElement != null)
                        {
                            UIManager.RegisterUiEffect(effects[i]);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("[DoozyUI] The notification [" + gameObject.name + "] has unassigned array slots for the Effects. To fix this, just remove the unused slots in the array or assign UIEffects to them.");
                    }

                }
            }
        }
        #endregion

        #region Unregister from UIManager
        void UnregisterFromUIManager()
        {
            if (notificationContainer == null)
            {
                Debug.LogError("[DoozyUI] The notification [" + gameObject.name + "] does not have an UIElement linked to the notification field. This should not happen and you will get unexpected results.");
            }
            else
            {
                UIManager.UnregisterUiElement(notificationContainer);
            }

            if (overlay != null)
            {
                UIManager.UnregisterUiElement(overlay);
            }

            if (specialElements != null && specialElements.Length > 0)
            {
                for (int i = 0; i < specialElements.Length; i++)
                {
                    if (specialElements[i] != null)
                    {
                        UIManager.UnregisterUiElement(specialElements[i]);
                    }
                }
            }

            if (effects != null && effects.Length > 0)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    if (effects[i] != null)
                    {
                        if (effects[i].targetUIElement != null)
                        {
                            UIManager.UnregisterUiEffect(effects[i]);
                        }
                    }
                }
            }
        }
        #endregion

        #region Unregister from NotificationQueue
        void UnregisterFromNotificationQueue(NotificationData nData)
        {
            if (nData.addToNotificationQueue)
            {
                UIManager.UnregisterFromNotificationQueue(nData);
            }
        }
        #endregion

        #region Get IN ANimations Time and Delay
        private float GetInAnimationsTimeAndDelay()
        {
            float moveInTimeAndDelay = 0;
            if (notificationContainer.moveIn.enabled)
            {
                moveInTimeAndDelay = notificationContainer.moveIn.time + notificationContainer.moveIn.delay;
            }

            float rotationInTimeAndDelay = 0;
            if (notificationContainer.rotationIn.enabled)
            {
                rotationInTimeAndDelay = notificationContainer.rotationIn.time + notificationContainer.rotationIn.delay;
            }

            float scaleInTimeAndDelay = 0;
            if (notificationContainer.scaleIn.enabled)
            {
                scaleInTimeAndDelay = notificationContainer.scaleIn.time + notificationContainer.scaleIn.delay;
            }

            float fadeInTimeAndDelay = 0;
            if (notificationContainer.fadeIn.enabled)
            {
                fadeInTimeAndDelay = notificationContainer.fadeIn.time + notificationContainer.fadeIn.delay;
            }

            float inAnimationsTimeAndDelay = Mathf.Max(moveInTimeAndDelay, rotationInTimeAndDelay, scaleInTimeAndDelay, fadeInTimeAndDelay);

            return inAnimationsTimeAndDelay;
        }
        #endregion

        #region Get OUT ANimations Time and Delay
        private float GetOutAnimationsTimeAndDelay()
        {
            float moveOutTimeAndDelay = 0;
            if (notificationContainer.moveOut.enabled)
            {
                moveOutTimeAndDelay = notificationContainer.moveOut.time + notificationContainer.moveOut.delay;
            }

            float rotationOutTimeAndDelay = 0;
            if (notificationContainer.rotationOut.enabled)
            {
                rotationOutTimeAndDelay = notificationContainer.rotationOut.time + notificationContainer.rotationOut.delay;
            }

            float scaleOutTimeAndDelay = 0;
            if (notificationContainer.scaleOut.enabled)
            {
                scaleOutTimeAndDelay = notificationContainer.scaleOut.time + notificationContainer.scaleOut.delay;
            }

            float fadeOutTimeAndDelay = 0;
            if (notificationContainer.fadeOut.enabled)
            {
                fadeOutTimeAndDelay = notificationContainer.fadeOut.time + notificationContainer.fadeOut.delay;
            }

            float outAnimationsTimeAndDelay = Mathf.Max(moveOutTimeAndDelay, rotationOutTimeAndDelay, scaleOutTimeAndDelay, fadeOutTimeAndDelay);

            return outAnimationsTimeAndDelay;
        }
        #endregion

        #region Back Button Event
        void BackButtonEvent()
        {
            StartCoroutine(HideNotification(0f));
            StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay()));
        }
        #endregion

        #region DestroyAfterTime
        IEnumerator DestroyAfterTime(float delay)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + delay)
            {
                yield return null;
            }
            UIManager.EnableButtonClicks();
            if (notificationData.hideCallback != null)
                notificationData.hideCallback.Invoke();
            Destory(gameObject);
        }

        private void Destory(GameObject go)
        {
            Destroy(go);
        }
        #endregion
    }
}
