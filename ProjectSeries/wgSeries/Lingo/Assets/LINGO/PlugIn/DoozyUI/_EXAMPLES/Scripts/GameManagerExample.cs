using UnityEngine;
using System.Collections;
using DG.Tweening;
using DoozyUI;

public class GameManagerExample : MonoBehaviour
{

    // You will find below code examples of how to interact with the UI

    #region Public Variables
    public ParticleSystem shield;          //We use this to demonstrate the Pause/Unpause Game feature. We activate it when we enter play mode (when we show the InGameHud)
    public ParticleSystem sparklyFade;     //This is also used to better see the Pause/Unpause Game feature, but you can start or stop it by pressing the DoozyUI logo (on the bottom of the MainMenu screen)

    [Space(10)]
    public string menuMusic;                //the sound filename we want to use as menu music
    [Range(0, 1)]
    public float musicVolume = 0.5f;        //the preset music volume

    [Space(20)]
    public Sprite[] icons;
    public string[] iconNames;
    #endregion

    #region Private Variables
    private AudioSource menuMusicAudioSource; //the audiosource that plays the music
    private WaitForSeconds checkMusicInterval = new WaitForSeconds(0.5f); //listener update time
    private int randomIconIndex = 0;
    #endregion

    void Start()
    {
        InitMusic();
    }

    //For this method to work we have an UITrigger attached to this gameObject and it is set to listen for All Game Events.
    //Also the method selected in the trigger is from the 'Dynamic string' section and NOT from the 'Static parameter' list. (this is important)
    public void OnGameEvent(string gameEvent)
    {
        switch (gameEvent)
        {
            case "Notification_DoozyUI": //shows the DoozyUI notification
                UIManager.ShowNotification("Notification_DoozyUI", 5f, false);
                break;

            case "UpdateSoundSettings":
                UpdateMusicState();
                break;


            #region EXAMPLE 1 - notifications

            case "Example_1_Notification_1":
                UIManager.ShowNotification("Example_1_Notification_1", 5f, false, "This is a 5 seconds notification! \n - click on it to close -");
                break;

            case "Example_1_Notification_2":
                UIManager.ShowNotification("Example_1_Notification_2", -1f, false, "About", "DoozyUI is a sure way to create blazing fast user interfaces with all kinds of animations that you can imagine.", null, new string[1] { "CloseNotificationButton" }, new string[1] { "Got it!" });
                break;

            case "Example_1_Notification_3":
                UIManager.ShowNotification("Example_1_Notification_3", -1f, false, new string[2] { "OkButton", "CancelButton" }, new string[2] { "Ok", "Cancel" });
                break;

            case "Example_1_Notification_4":
                randomIconIndex = Random.Range(0, icons.Length); //we get a random icon index so that we can show/simulate a different notification each time
                UIManager.ShowNotification("Example_1_Notification_4", 3f, true, iconNames[randomIconIndex], icons[randomIconIndex]);
                break;

            case "Example_1_Notification_5":
                UIManager.ShowNotification("Example_1_Notification_5", 4f, true, icons[Random.Range(0, icons.Length)]); //we select a random sprite to show as the main icon
                break;

            case "Example_1_Notification_6":
                UIManager.ShowNotification("Example_1_Notification_6", -1f, true, "Click anywhere to close this notification!");
                break;

            #endregion

            #region EXAMPLE 2 - notifications

            case "Example_2_Notification_1":
                UIManager.ShowNotification("Example_2_Notification_1", 5f, false, "This is a 5 seconds notification! \n - click on it to close -");
                break;

            case "Example_2_Notification_2":
                UIManager.ShowNotification("Example_2_Notification_2", -1f, false, "One Button Notification", "Press the OK button to close this notification!", null, new string[1] { "CloseNotificationButton" }, new string[1] { "OK" });
                break;

            case "Example_2_Notification_3":
                UIManager.ShowNotification("Example_2_Notification_3", -1f, false, "Level Completed", new string[2] { "NextLevelButton", "ReplayButton" }, new string[2] { "Next Level", "Replay" });
                break;

            case "Example_2_Notification_4":
                UIManager.ShowNotification("Example_2_Notification_4", 3f, true, icons[Random.Range(0, icons.Length)]);
                break;

            case "Example_2_Notification_5":
                randomIconIndex = Random.Range(0, icons.Length); //we get a random icon index so that we can show/simulate a different notification each time
                UIManager.ShowNotification("Example_2_Notification_5", 4f, true, iconNames[randomIconIndex], icons[randomIconIndex]);
                break;

            case "Example_2_Notification_6":
                UIManager.ShowNotification("Example_2_Notification_6", -1f, true, "About", "DoozyUI is a sure way to create blazing fast user interfaces with all kinds of animations that you can imagine.", null, new string[1] { "CloseNotificationButton" }, new string[1] { "OK" });
                break;

            #endregion

            #region EXAMPLE 3 - notifications

            case "Example_3_Notification_1":
                UIManager.ShowNotification("Example_3_Notification_1", 5f, false, "Quick notification! \n - click to close -");
                break;

            case "Example_3_Notification_2":
                UIManager.ShowNotification("Example_3_Notification_2", -1f, false, "Three Button Notification", "This notification has three buttons \n click any of them to close", null, new string[3] { "Button1", "Button2", "Button3" }, new string[3] { "one", "two", "three" });
                break;

            case "Example_3_Notification_3":
                UIManager.ShowNotification("Example_3_Notification_3", -1f, false, new string[2] { "OkButton", "CancelButton" }, new string[2] { "Ok", "Cancel" });
                break;

            case "Example_3_Notification_4":
                randomIconIndex = Random.Range(0, icons.Length); //we get a random icon index so that we can show/simulate a different notification each time
                UIManager.ShowNotification("Example_3_Notification_4", 3f, true, iconNames[randomIconIndex], icons[randomIconIndex]);
                break;

            case "Example_3_Notification_5":
                randomIconIndex = Random.Range(0, icons.Length); //we get a random icon index so that we can show/simulate a different notification each time
                UIManager.ShowNotification("Example_3_Notification_5", 3f, true, icons[randomIconIndex]);
                break;

            case "Example_3_Notification_6":
                UIManager.ShowNotification("Example_3_Notification_6", -1f, true, "About", "DoozyUI is a sure way to create blazing fast user interfaces with all kinds of animations that you can imagine.", null, new string[1] { "CloseNotificationButton" }, new string[1] { "OK" });
                break;

            #endregion

            #region EXAMPLE 4 - notifications

            case "Example_4_Notification_1":
                UIManager.ShowNotification("Example_4_Notification_1", 5f, false, "", "Quick notification \n 5 seconds lifetime");
                break;

            case "Example_4_Notification_2":
                UIManager.ShowNotification("Example_4_Notification_2", 4f, false, "", "Your message here! \n click to hide");
                break;

            case "Example_4_Notification_3":
                UIManager.ShowNotification("Example_4_Notification_3", -1, false, "Modal window", "To close this window, click any button. \n This notification has 3 buttons available, but we chose to use only 2.", null, new string[2] { "button 1", "button 2" }, new string[2] { "1", "2" });
                break;

            case "Example_4_Notification_4":
                UIManager.ShowNotification("Example_4_Notification_4", 2f, true);
                break;

            case "Example_4_Notification_5":
                UIManager.ShowNotification("Example_4_Notification_5", 3f, true);
                break;

            case "Example_4_Notification_6":
                UIManager.ShowNotification("Example_4_Notification_6", 5f, true, "Doozy UI", "COMPLETE UI MANAGEMENT SYSTEM");
                break;

                #endregion
        }
    }

