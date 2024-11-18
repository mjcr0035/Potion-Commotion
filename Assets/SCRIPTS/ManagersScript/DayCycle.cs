using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayCycle : MonoBehaviour
{
    //matthew - this script handles the passage of time in both endless in story. bools are checked to raise intensity
    //the longer the day goes. when the day is 'over' the game displays the end UI and returns to the main menu screen
    
    public bool startOfDay = false;
    public bool midDay = false;
    public bool endOfDay = false;

    private bool startOfDayFlag;
    private bool midDayFlag;
    private bool endOfDayFlag;

    public float initialDayCountdown;
    public float dayCountdown;
    public float intensityTimer;
    public bool dayActive;
    
    public int EndlessHP = 3;
    public GameObject DayTimerUIElement;
    public GameObject HPParent;
    public GameObject HP1;
    public GameObject HP2;
    public GameObject HP3;

    public TextMeshProUGUI TimerText;

    public GameManager gameManager;
    public HappinessTimer happinessTimer;
    public CustomerManager customerManager;

    public Animator intensitySwitchAC;
    public TextMeshProUGUI intensitySwitchText;
    public TextMeshProUGUI intensitySwitchTextBG;
    public Animator clockHandAC;

    public GameObject clockUI;
    public GameObject levelStartUI;
    public GameObject[] otherStartUI;
    public GameObject levelEndUI;
    public GameObject levelEndUIEndless;

    public AudioClip quotaFailedSound;
    public AudioClip quotaMetSound;
    public AudioClip quotaExceededSound;
    public AudioClip intensitySwitch;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dayActive = false;

        ShowLevelUI();
    }

    // Update is called once per frame
    void Update()
    {

        if (dayActive)
        {
            if (dayCountdown > 0)
            {
                dayCountdown -= Time.deltaTime;
                intensityTimer += Time.deltaTime;
                DayTimerUpdate(dayCountdown);
            }
            else
            {
                dayCountdown = 0;
                dayActive = false;
                DayOver();
            }

        }
        
        //checks if intensity timer reaches ranges between the 1st,2nd, and 3rd 'thirds' of the day.

        if (dayActive)
        {
            if (intensityTimer >= 0 && intensityTimer < (initialDayCountdown / 3)) //level 1 / start of day (calm)
            {
                IntensityUpdate(true, false, false);

            }
            if (intensityTimer >= (initialDayCountdown / 3) && intensityTimer < (2 * initialDayCountdown / 3)) //level 2 / midday (rush)
            {
                IntensityUpdate(false, true, false);

            }
            if (intensityTimer >= (2 * initialDayCountdown / 3) && intensityTimer <= initialDayCountdown) //level3 / end of day (intense)
            {
                IntensityUpdate(false, false, true);

            }
        }
       

    }

    //updates countdown timer and ui 
    public void DayTimerUpdate(float currentDayTime)
    {
        currentDayTime += 1;

        float minutes = Mathf.FloorToInt(currentDayTime / 60);
        float seconds = Mathf.FloorToInt(currentDayTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        //clockHand.rotation = Quaternion.Euler(0, 0, (180 - currentDayTime));
        //Debug.Log(currentDayTime);
    }

    //updates intensity day variables and displays respective ui
    public void IntensityUpdate (bool startofday, bool midday, bool endofday)
    {
        startOfDay = startofday;
        midDay = midday;
        endOfDay = endofday;

        //fades in audio track at key point in the day, displays intensity change UI, plays intensity switch cue

        if (startOfDay && !startOfDayFlag)
        {
            Debug.Log("its the start of the day!");
            
            startOfDayFlag = true;
        }
        else if (midDay && !midDayFlag)
        {
            Debug.Log("its midday!");

            AudioManager.Instance.PlaySoundFXClip(intensitySwitch, transform, 0.4f, 1f, "intensitySFX");
            StartCoroutine(AudioManager.Instance.FadeTrack(1));
            
            intensitySwitchText.text = "Midday";
            intensitySwitchTextBG.text = "Midday";
            intensitySwitchAC.SetTrigger("Show");
            
            midDayFlag = true;
        }
        else if (endOfDay && !endOfDayFlag)
        {
            Debug.Log("its the end of the day!");

            AudioManager.Instance.PlaySoundFXClip(intensitySwitch, transform, 0.4f, 1f, "intensitySFX");
            StartCoroutine(AudioManager.Instance.FadeTrack(2));
            
            intensitySwitchText.text = "End of Day";
            intensitySwitchTextBG.text = "End of Day";
            intensitySwitchAC.SetTrigger("Show");
            
            endOfDayFlag = true;
        }
    }

    //stops timers and displays the level over UI
    public void DayOver()
    {
        levelEndUI.SetActive(true);

        AudioManager.Instance.currentMusicTrack.volume = 0.05f;

        if (customerManager.quotaFailed)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaFailedSound, transform, 0.4f, 1f, "QuotaSFX");
        }
        else if (customerManager.quotaMet)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaMetSound, transform, 0.4f, 1f, "QuotaSFX");
        }
        else if (customerManager.quotaExceeded)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaExceededSound, transform, 0.4f, 1f, "QuotaSFX");
        }

        happinessTimer.StopHappinessTimer();
        customerManager.waveCountdown = 9999999;

    }

    public void ShowLevelUI()
    {
        if (gameManager.levelOneSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 500;
            customerManager.dayText.text = "Day One";
            customerManager.loreText.text = "It's the first day of your brand new shop! You make sure all your ingredients are in stock and prepare yourself for customers to come in!";
        }
        else if (gameManager.levelTwoSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 1000;
            customerManager.dayText.text = "Day Two";
            customerManager.loreText.text = "On the next day, you notice a line forming outside your shop! You heat up your cauldron and empty your Sol collector for a wave of new customers.";
        }
        else if (gameManager.levelThreeSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 1500;
            customerManager.dayText.text = "Day Three";
            customerManager.loreText.text = "It's the last day of your potion shop's grand opening! Word has gotten around and your shift is longer! You get ready for a hectic day!";
        }
        else if (gameManager.endlessSelected)
        {
            otherStartUI[0].SetActive(true);
            DayTimerUIElement.SetActive(false);
            HPParent.SetActive(true);
            clockUI.SetActive(false);
        }
        else if (gameManager.tutorialSelected)
        {
            otherStartUI[1].SetActive(true);
        }
        
    }
    //hides level start quota and starts day cycle timer based on the level selected
    public void LevelStart()
    {
        customerManager.readyToCountDown = true;
        

        if (gameManager.levelOneSelected)
        {
            dayActive = true;
            initialDayCountdown = 120;
            dayCountdown = initialDayCountdown;
        }
        else if (gameManager.levelTwoSelected)
        {
            dayActive = true;
            initialDayCountdown = 180;
            dayCountdown = initialDayCountdown;

        }
        else if (gameManager.levelThreeSelected)
        {
            dayActive = true;
            initialDayCountdown = 240;
            dayCountdown = initialDayCountdown;
        }
        else if (gameManager.endlessSelected)
        {
            dayActive = true;
            
        }

        intensitySwitchText.text = "Start of Day";
        intensitySwitchTextBG.text = "Start of Day";
        intensitySwitchAC.SetTrigger("Show");

        AudioManager.Instance.PlaySoundFXClip(intensitySwitch, transform, 0.3f, 1f, "intensitySFX");

        clockHandAC.SetTrigger("Start");
        clockHandAC.speed = (60 / dayCountdown);

    }

    public void BackToMenu()
    {
        gameManager.levelOneSelected = false;
        gameManager.levelTwoSelected = false;
        gameManager.levelThreeSelected = false;
        gameManager.endlessSelected = false;
        gameManager.tutorialSelected = false;

        SceneManager.LoadScene(0);
    }

    public void LoseHP()
    {
        if (gameManager.endlessSelected)
        {
            EndlessHP--;

            if (EndlessHP == 2)
            {
                HP3.SetActive(false);
            }
            if (EndlessHP == 1)
            {
                HP2.SetActive(false);
            }
            if (EndlessHP == 0)
            {
                HP1.SetActive(false);
                levelEndUIEndless.SetActive(true);
                dayActive = false;
            }
        }
    }
}
