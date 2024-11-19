using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    public float moveDistance = 100.0f;  // Distance to move to the left
    public float movementSpeed = 2.0f; // Speed at which the object will move

    //audioclips
    public AudioClip[] potionPickupSounds;
    public AudioClip[] potionDropSounds;
    public AudioClip conveyorSound;

    public float ConveyorY = -3.5f;
   

    


    void Start()
    {
        
    }

    // Called when the mouse is pressed down on the object
    private void OnMouseDown()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        offset.z = 0;
        isDragging = true;

        AudioManager.Instance.PlayRandomSoundFXClip(potionPickupSounds, transform, 0.6f, Random.Range(1f, 1.2f));
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
        AudioManager.Instance.PlayRandomSoundFXClip(potionDropSounds, transform, 0.6f, Random.Range(1f, 1.2f));
    }

    // Called continuously while the object is inside another collider marked as "Is Trigger"
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object is not being dragged and it's inside the collider with the specified tag
        if (!isDragging && other.CompareTag("Conveyor"))
        {

            //ensures audioclip will only play once while dragging (hopefully)
            if (!GameObject.Find("ConveyorSFX"))
            {
                AudioManager.Instance.PlaySoundFXClip(conveyorSound, transform, 0.3f, 1f, "ConveyorSFX");
            }
            
            //Snap to conveyor height
            // Move the object to the left smoothly over time
            transform.position = new Vector2(transform.position.x - moveDistance * Time.deltaTime * movementSpeed, ConveyorY);

          
        }
    }

}





