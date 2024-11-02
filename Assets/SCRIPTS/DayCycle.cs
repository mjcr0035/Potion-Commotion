using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayCycle : MonoBehaviour
{
    //matthew - this script handles the passage of time in both endless in story. bools are checked to raise intensity
    //the longer the day goes. when the day is 'over' the game returns to the main menu screen
    
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


    public GameObject levelStartUI;
    public GameObject levelEndUI;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dayActive = false;
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


      if (intensityTimer < 60) //start of day (calm)
      {
          startOfDay = true;
          
          //update UI
      
      } else if (intensityTimer < 120) //midday (rush)
      {
          startOfDay = false;
          midDay = true;
          
          //update UI
      }
      else if (intensityTimer < 180) //end of day (intense)
      {
          startOfDay = false;
          midDay = false;
          endOfDay = true;
          
          //update UI
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

    //stops timers and displays the level over UI
    public void DayOver()
    {
        levelEndUI.SetActive(true);
        happinessTimer.StopHappinessTimer();
        customerManager.waveCountdown = 9999999;

    }

    //hides level start quota and starts day cycle timer based on the level selected
    public void LevelStart()
    {
        levelStartUI.SetActive(false);
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
