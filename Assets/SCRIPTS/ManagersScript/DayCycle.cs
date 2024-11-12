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


    public GameObject levelStartUI;
    public GameObject[] otherStartUI;
    public GameObject levelEndUI;
    public GameObject levelEndUIEndless;

    public AudioClip quotaFailedSound;
    public AudioClip quotaMetSound;
    public AudioClip quotaExceededSound;

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

        if (intensityTimer >= 0 && intensityTimer <= 60) //level 1 / start of day (calm)
        {
            IntensityUpdate(true, false, false);
            
        }
        if (intensityTimer >= 61 && intensityTimer <= 120) //level 2 / midday (rush)
        {
            IntensityUpdate(false, true, false);
            
        }
        if (intensityTimer >= 121 && intensityTimer <= 180) //level3 / end of day (intense)
        {
            IntensityUpdate(false, false, true);
            
        }

    }

    //updates countdown timer and ui 
    public void DayTimerUpdate(float currentDayTime)
    {
        currentDayTime += 1;

        float minutes = Mathf.FloorToInt(currentDayTime / 60);
        float seconds = Mathf.FloorToInt(currentDayTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }

    //updates intensity day variables and displays respective ui
    public void IntensityUpdate (bool startofday, bool midday, bool endofday)
    {
        startOfDay = startofday;
        midDay = midday;
        endOfDay = endofday;

        //fades in audio track at key point in the day and displays intensity change UI

        if (startOfDay && !startOfDayFlag)
        {
            Debug.Log("its the start of the day!");
            startOfDayFlag = true;
        }
        else if (midDay && !midDayFlag)
        {
            StartCoroutine(AudioManager.Instance.FadeTrack(1));
            midDayFlag = true;
        }
        else if (endOfDay && !endOfDayFlag)
        {
            StartCoroutine(AudioManager.Instance.FadeTrack(2));
            endOfDayFlag = true;
        }
    }

    //stops timers and displays the level over UI
    public void DayOver()
    {
        levelEndUI.SetActive(true);

        AudioManager.Instance.currentMusicTrack.volume = 0.1f;

        if (customerManager.quotaFailed)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaFailedSound, transform, 0.6f, 1f, "QuotaSFX");
        }
        else if (customerManager.quotaMet)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaMetSound, transform, 0.6f, 1f, "QuotaSFX");
        }
        else if (customerManager.quotaExceeded)
        {
            AudioManager.Instance.PlaySoundFXClip(quotaExceededSound, transform, 0.6f, 1f, "QuotaSFX");
        }

        happinessTimer.StopHappinessTimer();
        customerManager.waveCountdown = 9999999;

    }

    public void ShowLevelUI()
    {
        if (gameManager.levelOneSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 250;
            customerManager.loreText.text = "On the first day...";
        }
        else if (gameManager.levelTwoSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 750;
            customerManager.dayText.text = "Day Two";
            customerManager.loreText.text = "On the second day...";
        }
        else if (gameManager.levelThreeSelected)
        {
            levelStartUI.SetActive(true);
            customerManager.solQuota = 1250;
            customerManager.dayText.text = "Day Three";
            customerManager.loreText.text = "On the third day...";
        }
        else if (gameManager.endlessSelected)
        {
            otherStartUI[0].SetActive(true);
            DayTimerUIElement.SetActive(false);
            HPParent.SetActive(true);
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
            dayCountdown = 90;
        }
        else if (gameManager.levelTwoSelected)
        {
            dayActive = true;
            dayCountdown = 150;
        }
        else if (gameManager.levelThreeSelected)
        {
            dayActive = true;
            dayCountdown = 210;
        }
        
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
        if(gameManager.endlessSelected)
        {
            EndlessHP--;

            if(EndlessHP==2)
            {
                HP3.SetActive(false);
            }
            if(EndlessHP==1)
            {
                HP2.SetActive(false);
            }
            if(EndlessHP==0)
            {
                HP1.SetActive(false);
                levelEndUIEndless.SetActive(true);
                customerManager.readyToCountDown=false;
            }
        }
    }
}
