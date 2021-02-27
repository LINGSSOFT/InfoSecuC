using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DoozyUI;

public class DemoNotificationText : MonoBehaviour
{
    public Text title;
    public Text message;

    int randomSelection = 0;

    void Start()
    {
        UpdateNotificationText();
    }

    void OnEnable()
    {
        UpdateNotificationText();
    }

    private string[] notificationTitles = new string[]
    {
        "Responsive UI",
        "Custom Animation Presets",
        "No Code Needed",
        "Fun",
        "Easy",
        "Total Control"
    };

    private string[] notificationMessages = new string[]
    {
        "UIManager calculates all the resolution and aspect ratio changes and adjusts the animations accordingly. Because of this all the animations look and feel the same in both Landscape and Portrait Modes.",
        "Save your animations and load them in other projects. (simple .xml files)",
        "Autonomus UI Navigation System that handles: Android Back Button, Game Pause/Unpause, Sounds ON/OFF and Application.Quit. Powerful & flexible event-driven script for no-code control of UI Elements and UI Buttons.",
        "Create all kinds of animations in Play Mode and then save them as presets.",
        "Create intuitive menus without writing a single line of code with a WYSIWYG approach.",
        "Fine tune all the animations settings from timings, to delays, to ease functions."
    };

    public void UpdateNotificationText()
    {
        randomSelection = Random.Range(0, notificationTitles.Length);
        title.text = notificationTitles[randomSelection];
        message.text = notificationMessages[randomSelection];
    }

}
