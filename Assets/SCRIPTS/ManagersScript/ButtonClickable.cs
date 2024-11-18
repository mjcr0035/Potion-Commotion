using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button tutorialButton;

    private void Start()
    {
        // Load the button states from PlayerPrefs
        bool buttonsEnabled = PlayerPrefs.GetInt("ButtonsEnabled", 0) == 1;
        button1.interactable = buttonsEnabled;
        button2.interactable = buttonsEnabled;

        // Add a listener to the tutorial button to enable other buttons when clicked
        tutorialButton.onClick.AddListener(EnableOtherButtons);

        
    }

    private void EnableOtherButtons()
    {
        button1.interactable = true;
        button2.interactable = true;

        // Save the state so that it persists across scenes
        PlayerPrefs.SetInt("ButtonsEnabled", 1);
        PlayerPrefs.Save();
    }
}

