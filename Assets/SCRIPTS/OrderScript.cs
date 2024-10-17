using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{
    [SerializeField] Sprite[] potionImages;

    [SerializeField] SpriteRenderer spriteRenderer;

    CustomerPrefs customerPrefs;

    int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        customerPrefs = FindObjectOfType<CustomerPrefs>();
        randomPotion();
    }

    void randomPotion()
    {
        randomNumber = Random.Range(0, potionImages.Length);
        spriteRenderer.sprite = potionImages[randomNumber];
        Debug.Log(randomNumber);
        //include ref to customerprefs potion field
        //customerPrefs.targetObjectName = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
