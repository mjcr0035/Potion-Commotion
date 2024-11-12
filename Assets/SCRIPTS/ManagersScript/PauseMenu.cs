using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;

    public GameObject PauseMenuUI;
    public GameObject PauseBtn;
    public GameObject SettingsUI;
    public Slider soundSlider;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
    }
    
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        PauseBtn.SetActive(true);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        PauseBtn.SetActive(false);
        Time.timeScale = 0;
        GamePaused = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Settings()
    {
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }
    
    public void BackToPause()
    {
        SettingsUI.SetActive(false);
        PauseMenuUI.SetActive(true);
    }

    public void VolumeSlider()
    {
        AudioManager.Instance.currentMusicTrack.volume = (soundSlider.value);
    }
}

