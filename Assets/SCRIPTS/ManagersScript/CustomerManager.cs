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

    //happiness vars
    public int customerHappiness;
    public HappinessTimer happinessTimer;

    public GameManager gameManager;
    public DayCycle dayCycle;

    //audio
    public AudioClip customerEnterSound;
    public AudioClip solGainedSound;
    public AudioClip orderCorrectSound;
    public AudioClip orderIncorrectSound;

    //money vars
    public int moneyGained;
    public int moneyTotal;
    public TextMeshProUGUI moneyGainedText;
    public TextMeshProUGUI moneyTotalText;
    public TextMeshProUGUI moneyFinalText;
    public TextMeshProUGUI moneyEndlessText;

    public int potionVal;

    //spawner vars
    [SerializeField] public float waveCountdown;
    public bool readyToCountDown;
    public GameObject newCustomer;

    public GameObject endText;

    public GameObject TUTORIAL4;


    private void Start()
    {
        FailureUI.SetActive(false);
        SuccessUI.SetActive(false);
        moneyGainedText.enabled = false;

        gameManager = FindObjectOfType<GameManager>();
        
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

            AudioManager.Instance.PlaySoundFXClip(customerEnterSound, transform, 0.6f, 1f, "CustomerEnterSFX");

            newCustomer = GameObject.FindWithTag("Customer");

            customerSprite = newCustomer.GetComponent<SpriteRenderer>();
            customerAC = newCustomer.GetComponent<Animator>();
            
           
            happinessUI.SetActive(true);
            feedbackUI.SetActive(true);

            //starts the timer at a random number every spawn, patience depends on time of day
            if (dayCycle.startOfDay && !gameManager.tutorialSelected)
            {
                happinessTimer.StartHappinessTimer(Random.Range(20, 25));
            }
            else if (dayCycle.midDay && !gameManager.tutorialSelected)
            {
                happinessTimer.StartHappinessTimer(Random.Range(15, 20));

            }
            else if (dayCycle.endOfDay && !gameManager.tutorialSelected)
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

            if (gameManager.tutorialSelected)
            {
                TUTORIAL4.gameObject.SetActive(true);
            }
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

        //ordercorrect sound here
        AudioManager.Instance.PlaySoundFXClip(orderCorrectSound, transform, 0.5f, Random.Range(0.9f, 1.3f), "OrderCorrectSFX");
        AudioManager.Instance.PlaySoundFXClip(solGainedSound, transform, 0.7f, Random.Range(0.9f, 1.3f), "SolGainedSFX");

        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " SOL";
        moneyTotalText.text = moneyTotal.ToString() + " S";
        moneyFinalText.text = moneyTotal.ToString();
        moneyEndlessText.text = moneyTotal.ToString();
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

        //orderincorrect sound here
        AudioManager.Instance.PlaySoundFXClip(orderIncorrectSound, transform, 0.5f, Random.Range(0.9f, 1.3f), "OrderInorrectSFX");
        AudioManager.Instance.PlaySoundFXClip(solGainedSound, transform, 0.7f, Random.Range(0.9f, 1.3f), "SolGainedSFX");

        moneyTotal += moneyGained;

        moneyGainedText.text = "+ " + moneyGained.ToString() + " SOL";
        moneyTotalText.text = moneyTotal.ToString() + " S";
        moneyFinalText.text = moneyTotal.ToString();
        moneyEndlessText.text = moneyTotal.ToString();

        if (gameManager.endlessSelected)
        {
            dayCycle.LoseHP();
        }
    }


}
