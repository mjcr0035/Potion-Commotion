using System.Collections;
using System.Collections.Generic;
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

    //needed to ensure customer instantiation is a child of register screen
    public GameObject registerParent;

    public GameObject orderUI;

    public GameObject happinessUI;

    private GameObject newCustomer;

    public int customerHappiness;

    public HappinessTimer happinessTimer;

        
    // Update is called once per frame
    void Update()
    {
        //temporary customer spawner until we figure out how we want days to work and stuff
        //could make this an on screen button instead so it works on mobile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        //if customer prefabs are assigned, and there is no customer on screen currently, spawn customer.
        //assigns customer animator and sprite renderer
        //activates orderUI and happiness UI
        //triggers happiness timer
                
        if (customerPrefabs != null && !newCustomer)
        {
            int customerSpawnIndex = Random.Range(0, customerPrefabs.Length);
            Instantiate(customerPrefabs[customerSpawnIndex], registerParent.transform);

            newCustomer = GameObject.FindWithTag("Customer");

            customerSprite = newCustomer.GetComponent<SpriteRenderer>();
            customerAC = newCustomer.GetComponent<Animator>();
            
            happinessUI.SetActive(true);

            //starts the timer at a random number every spawn
            //happinessTimer.StartHappinessTimer(Random.Range(7,10));
            happinessTimer.StartHappinessTimer(10);

            orderUI.SetActive(true);
                       
        }
        
    }
    
    public IEnumerator DespawnCustomer()
    {
        //if a customer is on screen, disables UI elements and plays despawn animation
        //then waits until animation is over and destroys the newcustomer
        
        if (newCustomer)
        {
            Debug.Log("customer despawning!!!");

            happinessUI.SetActive(false);
            orderUI.SetActive(false);

            customerAC.SetTrigger("Despawn");

            yield return new WaitForSeconds(2);

            Destroy(newCustomer);
        }
               
    }
    
    //public void DespawnCustomer2()
    //{
    //    //disable orderUI and happinessUI
    //    //play exit animation,
    //    //wait till animation is over
    //    //destroy that gameobject with the customer tag?
    //    //pause happiness timer?
    //    //customerHappiness = 10;

    //    Debug.Log("customer despawning!!!");

    //    happinessUI.SetActive(false);
    //    orderUI.SetActive(false);
    //    customerAC.SetTrigger("Despawn");

    //    if (customerAC.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
    //    {
    //        //waits for animation to end before destroying the customer
            
    //        Debug.Log("animation is done now");
    //        Destroy(newCustomer);
    //    }
        

        
    //}

    
}
