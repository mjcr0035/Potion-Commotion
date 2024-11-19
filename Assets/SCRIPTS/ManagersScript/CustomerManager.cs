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
    public GameManager gameManager;
    public DayCycle dayCycle;
    public GameObject TUTORIAL4;

    [Header("CUSTOMER REFS")]
    public GameObject[] customerPrefabs;
    public Animator customerAC;
    public SpriteRenderer customerSprite;
    public string customerName;
    
    // to ensure customer is a child of register screen
    public GameObject registerParent;

    //ui vars
    [Header("UI VARS")]
    public GameObject happinessUI;
    public GameObject feedbackUI;
    public GameObject SuccessUI;
    public GameObject FailureUI;

    //happiness vars
    [Header("HAPPINESS VARS")]
    public int customerHappiness;
    public HappinessTimer happinessTimer;

    //audio
    [Header("AUDIO CLIPS")]
    public AudioClip customerEnterSound;
    public AudioClip solGainedSound;
    public AudioClip orderCorrectSound;
    public AudioClip orderIncorrectSound;
    public AudioClip wolfHappy;
    public AudioClip wolfMad;
    public AudioClip dragonHappy;
    public AudioClip dragonMad;
    public AudioClip snakeHappy;
    public AudioClip snakeMad;
    public AudioClip pinHappy;
    public AudioClip pinMad;

    //money vars
    [Header("MONEY VARS")]
    public int moneyGained;
    public int moneyTotal;
    public int solQuota;
    public int potionVal;
    public bool quotaFailed = false;
    public bool quotaMet = false;
    public bool quotaExceeded = false;

    //money and UI text vars
    [Header("TEXT VARS")]

    public TextMeshProUGUI moneyGainedText;
    public TextMeshProUGUI moneyTotalText;
    public TextMeshProUGUI solQuotaText;
    public TextMeshProUGUI solQuotaMetText;
    public TextMeshProUGUI finalMoneyText;
    public TextMeshProUGUI endlessFinalMoneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI loreText;

    //spawner vars
    [Header("SPAWN VARS")]
    [SerializeField] public float waveCountdown;
    public bool readyToCountDown;
    public GameObject newCustomer;



    private void Start()
    {
        FailureUI.SetActive(false);
        SuccessUI.SetActive(false);
        moneyGainedText.enabled = false;
        quotaFailed = true;
        quotaMet = false;
        quotaExceeded = false;

        gameManager = FindObjectOfType<GameManager>();
        
        solQuotaText.text = solQuota.ToString() + " SOL";

    }

    // Update is called once per frame
    void Update()
    {
       
        if (readyToCountDown == true && dayCycle.dayActive)
        {
            waveCountdown -= Time.deltaTime;
        }

        if (waveCountdown <= 0 && dayCycle.dayActive)
        {
            readyToCountDown = false;
            SpawnCustomer();
        }
        
    }

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
            customerName = newCustomer.name;
           
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
        
        moneyGainedText.enabled = true;

        potionVal = Random.Range(60, 100);
                        
        moneyGained = potionVal + (10 * Mathf.RoundToInt(happinessTimer.remainingDuration));

        Debug.Log("order correct, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);

        //AudioManager.Instance.PlaySoundFXClip(orderCorrectSound, transform, 0.5f, Random.Range(0.9f, 1.3f), "OrderCorrectSFX");

        MoneyUpdate();

        CustomerSFXCorrect();

    }
    
    public void OrderIncorrect()
    {
        FailureUI.SetActive(true);
        SuccessUI.SetActive(false);

        moneyGainedText.enabled = true;

        potionVal = Random.Range(0, 15);

        moneyGained = potionVal + (2 * Mathf.RoundToInt(happinessTimer.remainingDuration));

        Debug.Log("order incorrect, customer left this much gold:" + moneyGained + " you had this many seconds left: " + happinessTimer.remainingDuration);

        //AudioManager.Instance.PlaySoundFXClip(orderIncorrectSound, transform, 0.5f, Random.Range(0.9f, 1.3f), "OrderIncorrectSFX");

        MoneyUpdate();

        CustomerSFXIncorrect();

        if (gameManager.endlessSelected)
        {
            dayCycle.LoseHP();
        }

    }

    //updates money gained and total, as well as level end UI total, and updates quota performance
    public void MoneyUpdate()
    {
        
        AudioManager.Instance.PlaySoundFXClip(solGainedSound, transform, 0.5f, Random.Range(0.9f, 1.3f), "SolGainedSFX");

        moneyTotal += moneyGained;
        moneyGainedText.text = "+ " + moneyGained.ToString() + " SOL";
        moneyTotalText.text = moneyTotal.ToString() + " S";
        finalMoneyText.text = moneyTotal.ToString() + " SOL!";
        endlessFinalMoneyText.text = moneyTotal.ToString() + " SOL!";
        Debug.Log(endlessFinalMoneyText);

        //checks if total was under, close to, or exceeded sol quota
        if (moneyTotal <= (solQuota - 51))
        {
            solQuotaMetText.text = "You didn't meet the Sol Quota!";
            quotaFailed = true;
        }
        else if (moneyTotal >= Mathf.Min((solQuota - 49), solQuota) && moneyTotal <= Mathf.Max(solQuota, (solQuota + 100)))
        {
            solQuotaMetText.text = "You met the Sol Quota!";
            quotaFailed = false;
            quotaMet = true;
        }
        else if (moneyTotal >= (solQuota + 200))
        {
            solQuotaMetText.text = "You blew past the Sol Quota!";
            quotaFailed = false;
            quotaMet = false;
            quotaExceeded = true;
        }
    }

    public void CustomerSFXCorrect()
    {

        if (customerSprite.name == "Wolf(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(wolfHappy, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Dragon(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(dragonHappy, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Pin(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(pinHappy, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Snake(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(snakeHappy, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }

    }

    public void CustomerSFXIncorrect()
    {

        if (customerSprite.name == "Wolf(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(wolfMad, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Dragon(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(dragonMad, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Pin(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(pinMad, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }
        else if (customerSprite.name == "Snake(Clone)")
        {
            AudioManager.Instance.PlaySoundFXClip(snakeMad, transform, 0.7f, Random.Range(0.9f, 1.0f), "CustomerSFX");
        }

    }
}
