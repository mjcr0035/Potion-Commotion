using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{
    [SerializeField] Sprite[] potionImages;

    [SerializeField] SpriteRenderer spriteRenderer;


    int randomNumber;

    // Start is called before the first frame update
    void Start()
    {
        
        randomPotion();
    }

    void randomPotion()
    {
        randomNumber = Random.Range(0, potionImages.Length);
        spriteRenderer.sprite = potionImages[randomNumber];
        
        //requestedPotion = spriteRenderer.sprite;

        Debug.Log(randomNumber);
         
    }

}
