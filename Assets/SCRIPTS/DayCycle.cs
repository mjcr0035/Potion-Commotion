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
        else if (gameManager.tutorialSelected)
        {
            levelStartUI[4].SetActive(true);
        }
        
    }
    //hides level start quota and starts day cycle timer based on the level selected
    public void LevelStart()
    {
        customerManager.readyToCountDown = true;
        

        if (gameManager.levelOneSelected)
        {
            dayActive = true;
            dayCountdown = 60;
        }
        else if (gameManager.levelTwoSelected)
        {
            dayActive = true;
            dayCountdown = 120;
        }
        else if (gameManager.levelThreeSelected)
        {
            dayActive = true;
            dayCountdown = 180;
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

    public void TutorialSequence()
    {
        //Welcome text
        //wait for click
        //Spawn customer
        //Text prompts taking the customer's order
        //Wait for order to be taken
        //Pause satisfaction timer
        //Texts notes potion order, notes satisfaction timer, prompts switching to backroom
        //wait for switch
        //Text prompts to use recipe book
        //wait for recipe book to open
        //Text prompts dragging the correct ingredients
        //wait for 2 ingredients to be in the pot
        //Text prompts swirling
        //wait for new potion to spawn
        //Text prompts dragging to the conveyor belt
        //wait for potion to collide with conveyor
        //Text prompts to switch to storefront
        //wait for switch
        //Text prompts to drag potion to customer
        //wait for customer despawn
        //Text explains satisfaction and corectness relate to score
        //Wait for click
        //"You're ready, glhf"
        //set tutorialMode false
        //set readyToCountDown true

        //ok so kinda scratch that
        //this might not be the cleanest implementation but it works
        //instead of having a centralized tutorial function I'm just going to have the functionality daisy-chained through a series of UI elements
        //since everything which activates the tutorial UI is a different part of the game, different scripts tutorial their specific parts of the game
        //I think the only other things I need are screenswap and pot2d
    }
}
