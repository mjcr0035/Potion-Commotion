
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerPrefs : MonoBehaviour
{
    public string targetObjectName = "Potion1prefab"; // Set this to the name of the specific game object

    //private string SuccessUIObjectName = "SuccessUI";
    //private string FailureUIObjectName = "FailureUI";

    //NOTE: this is not the final way of doing this, just kind of grabbing refs to UI elements for the sake of the prototype -Jackson
    //public GameObject SuccessUIObjectRef;
    //public GameObject FailureUIObjectRef;

    //public CanvasRenderer successImage;
    //public CanvasRenderer failureImage;
        
    public CustomerManager customerManager;

    //--
    //matthew:
    //customer order UI stuff should be added here so that customer prefabs come with their orders by default!?
    //if not we can create a separate script for order randomization and tweak the script in here from that order script.
    //public SpriteRenderer OrderUISprite;
    //--



    // Start is called before the first frame update
    void Start()
    {
        //Display 1st ingredient in speech bubble

        //Display 2nd ingredient in speech bubble

        //Display finished product in speech bubble

        
        customerManager = FindObjectOfType<CustomerManager>();

        //SuccessUIObjectRef = GameObject.FindWithTag("SuccessUI");
        //FailureUIObjectRef = GameObject.FindWithTag("FailureUI");

        //successImage = SuccessUIObjectRef.GetComponent<Image>();
        //failureImage = SuccessUIObjectRef.GetComponent<Image>();
    }

    
    // Update is called once per frame
    void Update()
    {
        //if correct potion tag is dragged over player
            //display success UI
        //else
            //display fail UI

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object's name matches the specified name
        if (collision.gameObject.name == targetObjectName)
        {
            // Perform your logic here
            Debug.Log("Potion Received! Colliding with the target object: " + collision.gameObject.name);

            //StartCoroutine(HideAndShow(SuccessUIObjectName));


            //FailureUIObjectRef.SetActive(false);
            //SuccessUIObjectRef.SetActive(true);
            
            //successImage.enabled = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Potion"))
                    {
                        Destroy(obj); // Destroy any existing potions in the scene
                    }

            customerManager.StartCoroutine("DespawnCustomer");
            

        }
        else
        {
            //Wrong
            Debug.Log("Incorrect Potion!");

            //StartCoroutine(HideAndShow(FailureUIObjectName));

            //FailureUIObjectRef.SetActive(true);
            //SuccessUIObjectRef.SetActive(false);
            

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Potion"))
                    {
                        Destroy(obj); // Destroy any existing potions in the scene
                    }

            customerManager.StartCoroutine("DespawnCustomer");

        }
    }

    //private IEnumerator HideAndShow(string objectName)
    //{
    //    // Find the GameObject by name
    //    GameObject targetObject = GameObject.Find(objectName);
    //    
    //    // Check if the object was found
    //    if (targetObject != null)
    //    {
    //        // Hide the GameObject by disabling its Renderer
    //        Renderer renderer = targetObject.GetComponent<Renderer>();
    //        if (renderer != null)
    //        {
    //            renderer.enabled = false;
    //            Debug.Log(objectName + " has been hidden.");
    //        }
    //
    //        // Wait for 3 seconds
    //        yield return new WaitForSeconds(3f);
    //
    //        // Show the GameObject by enabling its Renderer
    //        if (renderer != null)
    //        {
    //            renderer.enabled = true;
    //            Debug.Log(objectName + " has been shown.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("GameObject with name " + objectName + " not found.");
    //    }
    //}

}