    //For this method to work we have an UITrigger attached to this gameObject and it is set to listen for All Button Clicks.
    //Also the method selected in the trigger is from the 'Dynamic string' section and NOT from the 'Static parameter' section. (this is important)
    public void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "GoToInGameHud":
                if (shield != null)
                    shield.Play(true);
                break;

            case "GoToMainMenu":
                if (shield != null)
                    shield.Stop(true);
                break;

            case "ToggleMusic":
                UpdateMusicState();
                break;
        }
    }

    #region Music methods - InitMusic, SetupMusic | IEnumerators - CheckMusicState
    /// <summary>
    /// Initializes the music settings.
    /// </summary>
    private void InitMusic()
    {
        menuMusicAudioSource = SetupMusic(menuMusic); //we check if the menuMusic filename exists in a Resources folder; if it does we create a new gameObject with an AudioSource attached and we return the reference to it

        if (menuMusicAudioSource != null)
        {
            menuMusicAudioSource.volume = musicVolume; //we set the volume to the value set in the inspector
            menuMusicAudioSource.mute = UIManager.isMusicOn;  //we check if the music is on or off
            menuMusicAudioSource.Play(); //we start the music (even if the volume is 0)
            StartCoroutine(CheckMusicState()); //we activate a listerer for the music on/off toggle; it will check the music state every 0.5 seconds (more efficeint than in the Update method)
        }
    }

    /// <summary>
    /// If it finds the song with the specified soundName in a Resources folder it will create, configure and return an AudioSource.
    /// </summary>
    /// <param name="soundName">The filename of a sound file in a Resources folder.</param>
    /// <returns></returns>
    private AudioSource SetupMusic(string soundName)
    {
        if (System.String.IsNullOrEmpty(soundName) == false)
        {
            AudioClip clip = Resources.Load(soundName) as AudioClip;
            if (clip == null)
            {
                Debug.Log("[DoozyUI] There is no file with the name [" + soundName + "] in any of the Resources folders.");
            }
            else
            {
                var tempGO = new GameObject("Music - " + clip.name); // create the temp object
                tempGO.transform.position = Vector3.zero; // set its position
                var aSource = tempGO.AddComponent<AudioSource>(); ; // add an audio source
                aSource.mute = !UIManager.isMusicOn;  //we check if the music is on or off
                aSource.clip = clip; // define the clip
                aSource.loop = true;
                return aSource; // return the AudioSource reference
            }
        }
        return null;
    }

    /// <summary>
    /// Checks if the music is turned on or off at specified intervals.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckMusicState()
    {
        while (true)
        {
            yield return checkMusicInterval;
            UpdateMusicState();
        }
    }

    private void UpdateMusicState()
    {
        if (menuMusicAudioSource == null)
            return;

        if (UIManager.isMusicOn == menuMusicAudioSource.mute)
        {
            menuMusicAudioSource.mute = !UIManager.isMusicOn;
        }
    }
    #endregion

}
