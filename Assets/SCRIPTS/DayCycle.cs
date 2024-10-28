using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    //matthew - this script handles the passage of time in both endless in story. bools are checked to raise intensity
    //the longer the day goes. when the day is 'over' the game returns to the main menu screen
    
    public bool startDay;
    public bool midDay;
    public bool endofDay;

    public int dayCountdown;
    private bool dayGo;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dayCountdown = 30;

    }

    // Update is called once per frame
    void Update()
    {
        DayCycleGo();

        if (dayCountdown <= 10)
        {
            startDay = false;
            midDay = false;
            endofDay = true;
            //update UI

        } else if (dayCountdown <= 20)
        {
            startDay = false;
            midDay = true;
            endofDay = false;
            //update UI

        }
        else if (dayCountdown <= 30)
        {
            startDay = true;
            midDay = false;
            endofDay = false;
            //update UI

        }
    }

    public IEnumerator DayCycleGo()
    {
        while (dayGo)
        {
            if (gameManager.levelOneSelected)
            {
                dayCountdown = 10;
                dayCountdown--;
                yield return new WaitForSeconds(1f);
                yield return null;
            }
            else if (gameManager.levelTwoSelected)
            {
                dayCountdown = 20;
                dayCountdown--;
                yield return new WaitForSeconds(1f);
                yield return null;
            }
            else if (gameManager.levelThreeSelected)
            {
                dayCountdown = 30;
                dayCountdown--;
                yield return new WaitForSeconds(1f);
                yield return null;
            }
            yield return null;
        }


    }
}
