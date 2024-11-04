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

    public float dayCountdown;
    public float intensityTimer;
    public bool dayActive;

    public TextMeshProUGUI TimerText;

    public GameManager gameManager;
    public HappinessTimer happinessTimer;
    public CustomerManager customerManager;


    public GameObject[] levelStartUI;
    public GameObject levelEndUI;

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
            IntensityState(true, false, false);
            
        }
        if (intensityTimer >= 61 && intensityTimer <= 120) //level 2 / midday (rush)
        {
            IntensityState(false, true, false);
            
        }
        if (intensityTimer >= 121 && intensityTimer <= 180) //level3 / end of day (intense)
        {
            IntensityState(false, false, true);
            
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
    public void IntensityState (bool startofday, bool midday, bool endofday)
    {
        startOfDay = startofday;
        midDay = midday;
        endOfDay = endofday;

        //displays day cycle clock UI anim at each point

        if (startOfDay)
        {
            
        }
        else if (midDay)
        {
            
        }
        else if (endOfDay)
        {
            
        }
    }

    //stops timers and displays the level over UI
    public void DayOver()
    {
        levelEndUI.SetActive(true);
        happinessTimer.StopHappinessTimer();
        customerManager.waveCountdown = 9999999;

    }

    public void ShowLevelUI()
    {
        if (gameManager.levelOneSelected)
        {
            levelStartUI[0].SetActive(true);
        }
        else if (gameManager.levelTwoSelected)
        {
            levelStartUI[1].SetActive(true);
        }
        else if (gameManager.levelThreeSelected)
        {
            levelStartUI[2].SetActive(true);
        }
        else if (gameManager.endlessSelected)
        {
            levelStartUI[3].SetActive(true);
        }
    }
    //hides level start quota and starts day cycle timer based on the level selected
    public void LevelStart()
    {
        customerManager.readyToCountDown = true;
        dayActive = true;

        if (gameManager.levelOneSelected)
        {
            dayCountdown = 60;
        }
        else if (gameManager.levelTwoSelected)
        {
            dayCountdown = 120;
        }
        else if (gameManager.levelThreeSelected)
        {
            dayCountdown = 180;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
