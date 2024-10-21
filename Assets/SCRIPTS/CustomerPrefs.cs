
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerPrefs : MonoBehaviour
{
    
    public string requestedPotion;

    public CustomerManager customerManager;

    [SerializeField] Sprite[] potionImages;

    [SerializeField] SpriteRenderer spriteRenderer;

    int randomNumber;

    public GameObject targetObject;  //Speech Bubble Toggle

    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // Find the SoundManager if not assigned
        }

        customerManager = FindObjectOfType<CustomerManager>();
        
        soundManager.PlaySound(2);
        soundManager.PlaySound(3);
        randomPotion();

    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object's name matches the specified name
        if (collision.gameObject.name == requestedPotion)
        {
            //Correct potion received
            Debug.Log(collision.gameObject.name + " Received!");
            soundManager.PlaySound(4);
            soundManager.PlaySound(5);
                        

            customerManager.OrderCorrect();


            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Potion"))
                    {
                        Destroy(obj); // Destroy any existing potions in the scene
                    }
                        

            customerManager.StartCoroutine("DespawnCustomer");
            

        }
        else
        {
            //Wrong Potion
            Debug.Log("Incorrect Potion!");

            soundManager.PlaySound(4);
            soundManager.PlaySound(6);

            customerManager.OrderIncorrect();
                      

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Potion"))
                    {
                        Destroy(obj); // Destroy any existing potions in the scene
                    }

            
            customerManager.StartCoroutine("DespawnCustomer");

        }
    }

    void randomPotion()
    {
        randomNumber = Random.Range(0, potionImages.Length);
        spriteRenderer.sprite = potionImages[randomNumber];

        requestedPotion = spriteRenderer.sprite.name + "(Clone)";

        Debug.Log(requestedPotion);

    }

    void OnMouseDown()
    {
        // Make the target object visible when the sprite is clicked
        if (targetObject != null)
        {
            targetObject.SetActive(true);  // Set the object to active/visible
        }
    }


}
