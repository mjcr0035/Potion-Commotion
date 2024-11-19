using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    public float moveDistance = 100.0f;  // Distance to move to the left
    public float movementSpeed = 2.0f;   // Speed at which the object will move

    // Audio clips
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
        // Only allow interaction if the game is running (Time.timeScale == 1)
        if (Time.timeScale != 1) return;

        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        offset.z = 0;
        isDragging = true;

        // Play pickup sound
        AudioManager.Instance.PlayRandomSoundFXClip(potionPickupSounds, transform, 0.6f, Random.Range(1f, 1.2f));
    }

    // Called when the mouse is dragging the object
    private void OnMouseDrag()
    {
        // Only allow dragging if the game is running
        if (Time.timeScale != 1 || !isDragging) return;

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        newPosition.z = 0;
        transform.position = newPosition;
    }

    // Called when the mouse is released
    private void OnMouseUp()
    {
        // Only allow interaction if the game is running
        if (Time.timeScale != 1) return;

        isDragging = false;

        // Play drop sound
        AudioManager.Instance.PlayRandomSoundFXClip(potionDropSounds, transform, 0.6f, Random.Range(1f, 1.2f));
    }

    // Called continuously while the object is inside another collider marked as "Is Trigger"
    private void OnTriggerStay2D(Collider2D other)
    {
        // Only allow conveyor interaction if the game is running
        if (Time.timeScale != 1) return;

        // Check if the object is not being dragged and it's inside the collider with the specified tag
        if (!isDragging && other.CompareTag("Conveyor"))
        {
            // Ensure the audio clip plays only once while on the conveyor
            if (!GameObject.Find("ConveyorSFX"))
            {
                AudioManager.Instance.PlaySoundFXClip(conveyorSound, transform, 0.3f, 1f, "ConveyorSFX");
            }

            // Move the object to the left smoothly over time
            transform.position = new Vector2(transform.position.x - moveDistance * Time.deltaTime * movementSpeed, ConveyorY);
        }
    }
}
