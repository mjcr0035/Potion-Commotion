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

    public GameManager gameManager;
    public DayCycle dayCycle;

    //money vars
    public int moneyGained;
    public int moneyTotal;
    public TextMeshProUGUI moneyGainedText;
    public TextMeshProUGUI moneyTotalText;
    
    public int potionVal;

    //spawner vars
    [SerializeField] public float waveCountdown;
    public bool readyToCountDown;
    public GameObject endText;

    
    //alters behavior if in tutorial mode
    //public bool tutorialMode;


    private void Start()
    {
        FailureUI.SetActive(false);
        SuccessUI.SetActive(false);
        moneyGainedText.enabled = false;

        //TutorialSequence();
        
    }

    // Update is called once per frame
    void Update()
    {
       
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
    //

    public void SpawnCustomer()
    {
        //if customer prefabs are assigned, and there is no customer on screen currently, spawn customer.
        //assigns customer animator and sprite renderer
        //activates order, timer, and feedback ui
        //triggers happiness timer
        
        if (customerPrefabs != null && !newCustomer)
        {
            //put customer click to order in here to make sure timer starts after you take their order?

            
            FailureUI.SetActive(false);
            SuccessUI.SetActive(false);

            int customerSpawnIndex = Random.Range(0, customerPrefabs.Length);
            Instantiate(customerPrefabs[customerSpawnIndex], registerParent.transform);

            

            newCustomer = GameObject.FindWithTag("Customer");

            customerSprite = newCustomer.GetComponent<SpriteRenderer>();
            customerAC = newCustomer.GetComponent<Animator>();
            
           
            happinessUI.SetActive(true);
            feedbackUI.SetActive(true);

            //starts the timer at a random number every spawn, patience depends on time of day
            if (dayCycle.startOfDay)
            {
                happinessTimer.StartHappinessTimer(Random.Range(20, 25));
            }
            else if (dayCycle.midDay)
            {
                happinessTimer.StartHappinessTimer(Random.Range(15, 20));

            }
            else if (dayCycle.endOfDay)
            {
                happinessTimer.StartHappinessTimer(Random.Range(10, 15));
            }

            //happinessTimer.StartHappinessTimer(happinessTimer.duration);



        }
        
    }
    
    public IEnumerator DespawnCustomer()
    {
        //if a customer is on screen, disables UI elements and plays despawn animation
        //then waits until animation is over and destroys the newcustomer
        

        if (newCustomer)
        {
            happinessTimer.StopHappinessTimer();

            if (dayCycle.startOfDay)
            {
                waveCountdown = Random.Range(6, 10);
            }
            else if (dayCycle.midDay)
            {
                waveCountdown = Random.Range(4, 8);
            }
            else if (dayCycle.endOfDay)
            {
                waveCountdown = Random.Range(2, 4);
            }

            readyToCountDown = true;

            Debug.Log("customer despawning!!!");
            
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

        potionVal = Random.Range(60, 100);
                        
        moneyGained = potionVal + (10 * Mathf.RoundToInt(happinessTimer.remainingDuration));

        Debug.Log("order correct, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);
        
        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " SOL";
        moneyTotalText.text = moneyTotal.ToString() + " S";
    }
    
    public void OrderIncorrect()
    {
        FailureUI.SetActive(true);
        SuccessUI.SetActive(false);
        //animate moneygained trigger eventually
        moneyGainedText.enabled = true;

        potionVal = Random.Range(0, 15);

        moneyGained = potionVal + (2 * Mathf.RoundToInt(happinessTimer.remainingDuration));

        Debug.Log("order incorrect, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);
        
        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " SOL";
        moneyTotalText.text = moneyTotal.ToString() + " S";
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

    }


}
