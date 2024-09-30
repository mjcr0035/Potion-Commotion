using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    public float moveDistance = 100.0f;  // Distance to move to the left
    public float movementSpeed = 2.0f; // Speed at which the object will move

    // Called when the mouse is pressed down on the object
    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        offset.z = 0;
        isDragging = true;
    }

    // Called when the mouse is dragging the object
    private void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        newPosition.z = 0;
        transform.position = newPosition;
    }

    // Called when the mouse is released
    private void OnMouseUp()
    {
        isDragging = false;
    }

    // Called continuously while the object is inside another collider marked as "Is Trigger"
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object is not being dragged and it's inside the collider with the specified tag
        if (!isDragging && other.CompareTag("Counter"))
        {
            // Move the object to the left smoothly over time
            transform.position = new Vector2(transform.position.x - moveDistance * Time.deltaTime * movementSpeed, transform.position.y);
        }

        // Check if the object was dragged to the customer
        if (!isDragging && other.CompareTag("Customer"))
        {
            //debug if recieved
            //Debug.Log("Potion Delivered");
            
        }
    }
}





