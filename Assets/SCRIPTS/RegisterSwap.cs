using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RegisterSwap : MonoBehaviour
{
    //matthew:

    //targets objects that are going to be swapped out, disabled, or moved
    //get their transforms
    //on button press, setactive gameobjects
    
    //there is 10000% an easier way to do this but I didnt know if making a new function with parameters-
    //-would work or not so i left this as is!!
    
    //transforms are still being asked for so we can animate it later.

    //assets to modify
    public GameObject register;
    public GameObject backroom;
    
    public GameObject registerButton;
    public GameObject backroomButton;
    
    public Transform registerTransform;
    public Transform backroomTransform;
    
    public GameObject registerBackground;
    public GameObject backroomBackground;

    public GameObject gameplayCamera;
    public float lerpSpeed = 1.0f; //lerp speed

    Vector3 newPosition;
    bool SwitchingToRegister = false;
    bool SwitchingToBackroom = false;

    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //start on customer screen, set corresponding backgrounds, gameobjects, and buttons to t/f

        //backroom.gameObject.SetActive(false);
        //register.gameObject.SetActive(true);
        
        registerButton.gameObject.SetActive(false);
        backroomButton.gameObject.SetActive(true);

        //registerBackground.gameObject.SetActive(true);
        //backroomBackground.gameObject.SetActive(false);

        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // Find the SoundManager if not assigned
        }

    }

    public void Update(){
        if(SwitchingToBackroom){
            gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, backroomTransform.position, lerpSpeed * Time.deltaTime);
        }
        if(SwitchingToRegister){
            gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, registerTransform.position, lerpSpeed * Time.deltaTime);
        }
    }
    
    //swaps screen to register
    public void registerChange()
    {
        //screenSwap = true;
        //backroom.gameObject.SetActive(false);
        //register.gameObject.SetActive(true);
        
        registerButton.gameObject.SetActive(false);
        backroomButton.gameObject.SetActive(true);

        //registerBackground.gameObject.SetActive(true);
        //backroomBackground.gameObject.SetActive(false);

        SwitchingToRegister = true;
        SwitchingToBackroom = false;

        //gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, registerTransform.position, lerpSpeed * Time.deltaTime);

        soundManager.PlaySound(7);
        Debug.Log("screen changed to register");
    }

    //swaps screen to backroom
    public void backroomChange()
    {
        //screenSwap = false;
        //register.gameObject.SetActive(false);
        //backroom.gameObject.SetActive(true);

        backroomButton.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(true);

        //registerBackground.gameObject.SetActive(false);
        //backroomBackground.gameObject.SetActive(true);

        SwitchingToBackroom = true;
        SwitchingToRegister = false;

        //gameplayCamera.transform.position = Vector3.Lerp(gameplayCamera.transform.position, backroomTransform.position, lerpSpeed * Time.deltaTime);

        soundManager.PlaySound(7);
        Debug.Log("screen changed to backroom");
    }


}
