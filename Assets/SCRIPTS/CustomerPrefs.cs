
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerPrefs : MonoBehaviour
{
    public string targetObjectName = "Potion1"; // Set this to the name of the specific game object

    //targetobject[""] = potion1 potion2 potion3 


    //private string SuccessUIObjectName = "SuccessUI";
    //private string FailureUIObjectName = "FailureUI";

    //NOTE: this is not the final way of doing this, just kind of grabbing refs to UI elements for the sake of the prototype -Jackson
    

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


        //Display wanted potion in speech bubble
  
        
        customerManager = FindObjectOfType<CustomerManager>();

        
    }

 
    
    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object's name matches the specified name
        if (collision.gameObject.name == targetObjectName)
        {
            //Correct Potion
            Debug.Log("Potion Received! Colliding with the target object: " + collision.gameObject.name);

            //StartCoroutine(HideAndShow(SuccessUIObjectName));


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

            customerManager.OrderIncorrect();
            
            //StartCoroutine(HideAndShow(FailureUIObjectName));


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
