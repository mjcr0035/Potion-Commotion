using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    //matthew:
    //this script handles customer spawns and ideally orders as well in the future
    //spawns a random customer prefab from a list when spacebar is pressed, and sets their animator and sprite renderer
    //also despawns a customer when an order is delivered (see customerprefs.cs)
    
    public Animator customerAC;

    public SpriteRenderer customerSprite;

    public GameObject[] customerPrefabs;

    //ensures customer is a child of register screen for swapping to work
    //might be removed when swapping is updated to camera
    public GameObject registerParent;

    //ui vars
    //public GameObject orderUI;
    public GameObject happinessUI;
    public GameObject feedbackUI;
    public GameObject SuccessUI;
    public GameObject FailureUI;
    

    public GameObject newCustomer;

    //happiness vars
    public int customerHappiness;
    public HappinessTimer happinessTimer;

    //money vars
    public int moneyGained;
    public int moneyTotal;
    public TextMeshProUGUI moneyGainedText;
    public TextMeshProUGUI moneyTotalText;
    
    public int potionVal;

    //spawner vars
    [SerializeField] public float waveCountdown;
    private bool readyToCountDown;
    public GameObject endText;


    private void Start()
    {
        FailureUI.SetActive(false);
        SuccessUI.SetActive(false);
        moneyGainedText.enabled = false;

        readyToCountDown = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //temporary customer spawner until we figure out how we want days to work and stuff

      //if (Input.GetKeyDown(KeyCode.Space))
      //{
      //    SpawnCustomer();
      //}

        if (readyToCountDown == true)
        {
            waveCountdown -= Time.deltaTime;
        }

        if (waveCountdown <= 0)
        {
            readyToCountDown = false;
            SpawnCustomer();
        }

    }



    //matthew - day system:
    //on start, begin day timer
    //spawn waves of customers at random set points in waves, in the day
    //increment a day timer and time customer spawns to last the whole day
    //waves become more 'intense' as you go by making the timers shorter
    //^might not include for now because timers are still bugged. will have to fix happiness timer very soon
    //temporary fix:
    //spawns a few customers from customerprefabs then sends to main menu?

    public void SpawnCustomer()
    {
        //if customer prefabs are assigned, and there is no customer on screen currently, spawn customer.
        //assigns customer animator and sprite renderer
        //activates order, timer, and feedback ui
        //triggers happiness timer
        
        if (customerPrefabs != null && !newCustomer)
        {
            //put customer click to order in here to make sure timer starts after you take their order?

            //this is messy but if it works it works..........
            FailureUI.SetActive(false);
            SuccessUI.SetActive(false);

            int customerSpawnIndex = Random.Range(0, customerPrefabs.Length);
            Instantiate(customerPrefabs[customerSpawnIndex], registerParent.transform);

            newCustomer = GameObject.FindWithTag("Customer");

            customerSprite = newCustomer.GetComponent<SpriteRenderer>();
            customerAC = newCustomer.GetComponent<Animator>();
            
           
            happinessUI.SetActive(true);
            feedbackUI.SetActive(true);
            //orderUI.SetActive(true);
            

            //starts the timer at a random number every spawn
            //happinessTimer.StartHappinessTimer(Random.Range(7,10));
            happinessTimer.StartHappinessTimer(happinessTimer.duration);

            
                       
        }
        
    }
    
    public IEnumerator DespawnCustomer()
    {
        //if a customer is on screen, disables UI elements and plays despawn animation
        //then waits until animation is over and destroys the newcustomer
        

        if (newCustomer)
        {
            waveCountdown = Random.Range(4, 8);
            readyToCountDown = true;
            Debug.Log("customer despawning!!!");

            happinessTimer.StopHappinessTimer();

            customerAC.SetTrigger("Despawn");

            yield return new WaitForSeconds(2);

            Destroy(newCustomer);

            feedbackUI.SetActive(false);
            moneyGainedText.enabled = false;
        }
               
    }

    //formula for calculating score:
    //money = [potion value (0-10 for wrong potion and 90-100 for correct potion) + (10*remaining seconds on timer)] * [pot mix multiplier??]
    //this is the implementation for now, but it would be smarter to handle money awarding and correct potion checking in the same script.
    //^rn im thinking customerprefs should be adjusted to only have the actual customer preferences,
    //then a separate script that handles the potions dragging onto the customer and all that can have the rest of the stuff.
    public void OrderCorrect()
    {
        FailureUI.SetActive(false);
        SuccessUI.SetActive(true);
        //animate moneygained trigger eventually
        moneyGainedText.enabled = true;

        potionVal = Random.Range(85, 100);
                        
        moneyGained = potionVal + (10 * Mathf.RoundToInt(happinessTimer.remainingDuration));

        Debug.Log("order correct, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);
        
        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " GOLD";
        moneyTotalText.text = moneyTotal.ToString() + " G";
    }
    
    public void OrderIncorrect()
    {
        FailureUI.SetActive(true);
        SuccessUI.SetActive(false);
        //animate moneygained trigger eventually
        moneyGainedText.enabled = true;

        potionVal = Random.Range(0, 15);

        moneyGained = potionVal + (10 * Mathf.RoundToInt(happinessTimer.remainingDuration));
        
        


        Debug.Log("order incorrect, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);
        
        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " GOLD";
        moneyTotalText.text = moneyTotal.ToString() + " G";
    }

    


}
