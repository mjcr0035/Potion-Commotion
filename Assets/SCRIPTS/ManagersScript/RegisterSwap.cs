using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RegisterSwap : MonoBehaviour
{
    public GameObject registerButton;
    public GameObject backroomButton;

    public GameObject recipeButton;
    public GameObject recipeBook;

    public GameObject TUTORIAL2;
    

    public Transform registerTransform;
    public Transform backroomTransform;

    public GameManager gameManager;
    

    public GameObject gameplayCamera;
    public float lerpSpeed = 1.0f; //lerp speed
    //Vector3 backroomPosition = new Vector3(22.5f, 0, -1);
    //Vector3 registerPosition = new Vector3(0, 0, -1);
    bool SwitchingToRegister = false;
    bool SwitchingToBackroom = false;

    

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = FindObjectOfType<GameManager>();

        registerButton.gameObject.SetActive(false);
        backroomButton.gameObject.SetActive(true);

        recipeButton.gameObject.SetActive(false);
        recipeBook.gameObject.SetActive(false);

    }

    public void Update(){
        if(SwitchingToBackroom) 
        {

            gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, backroomTransform.position, lerpSpeed * Time.deltaTime);

        }
        if(SwitchingToRegister) 
        {

            gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, registerTransform.position, lerpSpeed * Time.deltaTime);

        }
    }
    
    //swaps screen to register
    public void registerChange()
    {
        
        registerButton.gameObject.SetActive(false);
        backroomButton.gameObject.SetActive(true);

        SwitchingToRegister = true;
        SwitchingToBackroom = false;

        //gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, registerTransform.position, lerpSpeed * Time.deltaTime);
        recipeButton.gameObject.SetActive(false);
        recipeBook.gameObject.SetActive(false);

        

        Debug.Log("screen changed to register");
    }

    //swaps screen to backroom
    public void backroomChange()
    {

        backroomButton.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(true);

        SwitchingToBackroom = true;
        SwitchingToRegister = false;

        //gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, backroomTransform.position, lerpSpeed * Time.deltaTime);
        recipeButton.gameObject.SetActive(true);
        recipeBook.gameObject.SetActive(false);

        if (gameManager.tutorialSelected)
        {
            TUTORIAL2.gameObject.SetActive(true);
        }

        Debug.Log("screen changed to backroom");
    }


}
