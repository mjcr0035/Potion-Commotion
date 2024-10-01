using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HappinessTimer : MonoBehaviour
{
    //matthew
    //this script has two methods that start and end a happiness timer of randomized duration
    //and manipulates a UI element that ticks down based on the duration
    //based off of implementation by @Game Dev Box on YT

    [SerializeField] private Image happinessBarFill;

    public GameObject happinessTimerUI;

    public CustomerManager customerManager;

    public float duration;

    private float remainingDuration;

    public bool timerActive = false;

    
    public void StartHappinessTimer(float duration)
    {
        //starts the happiness timer, then activates the happiness ui element and starts the update coroutine
        timerActive = true;
        
        happinessTimerUI.SetActive(true);

        remainingDuration = duration;

        StartCoroutine(UpdateHappinessBar());

    }

    private IEnumerator UpdateHappinessBar()
    {
        //counts down from inputted duration, and updates the ui correspondingly
        while (remainingDuration >= 0)
        {
            happinessBarFill.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);

            remainingDuration--;

            yield return new WaitForSeconds(1f);
            
            yield return null;
        }

        StopHappinessTimer();
    }



    private void StopHappinessTimer()
    {
        //stops the timer, despawns the customer, and hides the UI
        Debug.Log("timer ended");

        StartCoroutine(customerManager.DespawnCustomer());

        timerActive = false;

        happinessTimerUI.SetActive(false);
    }
}
