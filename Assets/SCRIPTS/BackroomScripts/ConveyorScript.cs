using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    public GameObject Arrow;
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object is not being dragged and it's inside the collider with the specified tag
        if (other.CompareTag("Potion"))
        {
            Arrow.SetActive(false);
        }
    }
}
